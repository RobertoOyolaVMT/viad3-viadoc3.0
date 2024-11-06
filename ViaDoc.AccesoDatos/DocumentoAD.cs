using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.Utilitarios;
using ViaDoc.Utilitarios.logs;

namespace ViaDoc.AccesoDatos
{
    public class DocumentoAD
    {
        public DataSet verificaExisteDocumento(string txTipoDocumento, int ciCompania, string txEstablecimeinto, string txPuntoEmision,
                                               string txSecuencial, ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ObtenerClaveAcceso");
                conexion.agregarParametroSP("@txTipoDocumento", txTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int16, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimeinto, DbType.String, ParameterDirection.Input);
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
                    descripcionRetorno = "DataSet de consulta NULL";
                }

                
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception:" + ex.Message;
            }
            return dsResultado;
        }

        //Consulta Documentos Electronicos Para el proceso de firmas
        public DataSet DocumentosElectronicos(int opcion, int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                              string txCodigoPrincipal, int numeroRegistro, string txCodigoInterno, ref int codigoRetorno,
                                              ref string descripcionRetorno)
        {
            DataSet documentosElectronicos = new DataSet();
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServConsultaFactElectronica");
                conexion.agregarParametroSP("@versioEsquema", "", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@compania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPrincipal", txCodigoPrincipal, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@numeroRegistro", numeroRegistro, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoInterno", txCodigoInterno, DbType.String, ParameterDirection.Input);

                documentosElectronicos = conexion.EjecutarConsultaDatSet();
                
                if (documentosElectronicos != null)
                {
                    if (documentosElectronicos.Tables.Count > 0)
                        codigoRetorno = 0;
                    else
                        codigoRetorno = 1;
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                descripcionRetorno = "Exception:" + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }

            return documentosElectronicos;
        }

        public DataTable ConsultaComprobantePorEstado(int ciCompania, string estado, int numregistros, string citipodocumento,
                                                      string txClaveAcceso, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataTable dtComprobanteResponse = new DataTable();
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet companiaCertificado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServConsultaDocumentoFirmado");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@numeroRegistro", numregistros, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstadoRecepcionAutorizacion", estado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@citipodocumento", citipodocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                companiaCertificado = conexion.EjecutarConsultaDatSet();

                if (companiaCertificado != null)
                {
                    if (companiaCertificado.Tables.Count > 0)
                    {
                        codigoRetorno = 0;
                        dtComprobanteResponse = companiaCertificado.Tables[0];
                    }
                    else
                        codigoRetorno = 1;
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception:" + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return dtComprobanteResponse;
        }

        public DataSet ObtenerTipoDocumentos(string opcion, string ciTipoDocumentos, string ciEstado, int ciCompania,
                                             ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsTipoDocumentos = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServTipoDocumentos");
                conexion.agregarParametroSP("@opcion", opcion.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumentos", ciTipoDocumentos.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                dsTipoDocumentos = conexion.EjecutarConsultaDatSet();

                if (dsTipoDocumentos != null)
                {
                    if (dsTipoDocumentos.Tables.Count > 0)
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
                    descripcionRetorno = "Nose se obtuve ninguna Registro";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception" + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return dsTipoDocumentos;
        }


        public int ActualizarComprobantes(DataSet xmlComprobante)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            int i = 0;
            DataSet dsResultado = null;
           try
            {
                foreach (DataRow item in xmlComprobante.Tables[0].Rows)
                {
                    try
                    {
                         if (item["XmlEstado"].ToString().Trim().Length != 0)
                        {
                            //Da valores a los parametros del SP
                            conexion.tipoBase("Viadoc");
                            conexion.crearComandoSql("ViaDoc_WinServActualizaXmlComprobantes");
                            conexion.agregarParametroSP("@ciIdentityComprobante", Convert.ToInt32(item["Identity"].ToString()), DbType.Int32, ParameterDirection.Input);
                            conexion.agregarParametroSP("@ciCompania", Convert.ToInt32(item["CiCompania"].ToString()), DbType.Int32, ParameterDirection.Input);
                            conexion.agregarParametroSP("@ciTipoDocumento", item["CiTipoDocumento"].ToString(), DbType.String, ParameterDirection.Input);
                            conexion.agregarParametroSP("@txNumeroAutorizacion", item["ClaveAcceso"].ToString().Trim(), DbType.String, ParameterDirection.Input);
                            conexion.agregarParametroSP("@txFechaHoraAutorizacion", item["txFechaHoraAutorizacion"].ToString(), DbType.String, ParameterDirection.Input);
                            conexion.agregarParametroSP("@xmlDocumentoAutorizado", item["XmlComprobante"].ToString(), DbType.String, ParameterDirection.Input);
                            conexion.agregarParametroSP("@ciEstado", item["XmlEstado"].ToString().Trim(), DbType.String, ParameterDirection.Input);
                            conexion.agregarParametroSP("@txOpcion", item["CiContingenciaDet"].ToString().Trim(), DbType.String, ParameterDirection.Input);
                            conexion.agregarParametroSP("@txClaveAcceso", item["ClaveAcceso"].ToString().Trim(), DbType.String, ParameterDirection.Input);
                            conexion.agregarParametroSP("@ciNumeroIntento", int.Parse(item["ciNumeroIntento"].ToString().Trim()), DbType.Int32, ParameterDirection.Input);
                            conexion.agregarParametroSP("@MensajeError", item["MensajeError"].ToString().Trim(), DbType.String, ParameterDirection.Input);
                            dsResultado = conexion.EjecutarConsultaDatSet();

                            if (dsResultado != null)
                            {
                                
                                if (dsResultado.Tables.Count > 0)
                                {
                                   if (dsResultado.Tables[0].Rows.Count > 0)
                                    {
                                        int codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                                        if (codigoRetorno.Equals(0))
                                        {
                                            i = 0;
                                        }
                                        else
                                        {
                                            i = 1;
                                            string mensajeRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                                         }
                                    }
                                    else
                                    {
                                        i = 1;
                                    }
                                }
                                else
                                {
                                    i = 1;
                                }
                            }
                            else
                            {
                              i = 1;
                            }
                        }
                        else
                        {
                            //Agrega excepcion En caso de no haber un xml 
                            i = 1;
                            throw new System.ArgumentException("CiContingenciaDet: " + item["CiContingenciaDet"].ToString() + " Tipo DocumentoAD: " + item["CiTipoDocumento"].ToString() + " ClaveAcceso: " + item["ClaveAcceso"].ToString().Trim() + " Error: comprobante sin estado");
                        }
                    }
                    catch (Exception ex)
                    {
                        i = 1;
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error: "+ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                i = 1;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error: " + ex.Message);
            }
            finally
            {
                conexion.desconectar();
            }
            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("valor de i" +i);
            return i;
        }


        public void ActualizarComprobantesProcesados(int identity, int ciCompania, string ciTipoDocumento, string claveAcceso, string txFechaHoraAutorizacion,
                                                    string xmlComprobante, string xmlEstado, string ciContingenciaDet, int ciNumeroIntento, string mensajeError,
                                                    ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                if (xmlEstado.Trim().Length != 0)
                {

                    conexion.tipoBase("Viadoc");
                    conexion.crearComandoSql("ViaDoc_WinServActualizaXmlComprobantes");
                    conexion.agregarParametroSP("@ciIdentityComprobante", identity, DbType.Int32, ParameterDirection.Input);
                    conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                    conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                    conexion.agregarParametroSP("@txNumeroAutorizacion", claveAcceso, DbType.String, ParameterDirection.Input);
                    conexion.agregarParametroSP("@txFechaHoraAutorizacion", txFechaHoraAutorizacion.ToString(), DbType.String, ParameterDirection.Input);
                    conexion.agregarParametroSP("@xmlDocumentoAutorizado", xmlComprobante, DbType.String, ParameterDirection.Input);
                    conexion.agregarParametroSP("@ciEstado", xmlEstado, DbType.String, ParameterDirection.Input);
                    conexion.agregarParametroSP("@txOpcion", ciContingenciaDet, DbType.String, ParameterDirection.Input);
                    conexion.agregarParametroSP("@txClaveAcceso", claveAcceso, DbType.String, ParameterDirection.Input);
                    conexion.agregarParametroSP("@ciNumeroIntento", ciNumeroIntento, DbType.Int32, ParameterDirection.Input);
                    conexion.agregarParametroSP("@MensajeError", mensajeError, DbType.String, ParameterDirection.Input);
                    dsResultado = conexion.EjecutarConsultaDatSet();

                    if (dsResultado != null)
                    {
                        if (dsResultado.Tables.Count > 0 || dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                        }
                        else
                        {
                            codigoRetorno = 1;
                            descripcionRetorno = "Error Documentos no posee registros...";
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                        descripcionRetorno = "Error Documentos no posee DataSet viene Null...";
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "Error Documentos no posee Estado...";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error de actualizacion de datos: " + ex.ToString());
                codigoRetorno = 9999;
                descripcionRetorno = "Error Inesperado: " + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
        }
    }
}
