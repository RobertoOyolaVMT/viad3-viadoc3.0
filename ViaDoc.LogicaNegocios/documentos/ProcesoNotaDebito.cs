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
    public class ProcesoNotaDebito
    {
        private int ClaveAcceso = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CLAVE_ACCESO"]);
        private int TipoDocumento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_DOCUMENTO"]);
        private int Establecimiento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_ESTABLECIMIENTO"]);
        private int PuntoEmision = int.Parse(ConfigurationManager.AppSettings["LONGITUD_PUNTO_EMISION"]);
        private int Secuencial = int.Parse(ConfigurationManager.AppSettings["LONGITUD_SECUENCIAL"]);
        private int Fecha = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_EMISION"]);
        private int TipoIdentificacionSujeto = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_IDENTIFICACION_SUJETO"]);
        private int RazonSocialSujetoRetenido = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RAZON_SOCIAL_SUJETO_RE"]);
        private int Identificacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_IDENTIFICACION_SUJETO_RETENIDO"]);
        private int periodoFiscal = int.Parse(ConfigurationManager.AppSettings["LONGITUD_PERIODO_FISCAL"]);
        private int NumeroAutorizacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUMERO_AUTORIZACION"]);
        private int FechaHoraAutorizacion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_FECHA_HORA_AUTORIZACION"]);
        private int codigoRetencion = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_RETENCION"]);
        private int CodigoDocumentoSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_DOCUMENTO_SUSTENTO"]);
        private int NumeroDocumentoSustento = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NUM_DOCUMENTO_SUSTENTO"]);
        private int Nombre = int.Parse(ConfigurationManager.AppSettings["LONGITUD_NOMBRE"]);
        private int valor = int.Parse(ConfigurationManager.AppSettings["LONGITUD_VALOR"]);
        private int Codigo = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO"]);
        private int TipoIdentificacionComprador = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TIPO_IDENTIFICACION_COMPRADOR"]);
        private int RazonSocialComprador = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RAZON_SOCIAL_COMPRADOR"]);
        private int Rise = int.Parse(ConfigurationManager.AppSettings["LONGITUD_RISE"]);
        private int NumeroDocumentoModificado = 17;
        private int CodigôPorcentaje = int.Parse(ConfigurationManager.AppSettings["LONGITUD_CODIGO_PORCENTAJE"]);
        private int Tarifa = int.Parse(ConfigurationManager.AppSettings["LONGITUD_TARIFA"]);
        private int TipoEmision = int.Parse(ConfigurationManager.AppSettings["TIPO_EMISION"]);
        private NotaDebitoAD _guardarNotaDebito = new NotaDebitoAD();

        public DataSet verificaExisteDocumento(string tipoDocumento, int compania, string establecimeinto,
                                        string puntoEmision, string secuencial, ref int codigoRetorno,
                                        ref string mensajeRetorno)
        {
            DocumentoAD objMetodos = new DocumentoAD();
            return objMetodos.verificaExisteDocumento(tipoDocumento, compania, establecimeinto, puntoEmision,
                                                      secuencial, ref codigoRetorno, ref mensajeRetorno);
        }



        public void InsertarNotaDebito(int ciCompania, int ciTipoEmision, string txClaveAcceso, string ciTipoDocumento, string txEstablecimiento,
                                       string txPuntoEmision, string txSecuencial, string txFechaEmision, string ciTipoIdentificacionComprador,
                                       string txRazonSocialComprador, string txIdentificacionComprador, string txRise, string iTipoDocumentoModificado,
                                       string txNumeroDocumentoModificado, string txFechaEmisionDocumentoModificado, decimal qnTotalSinImpuestos, decimal qnValorTotal,
                                       int ciContingenciaDet, string txEmail, string txNumeroAutorizacion, string txFechaHoraAutorizacion,
                                       string xmlDocumentoAutorizado, string ciEstado, string ciAmbiente, string ciCodigoNumerico, string ruc,
                                       ref int codigoRetorno, ref string descripcionRetorno, ref int ciNotaDebito)
        {
            DataSet ds = new DataSet();
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }

                if (ciCompania != 0 && ciTipoDocumento != "" && ciTipoEmision != 0 && ciTipoIdentificacionComprador != "" && txClaveAcceso != ""
                    && txEstablecimiento != "" && txFechaEmision != "" && txFechaEmisionDocumentoModificado != "" && txIdentificacionComprador != ""
                    && txNumeroDocumentoModificado != "" && txPuntoEmision != "" && txRazonSocialComprador != "" && txSecuencial != "" && qnTotalSinImpuestos.ToString() != ""
                    && qnValorTotal.ToString() != "")
                {
                    validacion.validarCampos(ref ciTipoDocumento, "string", "ciTipoDocumento", TipoDocumento, ref descripcionRetorno);
                    validacion.validarCampos(ref txClaveAcceso, "string", "txClaveAcceso", ClaveAcceso, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaEmision, "txFechaEmision", ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciTipoIdentificacionComprador, "ciTipoIdentificacionComprador", TipoIdentificacionComprador, false, ref descripcionRetorno);
                    //validacion.validarCampos(ref txRazonSocialComprador, "string", "txRazonSocialComprador", RazonSocialComprador, ref descripcionRetorno);
                    validacion.validarCampos(ref txIdentificacionComprador, "string", "txIdentificacionComprador", Identificacion, ref descripcionRetorno);
                    validacion.validarCampos(ref txRise, "string", "txRise", Rise, ref descripcionRetorno);
                    validacion.validarCamposChar(ref iTipoDocumentoModificado, "iTipoDocumentoModificado", TipoDocumento, false, ref descripcionRetorno);
                    validacion.validarCampos(ref txNumeroDocumentoModificado, "string", "txNumeroDocumentoModificado", NumeroDocumentoModificado, ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaEmisionDocumentoModificado, "txFechaEmisionDocumentoModificado", ref descripcionRetorno);
                    validacion.ValidarFormatoFecha(ref txFechaHoraAutorizacion, "txFechaHoraAutorizacion", ref descripcionRetorno);
                    validacion.validarCamposChar(ref ciCodigoNumerico, "ciCodigoNumerico", 8, true, ref descripcionRetorno);//

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsRespuesta = _guardarNotaDebito.InsertarNotaDebito(
                                                                                   ciCompania,
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
                                                                                   iTipoDocumentoModificado,
                                                                                   txNumeroDocumentoModificado.Trim(),
                                                                                   txFechaEmisionDocumentoModificado.Trim(),
                                                                                   qnTotalSinImpuestos,
                                                                                   qnValorTotal,
                                                                                   ciContingenciaDet,
                                                                                   txEmail.Trim(),
                                                                                   txNumeroAutorizacion.Trim(),
                                                                                   txFechaHoraAutorizacion.Trim(),
                                                                                   xmlDocumentoAutorizado.Trim(),
                                                                                   ciEstado,
                                                                                   ciAmbiente,
                                                                                   ref codigoRetorno,
                                                                                   ref descripcionRetorno,
                                                                                   ref ciNotaDebito);


                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {// GUARDAR EN LA TABLA LOS CAMPOS VACIOS

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (ciTipoDocumento.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoDocumento - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (ciTipoEmision == 0) { validacion.agregarCamposObligatorios("ciTipoEmision - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (ciTipoIdentificacionComprador.Trim() == "") { validacion.agregarCamposObligatorios("ciTipoIdentificacionComprador - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txClaveAcceso.Trim() == "") { validacion.agregarCamposObligatorios("txClaveAcceso - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txFechaEmision.Trim() == "") { validacion.agregarCamposObligatorios("txFechaEmision - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txFechaEmisionDocumentoModificado.Trim() == "") { validacion.agregarCamposObligatorios("txFechaEmisionDocumentoModificado - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txNumeroDocumentoModificado.Trim() == "") { validacion.agregarCamposObligatorios("txNumeroDocumentoModificado - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txRazonSocialComprador.Trim() == "") { validacion.agregarCamposObligatorios("txRazonSocialComprador - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (qnTotalSinImpuestos == 0) { validacion.agregarCamposObligatorios("qnTotalSinImpuestos - Nota Debito Cabecera", ref descripcionRetorno); }
                    if (qnValorTotal == 0) { validacion.agregarCamposObligatorios("qnValorTotal - Nota Debito Cabecera", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }

        public void InsertarNotaDebitoImpuesto(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial, string txCodigo,
                                                 string txCodigoPorcentaje, string txTarifa, decimal qnBaseImponible, decimal qnValor,
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

                if (ciCompania != 0 && txCodigo != "" && txCodigoPorcentaje != "" && txEstablecimiento != "" && txPuntoEmision != "" && txSecuencial != ""
                    && txTarifa != "" && qnBaseImponible.ToString() != "" && qnValor.ToString() != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txPuntoEmision, "txPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigo, "string", "txCodigo", Codigo, ref descripcionRetorno);
                    validacion.validarCampos(ref txCodigoPorcentaje, "string", "txCodigoPorcentaje", CodigôPorcentaje, ref descripcionRetorno);
                    validacion.validarCampos(ref txTarifa, "string", "txTarifa", Tarifa, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsRespuesta = _guardarNotaDebito.InsertarNotaDebitoImpuesto(
                                                                             ciCompania,
                                                                             txEstablecimiento.Trim(),
                                                                             txPuntoEmision.Trim(),
                                                                             txSecuencial.Trim(),
                                                                             txCodigo.Trim(),
                                                                             txCodigoPorcentaje.Trim(),
                                                                             txTarifa.Trim(),
                                                                             qnBaseImponible,
                                                                             qnValor,
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (txCodigo.Trim() == "") { validacion.agregarCamposObligatorios("txCodigo - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (txCodigoPorcentaje.Trim() == "") { validacion.agregarCamposObligatorios("txCodigoPorcentaje - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (txTarifa.Trim() == "") { validacion.agregarCamposObligatorios("txTarifa - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (qnBaseImponible == 0) { validacion.agregarCamposObligatorios("qnBaseImponible - Nota Debito Impuesto", ref descripcionRetorno); }
                    if (qnValor == 0) { validacion.agregarCamposObligatorios("qnValor - Nota Debito Impuesto", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarNotaDebitoInfoAdicional(int ciCompania, string txEstablecimiento, string txPuntoEmision, string txSecuencial,
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
                    //validacion.validarCampos(ref txValor, "string", "txValor", valor, ref descripcionRetorno);


                    if (validacion.contadorError == 0)
                    {
                        DataSet dsRespuesta = _guardarNotaDebito.InsertarNotaDebitoInfoAdicional(
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

                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Nota Debito Info Adicional", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Nota Debito Info Adicional", ref descripcionRetorno); }
                    if (txPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txPuntoEmision - Nota Debito Info Adicional", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Nota Debito Info Adicional", ref descripcionRetorno); }
                    if (txNombre.Trim() == "") { validacion.agregarCamposObligatorios("txNombre - Nota Debito Info Adicional", ref descripcionRetorno); }
                    if (txValor.Trim() == "") { validacion.agregarCamposObligatorios("txValor - Nota Debito Info Adicional", ref descripcionRetorno); }
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }


        public void InsertarNotaDebitoMotivo(int ciCompania, string txEstablecimiento, string txtPuntoEmision, string txSecuencial, string txRazon,
                                             decimal qnValor, ref int codigoRetorno, ref string descripcionRetorno)
        {

            DataSet ds = new DataSet();
            try
            {
                Validacion validacion = new Validacion();
                if (ciCompania == 0)
                {
                    ciCompania = int.Parse(ConfigurationManager.AppSettings["COMPANIA"]);
                }
                if (ciCompania != 0 && txEstablecimiento != "" && txRazon != "" && txSecuencial != "" && txtPuntoEmision != "" && qnValor.ToString() != "")
                {
                    validacion.validarCamposChar(ref txEstablecimiento, "txEstablecimiento", Establecimiento, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txtPuntoEmision, "txtPuntoEmision", PuntoEmision, true, ref descripcionRetorno);//
                    validacion.validarCamposChar(ref txSecuencial, "txSecuencial", Secuencial, true, ref descripcionRetorno);//
                    validacion.validarCampos(ref txRazon, "string", "txRazon", RazonSocialComprador, ref descripcionRetorno);

                    if (validacion.contadorError == 0)
                    {
                        DataSet dsRespuesta = _guardarNotaDebito.InsertarNotaDebitoMotivo(
                                                                             ciCompania,
                                                                             txEstablecimiento.Trim(),
                                                                             txtPuntoEmision.Trim(),
                                                                             txSecuencial.Trim(),
                                                                             txRazon.Trim(),
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
                    if (ciCompania == 0) { validacion.agregarCamposObligatorios("ciCompania - Nota Debito Motivo", ref descripcionRetorno); }
                    if (txRazon.Trim() == "") { validacion.agregarCamposObligatorios("txRazon - Nota Debito Motivo", ref descripcionRetorno); }
                    if (qnValor == 0) { validacion.agregarCamposObligatorios("qnValor - Nota Debito Motivo", ref descripcionRetorno); }
                    if (txEstablecimiento.Trim() == "") { validacion.agregarCamposObligatorios("txEstablecimiento - Nota Debito Motivo", ref descripcionRetorno); }
                    if (txtPuntoEmision.Trim() == "") { validacion.agregarCamposObligatorios("txtPuntoEmision - Nota Debito Motivo", ref descripcionRetorno); }
                    if (txSecuencial.Trim() == "") { validacion.agregarCamposObligatorios("txSecuencial - Nota Debito Motivo", ref descripcionRetorno); }
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
