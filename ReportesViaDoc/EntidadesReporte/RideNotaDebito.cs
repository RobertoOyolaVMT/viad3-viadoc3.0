using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class RideNotaDebito
    {
        public bool BanderaGeneracionObjeto { get; set; }
        public string FechaHoraAutorizacion { get; set; }
        public string NumeroAutorizacion { get; set; }
        public InformacionTributaria _infoTributaria { get; set; }

        public InformacionNotaDebito _infoNotaDebito { get; set; }
        public List<Motivo> _motivos { get; set; }
        public List<InformacionAdicional> _infoAdicional { get; set; }

        public RideNotaDebito()
        {
            BanderaGeneracionObjeto = false;
            FechaHoraAutorizacion = "";
            NumeroAutorizacion = "";
            _infoTributaria = new InformacionTributaria();
            _infoNotaDebito = new InformacionNotaDebito();
            _motivos = new List<Motivo>();
            _infoAdicional = new List<InformacionAdicional>();
        }
    }
}
