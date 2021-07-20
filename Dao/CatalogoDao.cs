using controlfacturasws.App_Start;
using controlfacturasws.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace controlfacturasws.Dao
{
    public class CatalogoDao
    {
        MongoContext context = new MongoContext();
        public List<Catalogo> listar(String tipo)
        {
            var builder = Builders<Catalogo>.Filter;
            var filter = builder.Eq(u => u.tipo, tipo);
            List<Catalogo> catalogo = context.catalogo.Find(filter).ToList();
            return catalogo;
        }
    }
}