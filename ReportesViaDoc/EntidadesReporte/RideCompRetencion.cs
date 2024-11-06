using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class RideCompRetencion
    {
        public bool BanderaGeneracionObjeto { get; set; }
        public string FechaHoraAutorizacion { get; set; }
        public string NumeroAutorizacion { get; set; }
        public InformacionTributaria _infoTributaria { get; set; }
        public InformacionCompRetencion _infoCompRetencion { get; set; }
        public List<ImpuestoRetencion> _impuestos { get; set; }
        public List<InformacionAdicional> _infosAdicional { get; set; }

        public RideCompRetencion()
        {
            BanderaGeneracionObjeto = false;
            FechaHoraAutorizacion = "";
            NumeroAutorizacion = "";
            _infoTributaria = new InformacionTributaria();
            _infoCompRetencion = new InformacionCompRetencion();
            _impuestos = new List<ImpuestoRetencion>();
            _infosAdicional = new List<InformacionAdicional>();
        }

    }
}
