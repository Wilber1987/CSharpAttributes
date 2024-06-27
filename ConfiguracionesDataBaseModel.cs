using API.Controllers;
using CAPA_DATOS;
using CAPA_DATOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		internal Transactional_Configuraciones GetConfig(String prop)
		{
			Nombre = prop;
			return Find<Transactional_Configuraciones>();
		}

		internal List<Transactional_Configuraciones> GetIntereses()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.INTERESES.ToString())).ToList();
		}
		internal List<Transactional_Configuraciones> GetTheme()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.THEME.ToString())).ToList();
		}
		internal List<Transactional_Configuraciones> GetTypeNumbers()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.NUMBER.ToString())).ToList();
		}
		internal List<Transactional_Configuraciones> GetBeneficios()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.BENEFICIOS.ToString())).ToList();
		}

		internal List<Transactional_Configuraciones>  GetGeneralData()
		{
			return Get<Transactional_Configuraciones>()
			   .Where(x => x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.GENERAL_DATA.ToString())).ToList();
		}

		internal object? UpdateConfig(string? identity)
		{
			if (!AuthNetCore.HavePermission(CAPA_DATOS.Security.Permissions.ADMIN_ACCESS.ToString(), identity))
			{
				throw new Exception("no tienes permisos para configurar la aplicación");
			}
			if (Nombre.Equals(GeneralDataEnum.FIRMA_DIGITAL_APODERADO.ToString()))
			{
				var pic = (ModelFiles)FileService.upload("profiles\\", new ModelFiles
				{
					Value = Valor,
					Type = "png",
					Name = "profile"
				}).body;
				//Valor = pic.Value.Replace("wwwroot", "");
			}
			return this.Update();
		}
	}
	public class PageConfig
	{
		public PageConfig()
		{
			configuraciones = new Transactional_Configuraciones().Get<Transactional_Configuraciones>();
			TITULO = configuraciones.Find(c => c.Nombre.Equals(ConfiguracionesThemeEnum.TITULO.ToString()))?.Valor ?? TITULO;
			SUB_TITULO = configuraciones.Find(c => c.Nombre.Equals(ConfiguracionesThemeEnum.SUB_TITULO.ToString()))?.Valor ?? SUB_TITULO;
			NOMBRE_EMPRESA = configuraciones.Find(c => c.Nombre.Equals(ConfiguracionesThemeEnum.NOMBRE_EMPRESA.ToString()))?.Valor ?? NOMBRE_EMPRESA;
			LOGO_PRINCIPAL = configuraciones.Find(c => c.Nombre.Equals(ConfiguracionesThemeEnum.LOGO_PRINCIPAL.ToString()))?.Valor ?? LOGO_PRINCIPAL;

		}
		public string TITULO = "TEMPLATE";
		public string SUB_TITULO = "Template";
		public string NOMBRE_EMPRESA = "TEMPLATE";
		public string LOGO_PRINCIPAL = "logo.png";
		public string MEDIA_IMG_PATH = "/media/img/";
		public List<Transactional_Configuraciones> configuraciones = new List<Transactional_Configuraciones>();
	}
	
    public enum ConfiguracionesTypeEnum
    {
        INTERESES, BENEFICIOS, THEME, INTERESES_MORA,
        GENERAL_DATA, NUMBER
    }

    public enum ConfiguracionesThemeEnum
    {
        TITULO, SUB_TITULO, NOMBRE_EMPRESA, LOGO_PRINCIPAL,
        LOGO
    }

    public enum ConfiguracionesInteresesEnum
    {
        MORA_CONTRATOS_EMP
    }

    public enum InteresesPrestamosEnum
    {
        GASTOS_ADMINISTRATIVOS,
        COMISIONES,
        MANTENIMIENTO_VALOR,
        GASTOS_LEGALES,
        INTERES_NETO_CORRIENTE
    }

    public enum GeneralDataEnum
    {
        APODERADO, FIRMA_DIGITAL_APODERADO,  DATOS_APODERADO,
        APODERADO_VICEPRESIDENTE,
        DATOS_APODERADO_VICEPRESIDENTE,
        FIRMA_DIGITAL_APODERADO_VICEPRESIDENTE,
        CEDULA_APODERADO,
        CEDULA_APODERADO_VICEPRESIDENTE
    }



    public enum ConfiguracionesBeneficiosEnum
    {
        BENEFICIO_VENTA_ARTICULO_COMPRADO, BENEFICIO_VENTA_ARTICULO_EMPENO
    }
    public enum ConfiguracionesVencimientos
    {
        VENCIMINETO_VALORACION, VENCIMIENTO_CONTRATO
    }

    public class Config
    {
        public static PageConfig pageConfig()
        {
            return new PageConfig();
        }
    }

}
