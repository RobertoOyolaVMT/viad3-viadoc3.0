using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.portalweb;
using ViaDocEnvioCorreo.Negocios;

namespace Negocios
{
    public class RideComprobanteElectronico
    {
        public string CodigoError
        {
            get;
            set;
        }

        public string MensajeError
        {
            get;
            set;
        }

        public byte[] ByteRide
        {
            get;
            set;
        }

        public RideComprobanteElectronico()
        {
            this.CodigoError = "";
            this.MensajeError = "";
            this.ByteRide = new byte[1];
        }

        public RideComprobanteElectronico ConsultaRideJSON(string claveAcceso, string tipoDocumento)
        {

            int codigoRetorno = 0;
            string mensajeRetorno = string.Empty;

            ProcesoDocumento objDocumentos = new ProcesoDocumento();
            RideComprobanteElectronico objRide = new RideComprobanteElectronico();

            string txTipoDocumento = "";
            string numAutorizacion = "";
            string txFechaAutorizacin = "";
            string ciCompania = "";

            if (tipoDocumento.Trim().Equals("FAC"))
            {
                txTipoDocumento = "Factura";
                tipoDocumento = "01";
            }
            if (tipoDocumento.Trim().Equals("LIQ"))
            {
                txTipoDocumento = "Liquidacion";
                tipoDocumento = "03";
            }
            if (tipoDocumento.Trim().Equals("NC"))
            {
                txTipoDocumento = "NotaCredito";
                tipoDocumento = "04";
            }
            if (tipoDocumento.Trim().Equals("ND"))
            {
                txTipoDocumento = "NotaDebito";
                tipoDocumento = "05";
            }
            if (tipoDocumento.Trim().Equals("REM"))
            {
                txTipoDocumento = "GuiaRemision";
                tipoDocumento = "06";
            }
            if (tipoDocumento.Trim().Equals("RET"))
            {
                txTipoDocumento = "CompRetencion";
                tipoDocumento = "07";
            }
            try
            {
                string xmlComprobante = objDocumentos.ConsultarXMLDescargarWs(claveAcceso, txTipoDocumento, ref ciCompania, ref txFechaAutorizacin, ref numAutorizacion, ref codigoRetorno, ref mensajeRetorno);

                if (mensajeRetorno != "")
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error en busqueda de XML: " + mensajeRetorno);
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Se encontró el XML");
                }

                if (xmlComprobante.Equals(""))
                {
                    mensajeRetorno = "Se presentó un error con la descarga";
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Ingresó a descarga de RIDE WS");
                    ProcesoGenerarRideWeb objR = new ProcesoGenerarRideWeb();

                    byte[] buffer = objR.GenerarRideDocumentos(int.Parse(ciCompania), xmlComprobante, txFechaAutorizacin, numAutorizacion, tipoDocumento, ref codigoRetorno, ref mensajeRetorno);
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("pdf byte " + String.Join(" ", buffer) + "Mensaje error:" + mensajeRetorno.ToString() + "codigoRetorno" + codigoRetorno);
                    //if (mensajeRetorno.Trim().Equals("") || mensajeRetorno.Trim().Equals(string.IsNullOrEmpty(mensajeRetorno)))
                    //{
                        objRide.ByteRide = buffer;
                        objRide.CodigoError = "1";
                        objRide.MensajeError = mensajeRetorno;
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Hola que hace:" + JsonConvert.SerializeObject(objRide));
                    //}
                    //else
                    //{
                    //objRide.ByteRide = null;
                    //objRide.CodigoError = "999";
                    //objRide.MensajeError = mensajeRetorno;
                    //}
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Catch Genera Ride WS: " + ex.ToString());
            }
            return objRide;
        }

        public ViaDoc.EntidadNegocios.Retorno ElimarDocumentosElectronicos(string tipoDocumento, string idCompania, string establecimiento, string puntoEmision, string secuencial)
        {
            Retorno objRetorno = new Retorno();
            int codigoRetorno = 0;
            string mensajeRetorno = string.Empty;

            ProcesoDocumento objDocumentos = new ProcesoDocumento();

            if (tipoDocumento.Trim().Equals("") || idCompania.Trim().Equals("") || establecimiento.Trim().Equals("") || puntoEmision.Trim().Equals("") || secuencial.Trim().Equals(""))
            {
                objRetorno.codigoRetorno = 4;
                objRetorno.mensajeRetorno = "Debe enviar todos los datos obligatorio";

                return objRetorno;
            }

            if (tipoDocumento.Trim().Equals("FAC"))
            {
                tipoDocumento = "01";
            }
            if (tipoDocumento.Trim().Equals("LIQ"))
            {
                tipoDocumento = "03";
            }
            if (tipoDocumento.Trim().Equals("NC"))
            {
                tipoDocumento = "04";
            }
            if (tipoDocumento.Trim().Equals("ND"))
            {
                tipoDocumento = "05";
            }
            if (tipoDocumento.Trim().Equals("REM"))
            {
                tipoDocumento = "06";
            }
            if (tipoDocumento.Trim().Equals("RET"))
            {
                tipoDocumento = "07";
            }

            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Tipo documento: " + tipoDocumento);
            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Tipo documento procesado: " + tipoDocumento);
            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Id Compañia: " + idCompania);
            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Establecimiento: " + establecimiento);
            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Punto Emisión: " + puntoEmision);
            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Secuencial: " + secuencial);

            objDocumentos.ConsultaEliminarDocumentos(tipoDocumento, idCompania, establecimiento, puntoEmision, secuencial, ref codigoRetorno, ref mensajeRetorno);

            objRetorno.codigoRetorno = codigoRetorno;
            objRetorno.mensajeRetorno = mensajeRetorno;

            return objRetorno;
        }

        public List<Autorizar> ConsultaDocAutorizado(Documento documento, ref int codigoRetorno, ref string mensajeRetorno)
        {
            var objRetorno = new List<Autorizar>();
            ProcesoDocumento objDocumentos = new ProcesoDocumento();
            try
            {
                objRetorno = objDocumentos.ConsultarDocumentosAutorizar(documento, ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("ConsultaDocAutorizado", "RideComprobanteElectronico", ex.Message, null);
            }
            return objRetorno;
        }
    }
}
