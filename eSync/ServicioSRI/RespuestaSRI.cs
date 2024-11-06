using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace eSync.ServicioSRI
{

    public class RespuestaSRI
    {
        public string Estado { get; set; }
        public string ClaveAcceso { get; set; }
        public string ErrorIdentificador { get; set; }
        public string ErrorMensaje { get; set; }
        public string ErrorInfoAdicional { get; set; }
        public string ErrorTipo { get; set; }
        public string NumeroAutorizacion { get; set; }
        public string FechaAutorizacion { get; set; }
        public string Ambiente { get; set; }
        public string SxmlRespuesta { get; set; }
        public XmlDocument Comprobante = new XmlDocument();
        public XmlDocument ComprobanteR = new XmlDocument();
        public RespuestaSRI() { }
    }
}
