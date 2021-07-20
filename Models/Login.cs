using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace controlfacturasws.Models
{
    public class Login
    {
        public String id { get; set; }
        public String nombre { get; set; }
        public String email { get; set; }
        public String token {get;set; }
        public String rol { get; set; }
        public Configuracion configuracion { get; set; }
    }
}