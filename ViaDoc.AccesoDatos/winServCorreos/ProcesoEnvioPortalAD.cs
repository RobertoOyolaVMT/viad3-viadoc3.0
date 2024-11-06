using System;
using System.Data;
using System.Linq;

namespace ViaDoc.AccesoDatos.winServCorreos
{
    public class ProcesoEnvioPortalAD
    {
        public DataTable ConsultaDocumentosEnviarPortal(string proceso, int ciCompania, string tipoDocumento, int cantidad,
                                      string claveAcceso, ref int codigoRetorno, ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultadoCorreo = new DataSet();
            DataTable responseCorreos = new DataTable();
            codigoRetorno = 0;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServServicioEnvioPortal");
                conexion.agregarParametroSP("@proceso", proceso.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipoDocumento", tipoDocumento.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@cantidad", cantidad, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@claveAcceso", claveAcceso, DbType.String, ParameterDirection.Input);
                dsResultadoCorreo = conexion.EjecutarConsultaDatSet();
                if (dsResultadoCorreo != null)
                {
                    if (dsResultadoCorreo.Tables.Count > 0)
                    {
                        if (dsResultadoCorreo.Tables[0].Rows.Count > 0) responseCorreos = dsResultadoCorreo.Tables[0];
                        else codigoRetorno = 1;
                    }
                    else codigoRetorno = 1;
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "Nose se obtuve ninguna Registro";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return responseCorreos;
        }

        public DataTable ConsultaDocumentosEnviarPortalHistorico(string nombreHistorico, string proceso, int ciCompania, string tipoDocumento, int cantidad,
                                      string claveAcceso, ref int codigoRetorno, ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultadoCorreo = new DataSet();
            DataTable responseCorreos = new DataTable();
            codigoRetorno = 0;
            try
            {
                conexion.tipoBase(nombreHistorico);
                conexion.crearComandoSql("ViaDoc_WinServServicioEnvioPortal");
                conexion.agregarParametroSP("@proceso", proceso.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipoDocumento", tipoDocumento.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@cantidad", cantidad, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@claveAcceso", claveAcceso, DbType.String, ParameterDirection.Input);
                dsResultadoCorreo = conexion.EjecutarConsultaDatSet();
                if (dsResultadoCorreo != null)
                {
                    if (dsResultadoCorreo.Tables.Count > 0)
                    {
                        if (dsResultadoCorreo.Tables[0].Rows.Count > 0) responseCorreos = dsResultadoCorreo.Tables[0];
                        else codigoRetorno = 1;
                    }
                    else codigoRetorno = 1;
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "Nose se obtuve ninguna Registro";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return responseCorreos;
        }
    }
}
