using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ViaDoc.EntidadesNegocios
{
    public class EntityDocumentoError
    {
       

        public string ObtenerNumeroDocumentoXMLFirmado(string tipoDocumento, string xmlFirmado)
        {
            string numeroDocumento = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlFirmado);
                XmlNodeList CamposXML;
                switch (tipoDocumento)
                {
                    case "01":
                        CamposXML = xml.SelectNodes("factura/infoTributaria");
                        break;
                    case "07":
                        CamposXML = xml.SelectNodes("comprobanteRetencion/infoTributaria");
                        break;
                    case "05":
                        CamposXML = xml.SelectNodes("NotaDebito/infoTributaria");
                        break;
                    case "04":
                        CamposXML = xml.SelectNodes("notaCredito/infoTributaria");
                        break;
                    default:
                        CamposXML = xml.SelectNodes("guiaRemision/infoTributaria");
                        break;
                }
                XmlNode informacionXML = CamposXML.Item(0);
                string estable = informacionXML.SelectSingleNode("estab").InnerText;
                string puntoemi = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                string secuen = informacionXML.SelectSingleNode("secuencial").InnerText;
                numeroDocumento = estable + "-" + puntoemi + "-" + secuen;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }
            return numeroDocumento;
        }

        public string ObtenerFechaEmisionDocumentoXMLFirmado(string tipoDocumento, string xmlFirmado)
        {
            string fechaEmision = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlFirmado);
                XmlNodeList CamposXML;
                switch (tipoDocumento)
                {
                    case "01":
                        CamposXML = xml.SelectNodes("factura/infoFactura");
                        break;
                    case "07":
                        CamposXML = xml.SelectNodes("comprobanteRetencion/infoCompRetencion");
                        break;
                    case "05":
                        CamposXML = xml.SelectNodes("NotaDebito/infoNotaDebito");
                        break;
                    case "04":
                        CamposXML = xml.SelectNodes("notaCredito/infoNotaCredito");
                        break;
                    default:
                        CamposXML = xml.SelectNodes("guiaRemision/infoGuiaRemision");
                        break;
                }
                XmlNode informacionXML = CamposXML.Item(0);
                fechaEmision = informacionXML.SelectSingleNode("fechaEmision").InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return fechaEmision;
        }

        public string ObtenerNumeroDocumentoClaveAcceso(string claveAcceso)
        {
            string numDocumento = "";
            try
            {
                string estable = claveAcceso.Substring(24, 3);
                string puntoemi = claveAcceso.Substring(27, 3);
                string secuen = claveAcceso.Substring(29, 9);
                numDocumento = estable + "-" + puntoemi + "-" + secuen;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return numDocumento;
        }
    }
}
