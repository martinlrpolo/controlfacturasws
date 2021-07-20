using System.Collections.Generic;
using System.Web.Http;
using controlfacturasws.Models;
using controlfacturasws.Dao;
using System.Web;
using controlfacturasws.Services;
using System;
using System.Net.Http;
using System.Net;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;
using TestFacturacionColombia_V2;
using FE.Models.Doc;

namespace controlfacturasws.Controllers
{
    [Authorize]
    [RoutePrefix("api/documento")]
    public class DocumentoController : ApiController
    {
        [HttpGet]
        [Route("listar")]
        public IHttpActionResult listar()
        {
            DocumentoDao dao = new DocumentoDao();
            List<Documento> documentos = new List<Documento>();
            documentos = dao.listar();
            return Ok(documentos);
        }

        [HttpPost]
        [Route("agregar")]
        public HttpResponseMessage agregar([FromBody] dynamic json)
        {

            String data = Convert.ToString(json);

            FacturacionColombia fc = new FacturacionColombia();
            DocumentoDian documentoDian = Tools.DesSerializarJson(data);
            Models.Documento documento = new Documento();
            DocumentoDao documentoDao = new DocumentoDao();
            EnvioDianDao envioDianDao = new EnvioDianDao();
            EnvioDIAN envioDian = new EnvioDIAN();

            documento._id = documentoDian.TipoDocErp + documentoDian.Numero;
            documento.idTipo = documentoDian.TipoDocErp;
            documento.idUsuario = documentoDian.Receptor.Codigo;
            documento.numero = documentoDian.Numero;
            documento.fecha = documentoDian.FechaHoraDoc.ToString();

            // Si el usuario existe en el sistema
            UsuarioDao udao = new UsuarioDao();
            if(udao.consultar(documento.idUsuario) == null)
            {
                FE.Models.Service.RespEnvioDoc r = new FE.Models.Service.RespEnvioDoc();
                r.Enviado = false;
                r.Errores.Add(new FE.Models.Service.error { MensajeErr = "El usuario no existe" });
                return Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.BadRequest, r);
            }

            // Verificar que el documento ya no se haya enviado
            EnvioDianDao edao = new EnvioDianDao();
            var edian = edao.consultarPorDocumento(documento._id);
            if (edian != null)
            {
                FE.Models.Service.RespEnvioDoc r = new FE.Models.Service.RespEnvioDoc();
                r.Enviado = false;
                r.Errores.Add(new FE.Models.Service.error { MensajeErr = "Este documento ya se ha enviado / o ya existe" });
                return Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.BadRequest, r);
            }

            // Verificar campos completos
            /*
            Validacion val = new Validacion();
            if (!val.documento(documento))
            {
                String resp = "Se deben llenar todos los campos";
                return Request.CreateResponse<String>(HttpStatusCode.BadRequest, resp);
            }
            */


            bool res;
            res = fc.firmarXML(documentoDian, documento.numero);

            if(res == true)
            {
                // Convertir XML de Bytes a String
                String docurl = ConfigurationManager.AppSettings["DocUrl"] + @"XML\" + documento.numero + ".xml";
                byte[] file = System.IO.File.ReadAllBytes(docurl);
                documento.xml = Convert.ToBase64String(file);

                // Guardar documento en base de datos
                documento = documentoDao.agregar(documento);

                if(documento == null)
                {
                    FE.Models.Service.RespEnvioDoc r = new FE.Models.Service.RespEnvioDoc();
                    r.Enviado = false;
                    r.Errores.Add(new FE.Models.Service.error { MensajeErr = "No se ha podido guardar el documento en base de datos" });
                    return Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.BadRequest, r);
                }

                res = fc.comprimirXML(documento.numero);
                if(res == true)
                {
                    FE.Models.Service.RespEnvioDoc resp = fc.enviarDoc(documento.numero);
                    if(resp.Enviado == true)
                    {
                        // Agregar envio a base de datos
                        envioDian.idDocumento = documento._id;
                        envioDian.keyZip = resp.ZipKey;
                        envioDian.fechaHoraEnvio = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");

                        envioDian = envioDianDao.agregar(envioDian);

                        if(envioDian == null)
                        {
                            FE.Models.Service.RespEnvioDoc r = new FE.Models.Service.RespEnvioDoc();
                            r.Enviado = false;
                            r.Errores.Add(new FE.Models.Service.error { MensajeErr = "No se pudo guardar el envio en la base de datos" });
                            return Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.BadRequest, r);
                        }

                        var response = Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.OK, resp);
                        return response;
                    }
                    else
                    {
                        return Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.BadRequest, resp);
                    }
                }
                else
                {
                    FE.Models.Service.RespEnvioDoc r = new FE.Models.Service.RespEnvioDoc();
                    r.Enviado = false;
                    r.Errores.Add(new FE.Models.Service.error { MensajeErr = "Error al comprimir el documento, verificar ruta" });
                    return Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.BadRequest, r);
                }
            }
            else
            {
                FE.Models.Service.RespEnvioDoc r = new FE.Models.Service.RespEnvioDoc();
                r.Enviado = false;
                r.Errores.Add(new FE.Models.Service.error { MensajeErr = "Error al firmar, verificar ruta del documento o ruta del certificado" });
                return Request.CreateResponse<FE.Models.Service.RespEnvioDoc>(HttpStatusCode.BadRequest, r);

            }

        }


        // Cambiar KeyZip por IDDocumento
        [HttpGet]
        [Route("actualizardoc/{idDocumento}")]
        public HttpResponseMessage actualizarDoc(String idDocumento)
        {

            // Consultar en la base de datos el Keyzip por el id de documento
            EnvioDianDao edao = new EnvioDianDao();
            EnvioDIAN envio = new EnvioDIAN();
            envio = edao.consultarPorDocumento(idDocumento);

            if (envio != null)
            {
                
                if(envio.estado == "True")
                {
                    RespWS respws = new RespWS();
                    respws.IsValid = true;
                    respws.Descripcion = "El documento ya fue aceptado";

                    return Request.CreateResponse<RespWS>(HttpStatusCode.BadRequest, respws);
                }

                if (envio.estado == "False")
                {
                    RespWS respws = new RespWS();
                    respws.IsValid = false;
                    respws.Descripcion = "El documento fue rechazado";
                    return Request.CreateResponse<RespWS>(HttpStatusCode.BadRequest, respws);
                }

                // Asignar informacion faltante al envio DIAN
                envio.idDocumento = envio.documento._id;

                // Consultar estado en DIAN
                FacturacionColombia fc = new FacturacionColombia();
                RespWS resdian = fc.consultarDoc(envio.keyZip);

                envio.estado = resdian.IsValid.ToString();

                envio.fechaHoraAcuse = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
                envio.respuestaDIAN = resdian.Descripcion;
                var resd = edao.editar(envio);

                if(resd != null)
                {
                    // Agregar envio a cliente
                    EnvioCliente envioc = new EnvioCliente();
                    envioc.idDocumento = envio.idDocumento;
                    envioc.idUsuario = envio.documento.idUsuario;
                    envioc.fechaHoraEnvio = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");

                    EnvioClienteDao ecdao = new EnvioClienteDao();
                    var resc = ecdao.agregar(envioc);

                    if(resc != null)
                    {
                        if (resdian.IsValid == true)
                        {
                            // Si la factura fue aprobada

                            // Generar PDF
                            fc.generarPdf(envio.documento.numero);

                            // Preferencia
                            String preference = ConfigurationManager.AppSettings["emailpreference"];

                            // Enviar correo con informacion al cliente
                            EmailService email = new EmailService();
                            email.enviarEmail(envio.documento.idUsuario, preference, "CLIENTE", envio.documento.numero);

                            // Devoler PDF en respuesta HTTP
                            String docurl = ConfigurationManager.AppSettings["DocUrl"] + @"PDF\" + envio.documento.numero + ".pdf";
                            byte[] file = System.IO.File.ReadAllBytes(docurl);

                            RespWS respws = new RespWS();
                            respws.File = file;
                            respws.IsValid = true;
                            respws.Descripcion = "Proceso ejecutado correctamente, Verificar los envíos";
                            return Request.CreateResponse<RespWS>(HttpStatusCode.OK, respws);
                        }
                        else
                        {
                            // Si la factura fue rechazada
                            return Request.CreateResponse<RespWS>(HttpStatusCode.BadRequest, resdian);
                        }

                    }
                    else
                    {
                        RespWS respws = new RespWS();
                        respws.IsValid = false;
                        respws.Descripcion = "No se pudo agregar el envio del cliente";
                        return Request.CreateResponse<RespWS>(HttpStatusCode.BadRequest, respws);
                    }
                }
                else
                {
                    RespWS respws = new RespWS();
                    respws.IsValid = false;
                    respws.Descripcion = "No se pudo actualizar el envio en base de datos";
                    return Request.CreateResponse<RespWS>(HttpStatusCode.BadRequest, respws);
                }
            }
            else {
                RespWS respws = new RespWS();
                respws.IsValid = false;
                respws.Descripcion = "No se encontraron envios relacionados a este documento";
                return Request.CreateResponse<RespWS>(HttpStatusCode.BadRequest, respws);
            }
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("consultarpdf/{tipo}/{numdoc}")]
        public HttpResponseMessage consultarPdf(String numdoc, String tipo)
        {

            String tipoL = tipo.ToLower();
            String tipoU = tipo.ToUpper();

            String filePath = ConfigurationManager.AppSettings["DocUrl"] + tipoU + "/" + numdoc + "." + tipoL;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", numdoc + "." + tipoL);
                throw new HttpResponseException(response);
            }

            //Read the File into a Byte Array.
            byte[] bytes = File.ReadAllBytes(filePath);

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = bytes.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = numdoc + "." + tipoL;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(numdoc + "."+ tipoL));
            return response;

        }

    }
}
