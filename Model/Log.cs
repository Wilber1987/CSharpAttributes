using CAPA_DATOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataBaseModel {
   public class Log : EntityClass {
       [PrimaryKey(Identity = true)]
       public int? Id_Log { get; set; }
       public string? LogType { get; set; }
       public DateTime? Fecha { get; set; }
       public string? Message { get; set; }
       public string? Body { get; set; }
   }
}
