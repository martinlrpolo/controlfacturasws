using System;
using System.Configuration;
using System.IO;
using TestFacturacionColombia_V2;
using FE.Models.Doc;

namespace controlfacturasws.Services
{
    public class FacturacionColombia
    {
        WsDian WsDIAN;
        bool WsAutenticationOk = false;
        String rutaDoc;
        String rutaCertificado;

        public FacturacionColombia()
        {
            WsDIAN = new WsDian();
            MtxSoftware.CargarTest();
            Emisor.CargarTest();
            WsAutenticationOk = WsDIAN.AutenticationDian(MtxSoftware.SerialCertificado);
            rutaDoc = ConfigurationManager.AppSettings["DocUrl"];
            rutaCertificado = ConfigurationManager.AppSettings["CertificateUrl"];
            Tools.RutaApp = rutaDoc;
        }

        public bool firmarXML(DocumentoDian doc, String numeroDoc)
        {

            TestFE test = new TestFE();
            eFacturacionColombia_V2.Xml.InvoiceType Invoice;
            Invoice = test.CreateInvoice(doc);

            // Crear xml sin firmar
            Tools.XmlSerializeToFile(Invoice, WsDIAN.XmlNameSpaces, rutaDoc + @"XML\" + Invoice.ID.Value + "_sf.xml");

            // Firma el XML 
            XmlSignature.RutaCertificado = rutaCertificado;
            XmlSignature.NombreCertificado = "Certificado.p12";
            byte[] ff = XmlSignature.Firmar(numeroDoc);
            if (ff != null)
                return true;
            else
                return false;
        }

        public bool comprimirXML(String numeroDoc)
        {
            bool res = false;
            // Si el archivo existe, eliminarlo
            if (File.Exists(rutaDoc + @"ZIP\" + numeroDoc + ".zip"))
            {
                File.Delete(rutaDoc + @"ZIP\" + numeroDoc + ".zip");
            }
            res = Tools.createZipFile(numeroDoc + ".xml", numeroDoc + ".zip", numeroDoc + ".xml");
            return res;
        }

        public FE.Models.Service.RespEnvioDoc enviarDoc(String numeroDoc)
        {
            WsDIAN.FileZip = System.IO.File.ReadAllBytes(Tools.RutaApp + @"ZIP\" + numeroDoc + ".zip");
            FE.Models.Service.RespEnvioDoc RespWS = WsDIAN.EnviarDoc(numeroDoc);
            Console.WriteLine(WsDIAN.LastError);
            return RespWS;
        }

        public RespWS consultarDoc(String keyZip)
        {
            RespWS r = WsDIAN.ConsultarDoc(keyZip);
            return r;
        }

        public bool generarPdf(String numeroDoc)
        {
            try
            {
                // Si el archivo existe, eliminarlo
                if (File.Exists(rutaDoc + @"PDF\" + numeroDoc + ".pdf"))
                {
                    File.Delete(rutaDoc + @"PDF\" + numeroDoc + ".pdf");
                }
                PdfTools pdf = new PdfTools();
                if (pdf.CrearPDF(numeroDoc))
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return false;
            }
            
        }

        public String estrucutrarXml(String xml)
        {
            string replaceWith = "";
            String str = xml;
            String newStr = "";
            newStr = str.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
            return newStr;
        }
    }
}