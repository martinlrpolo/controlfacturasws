using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace controlfacturasws.Models
{
    public class DocumentoPdf
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement("documento")]
        public String documento { get; set; }

        [BsonElement("descargas")]
        public int descargas { get; set; }

        [BsonElement("pdf")]
        public String pdf { get; set; }
    }
}