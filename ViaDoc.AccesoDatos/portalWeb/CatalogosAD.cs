using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.portalWeb
{
    public class CatalogosAD
    {
        public DataSet ConsultaCatalogos(int opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebCargaCatalogos");
                conexion.agregarParametroSP("@Opcion", opcion, DbType.Int32, ParameterDirection.Input);            
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
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
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL";
            }
            return dsResultado;
        }

        public DataSet ConsultaCatalogosHistorico(string nombreHistorico, int opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase(nombreHistorico);

                conexion.crearComandoSql("ViaDoc_WebCargaCatalogos");
                conexion.agregarParametroSP("@Opcion", opcion, DbType.Int32, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
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
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL";
            }
            return dsResultado;
        }

    }
}
