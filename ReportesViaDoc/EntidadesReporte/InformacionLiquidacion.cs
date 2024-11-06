using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    class InformacionLiquidacion
    {
        public string FechaEmision { get; set; }
        public string DirEstablecimiento { get; set; }
        public string NumeroContribuyenteEspecial { get; set; }
        public string ObligadoContabilidad { get; set; }
        public string TipoIdentificacion { get; set; }
        public string RazonSocial { get; set; }
        public string Identificacion { get; set; }
        public string GuiaRemision { get; set; }
        public string TotalSinImpuestos { get; set; }
        public string TotalDescuento { get; set; }
        public List<TotalConImpuesto> _totalesConImpuesto { get; set; }
        public string Propina { get; set; }
        public string ImporteTotal { get; set; }
        public string Moneda { get; set; }
        public List<FormaPago> _formasPago { get; set; }

        public InformacionLiquidacion()
        {
            FechaEmision = "";
            DirEstablecimiento = "";
            NumeroContribuyenteEspecial = "";
            ObligadoContabilidad = "";
            TipoIdentificacion = "";
            RazonSocial = "";
            Identificacion = "";
            GuiaRemision = "";
            TotalSinImpuestos = "";
            TotalDescuento = "";
            _totalesConImpuesto = new List<TotalConImpuesto>();
            Propina = "";
            ImporteTotal = "";
            Moneda = "";
            _formasPago = new List<FormaPago>();
        }
    }
}
