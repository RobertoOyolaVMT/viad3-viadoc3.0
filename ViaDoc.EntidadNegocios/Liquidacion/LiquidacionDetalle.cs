using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.Liquidacion
{
    [DataContract]
    public class LiquidacionDetalle
    {
        [DataMember]
        public string codigoPrincipal { get; set; }
        [DataMember]
        public string codigoAuxiliar { get; set; }
        [DataMember]
        public string descripcion { get; set; }
        [DataMember]
        public int cantidad { get; set; }
        [DataMember]
        public decimal precioUnitario { get; set; }
        [DataMember]
        public decimal descuento { get; set; }
        [DataMember]
        public decimal precioTotalSinImpuesto { get; set; }
        [DataMember]
        public List<LiquidacionDetalleImpuesto> LiquidaciondetalleImpuesto = new List<LiquidacionDetalleImpuesto>();
        [DataMember]
        public List<LiquidacionDetalleAdicional> LiquidaciondetalleAdicional = new List<LiquidacionDetalleAdicional>();

        public LiquidacionDetalle()
        {
            LiquidaciondetalleImpuesto = new List<LiquidacionDetalleImpuesto>();
            LiquidaciondetalleAdicional = new List<LiquidacionDetalleAdicional>();
        }

    }
}
