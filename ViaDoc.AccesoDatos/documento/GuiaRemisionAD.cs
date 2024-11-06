using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.documento
{
    public class GuiaRemisionAD
    {


        public DataSet InsertarGuiaRemision(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento, 
                                            string txPuntoEmision, string txSecuencial, string txDireccionPartida, string txRazonSocialTransportista, 
                                            string ciTipoIdentificacionTransportista, string txRucTransportista, string txRise, string txFechaIniTransporte,
                                            string txFechaFinTransporte, string txPlaca, int ciContingenciaDet, string txEmail, string txNumeroAutorizacion, 
                                            string txFechaHoraAutorizacion, string xmlDocumentoAutorizado, string ciEstado, string ciAmbiente,
                                            ref int codigoRetorno, ref string descripcionRetorno, ref int ciGuiaRemision)
        {
            DataSet dsResultado = null;
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarGuiaRemision");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoEmision", ciTipoEmision, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txDireccionPartida", txDireccionPartida, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocialTransportista", txRazonSocialTransportista, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoIdentificacionTransportista", ciTipoIdentificacionTransportista, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRucTransportista", txRucTransportista, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRise", txRise, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaIniTransporte", txFechaIniTransporte, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaFinTransporte", txFechaFinTransporte, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPlaca", txPlaca, DbType.String, ParameterDirection.Input);                                
                conexion.agregarParametroSP("@ciContingenciaDet", ciContingenciaDet, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEmail", txEmail, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumeroAutorizacion", txNumeroAutorizacion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaHoraAutorizacion", txFechaHoraAutorizacion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@xmlDocumentoAutorizado", xmlDocumentoAutorizado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);
                
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                            ciGuiaRemision = int.Parse(dsResultado.Tables[0].Rows[0]["ciGuiaRemision"].ToString());
                        }
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


        public DataSet InsertarGuiaRemisionDestinatario(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                        string txIdentificacionDestinatario, string txRazonSocialDestinatario, string txDireccionDestinatario, 
                                                        string txMotivoTraslado, string txDocumentoAduaneroUnico, string txCodigoEstablecimientoDestino, 
                                                        string txRuta, string ciTipoDocumentoSustento, string txNumeroDocumentoSustento, 
                                                        string txNumeroAutorizacionDocumentoSustento, string txFechaEmisionDocumentoSustento,
                                                        ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = null;
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarGuiaRemisionDestinatario");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionDestinatario", txIdentificacionDestinatario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocialDestinatario", txRazonSocialDestinatario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txDireccionDestinatario", txDireccionDestinatario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txMotivoTraslado", txMotivoTraslado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txDocumentoAduaneroUnico", txDocumentoAduaneroUnico, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoEstablecimientoDestino", txCodigoEstablecimientoDestino, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRuta", txRuta, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumentoSustento", ciTipoDocumentoSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumeroDocumentoSustento", txNumeroDocumentoSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumeroAutorizacionDocumentoSustento", txNumeroAutorizacionDocumentoSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmisionDocumentoSustento", txFechaEmisionDocumentoSustento, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                        }
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


        public DataSet InsertarGuiaRemisionDestinatarioDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                               string txIdentificacionDestinatario, string txCodigoInterno, string txCodigoAdicional, 
                                                               string txDescripcion, decimal qnCantidad, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = null;
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarGuiaRemisionDestinatarioDetalle");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionDestinatario", txIdentificacionDestinatario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoInterno", txCodigoInterno, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoAdicional", txCodigoAdicional, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txDescripcion", txDescripcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnCantidad", qnCantidad, DbType.Decimal, ParameterDirection.Input);
                
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                        }
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



        public DataSet InsertarGuiaRemisionDestinatarioDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                                        string txIdentificacionDestinatario, string txCodigoInterno, string txNombre, string txValor,
                                                                        ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = null;
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarGuiaRemisionDestinatarioDetalleAdicional");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionDestinatario", txIdentificacionDestinatario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoInterno", txCodigoInterno, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombre", txNombre, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txValor", txValor, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                        }
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


        public DataSet InsertarGuiaRemisionInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                         string txNombre, string txValor, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = null;
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarGuiaRemisionInfoAdicional");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombre", txNombre, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txValor", txValor, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                        }
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


    }
}
