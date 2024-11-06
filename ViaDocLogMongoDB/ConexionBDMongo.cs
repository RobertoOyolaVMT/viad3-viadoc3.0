using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ViaDoc.Logs.Entidades;

namespace ViaDoc.Logs
{
    public class ConexionBDMongo
    {
        public IMongoDatabase ConectarMongo()
        {
            try
            {
                // Establece la cadena de conexion y nombre de la base de datos
                var connectionString = ConfigurationManager.ConnectionStrings["conexionMongo"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["baseMongo"];

                // Establece la conexion
                var _client = new MongoClient(connectionString);
                var _database = _client.GetDatabase(databaseName);

                // Valida si existe la base de datos
                if (_database != null)
                    return (IMongoDatabase)_database;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Utilitarios.logs.LogsFactura.LogsInicioFin("Error en la conexion de la BD Mongo: " + ex.ToString());
            }
            return null;
        }

        public void GuardarLogs(string nombreTabla, ModelLogs logs )
        {
            try
            {

                var db = ConectarMongo();  // Conexion a MongoDB
                if (db != null)
                {

                    var collection = db.GetCollection<ModelLogs>(nombreTabla, null);
                    logs.Id = ObjectId.GenerateNewId();
                    collection.InsertOne(logs);

                }
                else
                {
                    Utilitarios.logs.LogsFactura.LogsInicioFin("Error al conectar BD");
                }
            }
            catch (Exception ex)
            {
                Utilitarios.logs.LogsFactura.LogsInicioFin("Excepcion en GuardarLogs Mongo: " + ex.ToString());
            }
        }

        public List<ModelLogs> ConsultaLogs(string nombreTabla, string idCompania, string solucion, string fecha)
        {
            List<ModelLogs> listLogs = new List<ModelLogs>();
            try
            {
                var db = ConectarMongo();  // Conexion a MongoDB
                if (db != null)
                {
                    var collection = db.GetCollection<BsonDocument>(nombreTabla); // Capturamos coleccion

                    var builder = Builders<BsonDocument>.Filter;
                    var filter = builder.Eq("idCompania", idCompania) & builder.Eq("solucion", solucion) & builder.Eq("fecha", fecha);

                    var resultados = collection.Find(filter).ToList();

                    foreach (BsonDocument doc in resultados)
                    {
                        ModelLogs log = BsonSerializer.Deserialize<ModelLogs>(doc);
                        listLogs.Add(log);
                    }
                }
                else
                {
                    Utilitarios.logs.LogsFactura.LogsInicioFin("Error al conectar BD");
                }
            }
            catch (Exception ex)
            {
                Utilitarios.logs.LogsFactura.LogsInicioFin("Excepcion en GuardarLogs Mongo: " + ex.ToString());
            }

            return listLogs;
        }


        //public void creaLogMongo(string clase,
        //                         string codigoError,
        //                         string idCompania,
        //                         string numDocumento,
        //                         string metodo,
        //                         string mensajeError,
        //                         string fecha,
        //                         string hora,
        //                         string solucion)
        //{
        //    ModelLogs objL = new ModelLogs();
        //    objL.Clase = clase;
        //    objL.CodigoError = codigoError;
        //    objL.IdCompania = idCompania;
        //    objL.numDocumento = numDocumento;
        //    objL.Metodo = metodo;
        //    objL.MensajeError = mensajeError;
        //    objL.Fecha = fecha;
        //    objL.Hora = hora;
        //    objL.Solucion = solucion;

        //    GuardarLogs("Logs", objL);
        //}

        public List<ModelLogs> consultaLogMongo(string nombreTabla,
                                     string clase,
                                     string idCompania,
                                     string fecha,
                                     string solucion)
        {
            return ConsultaLogs(nombreTabla, idCompania, solucion, fecha);
        }
    }
}
