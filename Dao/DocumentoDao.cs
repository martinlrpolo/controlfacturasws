using controlfacturasws.App_Start;
using controlfacturasws.Models;
using controlfacturasws.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace controlfacturasws.Dao
{
    public class DocumentoDao
    {
        MongoContext context = new MongoContext();
        public List<Documento> listar()
        {
            List<Documento> documentos = context.documentos.Find(m => true).ToList();
            return documentos;
        }

        public Documento consultar(String id)
        {
            Documento documento = new Documento();
            var builder = Builders<Documento>.Filter;
            var filter = builder.Eq(u => u._id, id);
            try
            {
                documento = context.documentos.Find(filter).SingleOrDefault<Documento>();
                return documento;
            }
            catch (Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
        }

        internal Documento agregar(Documento documento)
        {
            try
            {
                var builder = Builders<Documento>.Filter;
                var filter = builder.Eq(u => u._id, documento._id);
                context.documentos.ReplaceOne(filter, documento, new UpdateOptions { IsUpsert = true });
                return documento;
            }
            catch (Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
        }
    }
}