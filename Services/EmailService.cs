using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System;
using controlfacturasws.Models;
using controlfacturasws.Dao;
using System.IO;
using Attachment = SendGrid.Helpers.Mail.Attachment;

namespace controlfacturasws.Services
{
    public class EmailService
    {
       
        public bool enviarEmail(String idUsuario, String tipo, String dirigido, String numeroDoc)
        {

            bool pdf = false;
            bool xml = false;

            try
            {
                Configuracion configuracion = new Configuracion();
                ConfiguracionDao configuracionDao = new ConfiguracionDao();
                EmailService emailService = new EmailService();

                configuracion = configuracionDao.consultar();
                Email email = new Email();
                
                email.numeroDoc = numeroDoc;
                email.correoOrigen = configuracion.correoAdmin;
                email.nombreOrigen = configuracion.nombreAdmin;

                if (dirigido == "CLIENTE")
                {
                    UsuarioDao usuarioDao = new UsuarioDao();
                    Usuario usuario = new Usuario();
                    usuario = usuarioDao.consultar(idUsuario);
                    if (usuario == null)
                        return false;

                    xml = usuario.xml;
                    pdf = true;

                    email.asunto = configuracion.asuntoEmailCliente;
                    email.textoHtml = configuracion.textoEmailCliente;
                    email.correoDestino = usuario.email;
                    email.nombreDestino = usuario.nombre;
                }
                else
                {
                    email.asunto = configuracion.asuntoEmailAdmin;
                    email.textoHtml = configuracion.textoEmailAdmin;
                    email.correoDestino = configuracion.correoAdmin;
                    email.nombreDestino = configuracion.nombreAdmin;
                    xml = false;
                    pdf = false;
                }

                if (tipo == "sendgrid")
                    emailService.sendGrid(email, xml, pdf);
                else
                    emailService.smtp(email, xml, pdf);
                    return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool sendGrid(Email email, bool xml, bool pdf)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(email.correoOrigen, email.nombreOrigen);
                var subject = email.asunto;
                var to = new EmailAddress(email.correoDestino, email.nombreDestino);
                var htmlContent = email.textoHtml;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

                if(pdf == true)
                {
                    String docurl = ConfigurationManager.AppSettings["DocUrl"] + @"PDF\" + email.numeroDoc + ".pdf";
                    byte[] file = System.IO.File.ReadAllBytes(docurl);
                    string documentString = Convert.ToBase64String(file);
                    var attachment = new Attachment { Filename = email.numeroDoc + ".pdf", Content = documentString };
                    msg.AddAttachment(attachment);
                }

                if(xml == true)
                {
                    String docurl = ConfigurationManager.AppSettings["DocUrl"] + @"XML\" + email.numeroDoc + ".xml";
                    byte[] file = System.IO.File.ReadAllBytes(docurl);
                    string documentString = Convert.ToBase64String(file);
                    var attachment = new Attachment { Filename = email.numeroDoc + ".xml", Content = documentString };
                    msg.AddAttachment(attachment);
                }
                client.SendEmailAsync(msg);
                return true;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return false;
            }
            
        }

        public bool smtp(Email email, bool xml, bool pdf)
        {
            
            try 
            {
                
                String host = ConfigurationManager.AppSettings["SMTPServer"];
                String username = ConfigurationManager.AppSettings["SMTPUsername"];
                String password = ConfigurationManager.AppSettings["SMTPPassword"];
                int port = Int32.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                bool ssl = bool.Parse(ConfigurationManager.AppSettings["SMTPSSL"]);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(host);
                mail.From = new MailAddress(email.correoOrigen);
                mail.To.Add(email.correoDestino);
                mail.Subject = email.asunto;
                mail.Body = email.textoHtml;
                SmtpServer.Port = port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                SmtpServer.EnableSsl = ssl;

                if (pdf == true)
                {
                    String docurl = ConfigurationManager.AppSettings["DocUrl"] + @"PDF\" + email.numeroDoc + ".pdf";
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(docurl);
                    mail.Attachments.Add(attachment);
                }

                if (xml == true)
                {
                    String docurl = ConfigurationManager.AppSettings["DocUrl"] + @"PDF\" + email.numeroDoc + ".pdf";
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(docurl);
                    mail.Attachments.Add(attachment);
                }

                SmtpServer.Send(mail);
                return true;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return false;
            }

        }

    }
}