using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.Net;
using System.IO;

namespace eSync
{
    public enum TipoEnvio { Normal, Lote }

    /// <summary>
    ///Sync ES LA CLASE QUE CONTENDRÁ LOS METODOS PARA ENVIO A RECEPCION Y AUTORIZACION DE DOCUMENTOS ELECTRÓNICOS
    /// </summary>
    public class Sync
    {
        public static String UrlRecepcion { get; set; }
        public static String UrlAutorizacion { get; set; }
        public static String DocumentoXml { get; set; }
        public static Int16 ReadWriteTimeout { get; set; }
        public static Int16 Timeout { get; set; }

        private static SoapResponse EnvioPeticionSoap(String CuerpoSoap, String UrlServicio)
        {
            SoapResponse resultado = new SoapResponse();
            HttpWebRequest httpWRQ = null;
            IWebProxy iProxy = null;
            String proxyAddress = "";
            WebProxy proxy = null;
            Uri newUri = null;
            Byte[] buffer = null;
            Stream post = null;
            HttpWebResponse httpWRP = null;
            Stream responseData = null;
            StreamReader responseReader = null;
            String soapEnvelope = "";

            try
            {
                soapEnvelope += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                soapEnvelope += "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";
                soapEnvelope += "<soap:Body>" + CuerpoSoap + "</soap:Body></soap:Envelope>";
                buffer = Encoding.ASCII.GetBytes(soapEnvelope);

                httpWRQ = (HttpWebRequest)WebRequest.Create(UrlServicio);

                iProxy = (IWebProxy)httpWRQ.Proxy;
                httpWRQ.AllowWriteStreamBuffering = false;
                httpWRQ.Method = "POST";
                httpWRQ.ContentType = "text/xml; charset=UTF-8";
                httpWRQ.ContentLength = buffer.Length;
                proxyAddress = iProxy.GetProxy(httpWRQ.RequestUri).ToString();

                if (ReadWriteTimeout > 0) httpWRQ.ReadWriteTimeout = ReadWriteTimeout * 1000;
                if (Timeout > 0) httpWRQ.Timeout = Timeout * 1000;

                newUri = new Uri(proxyAddress);
                proxy = new WebProxy() { Address = newUri };
                post = httpWRQ.GetRequestStream();
                post.Write(buffer, 0, buffer.Length);
                post.Close();
                post.Dispose();

                httpWRP = (HttpWebResponse)httpWRQ.GetResponse();
                responseData = httpWRP.GetResponseStream();
                responseReader = new StreamReader(responseData);

                resultado.RespuestaSoap = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                resultado.TieneExcepcion = true;
                resultado.Excepcion = ex;
                //AccesoDatos.SQL.ExcepcionesSql exception = new AccesoDatos.SQL.ExcepcionesSql();
                //exception.txClaseErrores = "Sync.cs";
                //exception.txMetodoError = "EnvioPeticionSoap";
                //exception.ExcepcionDocumentosElectronicos(ex, exception);
            }

            return resultado;
        }

        public static RecepcionResponse EnvioRecepcion()
        {
            RecepcionResponse resultado = new RecepcionResponse();
            SoapResponse resultadoSoap = null;
            Byte[] buffer = null;
            String xmlB64 = "";
            String cuerpoSoap = "";

            try
            {
                buffer = Encoding.UTF8.GetBytes(DocumentoXml);
                xmlB64 = Convert.ToBase64String(buffer);

                cuerpoSoap += "<ns2:validarComprobante xmlns:ns2=\"http://ec.gob.sri.ws.recepcion\">";
                cuerpoSoap += "<xml>" + xmlB64 + "</xml>";
                cuerpoSoap += "</ns2:validarComprobante>";

                resultadoSoap = EnvioPeticionSoap(cuerpoSoap, UrlRecepcion);

                if (!resultadoSoap.TieneExcepcion)
                    resultado.ProcesarRespuesta(resultadoSoap.RespuestaSoap);
                else
                {
                    resultado.TieneExcepcion = resultadoSoap.TieneExcepcion;
                    resultado.Excepcion = resultadoSoap.Excepcion;
                }
            }
            catch (Exception ex)
            {
                resultado.TieneExcepcion = true;
                resultado.Excepcion = ex;

                //AccesoDatos.SQL.ExcepcionesSql exception = new AccesoDatos.SQL.ExcepcionesSql();
                //exception.txClaseErrores = "WinServFirmasInicio.cs";
                //exception.txMetodoError = "EnvioRecepcion";
                //exception.ExcepcionDocumentosElectronicos(ex, exception);
            }
            return resultado;
        }

        public static AutorizacionResponse RecuperaAutorizacion(string claveAcceso)
        {
            AutorizacionResponse resultado = new AutorizacionResponse();
            SoapResponse resultadoSoap = null;
            //XmlDocument xmlDoc = null;
            //XmlNode nodo = null;
            //String claveAcceso = "";
            String cuerpoSoap = "";
            try
            {
                //xmlDoc = new XmlDocument();
                //xmlDoc.LoadXml(DocumentoXml);

                //nodo = xmlDoc.SelectSingleNode("//claveAcceso/node()");
                //claveAcceso = nodo.Value;

                cuerpoSoap += "<ns2:autorizacionComprobante xmlns:ns2=\"http://ec.gob.sri.ws.autorizacion\">";
                cuerpoSoap += "<claveAccesoComprobante>" + claveAcceso + "</claveAccesoComprobante>";
                cuerpoSoap += "</ns2:autorizacionComprobante>";

                resultadoSoap = EnvioPeticionSoap(cuerpoSoap, UrlAutorizacion);

                if (!resultadoSoap.TieneExcepcion)
                    resultado.ProcesarRespuesta(resultadoSoap.RespuestaSoap);
                else
                {
                    resultado.TieneExcepcion = resultadoSoap.TieneExcepcion;
                    resultado.Excepcion = resultadoSoap.Excepcion;
                }
            }
            catch (Exception ex)
            {
                resultado.TieneExcepcion = true;
                resultado.Excepcion = ex;
            }
            return resultado;
        }

        public static SoapResponse RecuperaAutorizacionLote()
        {
            SoapResponse resultadoSoap = null;
            XmlDocument xmlDoc = null;
            XmlNode nodo = null;
            String claveAcceso = "";
            String cuerpoSoap = "";

            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(DocumentoXml);

                nodo = xmlDoc.SelectSingleNode("//claveAcceso/node()");
                claveAcceso = nodo.Value;

                cuerpoSoap += "<ns2:autorizacionComprobanteLoteMasivo xmlns:ns2=\"http://ec.gob.sri.ws.autorizacion\">";
                cuerpoSoap += "<claveAccesoLote>" + claveAcceso + "</claveAccesoLote>";
                cuerpoSoap += "</ns2:autorizacionComprobanteLoteMasivo>";

                resultadoSoap = EnvioPeticionSoap(cuerpoSoap, UrlAutorizacion);
            }
            catch (Exception ex)
            {
                resultadoSoap.TieneExcepcion = true;
                resultadoSoap.Excepcion = ex;
            }

            return resultadoSoap;
        }

        public static SensorServicioResponse SensaServicio(String UrlServicio)
        {
            WebRequest wReq;
            SensorServicioResponse objResponse = new SensorServicioResponse();

            try
            {
                wReq = WebRequest.Create(UrlServicio);

                using (WebResponse wRes = wReq.GetResponse())
                {
                    using (Stream oStream = wRes.GetResponseStream())
                    {
                        objResponse.ServicioDisponible = oStream.CanRead;
                    }
                }
            }
            catch (WebException ex)
            {
                objResponse.TieneExcepcion = true;
                objResponse.ExcepcionWeb = ex;
            }

            return objResponse;
        }
    }
}
