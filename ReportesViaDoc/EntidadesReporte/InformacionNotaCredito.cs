using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class InformacionNotaCredito
    {
        public string FechaEmision { get; set; }
        public string DirEstablecimiento { get; set; }
        public string TipoIdentificacion { get; set; }
        public string RazonSocial { get; set; }
        public string Identificacion { get; set; }
        public string NumeroContribuyenteEspecial { get; set; }
        public string ObligadoContabilidad { get; set; }
        public string CodDocModificado { get; set; }
        public string NumDocModificado { get; set; }
        public string FechaEmisionDocSustento { get; set; }
        public string TotalSinImpuestos { get; set; }
        public string ValorModificacion { get; set; }
        public string Moneda { get; set; }
        public List<TotalConImpuesto> _totalesConImpuesto { get; set; }
        public string Motivo { get; set; }

        public InformacionNotaCredito()
        {
            FechaEmision = "";
            DirEstablecimiento = "";
            TipoIdentificacion = "";
            RazonSocial = "";
            Identificacion = "";
            NumeroContribuyenteEspecial = "";
            ObligadoContabilidad = "";
            CodDocModificado = "";
            NumDocModificado = "";
            FechaEmisionDocSustento = "";
            TotalSinImpuestos = "";
            ValorModificacion = "";
            Moneda = "";
            _totalesConImpuesto = new List<TotalConImpuesto>();
            Motivo = "";
        }
    }
}
