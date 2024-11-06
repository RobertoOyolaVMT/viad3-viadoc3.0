using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.portalWeb
{
    public class ReprocesoAD
    {
        public DataSet ConsutaReproceso(string compania,string Tipodocu, string NumDocu,string Fecha,string FechaHAsta,string CLaveAcceso,string Opcion,ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebReprocesoDocumentos");
                conexion.agregarParametroSP("@compania", compania, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipodocu", Tipodocu, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@numdocumento", NumDocu, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fecha", Fecha, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaHasta", FechaHAsta, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ClaveAcceso", CLaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@opcion", Opcion, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
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
            }

            return dsResultado;
        }
    }
}
