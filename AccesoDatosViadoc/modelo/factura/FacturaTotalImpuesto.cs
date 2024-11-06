using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatosViadoc.modelo.factura
{
    public class FacturaTotalImpuesto
    {
        public string ciCompania { get; set; }
        public string txEstablecimiento { get; set; }
        public string txPuntoEmision { get; set; }
        public string txSecuencial { get; set; }
        public string txCodigo { get; set; }
        public string txCodigoPorcentaje { get; set; }
        public string txTarifa { get; set; }
        public string qnDescuentoAdicional { get; set; }
        public string qnBaseImponible { get; set; }
        public string qnValor { get; set; }
    }
}
