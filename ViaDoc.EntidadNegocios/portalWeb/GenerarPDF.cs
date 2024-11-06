using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.portalWeb
{
    public class GenerarPDF
    {
        public string ClaveAcceso { get; set; }
        public string NumeroDocumento { get; set; }
        public string Estado { get; set; }
        public string FechaAutorizacion { get; set; }
        public string XmlDoc { get; set; }
    }
    public class GenerarPDFLista
    {
        public List<GenerarPDF> objGenereRide { set; get; }
    }
}
