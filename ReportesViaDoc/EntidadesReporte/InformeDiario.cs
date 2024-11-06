using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class InformeDiario
    {
        public string Documento { get; set; }
        public InformeDiarioColumna _columnaInforme { get; set; }

        public InformeDiario()
        {
            Documento = "";
            _columnaInforme = new InformeDiarioColumna();
        }
    }
}
