using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class InformacionTributaria
    {
        public string Ambiente { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string ClaveAcceso { get; set; }
        public string Establecimiento { get; set; }
        public string PuntoEmision { get; set; }
        public string Secuencial { get; set; }
        public string Ruc { get; set; }
        public string TipoEmision { get; set; }
        public string DirMatriz { get; set; }
        public string regimenMicroempresas { get; set; }
        public string AgenteRetencion { get; set; }
        public string contribuyenteRimpe { get; set; }
        public string regimenGeneral { get; set; } 
        public string CodigoDocumento { get; set; }

        public InformacionTributaria()
        {
            Ambiente = "";
            RazonSocial = "";
            NombreComercial = "";
            DirMatriz = "";
            regimenMicroempresas = "";
            AgenteRetencion = "";
            regimenGeneral = "";
            CodigoDocumento = "";
            ClaveAcceso = "";
            Establecimiento = "";
            PuntoEmision = "";
            Secuencial = "";
            Ruc = "";
            TipoEmision = "";
            contribuyenteRimpe = "";
        }

    }
}
