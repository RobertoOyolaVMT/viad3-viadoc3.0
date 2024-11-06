using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class ImpuestoRetencion
    {
        public string Codigo { get; set; }
        public string CodigoRetencion { get; set; }
        public string BaseImponible { get; set; }
        public string PorcentajeRetener { get; set; }
        public string ValorRetenido { get; set; }
        public string CodDocSustento { get; set; }
        public string NumDocSustento { get; set; }
        public string FechaEmisionDocSustento { get; set; }

        public ImpuestoRetencion()
        {
            Codigo = "";
            CodigoRetencion = "";
            BaseImponible = "";
            PorcentajeRetener = "";
            ValorRetenido = "";
            CodDocSustento = "";
            NumDocSustento = "";
            FechaEmisionDocSustento = "";
        }

    }
}
