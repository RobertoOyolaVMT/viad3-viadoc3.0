using System;
using System.Configuration;
using System.Data;
using ViaDoc.AccesoDatos;
using ViaDoc.AccesoDatos.documento;
using ViaDoc.Utilitarios;

namespace ViaDoc.LogicaNegocios.documentos
{
    public class ProcesoFactura
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

        private FacturaAD _guardarFactura = new FacturaAD();

        public DataSet verificaExisteDocumento(string tipoDocumento, int compania, string establecimeinto,
                                         string puntoEmision, string secuencial, ref int codigoRetorno,
                                         ref string mensajeRetorno)
        {
            DocumentoAD objMetodos = new DocumentoAD();
            return objMetodos.verificaExisteDocumento(tipoDocumento, compania, establecimeinto, puntoEmision,
                                                      secuencial, ref codigoRetorno, ref mensajeRetorno);
        }




        public void InsertarFactura(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento, string txPuntoEmision,
                                    string txSecuencial, string txFechaEmision, string ciTipoIdentificacionComprador, string txGuiaRemision, string txRazonSocialComprador,
                                    string txIdentificacionComprador, decimal qnTotalSinImpuestos, decimal qnTotalDescuento, decimal qnPropina, decimal qnImporteTotal,
                                    string txMoneda, int ciContingenciaDet, string txEmail, string txNumeroAutorizacion, string txFechaHoraAutorizacion, string ciCodigoNumerico,
                                    string xmlDocumentoAutorizado, string ciEstado, string ciAmbiente, ref int codigoRetorno, ref string descripcionRetorno, ref int ciFactura)
        {

            #region 4: Validar Campos Ingresados
            Validacion validacion = new Validacion();

            try
            {
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }

                if (!ciCompania.Equals("") && !ciTipoDocumento.Equals("") && !ciTipoEmision.Equals(0) && !ciTipoIdentificacionComprador.Equals("") &&
                    !txSecuencial.Equals("") && !txEstablecimiento.Equals("") && !txFechaEmision.Equals("") && !txPuntoEmision.Equals("") && !txIdentificacionComprador.Equals("")
                    && !txMoneda.Equals("") && !qnImporteTotal.Equals(""))
                {
                    #region Validar longitud de Cabecera Factura
                    validacion.validarCampos(ref ciTipoDocumento, "string", "ciTipoDocumento", TipoDocumento, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txFechaEmision, "string", "txFechaEmision", Fecha, ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoIdentificacionComprador, "ciTipoIdentificacionComprador", TipoIdentificacionComprador, false, ref descripcionRetorno);
                    validacion.validarCampos(ref txGuiaRemision, "string", "txGuiaRemision", GuiaRemision, ref descripcionRetorno);
                    validacion.validarCampos(ref txIdentificacionComprador, "string", "txIdentificacionComprador", Identificacion, ref descripcionRetorno);
                    validacion.validarCampos(ref txMoneda, "string", "txMoneda", Moneda, ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciCodigoNumerico, "ciCodigoNumerico", 8, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txClaveAcceso, "string", "txClaveAcceso", ClaveAcceso, ref descripcionRetorno);
                    #endregion Validar longitud de Cabecera Factura

                    if (validacion.contadorError == 0)
                    {

                        DataSet dsResultadoFactura = _guardarFactura.InsertarFactura(ciCompania,
                                                                                    ciTipoEmision,
                                                                                    txClaveAcceso.Trim(),
                                                                                    ciTipoDocumento,
                                                                                    txEstablecimiento.Trim(),
                                                                                    txPuntoEmision.Trim(),
                                                                                    txSecuencial.Trim(),
                                                                                    txFechaEmision.Trim(),
                                                                                    ciTipoIdentificacionComprador,
                                                                                    txGuiaRemision.Trim(),
                                                                                    txRazonSocialComprador.Trim(),
                                                                                    txIdentificacionComprador.Trim(),
                                                                                    qnTotalSinImpuestos,
                                                                                    qnTotalDescuento,
                                                                                    qnPropina,
                                                                                    qnImporteTotal,
                                                                                    txMoneda.Trim(),
                                                                                    ciContingenciaDet,
                                                                                    txEmail.Trim(),
                                                                                    txNumeroAutorizacion.Trim(),
                                                                                    txFechaHoraAutorizacion.Trim(),
                                                                                    xmlDocumentoAutorizado.Trim(),
                                                                                    ciEstado,
                                                                                    ciAmbiente,
                                                                                    ref codigoRetorno,
                                                                                    ref descripcionRetorno,
                                                                                    ref ciFactura);
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Ocurrio un error al registrar la factura, un campo llegò vacio o con 0");
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura Cabecera", ref descripcionRetorno); }
                    if (ciEstado.Trim() == "") { validacion.agregarCamposObligatorios("ciEstado - Factura Cabecera", ref descripcionRetorno); }
                    if (ciTipoDocumento.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoDocumento - Factura Cabecera", ref descripcionRetorno); }
                    if (ciTipoEmision == 0) { validacion.agregarCamposObligatorios("ciTipoEmision - Factura Cabecera", ref descripcionRetorno); }
                    if (txClaveAcceso.Trim() == "") { validacion.agregarCamposObligatorios("txClaveAcceso - Factura Cabecera", ref descripcionRetorno); }
                    if (ciTipoIdentificacionComprador.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoIdentificacionComprador - Factura Cabecera ", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Factura Cabecera", ref descripcionRetorno); }
                    if (txFechaEmision.Trim() == "") { validacion.agregarCamposObligatorios("txFechaEmision - Factura Cabecera", ref descripcionRetorno); }
                    if (txIdentificacionComprador.Trim() == "") { validacion.agregarCamposObligatorios("txIdentificacionComprador - Factura Cabecera", ref descripcionRetorno); }
                    if (txMoneda.Trim() == "") { validacion.agregarCamposObligatorios("txMoneda - Factura Cabecera", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Factura Cabecera", ref descripcionRetorno); }
                    if (txRazonSocialComprador.Trim() == "") { validacion.agregarCamposObligatorios("txRazonSocialComprador - Factura Cabecera", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Factura Cabecera", ref descripcionRetorno); }
                    if (qnTotalSinImpuestos == 0) { validacion.agregarCamposObligatorios("qnTotalSinImpuestos - Factura Cabecera", ref descripcionRetorno); }
                    if (qnImporteTotal == 0) { validacion.agregarCamposObligatorios("txPuntoEmision - Factura Cabecera", ref descripcionRetorno); }
                    if (qnPropina == 0) { validacion.agregarCamposObligatorios("qnPropina - Factura Cabecera", ref descripcionRetorno); }
                    if (qnTotalDescuento == 0) { validacion.agregarCamposObligatorios("qnTotalDescuento - Factura Cabecera", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }

            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
                ciFactura = 0;
            }

            #endregion 4: Validar Campos Ingresados
        }


        public void InsertarFacturaDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoPrincipal,
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
                    //validacion.validarCampos(ref txDescripcion, "string", "txDescripcion", RazonSocialComprador, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarFactura.InsertarFacturaDetalle(
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
            }
        }


        public void InsertarFacturaDetalleImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoPrincipal,
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
                        DataSet dsResultadoFactura = _guardarFactura.InsertarFacturaDetalleImpuesto(ciCompania, txEstablecimiento.Trim(), txPuntoEmision.Trim(), txSecuencial.Trim(),
                                                    txCodigoPrincipal.Trim(), txCodigo.Trim(), txCodigoPorcentaje.Trim(), txTarifa.Trim(), qnBaseImponible, qnValor, ref codigoRetorno,
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
            }
        }


        public void InsertarTotalImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigo, string txCodigoPorcentaje,
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
                        DataSet dsTotalImpuesto = _guardarFactura.InsertarFacturaTotalImpuesto(
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
            }
        }


        public void InsertarFacturaDetalleFormaPago(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txFormaPago, string txPlazo,
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
                        DataSet dsDetalleFormaPago = _guardarFactura.InsertarFacturaDetalleFormaPago(
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
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura Forma de Pago", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Factura Forma de Pago", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Factura Forma de Pago", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial  - Factura Forma de Pago", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
        }


        public void InsertarFacturaDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoPrincipal,
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
                        _guardarFactura.InsertarFacturaDetalleAdicional(
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
            }
        }

        public void InsertarFacturaDetalleReembolso(int ciCompania, string ciTipoDocumento, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
          string txNumDocumento, string txFechaEmision, string txIdProveedor, string txClaveAcceso, string subTotNoIva, string subTotIvaCero, string subTotIva,
          string subTotExcentoIva, string impIva, string impIce, string impIRBPNR, string valTotal, string detalle, string valorBase, string codigoImp, string codigoPorcentajeImp,
          string tarifaImp, string tipoIdProveedor, string codPaisPagoProveedor, string tipoProveedor, ref int codigoRetorno, ref string descripcionRetorno)
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
                    validacion.validarCampos(ref ciTipoDocumento, "string", "ciTipoDocumento", TipoDocumento, ref descripcionRetorno);
                    validacion.validarCampos(ref txNumDocumento, "string", "txNumDocumento", Nombre, ref descripcionRetorno);
                    validacion.validarCampos(ref txFechaEmision, "string", "txFechaEmision", Fecha, ref descripcionRetorno);
                    validacion.validarCampos(ref txIdProveedor, "string", "txIdProveedor", Identificacion, ref descripcionRetorno);
                    validacion.validarCampos(ref txClaveAcceso, "string", "txClaveAcceso", ClaveAcceso, ref descripcionRetorno);
                    validacion.validarCampos(ref subTotNoIva, "string", "subTotNoIva", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref subTotIvaCero, "string", "subTotIvaCero", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref subTotIva, "string", "subTotIva", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref subTotExcentoIva, "string", "subTotExcentoIva", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref impIva, "string", "impIva", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref impIce, "string", "impIce", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref impIRBPNR, "string", "impIRBPNR", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref valTotal, "string", "valTotal", valor, ref descripcionRetorno);
                    validacion.validarCampos(ref valorBase, "string", "valorBase", valor, ref descripcionRetorno);


                    if (validacion.contadorError == 0)
                    {
                        _guardarFactura.InsertarFacturaDetalleReembolso(
                                                                                   ciCompania,
                                                                                   ciTipoDocumento,
                                                                                   txEstablecimiento.Trim(),
                                                                                   txPuntoEmision.Trim(),
                                                                                   txSecuencial.Trim(),
                                                                                   txNumDocumento.Trim(),
                                                                                   txFechaEmision.Trim(),
                                                                                   txIdProveedor.Trim(),
                                                                                   txClaveAcceso,
                                                                                   subTotNoIva.Trim(),
                                                                                   subTotIvaCero.Trim(),
                                                                                   subTotIva.Trim(),
                                                                                   subTotExcentoIva.Trim(),
                                                                                   impIva,
                                                                                   impIce,
                                                                                   impIRBPNR,
                                                                                   valTotal,
                                                                                   detalle,
                                                                                   valorBase,
                                                                                   codigoImp,
                                                                                   codigoPorcentajeImp,
                                                                                   tarifaImp,
                                                                                   tipoIdProveedor,
                                                                                   codPaisPagoProveedor,
                                                                                   tipoProveedor,
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
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura Detalle Reembolso", ref descripcionRetorno); }
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Factura Detalle Adicional", ref descripcionRetorno); }
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
            }
        }


        public void InsertarFacturaInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txNombre,
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
                        DataSet dsInfoAdicional = _guardarFactura.InsertarFacturaInfoAdicional(
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
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania  - Factura Info Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Factura Info Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Factura Info Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Factura Info Adicional", ref descripcionRetorno); }
                    if (txNombre.Trim() == "") { validacion.agregarCamposObligatorios("txNombre - Factura Info Adicional", ref descripcionRetorno); }
                    if (txValor.Trim() == "") { validacion.agregarCamposObligatorios("txValor - Factura Info Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
        }
    }
}
