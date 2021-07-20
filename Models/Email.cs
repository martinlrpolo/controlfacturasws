using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace controlfacturasws.Models
{
    public class Email
    {
        public String nombreOrigen { get; set; }
        public String correoOrigen { get; set; }
        public String nombreDestino { get; set; }
        public String correoDestino { get; set; }
        public String asunto { get; set; }
        public String textoHtml { get; set; }
        public String numeroDoc { get; set; }

    }
}