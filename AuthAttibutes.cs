using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using APPCORE.Security;

namespace API.Controllers
{
	public class AuthControllerAttribute : ActionFilterAttribute
	{
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(20, 20); // Permite solo una operación concurrente
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
				// Ejecutar la acción del controlador
				var executedContext = await next();
				// Si la respuesta es grande, forzar la liberación de memoria
				if (executedContext.Result is ObjectResult result && result.Value is string responseString && responseString.Length > 1000)
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
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string? token = filterContext.HttpContext.Session.GetString("sessionKey");

			//LICENCIA
			if (DateTime.Now > new DateTime(2025, 10, 01))
			{
				Authenticate Aut = new()
				{
					AuthVal = false,
					Message = "Licence expired"
				};
				filterContext.Result = new ObjectResult(Aut) { StatusCode = 403 };
			}
			if (!AuthNetCore.Authenticate(token))
			{
				Authenticate Aut = new Authenticate
				{
					AuthVal = false
				};
				filterContext.Result = new ObjectResult(Aut);
			}
			if (PermissionsList.Length > 0 && !AuthNetCore.HavePermission(token, PermissionsList))
			{
				Authenticate Aut = new Authenticate
				{
					AuthVal = false,
					Message = "Inaccessible resource"
				};
				filterContext.Result = new ObjectResult(Aut) { StatusCode = 401 };
			}
		}
	}

	public class AdminAuthAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!AuthNetCore.HavePermission(Permissions.ADMIN_ACCESS.ToString(), filterContext.HttpContext.Session.GetString("sessionKey")))
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
