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
    public class EnvioDianDao
    {
        MongoContext context = new MongoContext();
        public List<EnvioDIAN> listar()
        {
            var query = from p in context.enviosDIAN.AsQueryable()
                        join o in context.documentos.AsQueryable()
                        on p.idDocumento equals o._id into documento
                        select new EnvioDIAN()
                        {
                            _id = p._id,
                            documento = documento.First(),
                            keyZip = p.keyZip,
                            fechaHoraEnvio = p.fechaHoraEnvio,
                            fechaHoraAcuse = p.fechaHoraAcuse,
                            estado = p.estado,
                            respuestaDIAN = p.respuestaDIAN
                        };
            var envios = query.ToList();
            return envios;
        }

        internal List<EnvioDIAN> listarPorEstado(String estado)
        {
            var query = from p in context.enviosDIAN.AsQueryable()
                        join o in context.documentos.AsQueryable()
                        on p.idDocumento equals o._id into documento
                        where p.estado == estado
                        select new EnvioDIAN()
                        {
                            _id = p._id,
                            documento = documento.First(),
                            keyZip = p.keyZip,
                            fechaHoraEnvio = p.fechaHoraEnvio,
                            fechaHoraAcuse = p.fechaHoraAcuse,
                            estado = p.estado,
                            respuestaDIAN = p.respuestaDIAN
                        };
            var envios = query.ToList();
            return envios;
        }

        public EnvioDIAN consultar(String id)
        {
            var query = from p in context.enviosDIAN.AsQueryable()
                        join o in context.documentos.AsQueryable()
                        on p.idDocumento equals o._id into documento
                        where p._id == id
                        select new EnvioDIAN()
                        {
                            _id = p._id,
                            documento = documento.First(),
                            keyZip = p.keyZip,
                            fechaHoraEnvio = p.fechaHoraEnvio,
                            fechaHoraAcuse = p.fechaHoraAcuse,
                            estado = p.estado,
                            respuestaDIAN = p.respuestaDIAN
                        };
            var envio = query.FirstOrDefault();
            return envio;
        }

        public EnvioDIAN consultarPorDocumento(String idDocumento)
        {
            try
            {
                var query = from p in context.enviosDIAN.AsQueryable()
                            join o in context.documentos.AsQueryable()
                            on p.idDocumento equals o._id into documento
                            where p.idDocumento == idDocumento
                            orderby p.fechaHoraEnvio descending
                            select new EnvioDIAN()
                            {
                                _id = p._id,
                                documento = documento.First(),
                                keyZip = p.keyZip,
                                fechaHoraEnvio = p.fechaHoraEnvio,
                                fechaHoraAcuse = p.fechaHoraAcuse,
                                estado = p.estado,
                                respuestaDIAN = p.respuestaDIAN
                            };
                var envio = query.FirstOrDefault();
                return envio;
            }
            catch
            {
                return null;
            }
            
        }

        public EnvioDIAN consultarPorDocEstado(String idDocumento, String estado)
        {
            try
            {
                var query = from p in context.enviosDIAN.AsQueryable()
                            join o in context.documentos.AsQueryable()
                            on p.idDocumento equals o._id into documento
                            where p.idDocumento == idDocumento
                            where p.estado == estado
                            orderby p.fechaHoraEnvio descending
                            select new EnvioDIAN()
                            {
                                _id = p._id,
                                documento = documento.First(),
                                keyZip = p.keyZip,
                                fechaHoraEnvio = p.fechaHoraEnvio,
                                fechaHoraAcuse = p.fechaHoraAcuse,
                                estado = p.estado,
                                respuestaDIAN = p.respuestaDIAN
                            };
                var envio = query.FirstOrDefault();
                return envio;
            }
            catch
            {
                return null;
            }
        }

        public EnvioDIAN agregar(EnvioDIAN envio)
        {
            try
            {
                context.enviosDIAN.InsertOne(envio);
                return envio;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
            
        }

        public EnvioDIAN editar(EnvioDIAN envio)
        {
            try
            {
                var builder = Builders<EnvioDIAN>.Filter;
                var filter = builder.Eq(u => u._id, envio._id);
                context.enviosDIAN.ReplaceOne(filter, envio);
                return envio;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
        }

        public bool elimiar(String id)
        {
            try
            {
                var builder = Builders<EnvioDIAN>.Filter;
                var filter = builder.Eq(u => u._id, id);
                context.enviosDIAN.DeleteOne(filter);
                return true;
            }
            catch (Exception ex)
            {
                LogService.escribir(ex);
                return false;
            }
        }
    }
}