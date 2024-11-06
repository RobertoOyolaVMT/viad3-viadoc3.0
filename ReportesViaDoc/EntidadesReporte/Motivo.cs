using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class Motivo
    {
        public string Razon { get; set; }
        public string Valor { get; set; }
        public Motivo()
        {
            Razon = "";
            Valor = "";
        }
    }
}
