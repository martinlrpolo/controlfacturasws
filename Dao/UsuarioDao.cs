using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using controlfacturasws.App_Start;
using controlfacturasws.Models;
using MongoDB.Bson;
using System;
using controlfacturasws.Services;

namespace controlfacturasws.Dao
{
    public class UsuarioDao
    {
        MongoContext context = new MongoContext();

        public List<Usuario> listar()
        {
            try
            {
                List<Usuario> usuarios = context.usuarios.Find(m => true).ToList();
                return usuarios;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
            
        }

        public Usuario consultar(String id)
        {
            try
            {
                Usuario usuario = new Usuario();
                var builder = Builders<Usuario>.Filter;
                var filter = builder.Eq(u => u._id, id);
                usuario = context.usuarios.Find(filter).SingleOrDefault<Usuario>();
                return usuario;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
        }

        public Usuario login(String id, String password)
        {
            Usuario usuario = new Usuario();

            var builder = Builders<Usuario>.Filter;

            var filterEmail = builder.Eq(u => u._id, id);
            var filterPassword = builder.Eq(u => u.password, password);

            var filter = builder.And(new [] {filterEmail, filterPassword});

            usuario = context.usuarios.Find(filter).SingleOrDefault<Usuario>();
            return usuario;

        }

        public Usuario agregar(Usuario usuario)
        {
            try
            {
                context.usuarios.InsertOne(usuario);
                return usuario;
            }
            catch(Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }
            
        }

        public Usuario editar(Usuario usuario)
        {
            try
            {
                var builder = Builders<Usuario>.Filter;
                var filter = builder.Eq(u => u._id, usuario._id);
                context.usuarios.ReplaceOne(filter, usuario);
                return usuario;
            }
            catch (Exception ex)
            {
                LogService.escribir(ex);
                return null;
            }

        }

        public bool elimiar(String id)
        {
            try
            {
                var builder = Builders<Usuario>.Filter;
                var filter = builder.Eq(u => u._id, id);
                context.usuarios.DeleteOne(filter);
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