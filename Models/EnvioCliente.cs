using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace controlfacturasws.Models
{
    public class EnvioCliente
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [JsonIgnore]
        [BsonElement("idDocumento")]
        public String idDocumento { get; set; }

        [BsonIgnore]
        public Documento documento { get; set; }

        
        [JsonIgnore]
        [BsonElement("idUsuario")]
        public String idUsuario { get; set; }
        
        [BsonIgnore]
        public Usuario usuario { get; set; }

        [BsonElement("fechaHoraEnvio")]
        public String fechaHoraEnvio { get; set; }

        [BsonElement("fechaHoraAcuse")]
        public String fechaHoraAcuse { get; set; }

        [BsonElement("estado")]
        public String estado { get; set; }

        [BsonElement("comentario")]
        public String comentario { get; set; }
    }
}