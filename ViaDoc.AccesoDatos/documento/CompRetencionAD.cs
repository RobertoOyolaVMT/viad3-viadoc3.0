using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.documento
{
    public class CompRetencionAD
    {

        public DataSet InsertarCompRetencion(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento,
                                             string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                             string txFechaEmision, string ciTipoIdentificacionSujetoRetenido,
                                             string txRazonSocialSujetoRetenido, string txIdentificacionSujetoRetenido,
                                             string txPeriodoFiscal, int ciContingenciaDet, string txEmail,
                                             string txNumeroAutorizacion, string txFechaHoraAutorizacion,
                                             string xmlDocumentoAutorizado, string ciEstado,
                                             ref int codigoRetorno, ref string descripcionRetorno, ref int ciCompRetencion)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarComprobanteRetencion");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoEmision", ciTipoEmision, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmision", txFechaEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoIdentificacionSujetoRetenido", ciTipoIdentificacionSujetoRetenido, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocialSujetoRetenido", txRazonSocialSujetoRetenido, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionSujetoRetenido", txIdentificacionSujetoRetenido, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPeriodoFiscal", txPeriodoFiscal, DbType.String, ParameterDirection.Input);
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
                            ciCompRetencion = int.Parse(dsResultado.Tables[0].Rows[0]["ciCompRetencion"].ToString());
                        }
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "DataSet de consulta NULL";
                    ciCompRetencion = 0;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception:" + ex.Message;
                ciCompRetencion = 0;
            }
            return dsResultado;
        }

        public DataSet InsertarCompRetencionDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                    string txSecuencial, int ciImpuesto, string txCodRetencion,
                                                    decimal qnBaseImponible, decimal qnPorcentajeRetener, decimal qnValorRetenido,
                                                    string txCodDocSustento, string txNumDocSustento,
                                                    string txFechaEmisionDocSustento, ref int codigoRetorno,
                                                    ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {

                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarComprobanteRetencionDetalle");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciImpuesto", ciImpuesto, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodRetencion", txCodRetencion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnBaseImponible", qnBaseImponible, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnPorcentajeRetener", qnPorcentajeRetener, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnValorRetenido", qnValorRetenido, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodDocSustento", txCodDocSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumDocSustento", txNumDocSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmisionDocSustento", txFechaEmisionDocSustento, DbType.String, ParameterDirection.Input);

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





        public DataSet InsertarComprobanteRetencionInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                                 string txSecuencial, string txNombre, string txValor,
                                                                 ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarComprobanteRetencionInfoAdicional");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombre", txNombre, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txValor", txValor.Contains('(') ? txValor.Replace("(", "").Replace(")", "") : txValor, DbType.String, ParameterDirection.Input);

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
        public DataSet InsertarComprobanteRetencionDocSustento(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                      string txSecuencial, string txFechaEmision, string txCodSustento, string txCodDocSustento,
                                                      string txFechaRegistroContable, string txcodImpuestoDocSustento, string txcodigoPorcentaje,
                                                      string txTotalSinImpuesto, string txImporteTotal,
                                                      string txBaseImponible, string txTarifa, string txValorImpuesto,
                                                      ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarCompRetencionDocSustento");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmision", txFechaEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodSustento", txCodSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodDocSustento", txCodDocSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaRegistroContable", txFechaRegistroContable, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodImpuestoDocSustento", txcodImpuestoDocSustento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPorcentaje", txcodigoPorcentaje, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTotalSinImpuesto", txTotalSinImpuesto, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txImporteTotal", txImporteTotal, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txBaseImponible", txBaseImponible, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTarifa", txTarifa, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txValorImpuesto", txValorImpuesto, DbType.String, ParameterDirection.Input);


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
        public DataSet InsertarComprobanteRetencionFormaPago(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                              string txSecuencial, string txFormaPago, string qnTotal, 
                                              ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarCompRetencionFormaPago");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFormaPago", txFormaPago, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnTotal", qnTotal, DbType.Decimal, ParameterDirection.Input);



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
