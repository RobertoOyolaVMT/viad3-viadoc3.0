using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.winServCorreos
{
    public class ProcesoCorreoAD
    {
        ConexionViaDoc conexion = new ConexionViaDoc();

        public DataTable ConsultaCorreosEnviar(string proceso, int ciCompania, string tipoDocumento, int cantidad,
                                        string claveAcceso, ref int codigoRetorno, ref string descripcionRetorno)
            {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultadoCorreo = new DataSet();
            DataTable responseCorreos = new DataTable();
            codigoRetorno = 0;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServServicioEnvioEmail");
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
                        if (dsResultadoCorreo.Tables[0].Rows.Count > 0)
                        {
                            responseCorreos = dsResultadoCorreo.Tables[0];
                            codigoRetorno = 0;
                        }
                        else
                            codigoRetorno = 1;
                    }
                    else
                        codigoRetorno = 1;
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin($"Metodo: ConsultaCorreosEnviar, Proceso:{proceso}, Excpetion: {ex.Message}, linea: {ex.Source}");
            }
            finally
            {
                conexion.desconectar();
            }
            return responseCorreos;
        }

        public DataTable ConsultaCorreosEnviarHistorico(string nombreHistorico, string proceso, int ciCompania, string tipoDocumento, int cantidad,
                                        string claveAcceso, ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultadoCorreo = new DataSet();
            DataTable responseCorreos = new DataTable();
            codigoRetorno = 0;
            try
            {
                conexion.tipoBase(nombreHistorico);
                conexion.crearComandoSql("ViaDoc_WinServServicioEnvioEmail");
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
                        if (dsResultadoCorreo.Tables[0].Rows.Count > 0)
                        {
                            responseCorreos = dsResultadoCorreo.Tables[0];
                            codigoRetorno = 0;
                        }
                        else
                            codigoRetorno = 1;
                    }
                    else
                        codigoRetorno = 1;
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin($"Metodo: ConsultaCorreosEnviarHistorico, Excpetion: {ex.Message}");
            }
            finally
            {
                conexion.desconectar();
            }
            return responseCorreos;
        }


        public DataTable ConsultaEnviarPortal(string proceso, int ciCompania, string tipoDocumento, int cantidad,
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
                        if (dsResultadoCorreo.Tables[0].Rows.Count > 0)
                            responseCorreos = dsResultadoCorreo.Tables[0];
                        else
                            codigoRetorno = 1;
                    }
                    else
                        codigoRetorno = 1;
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
