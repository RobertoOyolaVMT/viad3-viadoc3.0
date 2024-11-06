using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.Liquidacion
{
    public class LiquidacionReembolso
    {
        public string txCodPaisPagoProveedorReembolso { get; set; }
        public string txTipoProveedorReembolso { get; set; }
        public string codigo { get; set; }
        public string codigoPorcentaje { get; set; }
        public string tarifa { get; set; }

    }
}
