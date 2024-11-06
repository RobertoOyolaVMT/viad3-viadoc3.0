using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class InformeDiarioColumna
    {
        public string Columna { get; set; }
        public string Valor { get; set; }

        public InformeDiarioColumna()
        {
            Columna = "";
            Valor = "";
        }

    }
}
