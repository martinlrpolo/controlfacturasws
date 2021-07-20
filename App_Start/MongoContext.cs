using System;
using MongoDB.Driver;
using System.Configuration;
using controlfacturasws.Models;

namespace controlfacturasws.App_Start
{
    public class MongoContext
    {
        private IMongoDatabase database { get; }
        public MongoContext()
        {
            try
            {
                String host = ConfigurationManager.AppSettings["MongoHost"];
                String port = ConfigurationManager.AppSettings["MongoPort"];
                String databaseName = ConfigurationManager.AppSettings["MongoDatabase"];
                String connectionString = host + port;
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                var mongoClient = new MongoClient(settings);
                database = mongoClient.GetDatabase(databaseName);
            }
            catch(Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
         
        }

        public IMongoCollection<Usuario> usuarios
        {
            get
            {
                return database.GetCollection<Usuario>("Usuarios");
            }
        }

        public IMongoCollection<Documento> documentos
        {
            get
            {
                return database.GetCollection<Documento>("Documentos");
            }
        }

        public IMongoCollection<DocumentoPdf> documentosPdf
        {
            get
            {
                return database.GetCollection<DocumentoPdf>("DocumentosPDF");
            }
        }

        public IMongoCollection<EnvioCliente> enviosClientes
        {
            get
            {
                return database.GetCollection<EnvioCliente>("EnviosClientes");
            }
        }

        public IMongoCollection<EnvioDIAN> enviosDIAN
        {
            get
            {
                return database.GetCollection<EnvioDIAN>("EnviosDIAN");
            }
        }

        public IMongoCollection<Catalogo> catalogo
        {
            get
            {
                return database.GetCollection<Catalogo>("Catalogo");
            }
        }

        public IMongoCollection<Configuracion> configuracion
        {
            get
            {
                return database.GetCollection<Configuracion>("Configuraciones");
            }
        }

    }
}