using APPCORE.Security;
using DataBaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiEntityAdministrative_AccessController : ControllerBase
    {
        //Transactional_Configuraciones
        [HttpPost]
        [AuthController(Permissions.ADMIN_PANEL_ACCESS)]
        public List<Transactional_Configuraciones> getTransactional_Configuraciones(Transactional_Configuraciones Inst)
        {
            return Inst.Get<Transactional_Configuraciones>();
        }
        [HttpPost]
		[AuthController(Permissions.ADMIN_ACCESS)]
		public List<Transactional_Configuraciones> getConfiguraciones_Theme(Transactional_Configuraciones Inst)
		{
			return Inst.GetTheme();
		}        
        [HttpPost]
        [AuthController(Permissions.ADMIN_PANEL_ACCESS)]
        public List<Transactional_Configuraciones> getTransactional_Configuraciones_Theme(Transactional_Configuraciones Inst)
        {
            return Inst.GetTheme();
        }        
        [HttpPost]
        [AuthController(Permissions.ADMIN_PANEL_ACCESS)]
        public object saveTransactional_Configuraciones(Transactional_Configuraciones inst)
        {
            return inst?.Save();
        }
        [HttpPost]
        [AuthController(Permissions.ADMIN_PANEL_ACCESS)]
        public object? updateTransactional_Configuraciones(Transactional_Configuraciones inst)
        {
            return inst.UpdateConfig(HttpContext.Session.GetString("sessionKey"));
        }
        
        //endpoints de empresa
        [HttpPost]
        [AuthController(Permissions.GESTION_EMPEÑOS, Permissions.GESTION_PRESTAMOS)]
        public List<Transactional_Configuraciones> getConfiguraciones_Intereses(Transactional_Configuraciones Inst)
        {
            return Inst.GetIntereses();
        }
        [HttpPost]
        [AuthController(Permissions.GESTION_EMPEÑOS, Permissions.GESTION_PRESTAMOS)]
        public List<Transactional_Configuraciones> getConfiguraciones_Beneficios(Transactional_Configuraciones Inst)
        {
            return Inst.GetBeneficios();
        }
        
        [HttpPost]
        [AuthController]
		public List<Transactional_Configuraciones> getConfiguraciones_Configs(Transactional_Configuraciones Inst)
		{
			return Inst.GetTypeNumbers();
		}
    }
}
