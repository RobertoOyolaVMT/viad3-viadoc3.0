using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ViaDoc.Logs.Entidades
{
    public class ModelLogs
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("solucion")]
        [Required]
        public string Solucion { get; set; }
        [BsonElement("idCompania")]
        [Required]
        public string IdCompania { get; set; }
        [BsonElement("numDocumento")]
        [Required]  
        public string numDocumento { get; set; }
        [BsonElement("clase")]
        [Required]
        public string Clase { get; set; }
        [BsonElement("metodo")]
        [Required]
        public string Metodo { get; set; }
        [BsonElement("codigoError")]
        [Required]
        public string CodigoError { get; set; }
        [BsonElement("mensajeError")]
        [Required]
        public string MensajeError { get; set; }
        [BsonElement("hora")]
        [Required]
        public string Hora { get; set; }
        [BsonElement("fecha")]
        [Required]
        public string Fecha { get; set; }
    }
}
