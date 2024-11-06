using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;


namespace eSync.ServicioSRI
{
    public class WSHelper
    {
        static string URL_Envio;
        static string URL_Autorizacion;
        static string RutaXML;
        static string ContenidoXML;
        static string ClaveAcceso;



        public static string ConfigInicial(ConfigHelper config)
        {
            URL_Envio = config.URL_Envio;
            URL_Autorizacion = config.URL_Autorizacion;
            RutaXML = config.RutaXML;
            ClaveAcceso = config.ClaveAcceso;
            ContenidoXML = config.ContenidoXML;
            return "1";
        }

        private static string xmlEnvioRequestTemplate =
          String.Concat(
          "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
          " <SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"",
          " xmlns:ns1=\"http://ec.gob.sri.ws.recepcion\">",
          "  <SOAP-ENV:Body>",
          "    <ns1:validarComprobante>",
          "      <xml>{0}</xml>",
          "    </ns1:validarComprobante>",
          "  </SOAP-ENV:Body>",
          "</SOAP-ENV:Envelope>");

        private static string xmlAutorizacionRequestTemplate =
          String.Concat(
          "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
          " <SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"",
          " xmlns:ns1=\"http://ec.gob.sri.ws.autorizacion\">",
          "  <SOAP-ENV:Body>",
          "    <ns1:autorizacionComprobante>",
          "      <claveAccesoComprobante>{0}</claveAccesoComprobante>",
          "    </ns1:autorizacionComprobante>",
          "  </SOAP-ENV:Body>",
          "</SOAP-ENV:Envelope>");


        /// <summary>
        /// Envía el xml firmado a los webs services del SRI para su recepción.
        /// </summary>
        public static RespuestaSRI EnvioComprobante()
        {
            RespuestaSRI result = null;
            try
            {
                string ws_url = URL_Envio;
                string bytesEncodeBase64 = "";
                //certificado SSL
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Codifica el archivo a base 64
                if (RutaXML != "")
                {
                    bytesEncodeBase64 = XMLHelper.ConvertToBase64String(RutaXML);
                }
                if (ContenidoXML != "")
                {
                    bytesEncodeBase64 = XMLHelper.ConvertToBase64StringContenido(ContenidoXML);
                }
                //Crea el request del web service
                HttpWebRequest request = CreateWebRequest(ws_url, "POST");
                //Arma la cadena xml ara el envío al web service
                string stringRequest = string.Format(xmlEnvioRequestTemplate, bytesEncodeBase64);
                //Convierte la cadena en un documeto xml
                XmlDocument xmlRequest = XMLHelper.ConvertStringToDocument(stringRequest);               
                //Crea un flujo de datos (stream) y escribe el xml en la solicitud de respuesta del web service
                using (Stream stream = request.GetRequestStream())
                {
                    xmlRequest.Save(stream);                    
                }
                //Obtiene la respuesta del web service
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {                       
                        //Lee el flujo de datos (stream) de respuesta del web service
                        string soapResultStr = rd.ReadToEnd();
                        //Convierte la respuesta de string a xml para extraer el detalle de la respuesta del web service
                        XmlDocument soapResultXML = XMLHelper.ConvertStringToDocument(soapResultStr);
                        //Obtiene la respuesta detallada
                        result = XMLHelper.GetRespuestaRecepcion(soapResultXML);
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                result = new RespuestaSRI();
                result.Estado = "ERROR SRI";
                result.ErrorMensaje = ex.Message;
            }
            return result;
        }

        //internal static string ConfigInicial(ConfigHelper conf)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Envía la clave de acceso a los webs services del SRI para consultar ele estado de autorización.
        /// </summary>
        public static RespuestaSRI AutorizacionComprobante(out XmlDocument xml_doc)
        {
            RespuestaSRI result = null;
            XmlDocument xmlRequest = null;
            string ws_url = URL_Autorizacion;
            xml_doc = null;
            try
            {
                //Crea el request del web service
                HttpWebRequest request = CreateWebRequest(ws_url, "POST");

                // Establece el timeout infinito
                request.Timeout = System.Threading.Timeout.Infinite;
                request.ReadWriteTimeout = System.Threading.Timeout.Infinite;

                //Arma la cadena xml ara el envío al web service
                string stringRequest = string.Format(xmlAutorizacionRequestTemplate, ClaveAcceso);
                //Convierte la cadena en un documeto xml
                xmlRequest = XMLHelper.ConvertStringToDocument(stringRequest);
                xml_doc = xmlRequest;
                //Crea un flujo de datos (stream) y escribe el xml en la solicitud de respuesta del web service
                using (Stream stream = request.GetRequestStream())
                {
                    xmlRequest.Save(stream);
                }

                //Obtiene la respuesta del web service
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        //Lee el flujo de datos (stream) de respuesta del web service
                        string soapResultStr = rd.ReadToEnd();
                        //Convierte la respuesta de string a xml para extraer el detalle de la respuesta del web service
                        XmlDocument soapResultXML = XMLHelper.ConvertStringToDocument(soapResultStr);
                        //Obtiene la respuesta detallada
                        result = XMLHelper.GetRespuestaAutorizacion(soapResultXML, RutaXML);
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                result = new RespuestaSRI();
                result.Estado = "ERROR SRI";
                result.ErrorMensaje = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Crea y devuelve una instancia de objeto para la solicitud de respuesta desde una URI.
        /// </summary>
        /// <param name="uri">URI del recurso de internet</param>
        /// <param name="method">Método de solicitud</param>
        private static HttpWebRequest CreateWebRequest(string uri, string method)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Headers.Add("SOAP:Action");
            webRequest.ContentType = "application/soap+xml;charset=utf-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = method;
            webRequest.Timeout = -1;        

            return webRequest;
        }
    }
}
