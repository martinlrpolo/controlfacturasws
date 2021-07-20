using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using controlfacturasws.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace controlfacturasws.Models
{
    public class Usuario
    {
        [BsonElement("_id")]
        public String _id { get; set; }

        [BsonElement("nit")]
        public String nit { get; set; }

        [BsonElement("password")]
        public String password { get; set; }

        [BsonElement("nombre")]
        public String nombre { get; set; }

        [BsonElement("email")]
        public String email { get; set; }

        [BsonElement("emails")]
        public String[] emails { get; set; }

        [BsonElement("celular")]
        public String celular { get; set; }

        [BsonElement("rol")]
        public String rol { get; set; }

        [BsonElement("xml")]
        public bool xml { get; set; }

    }
}