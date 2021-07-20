using System.Collections.Generic;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;
using MongoDB.Bson;
using System;
using controlfacturasws.Services;


namespace controlfacturasws.Controllers
{
    [Authorize]
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {

        [HttpGet]
        [Route("listar")]
        public IHttpActionResult listar()
        {
            UsuarioDao dao = new UsuarioDao();
            List<Usuario> usuarios = new List<Usuario>();
            usuarios = dao.listar();

            if (usuarios != null)
                return Ok(usuarios);
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("consultar/{id}")]
        public IHttpActionResult consultar(String id)
        {
            UsuarioDao dao = new UsuarioDao();
            Usuario usuario = new Usuario();
            usuario = dao.consultar(id);

            if (usuario != null)
                return Ok(usuario);
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("agregar")]
        public IHttpActionResult agregar([FromBody] Usuario usuario)
        {
            usuario.password = Crypt.Encrypt(usuario.password);
            UsuarioDao dao = new UsuarioDao();
            usuario = dao.agregar(usuario);

            if (usuario != null)
                return Ok(usuario);
            else
                return BadRequest();
        }

        [HttpPut]
        [Route("editar")]
        public IHttpActionResult editar([FromBody] Usuario usuario)
        {
            Validacion val = new Validacion();
            if(!val.usuario(usuario))
            {
                return BadRequest();
            }
            UsuarioDao dao = new UsuarioDao();
            usuario.password = Crypt.Encrypt(usuario.password);
            usuario = dao.editar(usuario);

            if (usuario != null)
                return Ok(usuario);
            else
                return BadRequest();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IHttpActionResult eliminar(String id)
        {
            UsuarioDao dao = new UsuarioDao();
            bool res;
            res = dao.elimiar(id);

            if (res == true)
                return Ok(res);
            else
                return BadRequest();
        }

    }
}
