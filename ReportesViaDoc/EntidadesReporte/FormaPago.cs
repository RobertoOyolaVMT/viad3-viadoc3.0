using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class FormaPago
    {
        public string CodigoFormaPago { get; set; }
        public string Total { get; set; }
        public string Plazo { get; set; }
        public string UnidadTiempo { get; set; }
        public FormaPago()
        {
            CodigoFormaPago = "";
            Total = "";
            Plazo = "";
            UnidadTiempo = "";
        }

    }
}
