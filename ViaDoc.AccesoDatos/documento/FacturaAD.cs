using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.documento
{
    public class FacturaAD
    {
        public DataSet InsertarFactura(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento,
                                                       string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                       string txFechaEmision, string ciTipoIdentificacionComprador, string txGuiaRemision,
                                                       string txRazonSocialComprador, string txIdentificacionComprador,
                                                       decimal qnTotalSinImpuestos, decimal qnTotalDescuento, decimal qnPropina,
                                                       decimal qnImporteTotal, string txMoneda, int ciContingenciaDet, string txEmail,
                                                       string txNumeroAutorizacion, string txFechaHoraAutorizacion,
                                                       string xmlDocumentoAutorizado, string ciEstado, string ciAmbiente,
                                                       ref int codigoRetorno, ref string descripcionRetorno, ref int ciFactura)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFactura");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoEmision", ciTipoEmision, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmision", txFechaEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoIdentificacionComprador", ciTipoIdentificacionComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txGuiaRemision", txGuiaRemision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocialComprador", txRazonSocialComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionComprador", txIdentificacionComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnTotalSinImpuestos", qnTotalSinImpuestos, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnTotalDescuento", qnTotalDescuento, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnPropina", qnPropina, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnImporteTotal", qnImporteTotal, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@txMoneda", txMoneda, DbType.String, ParameterDirection.Input);
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
                        if(dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsResultado.Tables[0].Rows[0]["Respuesta"].ToString();
                            ciFactura = int.Parse(dsResultado.Tables[0].Rows[0]["ciFactura"].ToString());
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




        public DataSet InsertarFacturaDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                    string txSecuencial, string txCodigoPrincipal, string txCodigoAuxiliar,
                                                    string txDescripcion, int qnCantidad, decimal qnPrecioUnitario,
                                                    decimal qnDescuento, decimal qnPrecioTotalSinImpuesto,
                                                    ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFacturaDetalle");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPrincipal", txCodigoPrincipal, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoAuxiliar", txCodigoAuxiliar, DbType.String, ParameterDirection.Input);
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



        public DataSet InsertarFacturaTotalImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                    string txSecuencial, string txCodigo, string txCodigoPorcentaje,
                                                    string txTarifa, decimal qnDescuentoAdicional, decimal qnBaseImponible,
                                                    decimal qnValor, ref int codigoRetorno, ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFacturaTotalImpuesto");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigo", txCodigo, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPorcentaje", txCodigoPorcentaje, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTarifa", txTarifa, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnDescuentoAdicional", qnDescuentoAdicional, DbType.Int32, ParameterDirection.Input);
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



        public DataSet InsertarFacturaInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                       string txSecuencial, string txNombre, string txValor,
                                                       ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFacturaInfoAdicional");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombre", txNombre, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txValor", txValor.Contains('(')? txValor.Replace("(", "").Replace(")", "") : txValor, DbType.String, ParameterDirection.Input);

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



        public DataSet InsertarFacturaDetalleFormaPago(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                       string txSecuencial, string txFormaPago, string txPlazo,
                                                       string txUnidadTiempo, decimal qnTotal, ref int codigoRetorno,
                                                       ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {

                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFacturaDetalleFormaPago");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFormaPago", txFormaPago, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPlazo", txPlazo, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txUnidadTiempo", txUnidadTiempo, DbType.String, ParameterDirection.Input);
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



        public DataSet InsertarFacturaDetalleImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                      string txSecuencial, string txCodigoPrincipal, string txCodigo,
                                                      string txCodigoPorcentaje, string txTarifa, decimal qnBaseImponible,
                                                      decimal qnValor, ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFacturaDetalleImpuesto");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPrincipal", txCodigoPrincipal, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigo", txCodigo, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPorcentaje", txCodigoPorcentaje, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTarifa", txTarifa.Replace(",","."), DbType.String, ParameterDirection.Input);
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



        public DataSet InsertarFacturaDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                       string txSecuencial, string txCodigoPrincipal, string txNombre,
                                                       string txValor, ref int codigoRetorno, ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFacturaDetalleAdicional");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPrincipal", txCodigoPrincipal, DbType.String, ParameterDirection.Input);
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

        public DataSet InsertarFacturaDetalleReembolso(int ciCompania, string ciTipoDocumento, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
		  string txNumDocumento, string txFechaEmision, string txIdProveedor, string txClaveAcceso, string subTotNoIva, string subTotIvaCero, string subTotIva,
		  string subTotExcentoIva, string impIva, string impIce, string impIRBPNR, string valTotal, string detalle, string valorBase,
          string codigoImp, string codigoPorcentajeImp, string tarifaImp, string tipoIdProveedor, string codPaisPagoProveedor, string tipoProveedor, ref int codigoRetorno, ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarFacturaDetalleReembolso");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumDocumento", txNumDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmision", txFechaEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdProveedor", txIdProveedor, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@subTotNoIva", subTotNoIva, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@subTotIvaCero", subTotIvaCero, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@subTotIva", subTotIva, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@subTotExcentoIva", subTotExcentoIva, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@impIva", impIva, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@impIce", impIce, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@impIRBPNR", impIRBPNR, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@valTotal", valTotal, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@detalle", detalle, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@valorBase", valorBase, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@codigo", codigoImp, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@codigoPorcentaje", codigoPorcentajeImp, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tarifa", tarifaImp, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTipoIdProveedor", tipoIdProveedor, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@codPaisPagoProveedor", codPaisPagoProveedor, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipoProveedor", tipoProveedor, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["ciReembolso"].ToString());
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

