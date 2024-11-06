using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class DetalleDestinatario
    {
        public string CodigoInterno { get; set; }
        public string CodigoAdicional { get; set; }
        public string Descripcion { get; set; }
        public string Cantidad { get; set; }

        public DetalleDestinatario()
        {
            CodigoInterno = "";
            CodigoAdicional = "";
            Descripcion = "";
            Cantidad = "";
        }
    }
}
