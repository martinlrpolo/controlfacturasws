using System.Collections.Generic;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;
using System;

namespace controlfacturasws.Controllers
{
    [Authorize]
    [RoutePrefix("api/enviodian")]
    public class EnvioDIANController : ApiController
    {
        [HttpGet]
        [Route("listar")]
        public IHttpActionResult listar()
        {
            EnvioDianDao dao = new EnvioDianDao();
            List<EnvioDIAN> envios = new List<EnvioDIAN>();
            envios = dao.listar();
            return Ok(envios);
        }

      

        [HttpGet]
        [Route("listarxestado({idEstado}")]
        public IHttpActionResult listarPorEstado(String idEstado)
        {
            EnvioDianDao dao = new EnvioDianDao();
            List<EnvioDIAN> envios = new List<EnvioDIAN>();
            envios = dao.listarPorEstado(idEstado);

            if (envios != null)
                return Ok(envios);
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("consultar/{id}")]
        public IHttpActionResult consultar(String id)
        {
            EnvioDianDao dao = new EnvioDianDao();
            EnvioDIAN envio = new EnvioDIAN();
            envio = dao.consultar(id);

            if (envio != null)
                return Ok(envio);
            else
                return BadRequest();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IHttpActionResult eliminar(String id)
        {
            EnvioDianDao dao = new EnvioDianDao();
            bool res;
            res = dao.elimiar(id);

            if (res == true)
                return Ok(res);
            else
                return BadRequest();
        }

    }
}
