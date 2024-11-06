using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos;
using ViaDoc.AccesoDatos.documento;
using ViaDoc.Utilitarios;

namespace ViaDoc.LogicaNegocios.documentos
{
    public class ProcesoLiquidacion
    {
        #region Obtencion de Longitud de campos 
        private int ClaveAcceso = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CLAVE_ACCESO"));
        private int TipoDocumento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_TIPO_DOCUMENTO"));
        private int Establecimiento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_ESTABLECIMIENTO"));
        private int PuntoEmision = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_PUNTO_EMISION"));
        private int Secuencial = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_SECUENCIAL"));
        private int Fecha = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_FECHA_EMISION"));
        private int TipoIdentificacionSujeto = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_TIPO_IDENTIFICACION_SUJETO"));
        private int RazonSocialSujetoRetenido = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_RAZON_SOCIAL_SUJETO_RE"));
        private int Identificacion = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_IDENTIFICACION_SUJETO_RETENIDO"));
        private int periodoFiscal = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_PERIODO_FISCAL"));
        private int NumeroAutorizacion = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NUMERO_AUTORIZACION"));
        private int FechaHoraAutorizacion = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_FECHA_HORA_AUTORIZACION"));
        private int codigoRetencion = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_RETENCION"));
        private int CodigoDocumentoSustento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_DOCUMENTO_SUSTENTO"));
        private int NumeroDocumentoSustento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NUM_DOCUMENTO_SUSTENTO"));
        private int Nombre = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NOMBRE"));
        private int valor = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_VALOR"));
        private int Codigo = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO"));
        private int TipoIdentificacionComprador = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_TIPO_IDENTIFICACION_COMPRADOR"));
        private int RazonSocialComprador = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_RAZON_SOCIAL_COMPRADOR"));
        private int Rise = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_RISE"));
        private int NumeroDocumentoModificado = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NUMERO_DOCUMENTO_MODIFICADO"));
        private int CodigoPorcentaje = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_PORCENTAJE"));
        private int Tarifa = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_TARIFA"));
        private int GuiaRemision = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_GUIA_REMISION"));
        private int Moneda = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_MONEDA"));
        private int CodigoPrincipal = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_PRINCIPAL"));
        private int CodigoAuxiliar = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_AUXILIAR"));
        private int TipoEmision = int.Parse(ConfigurationManager.AppSettings.Get("TIPO_EMISION"));
        private int FormaPago = int.Parse(ConfigurationManager.AppSettings.Get("FORMA_PAGO"));
        private int Plazo = int.Parse(ConfigurationManager.AppSettings.Get("PLAZO"));
        private int UnidadTiempo = int.Parse(ConfigurationManager.AppSettings.Get("UNIDAD_TIEMPO"));
        #endregion Obtencion de Longitud de campos

        public LiquidacionAD _guardaLiquidacion = new LiquidacionAD();

        public DataSet verificaExisteDocumento(string tipoDocumento, int compania, string establecimeinto,
                                        string puntoEmision, string secuencial, ref int codigoRetorno,
                                        ref string mensajeRetorno)
        {
            DocumentoAD objMetodos = new DocumentoAD();
            return objMetodos.verificaExisteDocumento(tipoDocumento, compania, establecimeinto, puntoEmision,
                                                      secuencial, ref codigoRetorno, ref mensajeRetorno);
        }


        public void InsertarLiquidacion(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento, string txPuntoEmision,
                                   string txSecuencial, string txFechaEmision, string ciTipoIdentificacionProvedor, string txRazonSocialProvedor,
                                   string txIdentificacionProvedor, decimal qnTotalSinImpuestos, decimal qnTotalDescuento, string CodDocReembolso, decimal totalComprobantesReembolso, decimal totalBaseImponibleReembolso, 
                                   decimal totalImpuestoReembolso, decimal qnImporteTotal, string txMoneda, int ciContingenciaDet, string txEmail, 
                                   string txNumeroAutorizacion, string txFechaHoraAutorizacion, string ciCodigoNumerico,string xmlDocumentoAutorizado, 
                                   string ciEstado, string ciAmbiente, ref int codigoRetorno, ref string descripcionRetorno, ref int ciLiquidacion)
        {

            #region 4: Validar Campos Ingresados
            Validacion validacion = new Validacion();

            try
            {
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }

                if (ciCompania != 0 && ciTipoDocumento != "" && ciTipoEmision != 0 && ciTipoIdentificacionProvedor != "" &&
                    txSecuencial != "" && txEstablecimiento != "" && txFechaEmision != "" && txPuntoEmision != "" && txIdentificacionProvedor != ""
                    && txMoneda != "" && txRazonSocialProvedor != "" && qnImporteTotal != 0
                    && qnTotalSinImpuestos != 0)
                {
                    #region Validar longitud de Cabecera Liquidacion
                    validacion.validarCampos(ref ciTipoDocumento, "string", "ciTipoDocumento", TipoDocumento, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txFechaEmision, "string", "txFechaEmision", Fecha, ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoIdentificacionProvedor, "ciTipoIdentificacionComprador", TipoIdentificacionComprador, false, ref descripcionRetorno);
                    validacion.validarCampos(ref txIdentificacionProvedor, "string", "txIdentificacionComprador", Identificacion, ref descripcionRetorno);
                    validacion.validarCampos(ref txMoneda, "string", "txMoneda", Moneda, ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciCodigoNumerico, "ciCodigoNumerico", 8, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txClaveAcceso, "string", "txClaveAcceso", ClaveAcceso, ref descripcionRetorno);
                    #endregion Validar longitud de Cabecera Factura

                    if (validacion.contadorError == 0)
                    {

                        DataSet dsResultadoFactura = _guardaLiquidacion.InsertarLiquidacion(ciCompania,
                                                                                    ciTipoEmision,
                                                                                    txClaveAcceso,
                                                                                    ciTipoDocumento,
                                                                                    txEstablecimiento,
                                                                                    txPuntoEmision,
                                                                                    txSecuencial,
                                                                                    txFechaEmision,
                                                                                    ciTipoIdentificacionProvedor,
                                                                                    txRazonSocialProvedor,
                                                                                    txIdentificacionProvedor,
                                                                                    qnTotalSinImpuestos,
                                                                                    qnTotalDescuento,
                                                                                    CodDocReembolso,
                                                                                    totalComprobantesReembolso,
                                                                                    totalBaseImponibleReembolso,
                                                                                    totalImpuestoReembolso,
                                                                                    qnImporteTotal,
                                                                                    txMoneda,
                                                                                    ciContingenciaDet,
                                                                                    txEmail,
                                                                                    txNumeroAutorizacion,
                                                                                    txFechaHoraAutorizacion,
                                                                                    xmlDocumentoAutorizado,
                                                                                    ciEstado,
                                                                                    ciAmbiente,
                                                                                    ref codigoRetorno,
                                                                                    ref descripcionRetorno,
                                                                                    ref ciLiquidacion);
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Ocurrio un error al registrar la liquidacion, un campo llegò vacio o con 0");
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (ciEstado.Trim() == "") { validacion.agregarCamposObligatorios("ciEstado - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (ciTipoDocumento.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoDocumento - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (ciTipoEmision == 0) { validacion.agregarCamposObligatorios("ciTipoEmision - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (txClaveAcceso.Trim() == "") { validacion.agregarCamposObligatorios("txClaveAcceso - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (ciTipoIdentificacionProvedor.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoIdentificacionComprador - Liquidacion Cabecera ", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (txFechaEmision.Trim() == "") { validacion.agregarCamposObligatorios("txFechaEmision - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (txIdentificacionProvedor.Trim() == "") { validacion.agregarCamposObligatorios("txIdentificacionComprador - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (txMoneda.Trim() == "") { validacion.agregarCamposObligatorios("txMoneda - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (txRazonSocialProvedor.Trim() == "") { validacion.agregarCamposObligatorios("txRazonSocialComprador - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (qnTotalSinImpuestos == 0) { validacion.agregarCamposObligatorios("qnTotalSinImpuestos - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (qnImporteTotal == 0) { validacion.agregarCamposObligatorios("txPuntoEmision - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (CodDocReembolso == "") { validacion.agregarCamposObligatorios("qnPropina - Liquidacion Cabecera", ref descripcionRetorno); }
                    if (qnTotalDescuento == 0) { validacion.agregarCamposObligatorios("qnTotalDescuento - Liquidacion Cabecera", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }

            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
                ciLiquidacion = 0;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }

            #endregion 4: Validar Campos Ingresados
        }

        public void InsertarLiquidacionDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoPrincipal,
                                           string txCodigoAuxiliar, string txDescripcion, int qnCantidad, decimal qnPrecioUnitario, decimal qnDescuento,
                                           decimal qnPrecioTotalSinImpuesto, ref int codigoRetorno, ref string descripcionRetorno)
        {
            Validacion validacion = new Validacion();

            try
            {
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }
                if (ciCompania != 0 && txCodigoPrincipal != "" && txDescripcion != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != ""
                    && qnCantidad != 0)
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoPrincipal, "string", "txCodigoPrincipal", CodigoPrincipal, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoAuxiliar, "string", "txCodigoAuxiliar", CodigoAuxiliar, ref descripcionRetorno);
                    validacion.validarCampos(ref txDescripcion, "string", "txDescripcion", RazonSocialComprador, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardaLiquidacion.InsertarLiquidacionDetalle(
                                                                           ciCompania,
                                                                           txEstablecimiento.Trim(),
                                                                           txPuntoEmision.Trim(),
                                                                           txSecuencial.Trim(),
                                                                           txCodigoPrincipal.Trim(),
                                                                           txCodigoAuxiliar.Trim(),
                                                                           txDescripcion.Trim(),
                                                                           qnCantidad,
                                                                           qnPrecioUnitario,
                                                                           qnDescuento,
                                                                           qnPrecioTotalSinImpuesto,
                                                                           ref codigoRetorno,
                                                                           ref descripcionRetorno);
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura Detalle", ref descripcionRetorno); }
                    if (txCodigoPrincipal.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoPrincipal - Factura Detalle", ref descripcionRetorno); }
                    if (txDescripcion.Trim() == "") { validacion.agregarCamposObligatorios("txDescripcion - Factura Detalle", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Factura Detalle", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Factura Detalle", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Factura Detalle", ref descripcionRetorno); }
                    if (qnCantidad == 0) { validacion.agregarCamposObligatorios("qnCantidad - Factura Detalle", ref descripcionRetorno); }
                    if (qnDescuento == 0) { validacion.agregarCamposObligatorios("qnDescuento - Factura Detalle", ref descripcionRetorno); }
                    if (qnPrecioTotalSinImpuesto == 0) { validacion.agregarCamposObligatorios("qnPrecioTotalSinImpuesto - Factura Detalle", ref descripcionRetorno); }
                    if (qnPrecioUnitario == 0) { validacion.agregarCamposObligatorios("qnPrecioUnitario - Factura Detalle", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exepcion: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }

        public void InsertarLiquidacionDetalleImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoPrincipal,
                                                   string txCodigo, string txCodigoPorcentaje, string txTarifa, decimal qnBaseImponible, decimal qnValor,
                                                   ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }
                if (ciCompania != 0 && txCodigo != "" && txCodigoPorcentaje != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigo, "string", "txCodigo", Codigo, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoPorcentaje, "string", "txCodigoPorcentaje", CodigoPorcentaje, ref descripcionRetorno);


                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardaLiquidacion.InsertarLiquidacionDetalleImpuesto(ciCompania, 
                                                                                                           txEstablecimiento.Trim(),
                                                                                                           txPuntoEmision.Trim(),
                                                                                                           txSecuencial.Trim(),
                                                                                                           txCodigoPrincipal.Trim(),
                                                                                                           txCodigo.Trim(),
                                                                                                           txCodigoPorcentaje.Trim(),
                                                                                                           txTarifa.Trim(),
                                                                                                           qnBaseImponible,
                                                                                                           qnValor,
                                                                                                           ref codigoRetorno,
                                                                                                           ref descripcionRetorno);

                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura Detalle Impuesto", ref descripcionRetorno); }
                    if (txCodigo.Trim() == "") { validacion.agregarCamposObligatorios("txCodigo - Factura Detalle Impuesto", ref descripcionRetorno); }
                    if (txCodigoPorcentaje.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoPorcentaje - Factura Detalle Impuesto", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Factura Detalle Impuesto", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Factura Detalle Impuesto", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Factura Detalle Impuesto", ref descripcionRetorno); }
                    if (qnBaseImponible == 0) { validacion.agregarCamposObligatorios("qnBaseImponible - Factura Detalle Impuesto", ref descripcionRetorno); }
                    if (qnValor == 0) { validacion.agregarCamposObligatorios("qnValor - Factura Detalle Impuesto", ref descripcionRetorno); }
                    codigoRetorno = 1;
                    //dsRespuesta.Tables.Add(validacion.dtCamposObligatorios);
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exeption:  " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }

        public void InsertarTotalLiquidacion(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigo, string txCodigoPorcentaje,
                                           string txTarifa, decimal qnDescuentoAdicional, decimal qnBaseImponible, decimal qnValor, ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }
                if (ciCompania != 0 && txCodigo != "" && txCodigoPorcentaje != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigo, "string", "txCodigo", Codigo, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoPorcentaje, "string", "txCodigoPorcentaje", CodigoPorcentaje, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsTotalImpuesto = _guardaLiquidacion.InsertarLiquidacionTotalImpuesto(
                                                                                               ciCompania,
                                                                                               txEstablecimiento.Trim(),
                                                                                               txPuntoEmision.Trim(),
                                                                                               txSecuencial.Trim(),
                                                                                               txCodigo.Trim(),
                                                                                               txCodigoPorcentaje.Trim(),
                                                                                               txTarifa.Trim(),
                                                                                               qnDescuentoAdicional,
                                                                                               qnBaseImponible,
                                                                                               qnValor,
                                                                                               ref codigoRetorno,
                                                                                               ref descripcionRetorno);
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura total Impuesto", ref descripcionRetorno); }
                    if (txCodigo.Trim() == "") { validacion.agregarCamposObligatorios("txCodigo - Factura total Impuesto", ref descripcionRetorno); }
                    if (txCodigoPorcentaje.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoPorcentaje - Factura total Impuesto", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Factura total Impuesto", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Factura total Impuesto", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial  - Factura total Impuesto", ref descripcionRetorno); }
                    if (qnBaseImponible == 0) { validacion.agregarCamposObligatorios("qnBaseImponible - Factura total Impuesto", ref descripcionRetorno); }
                    if (qnValor == 0) { validacion.agregarCamposObligatorios("qnValor - Factura total Impuesto", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }

        public void InsertarLiquidacioDetalleFormaPago(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txFormaPago, string txPlazo,
                                                    string txUnidadTiempo, decimal qnTotal, ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }
                if (ciCompania != 0 && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txFormaPago, "string", "txFormaPago", FormaPago, ref descripcionRetorno);
                    validacion.validarCampos(ref txPlazo, "string", "txPlazo", Plazo, ref descripcionRetorno);
                    validacion.validarCampos(ref txUnidadTiempo, "string", "txUnidadTiempo", UnidadTiempo, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsDetalleFormaPago = _guardaLiquidacion.InsertarLiquidacionDetalleFormaPago(
                                                                                                        ciCompania,
                                                                                                        txEstablecimiento.Trim(),
                                                                                                        txPuntoEmision.Trim(),
                                                                                                        txSecuencial.Trim(),
                                                                                                        txFormaPago.Trim(),
                                                                                                        txPlazo.Trim(),
                                                                                                        txUnidadTiempo.Trim(),
                                                                                                        qnTotal,
                                                                                                        ref codigoRetorno,
                                                                                                        ref descripcionRetorno);

                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Liquidacion Forma de Pago", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Liquidacion Forma de Pago", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Liquidacion Forma de Pago", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial  - Liquidacion Forma de Pago", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }

        public void InsertarliquidacionDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoPrincipal,
                                                    string txNombre, string txValor, ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }
                if (ciCompania != 0 && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoPrincipal, "string", "txCodigoPrincipal", CodigoPrincipal, ref descripcionRetorno);
                    validacion.validarCampos(ref txNombre, "string", "txNombre", Nombre, ref descripcionRetorno);
                    validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);


                    if (validacion.contadorError == 0)
                    {
                        _guardaLiquidacion.InsertarLiquidacionDetalleAdicional(
                                                                                   ciCompania,
                                                                                   txEstablecimiento.Trim(),
                                                                                   txPuntoEmision.Trim(),
                                                                                   txSecuencial.Trim(),
                                                                                   txCodigoPrincipal.Trim(),
                                                                                   txNombre.Trim(),
                                                                                   txValor.Trim(),
                                                                                   ref codigoRetorno,
                                                                                   ref descripcionRetorno);
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura Detalle Adicional", ref descripcionRetorno); }
                    if (txCodigoPrincipal.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoPrincipal - Factura Detalle Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Factura Detalle Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Factura Detalle Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial  - Factura Detalle Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }

        public void InsertarLiquidacionInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txNombre,
                                                 string txValor, ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }

                if (ciCompania != 0 && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "" && txValor.Length != 0 && txNombre.Length != 0)
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txNombre, "string", "txNombre", Nombre, ref descripcionRetorno);
                    //validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsInfoAdicional = _guardaLiquidacion.InsertarLiquidacionInfoAdicional(
                                                                                                 ciCompania,
                                                                                                 txEstablecimiento.Trim(),
                                                                                                 txPuntoEmision.Trim(),
                                                                                                 txSecuencial.Trim(),
                                                                                                 txNombre.Trim(),
                                                                                                 txValor.Trim(),
                                                                                                 ref codigoRetorno,
                                                                                                 ref descripcionRetorno);

                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania  - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (txNombre.Trim() == "") { validacion.agregarCamposObligatorios("txNombre - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (txValor.Trim() == "") { validacion.agregarCamposObligatorios("txValor - Liquidacion Info Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }

        public void InsertarLiquidacionReembolso(int ciCompania, string txTipoIdentificacionProveedorReembolso, string txIdentificacionProveedorReembolso, string txCodPaisPagoProveedorReembolso,
                                                 string txTipoProveedorReembolso, string CodDocReembolso, string EstabDocReembolso, string PtoEmiDocReembolso, string SecuencialDocReembolso, string txFechaEmisionDocReembolso,
                                                 string numeroautorizacionDocReemb, string codigo, string codigoPorcentaje, string tarifa, decimal baseImponibleReembolso, decimal impuestoReembolso,
                                                  ref int codigoRetorno,  ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }

                if (ciCompania != 0 && EstabDocReembolso != "" && PtoEmiDocReembolso != "" && SecuencialDocReembolso != "" && tarifa.Length != 0 && baseImponibleReembolso != 0)
                {
                    validacion.validarCamposChar(ref EstabDocReembolso, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref PtoEmiDocReembolso, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref SecuencialDocReembolso, "txSecuencial", Secuencial, true, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsInfoAdicional = _guardaLiquidacion.InsertarLiquidacionReembolso( ciCompania,
                                                                                                   txTipoIdentificacionProveedorReembolso.Trim(),
                                                                                                   txIdentificacionProveedorReembolso.Trim(),
                                                                                                   txCodPaisPagoProveedorReembolso.Trim(),
                                                                                                   txTipoProveedorReembolso.Trim(),
                                                                                                   CodDocReembolso,
                                                                                                   EstabDocReembolso,
                                                                                                   PtoEmiDocReembolso,
                                                                                                   SecuencialDocReembolso,
                                                                                                   txFechaEmisionDocReembolso.Trim(),
                                                                                                   numeroautorizacionDocReemb,
                                                                                                   codigo,
                                                                                                   codigoPorcentaje,
                                                                                                   tarifa,
                                                                                                   baseImponibleReembolso,
                                                                                                   impuestoReembolso,
                                                                                                   ref  codigoRetorno,
                                                                                                   ref  descripcionRetorno);
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    if (ciCompania.Equals(0)) { validacion.agregarCamposObligatorios("ciCompania  - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (EstabDocReembolso.Equals("")) { validacion.agregarCamposObligatorios("EstabDocReembolso  - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (PtoEmiDocReembolso.Equals("")) { validacion.agregarCamposObligatorios("PtoEmiDocReembolso  - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (SecuencialDocReembolso.Equals("")) { validacion.agregarCamposObligatorios("SecuencialDocReembolso  - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (tarifa.Equals("")) { validacion.agregarCamposObligatorios("tarifa  - Liquidacion Info Adicional", ref descripcionRetorno); }
                    if (baseImponibleReembolso.Equals("")) { validacion.agregarCamposObligatorios("ciCompania  - Liquidacion Info Adicional", ref descripcionRetorno); }
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
        }
    }
}
