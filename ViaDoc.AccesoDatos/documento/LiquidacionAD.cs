using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.documento
{
    public class LiquidacionAD
    {

        public DataSet InsertarLiquidacion(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento,
                                                       string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                       string txFechaEmision, string ciTipoIdentificacionProvedor,
                                                       string txRazonSocialProvedor, string txIdentificacionProvedor,
                                                       decimal qnTotalSinImpuestos, decimal qnTotalDescuento, string CodDocReembolso, decimal totalComprobantesReembolso, 
                                                       decimal totalBaseImponibleReembolso, decimal totalImpuestoReembolso,
                                                       decimal qnImporteTotal, string txMoneda, int ciContingenciaDet, string txEmail,
                                                       string txNumeroAutorizacion, string txFechaHoraAutorizacion,
                                                       string xmlDocumentoAutorizado, string ciEstado, string ciAmbiente,
                                                       ref int codigoRetorno, ref string descripcionRetorno, ref int ciLiquidacion)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsLiquidaacion = new DataSet();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacion");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoEmision", ciTipoEmision, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmision", txFechaEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoIdentificacionProvedor", ciTipoIdentificacionProvedor, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocialProvedor", txRazonSocialProvedor, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionProvedor", txIdentificacionProvedor, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnTotalSinImpuestos", qnTotalSinImpuestos, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnTotalDescuento", qnTotalDescuento, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@codDocReembolso", CodDocReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@totalComprobantesReembolso", totalComprobantesReembolso, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@totalBaseImponibleReembolso", totalBaseImponibleReembolso, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@totalImpuestoReembolso", totalImpuestoReembolso, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@qnImporteTotal", qnImporteTotal, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@txMoneda", txMoneda, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciContingenciaDet", ciContingenciaDet, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEmail", txEmail, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumeroAutorizacion", txNumeroAutorizacion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaHoraAutorizacion", txFechaHoraAutorizacion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@xmlDocumentoAutorizado", xmlDocumentoAutorizado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);

                dsLiquidaacion = conexion.EjecutarConsultaDatSet();

                if (dsLiquidaacion != null)
                {
                    if (dsLiquidaacion.Tables.Count > 0)
                    {
                        if (dsLiquidaacion.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsLiquidaacion.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsLiquidaacion.Tables[0].Rows[0]["Respuesta"].ToString();
                            ciLiquidacion = int.Parse(dsLiquidaacion.Tables[0].Rows[0]["ciLiquidacion"].ToString());
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }

            return dsLiquidaacion;
        }

        public DataSet InsertarLiquidacionDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                    string txSecuencial, string txCodigoPrincipal, string txCodigoAuxiliar,
                                                    string txDescripcion, int qnCantidad, decimal qnPrecioUnitario,
                                                    decimal qnDescuento, decimal qnPrecioTotalSinImpuesto,
                                                    ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsLiquiDeta = new DataSet();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacionDetalle");
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

                dsLiquiDeta = conexion.EjecutarConsultaDatSet();

                if (dsLiquiDeta != null)
                {
                    if (dsLiquiDeta.Tables.Count > 0)
                    {
                        if (dsLiquiDeta.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = int.Parse(dsLiquiDeta.Tables[0].Rows[0]["idCodigo"].ToString());
                            descripcionRetorno = dsLiquiDeta.Tables[0].Rows[0]["Respuesta"].ToString();
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }
            return dsLiquiDeta;
        }

        public DataSet InsertarLiquidacionTotalImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                    string txSecuencial, string txCodigo, string txCodigoPorcentaje,
                                                    string txTarifa, decimal qnDescuentoAdicional, decimal qnBaseImponible,
                                                    decimal qnValor, ref int codigoRetorno, ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacionTotalImpuesto");
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }
            return dsResultado;
        }



        public DataSet InsertarLiquidacionInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                       string txSecuencial, string txNombre, string txValor,
                                                       ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacionInfoAdicional");
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }
            return dsResultado;
        }



        public DataSet InsertarLiquidacionDetalleFormaPago(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                       string txSecuencial, string txFormaPago, string txPlazo,
                                                       string txUnidadTiempo, decimal qnTotal, ref int codigoRetorno,
                                                       ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = new DataSet();
            try
            {

                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacionDetalleFormaPago");
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }
            return dsResultado;
        }



        public DataSet InsertarLiquidacionDetalleImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                      string txSecuencial, string txCodigoPrincipal, string txCodigo,
                                                      string txCodigoPorcentaje, string txTarifa, decimal qnBaseImponible,
                                                      decimal qnValor, ref int codigoRetorno, ref string descripcionRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = new DataSet();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacionDetalleImpuesto");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txEstablecimiento", txEstablecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPuntoEmision", txPuntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPrincipal", txCodigoPrincipal, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigo", txCodigo, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPorcentaje", txCodigoPorcentaje, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTarifa", txTarifa.Replace(",", "."), DbType.String, ParameterDirection.Input);
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }
            return dsResultado;
        }



        public DataSet InsertarLiquidacionDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                       string txSecuencial, string txCodigoPrincipal, string txNombre,
                                                       string txValor, ref int codigoRetorno, ref string descripcionRetorno)
        {

            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacionDetalleAdicional");
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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }
            return dsResultado;
        }

        public DataSet InsertarLiquidacionReembolso(int ciCompania, string txTipoIdentificacionProveedorReembolso, string txIdentificacionProveedorReembolso, string txCodPaisPagoProveedorReembolso,
                                                    string txTipoProveedorReembolso, string CodDocReembolso, string EstabDocReembolso, string PtoEmiDocReembolso, string SecuencialDocReembolso, string txFechaEmisionDocReembolso,
                                                    string numeroautorizacionDocReemb, string codigo, string codigoPorcentaje, string tarifa, decimal baseImponibleReembolso, decimal impuestoReembolso,
                                                    ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = null;
            ConexionViaDoc conexion = new ConexionViaDoc();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_InsertarLiquidacionReembolso");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTipoIdentificacionProveedorReembolso", txTipoIdentificacionProveedorReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionProveedorReembolso", txIdentificacionProveedorReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodPaisPagoProveedorReembolso", txCodPaisPagoProveedorReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txTipoProveedorReembolso", txTipoProveedorReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@CodDocReembolso", CodDocReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@EstabDocReembolso", EstabDocReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@PtoEmiDocReembolso", PtoEmiDocReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@SecuencialDocReembolso", SecuencialDocReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txFechaEmisionDocReembolso", txFechaEmisionDocReembolso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@numeroautorizacionDocReemb", numeroautorizacionDocReemb, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@codigo", codigo, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@codigoPorcentaje", codigoPorcentaje, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tarifa", tarifa, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@baseImponibleReembolso", baseImponibleReembolso, DbType.Decimal, ParameterDirection.Input);
                conexion.agregarParametroSP("@impuestoReembolso", impuestoReembolso, DbType.Decimal, ParameterDirection.Input);

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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(descripcionRetorno);
            }

            return dsResultado;
        }
    }
}
