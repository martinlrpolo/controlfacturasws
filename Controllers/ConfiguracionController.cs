using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;

namespace controlfacturasws.Controllers
{
    [Authorize]
    [RoutePrefix("api/configuracion")]
    public class ConfiguracionController : ApiController
    {
        [HttpGet]
        [Route("consultar")]
        public IHttpActionResult consultar()
        {
            ConfiguracionDao dao = new ConfiguracionDao();
            var res = dao.consultar();
            return Ok(res);
        }

        [HttpPut]
        [Route("editar")]
        public IHttpActionResult agregar([FromBody] Configuracion configuracion)
        {
            ConfiguracionDao dao = new ConfiguracionDao();
            configuracion = dao.editar(configuracion);

            if (configuracion != null)
                return Ok(configuracion);
            else
                return BadRequest();
        }


    }
}
