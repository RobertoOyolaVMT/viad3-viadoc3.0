using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.portalWeb
{
    public class Bienvenido
    {
        public DataSet ReporteInicio(int opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsRespuesra = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebPortalInicio");
                conexion.agregarParametroSP("@Option", opcion, DbType.Int32, ParameterDirection.Input);
                dsRespuesra = conexion.EjecutarConsultaDatSet();
                if (dsRespuesra != null)
                {
                    codigoRetorno = 0;
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
                mensajeRetorno = ex.Message;
            }

            return dsRespuesra;
        }
    }
}
