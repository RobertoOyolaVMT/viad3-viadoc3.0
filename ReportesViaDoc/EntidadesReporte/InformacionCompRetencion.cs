using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class InformacionCompRetencion
    {

        public string FechaEmision { get; set; }
        public string DirEstablecimiento { get; set; }
        public string NumeroContribuyenteEspecial { get; set; }
        public string ObligadoContabilidad { get; set; }
        public string TipoIdentificacion { get; set; }
        public string RazonSocial { get; set; }
        public string Identificacion { get; set; }
        public string PeriodoFiscal { get; set; } //campo usado para retenciones

        public InformacionCompRetencion()
        {
            FechaEmision = "";
            DirEstablecimiento = "";
            ObligadoContabilidad = "";
            NumeroContribuyenteEspecial = "";
            TipoIdentificacion = "";
            RazonSocial = "";
            Identificacion = "";
            PeriodoFiscal = "";

        }

    }
}
