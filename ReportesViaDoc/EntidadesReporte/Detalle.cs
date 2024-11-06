using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class Detalle
    {
        public string CodigoPrincipal { get; set; }
        public string CodigoAuxiliar { get; set; }
        public string Descripcion { get; set; }
        public string Cantidad { get; set; }
        public string PrecioUnitario { get; set; }
        public string Descuento { get; set; } 
        public string PrecioTotalSinImpuesto { get; set; }
        public List<Impuesto> _impuestos { get; set; }
        public Detalle()
        {
            CodigoPrincipal = "";
            CodigoAuxiliar = "";
            Descripcion = "";
            Cantidad = "";
            PrecioUnitario = "";
            Descuento = "";
            PrecioTotalSinImpuesto = "";
            _impuestos = new List<Impuesto>();
        }
    }
}
