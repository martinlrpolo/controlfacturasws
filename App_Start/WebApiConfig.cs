using controlfacturasws.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace controlfacturasws
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Rutas de API web
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.MessageHandlers.Add(new TokenValidationHandler());
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();
        }
    }
}
