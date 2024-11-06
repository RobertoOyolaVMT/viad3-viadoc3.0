using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class Impuesto
    {
        public string Codigo { get; set; }
        public string CodigoPorcentaje { get; set; }
        public string Tarifa { get; set; }
        public string BaseImponible { get; set; }
        public string Valor { get; set; }

        public Impuesto()
        {
            Codigo = "";
            CodigoPorcentaje = "";
            Tarifa = "";
            BaseImponible = "";
            Valor = "";
        }
    }
}
