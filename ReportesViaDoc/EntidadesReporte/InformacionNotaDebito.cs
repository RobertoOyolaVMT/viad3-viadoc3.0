using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class InformacionNotaDebito
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
        public List<Impuesto> _impuestos { get; set; }
        public string ValorTotal { get; set; }

        public InformacionNotaDebito()
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
            _impuestos = new List<Impuesto>();
            ValorTotal = "";
        }


    }
}
