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
    public class ProcesoCompRetencion
    {

        #region Obtencion de Longitud de campos 
        private int ClaveAcceso = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CLAVE_ACCESO"]);
        private int TipoDocumento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_DOCUMENTO"]);
        private int Establecimiento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_ESTABLECIMIENTO"]);
        private int PuntoEmision = int.Parse(ConfigurationManager.AppSettings["LONGITUD_PUNTO_EMISION"]);
        private int Secuencial = int.Parse(ConfigurationManager.AppSettings["LONGITUD_SECUENCIAL"]);
        private int Fecha = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_EMISION"]);
        private int TipoIdentificacionSujeto = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_IDENTIFICACION_SUJETO"]);
        private int RazonSocialSujetoRetenido = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RAZON_SOCIAL_SUJETO_RE"]);
        private int IdentificacionSujetoRetenido = int.Parse(ConfigurationManager.AppSettings["LONGITUD_IDENTIFICACION_SUJETO_RETENIDO"]);
        private int periodoFiscal = int.Parse(ConfigurationManager.AppSettings["LONGITUD_PERIODO_FISCAL"]);
        private int NumeroAutorizacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUMERO_AUTORIZACION"]);
        private int FechaHoraAutorizacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_HORA_AUTORIZACION"]);
        private int codigoRetencion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_RETENCION"]);
        private int CodigoDocumentoSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_DOCUMENTO_SUSTENTO"]);
        private int NumeroDocumentoSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUM_DOCUMENTO_SUSTENTO"]);
        private int Nombre = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NOMBRE"]);
        private int valor = int.Parse(ConfigurationManager.AppSettings["LONGITUD_VALOR"]);
        private int TipoEmision = int.Parse(ConfigurationManager.AppSettings["TIPO_EMISION"]);
        private int FechaEmision = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_EMISION"]); 
        private int DocSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_COD_SUSTENTO"]);
        private int CodDocSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_COD_DOC_SUSTENTO"]); 
        private int FechaReistroContable = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_REGISTRO_CONTABLE"]);
        //private int TipoEmision = int.Parse(ConfigurationManager.AppSettings["TIPO_EMISION"]);
        //private int TipoEmision = int.Parse(ConfigurationManager.AppSettings["TIPO_EMISION"]);
        //private int TipoEmision = int.Parse(ConfigurationManager.AppSettings["TIPO_EMISION"]);


        private CompRetencionAD _guardarCompRetencion = new CompRetencionAD();

        #endregion Obtencion de Longitud de campos 

        public DataSet verificaExisteDocumento(string tipoDocumento, int compania, string establecimeinto,
                                         string puntoEmision, string secuencial, ref int codigoRetorno,
                                         ref string mensajeRetorno)
        {
            DocumentoAD objMetodos = new DocumentoAD();
            return objMetodos.verificaExisteDocumento(tipoDocumento, compania, establecimeinto, puntoEmision,
                                                      secuencial, ref codigoRetorno, ref mensajeRetorno);
        }

        public void InsertarCompRetencion(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento,
                                              string txPuntoEmision, string txSecuencial, string txFechaEmision, string ciTipoIdentificacionSujetoRetenido,
                                              string txRazonSocialSujetoRetenido, string txIdentificacionSujetoRetenido, string txPeriodoFiscal,
                                              int ciContingenciaDet, string txEmail, string txNumeroAutorizacion, string txFechaHoraAutorizacion,
                                              string xmlDocumentoAutorizado, string ciEstado, string ciAmbiente, string ciCodigoNumerico, 
                                              ref int codigoRetorno, ref string descripcionRetorno, ref int ciCompRetencion)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && ciTipoDocumento != "" && ciTipoEmision != 0 && ciTipoIdentificacionSujetoRetenido != ""
                    && txClaveAcceso != "" && txEstablecimiento != "" && txFechaEmision != "" && txIdentificacionSujetoRetenido != ""
                    && txPeriodoFiscal != "" && txPuntoEmision != "" && txRazonSocialSujetoRetenido != "" && txSecuencial != "")
                {
                    validacion.validarCampos(ref ciTipoDocumento, "string", "ciTipoDocumento", TipoDocumento, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txFechaEmision, "string", "txFechaEmision", Fecha, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaEmision, "txFechaEmision", ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoIdentificacionSujetoRetenido, "ciTipoIdentificacionSujetoRetenido", TipoIdentificacionSujeto, false, ref descripcionRetorno);
                    //validacion.validarCampos(ref txRazonSocialSujetoRetenido, "string", "txRazonSocialSujetoRetenido", RazonSocialSujetoRetenido, ref descripcionRetorno);
                    validacion.validarCampos(ref txIdentificacionSujetoRetenido, "string", "txIdentificacionSujetoRetenido", IdentificacionSujetoRetenido, ref descripcionRetorno);
                    validacion.validarCampos(ref txPeriodoFiscal, "string", "txPeriodoFiscal", periodoFiscal, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaHoraAutorizacion, "txFechaHoraAutorizacion", ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciCodigoNumerico, "ciCodigoNumerico", 8, true, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarCompRetencion.InsertarCompRetencion(
                                                                       ciCompania,
                                                                       ciTipoEmision,
                                                                       txClaveAcceso.Trim(),
                                                                       ciTipoDocumento,
                                                                       txEstablecimiento.Trim(),
                                                                       txPuntoEmision.Trim(),
                                                                       txSecuencial.Trim(),
                                                                       txFechaEmision.Trim(),
                                                                       ciTipoIdentificacionSujetoRetenido,
                                                                       txRazonSocialSujetoRetenido.Trim(),
                                                                       txIdentificacionSujetoRetenido.Trim(),
                                                                       txPeriodoFiscal.Trim(),
                                                                       ciContingenciaDet,
                                                                       txEmail.Trim(),
                                                                       txNumeroAutorizacion.Trim(),
                                                                       txFechaHoraAutorizacion.Trim(),
                                                                       xmlDocumentoAutorizado,
                                                                       ciEstado,
                                                                       ref codigoRetorno,
                                                                       ref descripcionRetorno,
                                                                       ref ciCompRetencion);

                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Ocurrio un error al registrar la liquidacion, un campo llegò vacio o con 0");
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (ciTipoDocumento.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoDocumento - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (ciTipoEmision == 0) { validacion.agregarCamposObligatorios("ciTipoEmision - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txClaveAcceso.Trim() == "") { validacion.agregarCamposObligatorios("txClaveAcceso - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (ciTipoIdentificacionSujetoRetenido.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoIdentificacionSujetoRetenido - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txFechaEmision.Trim() == "") { validacion.agregarCamposObligatorios("txFechaEmision - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txIdentificacionSujetoRetenido.Trim() == "") { validacion.agregarCamposObligatorios("txIdentificacionSujetoRetenido - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txPeriodoFiscal.Trim() == "") { validacion.agregarCamposObligatorios("txPeriodoFiscal - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txRazonSocialSujetoRetenido.Trim() == "") { validacion.agregarCamposObligatorios("txRazonSocialSujetoRetenido - ComprRetencion Cabecera", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - ComprRetencion Cabecera", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }

        public void InsertarCompRetencionDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
                                                   int ciImpuesto, string txCodRetencion, decimal qnBaseImponible, decimal qnPorcentajeRetener,
                                                   decimal qnValorRetenido, string txCodDocSustento, string txNumDocSustento, string txFechaEmisionDocSustento,
                                                   ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txCodDocSustento != "" && txCodRetencion != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txCodRetencion, "string", "txCodRetencion", codigoRetencion, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodDocSustento, "string", "txCodDocSustento", CodigoDocumentoSustento, ref descripcionRetorno);
                    validacion.validarCampos(ref txNumDocSustento, "string", "txNumDocSustento", NumeroDocumentoSustento, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaEmisionDocSustento, "txFechaEmisionDocSustento", ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarCompRetencion.InsertarCompRetencionDetalle(
                                                                                   ciCompania,
                                                                                   txEstablecimiento.Trim(),
                                                                                   txPuntoEmision.Trim(),
                                                                                   txSecuencial.Trim(),
                                                                                   ciImpuesto,
                                                                                   txCodRetencion.Trim(),
                                                                                   qnBaseImponible,
                                                                                   qnPorcentajeRetener,
                                                                                   qnValorRetenido,
                                                                                   txCodDocSustento.Trim(),
                                                                                   txNumDocSustento.Trim(),
                                                                                   txFechaEmisionDocSustento.Trim(),
                                                                                   ref codigoRetorno,
                                                                                   ref descripcionRetorno
                                                                                   );
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (ciImpuesto == 0) { validacion.agregarCamposObligatorios("ciImpuesto - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (txCodDocSustento.Trim() == "") { validacion.agregarCamposObligatorios("txCodDocSustento - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (txCodRetencion.Trim() == "") { validacion.agregarCamposObligatorios("txCodRetencion - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (qnBaseImponible == 0) { validacion.agregarCamposObligatorios("qnBaseImponible - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (qnPorcentajeRetener == 0) { validacion.agregarCamposObligatorios("qnPorcentajeRetener - ComprRetencion Detalle", ref descripcionRetorno); }
                    if (qnValorRetenido ==0) { validacion.agregarCamposObligatorios("qnValorRetenido - ComprRetencion Detalle", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }

        public void InsertarComprobanteRetencionInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, 
                                                              string txSecuencial, string txNombre, string txValor,
                                                              ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txEstablecimiento.Trim() != "" && txPuntoEmision.Trim() != "" && txSecuencial.Trim() != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txNombre, "string", "txNombre", Nombre, ref descripcionRetorno);
                    //validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarCompRetencion.InsertarComprobanteRetencionInfoAdicional(
                                                                                               Convert.ToInt32(ciCompania),
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

                    if (ciCompania== 0) { validacion.agregarCamposObligatorios("ciCompania - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    if (txNombre.Trim() == "") { validacion.agregarCamposObligatorios("txNombre - CompRetencion Info Adicional", ref descripcionRetorno); }
                    if (txValor.Trim() == "") { validacion.agregarCamposObligatorios("txValor - CompRetencion Info Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }

            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }
        public void InsertarComprobanteRetencionDocSustento(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                                      string txSecuencial, string txFechaEmision, string txCodSustento, string txCodDocSustento,
                                                      string txFechaRegistroContable, string txcodImpuestoDocSustento, string txcodigoPorcentaje,
                                                      string txTotalSinImpuesto, string txImporteTotal,
                                                      string txBaseImponible, string txTarifa, string txValorImpuesto,
                                                      ref int codigoRetorno, ref string descripcionRetorno) 
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txEstablecimiento.Trim() != "" && txPuntoEmision.Trim() != "" && txSecuencial.Trim() != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txFechaEmision, "string", "txFechaEmision", FechaEmision, ref descripcionRetorno);
                    //validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarCompRetencion.InsertarComprobanteRetencionDocSustento(
                                                                                               Convert.ToInt32(ciCompania),
                                                                                               txEstablecimiento.Trim(),
                                                                                               txPuntoEmision.Trim(),
                                                                                               txSecuencial.Trim(),
                                                                                               txFechaEmision.Trim(),
                                                                                               txCodSustento.Trim(),
                                                                                               txCodDocSustento.Trim(),
                                                                                               txFechaRegistroContable.Trim(),
                                                                                               txcodImpuestoDocSustento.Trim(),
                                                                                               txcodigoPorcentaje.Trim(),
                                                                                               txTotalSinImpuesto.Trim(),
                                                                                               txImporteTotal.Trim(),
                                                                                               txBaseImponible.Trim(),
                                                                                               txTarifa.Trim(),
                                                                                               txValorImpuesto.Trim(),
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

                    //if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    //if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    //if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    //if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - ComprRetencion Info Adicional", ref descripcionRetorno); }
                    //if (txNombre.Trim() == "") { validacion.agregarCamposObligatorios("txNombre - CompRetencion Info Adicional", ref descripcionRetorno); }
                    //if (txValor.Trim() == "") { validacion.agregarCamposObligatorios("txValor - CompRetencion Info Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }

            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }
        public void InsertarComprobanteRetencionFormaPago(int ciCompania, string txEstablecimiento, string txPuntoEmision,
                                              string txSecuencial, string txFormaPago, string qnTotal,  
                                              ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txEstablecimiento.Trim() != "" && txPuntoEmision.Trim() != "" && txSecuencial.Trim() != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txFormaPago, "string", "txFechaEmision", FechaEmision, ref descripcionRetorno);
                    //validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarCompRetencion.InsertarComprobanteRetencionFormaPago(
                                                                                               Convert.ToInt32(ciCompania),
                                                                                               txEstablecimiento.Trim(),
                                                                                               txPuntoEmision.Trim(),
                                                                                               txSecuencial.Trim(),
                                                                                               txFormaPago.Trim(),
                                                                                               qnTotal.Trim(),
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - ComprRetencion forma Pago", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - ComprRetencion forma Pago", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - ComprRetencion forma Pago", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - ComprRetencion forma Pago", ref descripcionRetorno); }
                    if (txFormaPago.Trim() == "") { validacion.agregarCamposObligatorios("txFormaPago - CompRetencion forma Pago", ref descripcionRetorno); }
                    if (qnTotal.Trim() == "") { validacion.agregarCamposObligatorios("qnTotal - CompRetencion forma Pago", ref descripcionRetorno); }
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
