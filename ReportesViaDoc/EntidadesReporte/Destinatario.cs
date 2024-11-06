using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class Destinatario
    {
        public string IdentificacionDestinatario { get; set; }
        public string RazonSocialDestinatario { get; set; }
        public string DirDestinatario { get; set; }
        public string MotivoTraslado { get; set; }
        public string Ruta { get; set; }
        public string CodDocSustento { get; set; }
        public string NumDocSustento { get; set; }
        public string NumAutDocSustento { get; set; }
        public string FechaEmisionDocSustento { get; set; }
        public List<DetalleDestinatario> _detallesDestinatario { get; set; }
        public Destinatario()
        {
            IdentificacionDestinatario = "";
            RazonSocialDestinatario = "";
            DirDestinatario = "";
            MotivoTraslado = "";
            Ruta = "";
            CodDocSustento = "";
            NumAutDocSustento = "";
            FechaEmisionDocSustento = "";
            _detallesDestinatario = new List<DetalleDestinatario>();
        }
    }
}
