using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class RideGuiaRemision
    {
        public bool BanderaGeneracionObjeto { get; set; }
        public string NumeroAutorizacion { get; set; }
        public string FechaHoraAutorizacion { get; set; }
        public InformacionTributaria _infoTributaria { get; set; }
        public InformacionGuiaRemision _infoGuiaRemision { get; set; }
        public List<Destinatario> _destinatarios { get; set; }
        public List<InformacionAdicional> _infoAdicional { get; set; }
        public RideGuiaRemision()
        {
            BanderaGeneracionObjeto = false;
            NumeroAutorizacion = "";
            FechaHoraAutorizacion = "";
            _infoTributaria = new InformacionTributaria();
            _infoGuiaRemision = new InformacionGuiaRemision();
            _destinatarios = new List<Destinatario>();
            _infoAdicional = new List<InformacionAdicional>();
        }
    }
}
