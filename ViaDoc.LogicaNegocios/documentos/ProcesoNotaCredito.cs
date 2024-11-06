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
    public class ProcesoNotaCredito
    {
        private int ClaveAcceso = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CLAVE_ACCESO"));
        private int TipoDocumento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_TIPO_DOCUMENTO"));
        private int Establecimiento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_ESTABLECIMIENTO"));
        private int PuntoEmision = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_PUNTO_EMISION"));
        private int Secuencial = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_SECUENCIAL"));
        private int Fecha = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_FECHA_EMISION"));
        private int TipoIdentificacionSujeto = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_TIPO_IDENTIFICACION_SUJETO"));
        private int RazonSocialSujetoRetenido = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_RAZON_SOCIAL_SUJETO_RE"));
        private int IdentificacionSujetoRetenido = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_IDENTIFICACION_SUJETO_RETENIDO"));
        private int periodoFiscal = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_PERIODO_FISCAL"));
        private int NumeroAutorizacion = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NUMERO_AUTORIZACION"));
        private int FechaHoraAutorizacion = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_FECHA_HORA_AUTORIZACION"));
        private int codigoRetencion = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_RETENCION"));
        private int CodigoDocumentoSustento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_DOCUMENTO_SUSTENTO"));
        private int NumeroDocumentoSustento = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NUM_DOCUMENTO_SUSTENTO"));
        private int Nombre = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NOMBRE"));
        private int valor = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_VALOR"));
        private int Rise = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_RISE"));
        private int NUmeroDocumentoModificado = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_NUM_DOCUMENTO_MODIFICADO"));
        private int Moneda = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_MONEDA"));
        private int CodigoInterno = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_INTERNO"));
        private int CodigoAdicional = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_ADICIONAL"));
        private int Codigo = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO"));
        private int CodigôPorcentaje = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_CODIGO_PORCENTAJE"));
        private int Tarifa = int.Parse(ConfigurationManager.AppSettings.Get("LONGITUD_TARIFA"));
        private int TipoEmision = int.Parse(ConfigurationManager.AppSettings.Get("TIPO_EMISION"));

        private NotaCreditoAD _guardarNotaCredito = new NotaCreditoAD();

        public DataSet verificaExisteDocumento(string tipoDocumento, int compania, string establecimeinto,
                                         string puntoEmision, string secuencial, ref int codigoRetorno,
                                         ref string mensajeRetorno)
        {
            DocumentoAD objMetodos = new DocumentoAD();
            return objMetodos.verificaExisteDocumento(tipoDocumento, compania, establecimeinto, puntoEmision,
                                                      secuencial, ref codigoRetorno, ref mensajeRetorno);
        }



        public void InsertarNotaCredito(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento, 
                                        string txPuntoEmision, string txSecuencial, string txFechaEmision, string ciTipoIdentificacionComprador,
                                        string txRazonSocialComprador, string txIdentificacionComprador, string txRise, string ciTipoDocumentoModificado, 
                                        string txNumeroDocumentoModificado, string txFechaEmisionDocumentoModificado, decimal qnTotalSinImpuestos,
                                        decimal qnValorModificacion, string txMoneda, string txMotivo, int ciContingenciaDet, string txEmail, 
                                        string txNumeroAutorizacion, string txFechaHoraAutorizacion, string xmlDocumentoAutorizado, string ciEstado, 
                                        string ciAmbiente, string ciCodigoNumerico, string ruc, ref int codigoRetorno, ref string descripcionRetorno,
                                        ref int ciNotaCredito)
        {
            Validacion validacion = new Validacion();         
            try
            {
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }

                if (ciCompania != 0 && ciTipoDocumento != "" && ciTipoDocumentoModificado != "" && ciTipoEmision != 0 && 
                    ciTipoIdentificacionComprador != "" && txClaveAcceso != "" && txEstablecimiento != "" && txFechaEmision != "" && 
                    txFechaEmisionDocumentoModificado != "" && txIdentificacionComprador != "" && txMoneda != "" && txMotivo != "" && 
                    txNumeroDocumentoModificado != "" && txPuntoEmision != "" && txRazonSocialComprador != "" && txSecuencial != "" && 
                    qnTotalSinImpuestos != 0 && qnValorModificacion != 0)
                {
                    validacion.validarCampos(ref txClaveAcceso, "string", "txClaveAcceso", ClaveAcceso,ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoDocumento, "ciTipoDocumento", TipoDocumento, true, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaEmision, "txFechaEmision", ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoIdentificacionComprador, "ciTipoIdentificacionComprador", TipoIdentificacionSujeto, false, ref descripcionRetorno);
                    //validacion.validarCampos(ref txRazonSocialComprador, "string", "txRazonSocialComprador", RazonSocialSujetoRetenido, ref descripcionRetorno);
                    validacion.validarCampos(ref txIdentificacionComprador, "string", "txIdentificacionComprador", IdentificacionSujetoRetenido, ref descripcionRetorno);
                    validacion.validarCampos(ref txRise, "string", "txRise", Rise, ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoDocumentoModificado, "ciTipoDocumentoModificado", TipoDocumento, false, ref descripcionRetorno);
                    validacion.validarCampos(ref txNumeroDocumentoModificado, "string", "txNumeroDocumentoModificado", NUmeroDocumentoModificado, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaEmisionDocumentoModificado, "txFechaEmisionDocumentoModificado", ref descripcionRetorno);
                    validacion.validarCampos(ref txMoneda, "string", "txMoneda", Moneda, ref descripcionRetorno);
                    validacion.validarCampos(ref txMotivo, "string", "txMotivo", RazonSocialSujetoRetenido, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaHoraAutorizacion, "txFechaHoraAutorizacion", ref descripcionRetorno);
                    Utilitarios.logs.LogsFactura.LogsInicioFin("Log 11 ");
                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarNotaCredito.InsertarNotaCredito(ciCompania,
                                                                                             ciTipoEmision,
                                                                                             txClaveAcceso.Trim(),
                                                                                             ciTipoDocumento,
                                                                                             txEstablecimiento.Trim(),
                                                                                             txPuntoEmision.Trim(),
                                                                                             txSecuencial.Trim(),
                                                                                             txFechaEmision.Trim(),
                                                                                             ciTipoIdentificacionComprador,
                                                                                             txRazonSocialComprador.Trim(),
                                                                                             txIdentificacionComprador.Trim(),
                                                                                             txRise.Trim(),
                                                                                             ciTipoDocumentoModificado,
                                                                                             txNumeroDocumentoModificado.Trim(),
                                                                                             txFechaEmisionDocumentoModificado.Trim(),
                                                                                             qnTotalSinImpuestos,
                                                                                             qnValorModificacion,
                                                                                             txMoneda.Trim(),
                                                                                             txMotivo.Trim(),
                                                                                             ciContingenciaDet,
                                                                                             txEmail.Trim(),
                                                                                             txNumeroAutorizacion.Trim(),
                                                                                             txFechaHoraAutorizacion.Trim(),
                                                                                             xmlDocumentoAutorizado,
                                                                                             ciEstado,
                                                                                             ciAmbiente,
                                                                                             ref codigoRetorno,
                                                                                             ref descripcionRetorno,
                                                                                             ref ciNotaCredito);
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Note Credito Cabecera", ref descripcionRetorno); }
                    if (ciTipoDocumento.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoDocumento - Note Credito Cabecera", ref descripcionRetorno); }
                    if (ciTipoEmision == 0) { validacion.agregarCamposObligatorios("ciTipoEmision - Note Credito Cabecera", ref descripcionRetorno); }
                    if (ciTipoIdentificacionComprador.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoIdentificacionComprador - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txClaveAcceso.Trim() == "") { validacion.agregarCamposObligatorios("txClaveAcceso - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txFechaEmision.Trim() == "") { validacion.agregarCamposObligatorios("txFechaEmision - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txFechaEmisionDocumentoModificado.Trim() == "") { validacion.agregarCamposObligatorios("txFechaEmisionDocumentoModificado  - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txIdentificacionComprador.Trim() == "") { validacion.agregarCamposObligatorios("txIdentificacionComprador - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txMoneda.Trim() == "") { validacion.agregarCamposObligatorios("txMoneda - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txMotivo.Trim() == "") { validacion.agregarCamposObligatorios("txMotivo - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txNumeroDocumentoModificado.Trim() == "") { validacion.agregarCamposObligatorios("txNumeroDocumentoModificado - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txRazonSocialComprador.Trim() == "") { validacion.agregarCamposObligatorios("txRazonSocialComprador - Note Credito Cabecera", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Note Credito Cabecera", ref descripcionRetorno); }
                    if (qnTotalSinImpuestos == 0) { validacion.agregarCamposObligatorios("qnTotalSinImpuestos - Note Credito Cabecera", ref descripcionRetorno); }
                    if (qnValorModificacion == 0) { validacion.agregarCamposObligatorios("qnValorModificacion - Note Credito Cabecera", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarNotaCreditoDEtalle(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoInterno, 
                                               string txCodigoAdicional, string txDescripcion, int qnCantidad, decimal qnPrecioUnitario, decimal qnDescuento, 
                                               decimal qnPrecioTotalSinImpuesto, ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }

                if (ciCompania != 0 && txCodigoInterno != "" && txDescripcion != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "" )
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoInterno, "string", "txCodigoInterno", CodigoInterno, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoAdicional, "string", "txCodigoAdicional", CodigoAdicional, ref descripcionRetorno);
                    validacion.validarCampos(ref txDescripcion, "string", "txDescripcion", RazonSocialSujetoRetenido, ref descripcionRetorno);


                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarNotaCredito.InsertarNotaCreditoDEtalle(ciCompania,
                                                                                                    txEstablecimiento.Trim(),
                                                                                                    txPuntoEmision.Trim(),
                                                                                                    txSecuencial.Trim(),
                                                                                                    txCodigoInterno.Trim(),
                                                                                                    txCodigoAdicional.Trim(),
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Note Credito Detalle", ref descripcionRetorno); }
                    if (txCodigoInterno.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoInterno - Note Credito Detalle", ref descripcionRetorno); }
                    if (txDescripcion.Trim() == "") { validacion.agregarCamposObligatorios("txDescripcion - Note Credito Detalle", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Note Credito Detalle", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Note Credito Detalle", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Note Credito Detalle", ref descripcionRetorno); }
                    if (qnCantidad == 0) { validacion.agregarCamposObligatorios("qnCantidad - Note Credito Detalle", ref descripcionRetorno); }
                    if (qnPrecioTotalSinImpuesto == 0) { validacion.agregarCamposObligatorios("qnPrecioTotalSinImpuesto - Note Credito Detalle ", ref descripcionRetorno); }
                    if (qnPrecioUnitario == 0) { validacion.agregarCamposObligatorios("qnPrecioUnitario - Note Credito Detalle", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarNotaCreditoDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                          string txCodigoInterno, string txNombre, string txValor, ref int codigoRetorno, 
                                                          ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }

                if (ciCompania != 0 && txCodigoInterno != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoInterno, "string", "txCodigoInterno", CodigoInterno, ref descripcionRetorno);
                    validacion.validarCampos(ref txNombre, "string", "txNombre", Nombre, ref descripcionRetorno);
                    //validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarNotaCredito.InsertarNotaCreditoDetalleAdicional(ciCompania,
                                                                                                             txEstablecimiento.Trim(),
                                                                                                             txPuntoEmision.Trim(),
                                                                                                             txSecuencial.Trim(),
                                                                                                             txCodigoInterno.Trim(),
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
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Note Credito Detalle Adicional", ref descripcionRetorno); }
                    if (txCodigoInterno.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoInterno - Note Credito Detalle Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Note Credito Detalle Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Note Credito Detalle Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Note Credito Detalle Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarNotaCreditoDetalleImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigoInterno, 
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

                if (ciCompania != 0 && txCodigo != "" && txCodigoInterno != "" && txCodigoPorcentaje != "" && txEstablecimiento != "" && txPuntoEmision != "" && 
                    txSecuencial != "" && txTarifa != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoInterno, "string", "txCodigoInterno", CodigoInterno, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigo, "string", "txCodigo", Codigo, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoPorcentaje, "string", "txCodigoPorcentaje", CodigôPorcentaje, ref descripcionRetorno);
                    validacion.validarCampos(ref txTarifa, "string", "txTarifa", Tarifa, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarNotaCredito.InsertarNotaCreditoDetalleImpuesto(ciCompania,
                                                                                                            txEstablecimiento.Trim(),
                                                                                                            txPuntoEmision.Trim(),
                                                                                                            txSecuencial.Trim(),
                                                                                                            txCodigoInterno.Trim(),
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
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (txCodigoInterno.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoInterno - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (txCodigo.Trim() == "") { validacion.agregarCamposObligatorios("txCodigo - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (txCodigoPorcentaje.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoPorcentaje - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (txTarifa.Trim() == "") { validacion.agregarCamposObligatorios("txTarifa - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (qnBaseImponible == 0) { validacion.agregarCamposObligatorios("qnBaseImponible - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (qnValor == 0) { validacion.agregarCamposObligatorios("qnValor - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Note Credito Detalle Impuesto", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }

            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarNotaCreditoInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txNombre, 
                                                     string txValor, ref int codigoRetorno, ref string descripcionRetorno)
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
                    validacion.validarCampos(ref txNombre, "string", "txNombre", Nombre, ref descripcionRetorno);
                    //validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarNotaCredito.InsertarNotaCreditoInfoAdicional(ciCompania,
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
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Note Credito Info Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Note Credito Info Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Note Credito Info Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Note Credito Info Adicional", ref descripcionRetorno); }
                    if (txNombre.Trim() == "") { validacion.agregarCamposObligatorios("txNombre - Guia Remision Info Adicional", ref descripcionRetorno); }
                    if (txValor.Trim() == "") { validacion.agregarCamposObligatorios("txValor - Guia Remision Info Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }

        public void InsertarNotaCreditoTotalImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigo, 
                                                     string txCodigoPorcentaje, decimal qnBaseImponible, decimal qnValor, ref int codigoRetorno, 
                                                     ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0 )
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings.Get("COMPANIA"));
                }
                if (ciCompania != 0 && txCodigo != "" && txCodigoPorcentaje != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref  descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigo, "string", "txCodigo", Codigo, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarNotaCredito.InsertarNotaCreditoTotalImpuesto(
                                                                                                           ciCompania,
                                                                                                           txEstablecimiento.Trim(),
                                                                                                           txPuntoEmision.Trim(),
                                                                                                           txSecuencial.Trim(),
                                                                                                           txCodigo.Trim(),
                                                                                                           txCodigoPorcentaje.Trim(),
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
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Nota de Credito Total Impuestos", ref descripcionRetorno); }
                    if (txCodigo.Trim() == "") { validacion.agregarCamposObligatorios("txCodigo - Nota de Credito Total Impuestos", ref descripcionRetorno); }
                    if (txCodigoPorcentaje.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoPorcentaje - Nota de Credito Total Impuestos", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Nota de Credito Total Impuestos", ref descripcionRetorno); }
                    if (qnBaseImponible == 0) { validacion.agregarCamposObligatorios("qnBaseImponible - Nota de Credito Total Impuestos", ref descripcionRetorno); }
                    if (qnValor == 0) { validacion.agregarCamposObligatorios("qnValor - Nota de Credito Total Impuestos", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Nota de Credito Total Impuestos", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }
    }
}
