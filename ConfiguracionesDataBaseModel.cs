using API.Controllers;
using CAPA_DATOS;
namespace DataBaseModel
{
	public class Transactional_Configuraciones : EntityClass
	{
		[PrimaryKey(Identity = true)]
		public int? Id_Configuracion { get; set; }
		public string? Nombre { get; set; }
		public string? Descripcion { get; set; }
		public string? Valor { get; set; }
		public string? Tipo_Configuracion { get; set; }
		internal Transactional_Configuraciones? GetConfig(String prop)
		{
			Nombre = prop;
			return Find<Transactional_Configuraciones>();
		}
		internal List<Transactional_Configuraciones> GetTheme()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion != null &&
				 x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.THEME.ToString())).ToList();
		}
		internal List<Transactional_Configuraciones> GetTypeNumbers()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion != null &&
				 x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.NUMBER.ToString())).ToList();
		}

		internal List<Transactional_Configuraciones> GetGeneralData()
		{
			return Get<Transactional_Configuraciones>()
			   .Where(x => x.Tipo_Configuracion != null &&
				x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.GENERAL_DATA.ToString())).ToList();
		}

		internal object? UpdateConfig(string? identity)
		{
			if (!AuthNetCore.HavePermission(CAPA_DATOS.Security.Permissions.ADMIN_ACCESS.ToString(), identity))
			{
				throw new Exception("no tienes permisos para configurar la aplicación");
			}
			return this.Update();
		}
	}
	public class PageConfig
	{
		public PageConfig()
		{
			configuraciones = new Transactional_Configuraciones().Get<Transactional_Configuraciones>();
			TITULO = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.TITULO.ToString()))?.Valor ?? TITULO;
			SUB_TITULO = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.SUB_TITULO.ToString()))?.Valor ?? SUB_TITULO;
			NOMBRE_EMPRESA = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.NOMBRE_EMPRESA.ToString()))?.Valor ?? NOMBRE_EMPRESA;
			LOGO_PRINCIPAL = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.LOGO_PRINCIPAL.ToString()))?.Valor ?? LOGO_PRINCIPAL;
			MEDIA_IMG_PATH = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.MEDIA_IMG_PATH.ToString()))?.Valor ?? MEDIA_IMG_PATH;
			VERSION = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.VERSION.ToString()))?.Valor ?? VERSION;

		}
		public string TITULO = "TEMPLATE";
		public string SUB_TITULO = "Template";
		public string NOMBRE_EMPRESA = "TEMPLATE";
		public string LOGO_PRINCIPAL = "logo.png";
		public string MEDIA_IMG_PATH = "/media/img/";
		public string VERSION = "2024.07";
		public List<Transactional_Configuraciones> configuraciones = new List<Transactional_Configuraciones>();
	}

	public enum ConfiguracionesTypeEnum
	{
		THEME, GENERAL_DATA, NUMBER
	}

	public enum ConfiguracionesThemeEnum
	{
		TITULO, SUB_TITULO, NOMBRE_EMPRESA, LOGO_PRINCIPAL, LOGO, MEDIA_IMG_PATH,
        VERSION
    }

	public class Config
	{
		public static PageConfig pageConfig()
		{
			return new PageConfig();
		}
	}

}
