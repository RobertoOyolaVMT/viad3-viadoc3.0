using ConexionBD;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilViadoc.Logs;


namespace AccesoDatosViadoc
{
    public class ConexionMurano
    {
        BaseDatos dbmysql;

        public DataSet consultaFacturas(ref Retorno retorno)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                dbmysql = new ConexionMysql(ConfigurationManager.ConnectionStrings["murano"].ConnectionString);

                dbmysql.Conectar();

                dbmysql.CrearComando("SP_FACTURAS_VIADOC ", CommandType.StoredProcedure);

                ds = dbmysql.EjecutarConsultaDataSet();

                if(ds != null)
                {
                    retorno.codigoRetorno = "0";
                }
                else
                {
                    retorno.codigoRetorno = "1";
                }
            }
            catch (Exception ex)
            {
                //LogsFactura.grabaLogsException("consultaFacturasPendientes", "ConexionMysql", ex.Message, ex.StackTrace);
                retorno.codigoRetorno = "9";
                retorno.mensajeRetorno = "Exception: " + ex.Message;
            }

            finally
            {
                dbmysql.Desconectar();
              
            }

            return ds;
        }
    }
}
