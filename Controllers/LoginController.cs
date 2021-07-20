using System.Collections.Generic;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;
using controlfacturasws.Services;


namespace controlfacturasws.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/acceso")]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("login")]
        public IHttpActionResult login([FromBody] Usuario usuario)
        {
            UsuarioDao dao = new UsuarioDao();
            usuario.password = Crypt.Encrypt(usuario.password);
            usuario = dao.login(usuario._id, usuario.password);

            if(usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                Configuracion config = new Configuracion();
                ConfiguracionDao configdao = new ConfiguracionDao();
                Login logininfo = new Login();
                logininfo.id = usuario._id;
                logininfo.nombre = usuario.nombre;
                logininfo.email = usuario.email;
                logininfo.rol = usuario.rol;
                logininfo.token = TokenGenerator.GenerateTokenJwt(logininfo.email);
                logininfo.configuracion = configdao.consultar();
                return Ok(logininfo);
            }

            
        }

    }
}
