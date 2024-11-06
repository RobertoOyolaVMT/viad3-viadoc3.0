using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Drawing;

namespace ReportesViaDoc
{
    /// <summary>
    /// clase que permitira manipular la respuestaSOA del SRI (Autorizacion de comprobantes), como tambien permitira serializar y descerializar un objeto
    /// </summary>
    public static class RetornarXml
    {

        /// <summary>
        /// Retorna el comprobante autorizado que se encuentra dentro de la respuestaSoap.
        /// </summary>
        /// <param name="RespuestaSoap">respuesta que da el sri mediante comunicacion SOA</param>
        /// <returns></returns>
        public static String RetornaComprobanteAutorizado(String RespuestaSoap)
        {
            try
            {
                XmlDocument Respuesta = new XmlDocument();
                //RespuestaSoap = File.ReadAllText("C:\\ComprobantesXML\\0610201407099012613500120010070000000399846951113.xml", Encoding.UTF8);

                XmlNodeList xmlAutorizaciones = null;
                XmlNode xmlAutorizacion = null;
                XmlNode nodo = null;
                Boolean Autorizado = false;
                Respuesta.LoadXml(RespuestaSoap);
                xmlAutorizaciones = Respuesta.SelectNodes("//autorizacion");
                if (xmlAutorizaciones.Count > 0)
                {
                    // xmlAutorizaciones = new List<Autorizacion>();

                    foreach (XmlNode _xmlAutorizacion in xmlAutorizaciones)
                    {
                        xmlAutorizacion = _xmlAutorizacion.Clone();
                        nodo = xmlAutorizacion.SelectSingleNode("//estado/node()");
                        if (nodo != null)
                        {
                            if (nodo.Value == "AUTORIZADO")
                            {
                                RespuestaSoap = _xmlAutorizacion.OuterXml;
                                Autorizado = true;
                            }
                            else
                            {
                                if (Autorizado == false)
                                {
                                    if (nodo.Value == "NO AUTORIZADO")
                                        RespuestaSoap = _xmlAutorizacion.OuterXml;
                                    else
                                    {
                                        RespuestaSoap = _xmlAutorizacion.OuterXml;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch
            {
                RespuestaSoap = "";
            }

            return RespuestaSoap;
        }

        /// <summary>
        /// Captura el comprobante sin firma de un xmlAutorizado para serializar
        /// </summary>
        /// <param name="XmlAutSOA">comprobante Autorizado</param>
        /// <returns></returns>
        public static String RetornaComprobanteAutorizadoSinFirma(String XmlAutSOA)
        {
            XmlDocument Respuesta = new XmlDocument();
            XmlNode nodo = null;
            Respuesta.LoadXml(XmlAutSOA);
            nodo = Respuesta.SelectSingleNode("//comprobante/node()");
            if (nodo != null && nodo.ToString() != "")
                XmlAutSOA = nodo.InnerText;
            Respuesta.LoadXml(XmlAutSOA);
            System.Xml.XmlNode node = Respuesta.DocumentElement;
            node.RemoveChild(node.LastChild);
            //XmlAutSOA = Respuesta.InnerXml;

            return XmlAutSOA;
        }

        /// <summary>
        /// permite DesSerializar un archivo xml indicandole el type del objeto a convertir
        /// </summary>
        /// <param name="objectData">XML que se desea desSerializar</param>
        /// <param name="type">Type del objeto al que se desea convertir</param>
        /// <returns>Retorna el objeto DesSerializado listo para realizar un casting al objeto de tipo asignado (TYPE)</returns>

        public static Object desSerializar(this string objectData, Type type)
        {
            Object result;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(type);
                using (System.IO.TextReader reader = new System.IO.StringReader(objectData))
                {
                    result = serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// Permite serializar cualquier objeto y lo convierte a un archivo XML
        /// </summary>
        /// <param name="obj">Objeto que se desea Serializar</param>
        /// <returns></returns>
        public static string serializar(Object obj)
        {
            string resultXml = "";
            try
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                var xns = new System.Xml.Serialization.XmlSerializerNamespaces();
                xns.Add(string.Empty, string.Empty);
                xml.Serialize(ms, obj, xns);
                resultXml = Encoding.UTF8.GetString(ms.ToArray());

                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.InnerXml = resultXml;
                xmlDoc.LoadXml(resultXml);

                XmlDeclaration xmlDeclaration = (XmlDeclaration)xmlDoc.FirstChild;
                xmlDeclaration.Encoding = "UTF-8";
                resultXml = xmlDoc.InnerXml;

            }
            catch (Exception)
            {
                resultXml = "";
            }

            return resultXml;

        }

        /// <summary>
        /// convierte a un arreglo de Byte[] un objeto de tipo Image del System.Drawing
        /// </summary>
        /// <param name="img">objeto Image</param>
        /// <returns>Retorna el Arreglo de Byte[]</returns>
        public static byte[] ImageToByte2(System.Drawing.Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

    }
}
