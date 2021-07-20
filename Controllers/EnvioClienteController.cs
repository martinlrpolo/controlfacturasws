using System.Collections.Generic;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;
using System;
using controlfacturasws.Services;
using System.Configuration;

namespace controlfacturasws.Controllers
{
    [Authorize]
    [RoutePrefix("api/enviocliente")]
    public class EnvioClienteController : ApiController
    {
        [HttpGet]
        [Route("listar")]
        public IHttpActionResult listar()
        {
            EnvioClienteDao dao = new EnvioClienteDao();
            List<EnvioCliente> envios = new List<EnvioCliente>();
            envios = dao.listar();
            return Ok(envios);
        }

        [HttpGet]
        [Route("listarxusuario/{codigo}")]
        public IHttpActionResult listarPorUsuario(String codigo)
        {
            EnvioClienteDao dao = new EnvioClienteDao();
            List<EnvioCliente> envios = new List<EnvioCliente>();
            envios = dao.listarPorUsuario(codigo);
            return Ok(envios);
        }

        [HttpPost]
        [Route("agregar")]
        public IHttpActionResult agregar([FromBody] EnvioCliente envio)
        {
            EnvioClienteDao dao = new EnvioClienteDao();
            envio = dao.agregar(envio);

            if (envio != null)
                return Ok(envio);
            else
                return BadRequest();
        }

        [HttpPut]
        [Route("editar")]
        public IHttpActionResult editar([FromBody] EnvioCliente envio)
        {
            envio.idUsuario = envio.usuario._id;
            envio.idDocumento = envio.documento._id;
            envio.fechaHoraAcuse = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
            EnvioClienteDao dao = new EnvioClienteDao();
            envio = dao.editar(envio);

            if (envio != null)
            {
                // Preferencia
                String preference = ConfigurationManager.AppSettings["emailpreference"];

                if(envio.estado == "False")
                {
                    // Enviar cliente a administrador
                    EmailService email = new EmailService();
                    email.enviarEmail(null, preference, "ADMIN", envio.documento.numero);
                }
                
                return Ok(envio);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("consultar/{id}")]
        public IHttpActionResult consultar(String id)
        {
            EnvioClienteDao dao = new EnvioClienteDao();
            EnvioCliente envio = new EnvioCliente();
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
            EnvioClienteDao dao = new EnvioClienteDao();
            bool res;
            res = dao.elimiar(id);

            if (res == true)
                return Ok(res);
            else
                return BadRequest();
        }

    }
}
