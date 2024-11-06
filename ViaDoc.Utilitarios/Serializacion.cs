using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ViaDoc.Utilitarios
{
    public static class Serializacion
    {
        public static string serializar(Object obj)
        {
            string resultXml = "";
            try
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                var xns = new XmlSerializerNamespaces();
                xns.Add(string.Empty, string.Empty);
                //xns.Add("xsd","http://www.w3.org/2001/XMLSchema");
                //xns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");

                xml.Serialize(ms, obj, xns);
                resultXml = Encoding.UTF8.GetString(ms.ToArray());

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.InnerXml = resultXml;
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
    }
}