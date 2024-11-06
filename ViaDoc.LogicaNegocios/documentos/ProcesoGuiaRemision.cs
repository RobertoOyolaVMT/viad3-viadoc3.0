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
    public class ProcesoGuiaRemision
    {
        private int ClaveAcceso = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CLAVE_ACCESO"]);
        private int TipoDocumento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_DOCUMENTO"]);
        private int Establecimiento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_ESTABLECIMIENTO"]);
        private int PuntoEmision = int.Parse(ConfigurationManager.AppSettings["LONGITUD_PUNTO_EMISION"]);
        private int Secuencial = int.Parse(ConfigurationManager.AppSettings["LONGITUD_SECUENCIAL"]);
        private int Fecha = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_EMISION"]);
        private int TipoIdentificacionSujeto = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_IDENTIFICACION_SUJETO"]);

        private int Identificacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_IDENTIFICACION_SUJETO_RETENIDO"]);

        private int NumeroAutorizacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUMERO_AUTORIZACION"]);
        private int FechaHoraAutorizacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_HORA_AUTORIZACION"]);
        private int CodigoDocumentoSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_DOCUMENTO_SUSTENTO"]);
        private int NumeroDocumentoSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUM_DOCUMENTO_SUSTENTO"]);
        private int Nombre = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NOMBRE"]);
        private int valor = int.Parse(ConfigurationManager.AppSettings["LONGITUD_VALOR"]);
        private int Codigo = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO"]);
        private int TipoIdentificacionComprador = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_IDENTIFICACION_COMPRADOR"]);
        private int RazonSocial = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RAZON_SOCIAL_COMPRADOR"]);
        private int Rise = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RISE"]);
        private int NumeroDocumentoModificado = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUMERO_DOCUMENTO_MODIFICADO"]);
        private int CodigôPorcentaje = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_PORCENTAJE"]);
        private int Tarifa = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TARIFA"]);
        private int DireccionPartida = int.Parse(ConfigurationManager.AppSettings["LONGITUD_DIRECCION_PARTIDA"]);
        private int RazonSocialTransporte = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RAZON_SOCIAL_TRANPORTE"]);
        private int TipoIdentificacionTransporte = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_IDENTIFICACION_TRANSPORTE"]);
        private int RucTransportistas = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RUC_TRANPORTISTA"]);
        private int Placa = int.Parse(ConfigurationManager.AppSettings["LONGITUD_PLACA"]);
        private int DireccionDestinatario = int.Parse(ConfigurationManager.AppSettings["LONGITUD_DIRECCION_DESTINATARIO"]);
        private int MotivoTraslado = int.Parse(ConfigurationManager.AppSettings["LONGITUD_MOTIVO_TRASLADO"]);
        private int DocumentoAduanero = int.Parse(ConfigurationManager.AppSettings["LONGITUD_DOCUMENTO_ADUANERO_UNICO"]);
        private int CodigoEstablecimientoDestino = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_ESTABLECIMIENTO_DESTINO"]);
        private int Ruta = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RUTA"]);
        private int TipoDocumentoSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_DOCUMENTO_SUSTENTO"]);
        private int NumeroDocumentoSusten = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUM_DOCUMENTO_SUSTENTO"]);
        private int NumeroAutorizacionDocumentoSuistento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUM_AUTORIZACION_DOCUMENTO_SUSTENTO"]);
        private int CodigoInterno = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_INTERNO"]);
        private int CodigoAdicional = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_ADICIONAL"]);
        private int TipoEmision = int.Parse(ConfigurationManager.AppSettings["TIPO_EMISION"]);

        private GuiaRemisionAD _guardarGuiaRemision = new GuiaRemisionAD();

        public DataSet verificaExisteDocumento(string tipoDocumento, int compania, string establecimeinto,
                                         string puntoEmision, string secuencial, ref int codigoRetorno,
                                         ref string mensajeRetorno)
        {
            DocumentoAD objMetodos = new DocumentoAD();
            return objMetodos.verificaExisteDocumento(tipoDocumento, compania, establecimeinto, puntoEmision,
                                                      secuencial, ref codigoRetorno, ref mensajeRetorno);
        }



        public void InsertarGuiaRemision(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento, 
                                           string txPuntoEmision, string txSecuencial, string txDireccionPartida, string txRazonSocialTransportista, 
                                           string ciTipoIdentificacionTransportista, string txRucTransportista, string txRise, string txFechaIniTransporte,
                                           string txFechaFinTransporte, string txPlaca, int ciContingenciaDet, string txEmail, string txNumeroAutorizacion, 
                                           string txFechaHoraAutorizacion, string xmlDocumentoAutorizado, string ciEstado, string ciAmbiente, 
                                           string ciCodigoNumerico, string ruc, ref int codigoRetorno, ref string descripcionRetorno, ref int ciGuiaRemision)
        {

            DataSet ds = new DataSet();

            // txFechaFinTransporte = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime( txFechaFinTransporte));

            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }
                if (ciCompania != 0 && ciTipoDocumento != "" && ciTipoEmision != 0 && ciTipoIdentificacionTransportista != "" && txClaveAcceso != "" 
                    && txDireccionPartida != "" && txEstablecimiento != "" && txFechaFinTransporte != "" && txFechaIniTransporte != "" && txPlaca != "" 
                    && txPuntoEmision != "" && txRazonSocialTransportista != "" && txRucTransportista != "" && txSecuencial != "")
                {
                    validacion.validarCampos(ref txClaveAcceso, "string", "txClaveAcceso", ClaveAcceso, ref descripcionRetorno);
                    validacion.validarCampos(ref ciTipoDocumento, "string", "ciTipoDocumento", TipoDocumento, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txDireccionPartida, "string", "txDireccionPartida", DireccionPartida, ref descripcionRetorno);
                    //validacion.validarCampos(ref txRazonSocialTransportista, "string", "txRazonSocialTransportista", RazonSocialTransporte, ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoIdentificacionTransportista, "ciTipoIdentificacionTransportista", TipoIdentificacionTransporte, false, ref descripcionRetorno);
                    validacion.validarCampos(ref txRucTransportista, "string", "txRucTransportista", RucTransportistas, ref descripcionRetorno);
                    validacion.validarCampos(ref txRise, "string", "txRise", Rise, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaFinTransporte, "txFechaFinTransporte", ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaIniTransporte, "txFechaIniTransporte", ref descripcionRetorno);
                    validacion.validarCampos(ref txPlaca, "string", "txPlaca", Placa, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaHoraAutorizacion, "txFechaHoraAutorizacion", ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {

                        DataSet dsResultadoFactura = _guardarGuiaRemision.InsertarGuiaRemision(
                                                                        ciCompania,
                                                                        ciTipoEmision,
                                                                        txClaveAcceso.Trim(),
                                                                        ciTipoDocumento,
                                                                        txEstablecimiento.Trim(),
                                                                        txPuntoEmision.Trim(),
                                                                        txSecuencial.Trim(),
                                                                        txDireccionPartida.Trim(),
                                                                        txRazonSocialTransportista.Trim(),
                                                                        ciTipoIdentificacionTransportista,
                                                                        txRucTransportista.Trim(),
                                                                        txRise.Trim(),
                                                                        txFechaIniTransporte.Trim(),
                                                                        txFechaFinTransporte.Trim(),
                                                                        txPlaca.Trim(),
                                                                        ciContingenciaDet,
                                                                        txEmail.Trim(),
                                                                        txNumeroAutorizacion.Trim(),
                                                                        txFechaHoraAutorizacion.Trim(),
                                                                        xmlDocumentoAutorizado,
                                                                        ciEstado,
                                                                        ciAmbiente,
                                                                        ref codigoRetorno,
                                                                        ref descripcionRetorno,
                                                                        ref ciGuiaRemision);

                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (ciTipoDocumento.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoDocumento - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (ciTipoEmision == 0) { validacion.agregarCamposObligatorios("ciTipoEmision - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (ciTipoIdentificacionTransportista.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoIdentificacionTransportista - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txClaveAcceso.Trim() == "") { validacion.agregarCamposObligatorios("txClaveAcceso - Guia Remision Cabcera ", ref descripcionRetorno); }
                    if (txDireccionPartida.Trim() == "") { validacion.agregarCamposObligatorios("txDireccionPartida - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txFechaFinTransporte.Trim() == "") { validacion.agregarCamposObligatorios("txFechaFinTransporte - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txFechaIniTransporte.Trim() == "") { validacion.agregarCamposObligatorios("txFechaIniTransporte - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txPlaca.Trim() == "") { validacion.agregarCamposObligatorios("txPlaca - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txRazonSocialTransportista.Trim() == "") { validacion.agregarCamposObligatorios("txRucTransportista - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txRucTransportista.Trim() == "") { validacion.agregarCamposObligatorios("qnTotalSinImpuestos - Guia Remision Cabcera", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("RazonSocial - Guia Remision Cabcera", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarGuiaRemisionDestinatario(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                       string txIdentificacionDestinatario, string txRazonSocialDestinatario, string txDireccionDestinatario, 
                                                       string txMotivoTraslado, string txDocumentoAduaneroUnico, string txCodigoEstablecimientoDestino, 
                                                       string txRuta, string ciTipoDocumentoSustento, string txNumeroDocumentoS, 
                                                       string txNumeroAutorizacionDocumentoSustento, string txFechaEmisionDocumentoSustento,
                                                       ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txDireccionDestinatario != "" && txEstablecimiento != "" && txIdentificacionDestinatario != "" && txMotivoTraslado != "" 
                    && txPuntoEmision != "" && txRazonSocialDestinatario != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txIdentificacionDestinatario, "string", "txIdentificacionDestinatario", Identificacion, ref descripcionRetorno);
                    validacion.validarCampos(ref txRazonSocialDestinatario, "string", "txRazonSocialDestinatario", RazonSocial, ref descripcionRetorno);
                    validacion.validarCampos(ref txDireccionDestinatario, "string", "txDireccionDestinatario", DireccionDestinatario, ref descripcionRetorno);
                    validacion.validarCampos(ref txMotivoTraslado, "string", "txMotivoTraslado", MotivoTraslado, ref descripcionRetorno);
                    validacion.validarCampos(ref txDocumentoAduaneroUnico, "string", "txDocumentoAduaneroUnico", DocumentoAduanero, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoEstablecimientoDestino, "string", "txCodigoEstablecimientoDestino", CodigoEstablecimientoDestino, ref descripcionRetorno);
                    validacion.validarCampos(ref txRuta, "string", "txRuta", Ruta, ref descripcionRetorno);
                    //validacion.validarCamposChar(ref ciTipoDocumentoSustento, "ciTipoDocumentoSustento", TipoDocumentoSustento, false, ref descripcionRetorno);
                    //validacion.validarCampos(ref txNumeroDocumentoS, "string", "txNumeroDocumentoS", NumeroDocumentoSusten, ref descripcionRetorno);
                    //validacion.validarCampos(ref txNumeroAutorizacionDocumentoSustento, "string", "txNumeroAutorizacionDocumentoSustento", NumeroAutorizacionDocumentoSuistento, ref descripcionRetorno);
                    //validacion.ValidarFormatoFecha(ref txFechaEmisionDocumentoSustento, "txFechaEmisionDocumentoSustento", ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {

                        DataSet dsResultadoFactura = _guardarGuiaRemision.InsertarGuiaRemisionDestinatario(
                                                                                                          ciCompania,
                                                                                                          txEstablecimiento.Trim(),
                                                                                                          txPuntoEmision.Trim(),
                                                                                                          txSecuencial.Trim(),
                                                                                                          txIdentificacionDestinatario.Trim(),
                                                                                                          txRazonSocialDestinatario.Trim(),
                                                                                                          txDireccionDestinatario.Trim(),
                                                                                                          txMotivoTraslado.Trim(),
                                                                                                          txDocumentoAduaneroUnico.Trim(),
                                                                                                          txCodigoEstablecimientoDestino.Trim(),
                                                                                                          txRuta.Trim(),
                                                                                                          ciTipoDocumentoSustento,
                                                                                                          txNumeroDocumentoS.Trim(),
                                                                                                          txNumeroAutorizacionDocumentoSustento.Trim(),
                                                                                                          txFechaEmisionDocumentoSustento.Trim(),
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Guia Remision Destinatario", ref descripcionRetorno); }
                    if (txDireccionDestinatario.Trim() == "") { validacion.agregarCamposObligatorios("txDireccionDestinatario - Guia Remision Destinatario", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Guia Remision Destinatario", ref descripcionRetorno); }
                    if (txIdentificacionDestinatario.Trim() == "") { validacion.agregarCamposObligatorios("txIdentificacionDestinatario - Guia Remision Destinatario", ref descripcionRetorno); }
                    if (txMotivoTraslado.Trim() == "") { validacion.agregarCamposObligatorios("txMotivoTraslado - Guia Remision Destinatario", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Guia Remision Destinatario ", ref descripcionRetorno); }
                    if (txRazonSocialDestinatario.Trim() == "") { validacion.agregarCamposObligatorios("txRazonSocialDestinatario - Guia Remision Destinatario", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Guia Remision Destinatario", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarGuiaRemisionDestinatarioDetalle(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                            string txIdentificacionDestinatario, string txCodigoInterno, string txCodigoAdicional, 
                                                            string txDescripcion, decimal qnCantidad, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txCodigoInterno != "" && txDescripcion != "" && txEstablecimiento != "" && txIdentificacionDestinatario != "" && txPuntoEmision != "" && txSecuencial != "" && qnCantidad != 0)
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref  descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txIdentificacionDestinatario, "string", "txIdentificacionDestinatario", Identificacion, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoInterno, "string", "txCodigoInterno", CodigoInterno, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoAdicional, "string", "txCodigoAdicional", CodigoAdicional, ref descripcionRetorno);
                    validacion.validarCampos(ref txDescripcion, "string", "txDescripcion", RazonSocial, ref descripcionRetorno);


                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarGuiaRemision.InsertarGuiaRemisionDestinatarioDetalle(
                                                                                                                 ciCompania,
                                                                                                                 txEstablecimiento.Trim(),
                                                                                                                 txPuntoEmision.Trim(),
                                                                                                                 txSecuencial.Trim(),
                                                                                                                 txIdentificacionDestinatario.Trim(),
                                                                                                                 txCodigoInterno.Trim(),
                                                                                                                 txCodigoAdicional.Trim(),
                                                                                                                 txDescripcion.Trim(),
                                                                                                                 qnCantidad,
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    if (txCodigoInterno.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoInterno - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    if (txDescripcion.Trim() == "") { validacion.agregarCamposObligatorios("txDescripcion - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    if (txIdentificacionDestinatario.Trim() == "") { validacion.agregarCamposObligatorios("txIdentificacionDestinatario - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    if (qnCantidad == 0) { validacion.agregarCamposObligatorios("qnCantidad - Guia Remision Destinatario Detalle", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarGuiaRemisionDestinatarioDetalleAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                                     string txIdentificacionDestinatario, string txCodigoInterno, string txNombre, string txValor,
                                                                     ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet ds = new DataSet();
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txCodigoInterno != "" && txEstablecimiento != "" && txIdentificacionDestinatario != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txIdentificacionDestinatario, "string", "txIdentificacionDestinatario", Identificacion, ref descripcionRetorno);//
                    validacion.validarCampos(ref txCodigoInterno, "int", "txCodigoInterno", CodigoInterno, ref descripcionRetorno);
                    validacion.validarCampos(ref txNombre, "string", "txNombre", Nombre, ref descripcionRetorno);//
                    validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);//


                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarGuiaRemision.InsertarGuiaRemisionDestinatarioDetalleAdicional(
                                                                                                                             ciCompania,
                                                                                                                             txEstablecimiento.Trim(),
                                                                                                                             txPuntoEmision.Trim(),
                                                                                                                             txSecuencial.Trim(),
                                                                                                                             txIdentificacionDestinatario.Trim(),
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Guia Remision Destinatario Detalle Adicional", ref descripcionRetorno); }
                    if (txCodigoInterno.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoInterno - Guia Remision Destinatario Detalle Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Guia Remision Destinatario Detalle Adicional", ref descripcionRetorno); }
                    if (txIdentificacionDestinatario.Trim() == "") { validacion.agregarCamposObligatorios("txIdentificacionDestinatario - Guia Remision Destinatario Detalle Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Guia Remision Destinatario Detalle Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Guia Remision Destinatario Detalle Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }

            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarGuiaRemisionInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, 
                                                      string txNombre, string txValor, ref int codigoRetorno, ref string descripcionRetorno)
        {

            DataSet ds = new DataSet();
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txNombre, "string", "txNombre", Nombre, ref descripcionRetorno);
                    validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);


                    if (validacion.contadorError == 0)
                    {
                        DataSet dsResultadoFactura = _guardarGuiaRemision.InsertarGuiaRemisionInfoAdicional(
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Guia Remision Info Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Guia Remision Info Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Guia Remision Info Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Guia Remision Info Adicional", ref descripcionRetorno); }
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
    }
}
