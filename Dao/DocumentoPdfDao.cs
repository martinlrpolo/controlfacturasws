using System.Collections.Generic;
using System.Linq;
using controlfacturasws.App_Start;
using controlfacturasws.Models;
using MongoDB.Driver;

namespace controlfacturasws.Dao
{
    public class DocumentoPdfDao
    {
        MongoContext context = new MongoContext();
        public List<DocumentoPdf> listar()
        {
            List<DocumentoPdf> documentos = context.documentosPdf.Find(m => true).ToList();
            return documentos;
        }
    }
}