using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Web;

namespace controlfacturasws.Models
{
    public class Documento
    {
        [BsonId]
        [BsonElement("_id")]
        public String _id { get; set; }

        //[JsonIgnore]
        [BsonElement("idTipo")]
        public String idTipo { get; set; }

        [BsonIgnore]
        public Catalogo tipo { get; set; }

        [JsonIgnore]
        [BsonElement("idUsuario")]
        public String idUsuario { get; set; }

        [BsonIgnore]
        public Usuario usuario { get; set; }

        [BsonElement("numero")]
        public String numero { get; set; }

        [BsonElement("fecha")]
        public String fecha { get; set; }

        [BsonElement("xml")]
        public String xml { get; set; }

    }
}