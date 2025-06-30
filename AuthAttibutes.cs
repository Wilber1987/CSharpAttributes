using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using APPCORE.Security;

namespace API.Controllers
{
	public class AuthControllerAttribute : ActionFilterAttribute
	{
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1000, 1000); // Permite solo una operación concurrente
		public Permissions[] PermissionsList { get; set; }
		public AuthControllerAttribute()
		{
			PermissionsList = [];
		}
		public AuthControllerAttribute(params Permissions[] permissionsList)
		{
			PermissionsList = permissionsList ?? [];
		}
		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Intentar entrar al semáforo
			await _semaphore.WaitAsync();
			try
			{
				string? token = context.HttpContext.Session.GetString("sessionKey");

				// LICENCIA
				if (DateTime.Now > new DateTime(2025, 10, 01))
				{
					context.Result = new ObjectResult(new Authenticate
					{
						AuthVal = false,
						Message = "Licence expired"
					})
					{ StatusCode = 403 };

					return; // <- Detiene ejecución
				}
				// Autenticación
				if (!AuthNetCore.Authenticate(token))
				{
					context.Result = new ObjectResult(new Authenticate
					{
						AuthVal = false
					});
					return;
				}
				// Permisos
				if (PermissionsList.Length > 0 && !AuthNetCore.HavePermission(token, PermissionsList))
				{
					context.Result = new ObjectResult(new Authenticate
					{
						AuthVal = false,
						Message = "Inaccessible resource"
					})
					{ StatusCode = 401 };

					return;
				}

				// Si pasa todo, continúa con la acción
				var executedContext = await next();
				// Forzar recolección si la respuesta es grande
				if (executedContext.Result is ObjectResult result && result.Value is string responseString)
				{
					GC.Collect();
					GC.WaitForPendingFinalizers();
				}
			}
			finally
			{
				_semaphore.Release();
			}
		}		
	}

	public class SemaphoreControllerAttribute : ActionFilterAttribute
	{
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(20, 20); // Permite solo una operación concurrente		

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Intentar entrar al semáforo
			await _semaphore.WaitAsync();
			try
			{
				// Ejecutar la acción del controlador
				var executedContext = await next();
				// Si la respuesta es grande, forzar la liberación de memoria
				if (executedContext.Result is ObjectResult result && result.Value is string responseString)
				{
					GC.Collect(); // Solo si la respuesta es grande
					GC.WaitForPendingFinalizers();
				}
			}
			finally
			{
				// Liberar el semáforo para permitir la siguiente operación
				_semaphore.Release();
			}
		}

	}

	public class AdminAuthAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!AuthNetCore.HavePermission(Permissions.ADMIN_PANEL_ACCESS.ToString(), filterContext.HttpContext.Session.GetString("seassonKey")))
			{
				Authenticate Aut = new Authenticate();
				Aut.AuthVal = false;
				Aut.Message = "Inaccessible resource";
				filterContext.Result = new ObjectResult(Aut) { StatusCode = 401 };
			}
		}
	}
	public class AnonymousAuthAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			AuthNetCore.AnonymousAuthenticate();
		}
	}
	class Authenticate
	{
		public bool AuthVal { get; set; }
		public string? Message { get; set; }
	}
}
