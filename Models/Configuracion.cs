using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace controlfacturasws.Models
{
    public class Configuracion
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [BsonElement("logo")]
        public String logo { get; set; }

        [BsonElement("nombreEmpresa")]
        public String nombreEmpresa { get; set; }

        [BsonElement("nombreAdmin")]
        public String nombreAdmin { get; set; }

        [BsonElement("correoAdmin")]
        public String correoAdmin { get; set; }

        [BsonElement("asuntoEmailCliente")]
        public String asuntoEmailCliente { get; set; }

        [BsonElement("asuntoEmailAdmin")]
        public String asuntoEmailAdmin { get; set; }

        [BsonElement("textoEmailCliente")]
        public String textoEmailCliente { get; set; }

        [BsonElement("textoEmailAdmin")]
        public String textoEmailAdmin { get; set; }
    }
}