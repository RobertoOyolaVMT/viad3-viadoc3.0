using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace eSync.ServicioSRI
{
   public class XMLHelper
    {
        public static XmlDocument ConvertStringToDocument(string xml_string)
        {
            XmlDocument result = new XmlDocument();
            result.LoadXml(xml_string);

            return result;
        }

        public static XmlDocument ConvertFileToDocument(string file_path)
        {
            XmlDocument result = new XmlDocument();
            result.Load(file_path);

            return result;
        }


        /// <summary>
        /// Convierte el documento en string Base64
        /// </summary>
        /// <param name="file_path">Ruta del archivo a aconvertir</param>
        public static string ConvertToBase64String(string file_path)
        {
            byte[] binarydata = File.ReadAllBytes(file_path);
            return Convert.ToBase64String(binarydata, 0, binarydata.Length);
        }
        public static string ConvertToBase64StringContenido(string contenido)
        {
            Byte[] buffer = null;
            buffer = Encoding.UTF8.GetBytes(contenido);
            return Convert.ToBase64String(buffer);
        }

        private static string GetNodeValue(string rootNodo, string infoNodo, XmlDocument doc)
        {
            string result = null;
            string node_path = "//" + rootNodo + "//" + infoNodo;

            XmlNode node = doc.SelectSingleNode(node_path);

            if (node != null)
            {
                result = node.InnerText;
            }

            return result;
        }

        /// <summary>
        /// Devuelve la respuesta de la solicitud de recepción del comprobante en una estructura detallada.
        /// </summary>
        /// <param name="xml_doc">Documento XML de respuesta</param>
        public static RespuestaSRI GetRespuestaRecepcion(XmlDocument xml_doc)
        {
            RespuestaSRI result = new RespuestaSRI();
            result.Estado = GetNodeValue("RespuestaRecepcionComprobante", "estado", xml_doc);


            if (result.Estado != "RECIBIDA")
            {
                result.ClaveAcceso = GetNodeValue("comprobante", "claveAcceso", xml_doc);
                result.ErrorIdentificador = GetNodeValue("mensaje", "identificador", xml_doc);
                result.ErrorMensaje = GetNodeValue("mensaje", "mensaje", xml_doc);
                result.ErrorInfoAdicional = GetNodeValue("mensaje", "informacionAdicional", xml_doc);
                result.ErrorTipo = GetNodeValue("mensaje", "tipo", xml_doc);

                //Verificar
                if(result.ErrorIdentificador == "43")
                {
                    //result.Estado = "";
                }
            }

            return result;
        }

        /// <summary>
        /// Devuelve la respuesta de la solicitud de autorización del comprobante en una estructura detallada.
        /// </summary>
        /// <param name="xml_doc">Documento XML de respuesta</param>
        public static RespuestaSRI GetRespuestaAutorizacion(XmlDocument xml_doc, string rutaxml)
        {
            RespuestaSRI result = new RespuestaSRI();
            // XmlDocument DocXML = new XmlDocument();
            string pathLevelAutorizacion = "RespuestaAutorizacionComprobante/autorizaciones/autorizacion[last()]";

            string pathLevelAutorizaciones = "RespuestaAutorizacionComprobante/autorizaciones[last()]";

            string pathLevelMensajes = "RespuestaAutorizacionComprobante/autorizaciones/autorizacion/mensajes[last()]/mensaje";

            result.Estado = GetNodeValue(pathLevelAutorizacion, "estado", xml_doc);

            if (result.Estado == "AUTORIZADO")
            {
                result.NumeroAutorizacion = GetNodeValue(pathLevelAutorizacion, "numeroAutorizacion", xml_doc);
                result.FechaAutorizacion = GetNodeValue(pathLevelAutorizacion, "fechaAutorizacion", xml_doc);
                result.Ambiente = GetNodeValue(pathLevelAutorizacion, "ambiente", xml_doc);
                string mensaje = GetNodeValue(pathLevelAutorizacion, "mensaje", xml_doc);
                string xml = GetNodeValue(pathLevelAutorizacion, "comprobante", xml_doc);



                string cabecera = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";

                cabecera += "<autorizacion>";
                cabecera += " <estado>";
                cabecera += result.Estado;
                cabecera += "</estado>";
                cabecera += " <numeroAutorizacion>";
                cabecera += result.NumeroAutorizacion;
                cabecera += "</numeroAutorizacion> ";

                cabecera += " <fechaAutorizacion>";
                cabecera += result.FechaAutorizacion;
                cabecera += "</fechaAutorizacion> ";
                cabecera += " <ambiente>";
                cabecera += result.Ambiente;
                cabecera += "</ambiente> ";
                string xml2 = xml;
                xml2 = xml2.Replace("<", "&lt;");      // No code
                xml2 = xml2.Replace(">", "&gt;");
                XmlDocument doc = new XmlDocument();
                // Loads the XML from the string
                doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                doc.LoadXml(cabecera + "<comprobante>" + "<![CDATA[" + xml2 + "]]>" + "</comprobante><mensajes/></autorizacion>");

                string documentootro = "";
                    documentootro = "<autorizacion>";
                documentootro += " <estado>";
                documentootro += result.Estado;
                documentootro += "</estado>";
                documentootro += " <numeroAutorizacion>";
                documentootro += result.NumeroAutorizacion;
                documentootro += "</numeroAutorizacion> ";

                documentootro += " <fechaAutorizacion>";
                documentootro += result.FechaAutorizacion;
                documentootro += "</fechaAutorizacion> ";
                documentootro += " <ambiente>";
                documentootro += result.Ambiente;
                documentootro += "</ambiente> ";
                 documentootro +=  "<comprobante>" + xml2 + "</comprobante><mensajes/></autorizacion>";
                //XmlElement xe = doc.DocumentElement;


                /* result.Comprobante.LoadXml(xml);
                 result.Comprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                 XDocument DocXML;
                 XDocument xdoc;
                 using (var nodeReader = new XmlNodeReader(result.Comprobante))
                 {
                    // nodeReader.MoveToContent();
                     xdoc = XDocument.Load(nodeReader);
                     xdoc.Declaration=new XDeclaration("1.0", "UTF-8", "yes");
                     
                 }


                 XElement CDATANode = new XElement("comprobante", xml2);
        
                 //CDATANode.Add(new XCData(xdoc.Declaration.ToString() +xdoc.ToString()));
                 XElement rootnode = new XElement("mensajes");
                
                 //xlm.LoadXml(xdoc.Declaration.ToString());
              
            // new XmlNodeReader(xlm),
                     DocXML = new XDocument(
                     new XDeclaration("1.0", "UTF-8", "yes"),
                     new XElement("autorizacion",
                     new XElement("estado", result.Estado),
                     new XElement("numeroAutorizacion", result.NumeroAutorizacion),
                     new XElement("fechaAutorizacion",  result.FechaAutorizacion),
                      new XElement("ambiente", result.Ambiente),
                     CDATANode, rootnode)
                     );
                     var sb = new StringBuilder();
                     var sw = new StringWriterUtf8(sb);
                   
                   
                     DocXML.Save(sw);

                    var xmlDocument = new XmlDocument();
                    XmlNodeReader x = new XmlNodeReader(xlm);
                     using (var xmlReader = DocXML.CreateReader())
                     {
                         xmlDocument.LoadXml(wr);
                     }
                    xmlDocument.LoadXml(sw.ToString());*/
                result.ComprobanteR = doc;
                result.SxmlRespuesta = documentootro;

                /* if (rutaxml!="")
                 {
                     string val = GuardaXml(result.Comprobante, result.NumeroAutorizacion, rutaxml);

                 }*/


            }
            else if (result.Estado == "NO AUTORIZADO")
            {
                result.FechaAutorizacion = GetNodeValue(pathLevelAutorizacion, "fechaAutorizacion", xml_doc);
                result.Ambiente = GetNodeValue(pathLevelAutorizacion, "ambiente", xml_doc);
                result.ErrorIdentificador = GetNodeValue(pathLevelMensajes, "identificador", xml_doc);
                result.ErrorMensaje = GetNodeValue(pathLevelMensajes, "mensaje", xml_doc) + GetNodeValue(pathLevelMensajes, "informacionAdicional", xml_doc);
                result.ErrorTipo = GetNodeValue(pathLevelMensajes, "tipo", xml_doc);
            }

            return result;
        }


        public static string GetInfoTributaria(string info, XmlDocument xml_doc)
        {
            return GetNodeValue("infoTributaria", info, xml_doc);
        }

        public static string GetInfoFactura(string info, XmlDocument xml_doc)
        {
            return GetNodeValue("infoFactura", info, xml_doc);
        }

        private static string GuardaXml(XmlDocument xml_doc, string numautorizacion, string rutaxml)
        {
            //Genera y guarda el XML autorizado
            XmlDocument miXML = new XmlDocument();

            miXML = xml_doc;
            miXML.Save(@rutaxml + numautorizacion + ".xml");


            return "1";

        }
    }
}
