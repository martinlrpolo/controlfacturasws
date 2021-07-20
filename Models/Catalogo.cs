using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace controlfacturasws.Models
{
    public class Catalogo
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [BsonElement("nombre")]
        public String nombre { get; set; }

        [BsonElement("tipo")]
        public String tipo { get; set; }

        [BsonElement("color")]
        public String color { get; set; }

    }
}