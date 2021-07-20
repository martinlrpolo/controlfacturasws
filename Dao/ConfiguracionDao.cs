using controlfacturasws.App_Start;
using controlfacturasws.Models;
using controlfacturasws.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace controlfacturasws.Dao
{
    public class ConfiguracionDao
    {
        MongoContext context = new MongoContext();
        public Configuracion consultar()
        {
            try
            {
                return context.configuracion.Find(m => true).First();
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
            
        }

        public Configuracion editar(Configuracion configuracion)
        {
            try
            {
                var builder = Builders<Configuracion>.Filter;
                var filter = builder.Eq(u => u._id, configuracion._id);
                context.configuracion.ReplaceOne(filter, configuracion);
                return configuracion;
            }
            catch (Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
        }
    }
}