using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    class RideLiquidacion
    {
        public bool BanderaGeneracionObjeto { get; set; }
        public string FechaHoraAutorizacion { get; set; }
        public string NumeroAutorizacion { get; set; }
        public InformacionTributaria _infoTributaria { get; set; }
        public InformacionLiquidacion _infoLiquidacion { get; set; }
        public List<Detalle> _detalles { get; set; }
        public List<InformacionAdicional> _infosAdicional { get; set; }
        public RideLiquidacion()
        {
            BanderaGeneracionObjeto = false;
            FechaHoraAutorizacion = "";
            NumeroAutorizacion = "";
            _infoTributaria = new InformacionTributaria();
            _infoLiquidacion = new InformacionLiquidacion();
            _detalles = new List<Detalle>();
            _infosAdicional = new List<InformacionAdicional>();
        }
    }
}
