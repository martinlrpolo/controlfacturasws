using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using controlfacturasws.Models;

namespace controlfacturasws.Services
{
    public class Validacion
    {
        public bool usuario(Usuario usuario)
        {
            if (usuario == null)
                return false;
            if (usuario._id == null || usuario._id == "")
                return false;
            else if (usuario.password == null || usuario.password == "")
                return false;
            else if (usuario.nombre == null || usuario.nombre == "")
                return false;
            else if (usuario.email == null || usuario.email == "")
                return false;
            else if (usuario.rol == null)
                return false;
            else
                return true;
        }

        public bool documento(Documento documento)
        {
            if (documento._id == null || documento._id == "")
                return false;
            else if (documento.tipo == null || documento.tipo._id == "")
                return false;
            else if (documento.usuario == null || documento.usuario._id == "")
                return false;
            else if (documento.numero == null || documento.numero == "")
                return false;
            else
                return true;
        }
    }
}