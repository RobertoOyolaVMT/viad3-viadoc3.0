using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ViaDoc.AccesoDatos.documento
{
    public class NotaCreditoAD
    {


        public DataSet InsertarNotaCredito(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento, 
                                           string txPuntoEmision, string txSecuencial, string txFechaEmision, string ciTipoIdentificacionComprador,
                                           string txRazonSocialComprador, string txIdentificacionComprador, string txRise, string ciTipoDocumentoModificado, 
                                           string txNumeroDocumentoModificado, string txFechaEmisionDocumentoModificado, decimal qnTotalSinImpuestos,
                                           decimal qnValorModificacion, string txMoneda, string txMotivo, int ciContingenciaDet, string txEmail, 
                                           string txNumeroAutorizacion, string txFechaHoraAutorizacion, string xmlDocumentoAutorizado, string ciEstado, 
                                           string ciAmbiente, ref int codigoRetorno, ref string descripcionRetorno, ref int ciNotaCredito)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarNotaCredito");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoEmision", ciTipoEmision, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmision", txFechaEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoIdentificacionComprador", ciTipoIdentificacionComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocialComprador", txRazonSocialComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionComprador", txIdentificacionComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRise", txRise, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumentoModificado", ciTipoDocumentoModificado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumeroDocumentoModificado", txNumeroDocumentoModificado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmisionDocumentoModificado", txFechaEmisionDocumentoModificado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnTotalSinImpuestos", qnTotalSinImpuestos, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnValorModificacion", qnValorModificacion, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@txMoneda", txMoneda, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txMotivo", txMotivo, DbType.String, ParameterDirection.Input);
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
                            ciNotaCredito = int.Parse(dsResultado.Tables[0].Rows[0]["ciNotaCredito"].ToString());
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


        public DataSet InsertarNotaCreditoDEtalle(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoInterno, 
                                                  string txCodigoAdicional, string txDescripcion, int qnCantidad, decimal qnPrecioUnitario, decimal qnDescuento, 
                                                  decimal qnPrecioTotalSinImpuesto, ref int codigoRetorno, ref string descripcionRetorno)

        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarNotaCreditoDetalle");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoInterno", txCodigoInterno, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoAdicional", txCodigoAdicional, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txDescripcion", txDescripcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnCantidad", qnCantidad, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnPrecioUnitario", qnPrecioUnitario, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnDescuento", qnDescuento, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnPrecioTotalSinImpuesto", qnPrecioTotalSinImpuesto, DbType.Decimal, ParameterDirection.Input);

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


        public DataSet InsertarNotaCreditoDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                           string txCodigoInterno, string txNombre, string txValor, ref int codigoRetorno, 
                                                           ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarNotaCreditoDetalleAdicional");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoInterno", txCodigoInterno, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombre", txNombre, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@txValor", txValor, DbType.Decimal, ParameterDirection.Input);
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


        public DataSet InsertarNotaCreditoDetalleImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoInterno, 
                                                          string txCodigo, string txCodigoPorcentaje, string txTarifa, decimal qnBaseImponible, decimal qnValor,
                                                          ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarNotaCreditoDetalleImpuesto");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoInterno", txCodigoInterno, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigo", txCodigo, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPorcentaje", txCodigoPorcentaje, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTarifa", txTarifa, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnBaseImponible", qnBaseImponible, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnValor", qnValor, DbType.Decimal, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();
                Utilitarios.logs.LogsFactura.LogsInicioFin("12");
                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                            Utilitarios.logs.LogsFactura.LogsInicioFin("13: " + descripcionRetorno);
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
                Utilitarios.logs.LogsFactura.LogsInicioFin("13: " + descripcionRetorno);
            }
            return dsResultado;
        }


        public DataSet InsertarNotaCreditoInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                        string txNombre, string txValor, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarNotaCreditoInfoAdicional");
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



        public DataSet InsertarNotaCreditoTotalImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigo, 
                                                        string txCodigoPorcentaje, decimal qnBaseImponible, decimal qnValor, ref int codigoRetorno, 
                                                        ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarNotaCreditoTotalImpuesto");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigo", txCodigo, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPorcentaje", txCodigoPorcentaje, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnBaseImponible", qnBaseImponible, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnValor", qnValor, DbType.Decimal, ParameterDirection.Input);
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
