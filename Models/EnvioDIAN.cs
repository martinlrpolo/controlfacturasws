using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;

namespace controlfacturasws.Models
{
    public class EnvioDIAN
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [JsonIgnore]
        [BsonElement("idDocumento")]
        public String idDocumento { get; set; }

        [BsonIgnore]
        public Documento documento { get; set; }

        [BsonElement("keyZip")]
        public String keyZip { get; set; }

        [BsonElement("fechaHoraEnvio")]
        public String fechaHoraEnvio { get; set; }

        [BsonElement("fechaHoraAcuse")]
        public String fechaHoraAcuse { get; set; }

        [BsonElement("estado")]
        public String estado { get; set; }

        [BsonElement("respuestaDIAN")]
        public String respuestaDIAN { get; set; }
    }
}