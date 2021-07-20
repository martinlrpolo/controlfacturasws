using System.Collections.Generic;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;

namespace controlfacturasws.Controllers
{
    [Authorize]
    [RoutePrefix("api/catalogo")]
    public class CatalogoController : ApiController
    {
        [HttpGet]
        [Route("listar/{tipo}")]
        public IHttpActionResult listar(string tipo)
        {
            CatalogoDao dao = new CatalogoDao();
            List<Catalogo> roles = new List<Catalogo>();
            roles = dao.listar(tipo);
            return Ok(roles);
        }
    }
}
