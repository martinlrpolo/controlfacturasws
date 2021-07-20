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
    public class EnvioClienteDao
    {
        MongoContext context = new MongoContext();
        public List<EnvioCliente> listar()
        {
            var query = from p in context.enviosClientes.AsQueryable()
                        join o in context.documentos.AsQueryable()
                        on p.idDocumento equals o._id into documento
                        join u in context.usuarios.AsQueryable()
                        on p.idUsuario equals u._id into usuario
                        select new EnvioCliente(){
                            _id = p._id,
                            documento = documento.First(),
                            usuario = usuario.First(),
                            fechaHoraEnvio = p.fechaHoraEnvio,
                            fechaHoraAcuse = p.fechaHoraAcuse,
                            estado = p.estado,
                            comentario = p.comentario
                        };
            var envios = query.ToList();
            return envios;
        }

        public List<EnvioCliente> listarPorUsuario(string id)
        {
            var query = from p in context.enviosClientes.AsQueryable()
                        join o in context.documentos.AsQueryable()
                        on p.idDocumento equals o._id into documento
                        join u in context.usuarios.AsQueryable()
                        on p.idUsuario equals u._id into usuario
                        where p.idUsuario == id
                        select new EnvioCliente()
                        {
                            _id = p._id,
                            documento = documento.First(),
                            usuario = usuario.First(),
                            fechaHoraEnvio = p.fechaHoraEnvio,
                            fechaHoraAcuse = p.fechaHoraAcuse,
                            estado = p.estado,
                            comentario = p.comentario
                        };
            var envios = query.ToList();
            return envios;
        }

        public EnvioCliente editar(EnvioCliente envio)
        {

            try
            {
                var builder = Builders<EnvioCliente>.Filter;
                var filter = builder.Eq(u => u._id, envio._id);
                context.enviosClientes.ReplaceOne(filter, envio);
                return envio;
            }
            catch (Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
            
        }

        public EnvioCliente consultar(String id)
        {
            var query = from p in context.enviosClientes.AsQueryable()
                        join o in context.documentos.AsQueryable()
                        on p.idDocumento equals o._id into documento
                        join u in context.usuarios.AsQueryable()
                        on p.idUsuario equals u._id into usuario
                        where p._id == id
                        select new EnvioCliente()
                        {
                            _id = p._id,
                            documento = documento.First(),
                            usuario = usuario.First(),
                            fechaHoraEnvio = p.fechaHoraEnvio,
                            fechaHoraAcuse = p.fechaHoraAcuse,
                            estado = p.estado,
                            comentario = p.comentario
                        };
            var envio = query.First();
            return envio;
        }

        internal EnvioCliente agregar(EnvioCliente envio)
        {
            context.enviosClientes.InsertOne(envio);
            return envio;
        }

        public bool elimiar(String id)
        {
            try
            {
                var builder = Builders<EnvioCliente>.Filter;
                var filter = builder.Eq(u => u._id, id);
                context.enviosClientes.DeleteOne(filter);
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