using System;
using System.Threading.Tasks;
using System.Web.Http;
using controlfacturasws.Dao;
using controlfacturasws.Models;
using controlfacturasws.Services;

namespace controlfacturasws.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/prueba")]
    public class PruebasController : ApiController
    {
        [HttpGet]
        [Route("correosendgrid")]
        public IHttpActionResult correoSendGrid()
        {
            Email email = new Email();
            EmailService emailService = new EmailService();

            email.asunto = "Asunto de prueba";
            email.correoDestino = "martin97polo@gmail.com";
            email.nombreDestino = "Martin Rodriguez";
            email.textoHtml = "Cuerpo del mensaje";
            email.numeroDoc = "SETP990000206";

            /*
            Task res = emailService.sendGrid(email);
            return Ok(res);
            */

            return Ok();
        }

        [HttpGet]
        [Route("pdf")]
        public IHttpActionResult pdf()
        {
            FacturacionColombia fc = new FacturacionColombia();
            fc.generarPdf("SETP990000206");
            return Ok();
        }

        [HttpGet]
        [Route("config")]
        public IHttpActionResult config()
        {
            ConfiguracionDao dao = new ConfiguracionDao();
            return Ok(dao.consultar());
        }

        [HttpGet]
        [Route("log")]
        public IHttpActionResult log()
        {
            try
            {
                string str = string.Empty;
                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("Wrong Data");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                LogService.escribir(ex);
                return BadRequest();
            }
        }

    }
}
