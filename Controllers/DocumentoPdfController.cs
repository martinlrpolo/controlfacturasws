using System.Collections.Generic;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;

namespace controlfacturasws.Controllers
{
    [Authorize]
    [RoutePrefix("api/documentopdf")]
    public class DocumentoPdfController : ApiController
    {
        [HttpGet]
        [Route("listar")]
        public IHttpActionResult listar()
        {
            DocumentoPdfDao dao = new DocumentoPdfDao();
            List<DocumentoPdf> documentos = new List<DocumentoPdf>();
            documentos = dao.listar();
            return Ok(documentos);
        }

    }
}
