using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class RideFactura
    {
        public bool BanderaGeneracionObjeto { get; set; }
        public string FechaHoraAutorizacion { get; set; }
        public string NumeroAutorizacion { get; set; }
        public InformacionTributaria _infoTributaria { get; set; }
        public InformacionFactura _infoFactura { get; set; }
        public List<Detalle> _detalles { get; set; }
        public List<InformacionAdicional> _infosAdicional { get; set; }
        public List<ReembolsoGasto> _reembolsosGastos { get; set; }

        public RideFactura()
        {
            BanderaGeneracionObjeto = false;
            FechaHoraAutorizacion = "";
            NumeroAutorizacion = "";
            _infoTributaria = new InformacionTributaria();
            _infoFactura = new InformacionFactura();
            _detalles = new List<Detalle>();
            _infosAdicional = new List<InformacionAdicional>();
        }
    }
}
