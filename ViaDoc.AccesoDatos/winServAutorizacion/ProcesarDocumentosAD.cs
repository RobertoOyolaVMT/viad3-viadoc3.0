using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.winServAutorizacion
{
    public class ProcesarDocumentosAD
    {
        ConexionViaDoc conexion = new ConexionViaDoc();

        public DataTable ConsultaComprobanteFinales(int ciCompania, string TipoDocumento, string ciEstado)
        {
            DataTable documentosAutorizados = new DataTable();
            DataSet dsResultado = new DataSet();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ConsultarComprobantesFinalesAutorizacion");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@TipoDocumento", TipoDocumento.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@urlAutorizacion", ciEstado.Trim(), DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();
                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                        documentosAutorizados = dsResultado.Tables[0];
                    else
                        documentosAutorizados = null;
                }
                else
                    documentosAutorizados = null;
            }
            catch (Exception ex)
            {
                documentosAutorizados = null;
            }
            finally
            {
                conexion.desconectar();
            }

            return documentosAutorizados;
        }



        public bool RegistarDocumentosErrores(string xml)
        {
            DataSet dsResultado = new DataSet();
            int codigoError = 0;
            bool respuestaDocumentoEroores = false;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ServicioRecepcionAutorizacion_MigrarDocErrores");
                conexion.agregarParametroSP("@varXml", xml, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();
                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        codigoError = int.Parse(dsResultado.Tables[0].Rows[0][0].ToString());

                        if (codigoError == 1)
                            respuestaDocumentoEroores = true;

                    }
                }
            }
            catch (Exception ex)
            {
                respuestaDocumentoEroores = false;
            }
            finally
            {
                conexion.desconectar();
            }
            return respuestaDocumentoEroores;
        }


        public void ConsultaDocumentosEnProceso(string opcion, string ciCompania, string documento, ref DataSet ds, string claveAcceso, ref int codigoRetorno, ref string mensajeError)
        {
            DataSet dsResultado = new DataSet();
            int codigoError = 0;
            bool respuestaDocumentoEroores = false;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("SP_DocumentosEnProceso");
                if (opcion == "C")
                    conexion.agregarParametroSP("@opcion", "C", DbType.String, ParameterDirection.Input);
                else
                    conexion.agregarParametroSP("@opcion", "A", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@documento", documento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@claveAcceso", "", DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if(dsResultado != null)
                {
                    codigoRetorno = 0;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeError = ex.Message;
            }
        }

        public DataSet DocReproceso(string Tipodocu, string Fecha, string FechaHAsta, string CLaveAcceso, string Opcion, ref int codigoRetorno, ref string mensajeError)
        {
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebReprocesoDocumentos");
                conexion.agregarParametroSP("@compania", "9999", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipodocu", Tipodocu, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@numdocumento", "", DbType.String, ParameterDirection.Input);
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
                    mensajeError = "DataSet de consulta NULL";
                }


            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeError = ex.Message;
            }

            return dsResultado;
        }
    }
}
