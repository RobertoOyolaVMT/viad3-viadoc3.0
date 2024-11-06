using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.winServFirmas
{
    public class ProcesarDocumentosErroresAD
    {

        ConexionViaDoc conexion = new ConexionViaDoc();

        public DataSet ConsultarDocumentosErrores()
        {
            DataSet dsResultado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ServicioRecepcionAutorizacion_ConsultarDocErrores");
                dsResultado = conexion.EjecutarConsultaDatSet();

            }
            catch (Exception ex)
            {

                conexion.desconectar();
            }
            finally
            {
                conexion.desconectar();
            }
            return dsResultado;
        }
    }
} //Da valores a los parametros del SP

