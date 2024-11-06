using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.factura
{
    [DataContract]
    public class FacturaDetalleReembolso
    {
        [DataMember(Order = 1)]
        public string secuencial { get; set; }
        [DataMember(Order = 2)]
        public string numFacturaProveedor { get; set; }
        [DataMember(Order = 3)]
        public string tipo { get; set; }
        [DataMember(Order = 4)]
        public string fechaEmision { get; set; }
        [DataMember(Order = 5)]
        public string numIdentificacionProveedor { get; set; }
        [DataMember(Order = 6)]
        public string txTipoIdProveedor { get; set; }
        [DataMember(Order = 7)]
        public string codPaisPagoProveedor { get; set; }
        [DataMember(Order = 8)]
        public string tipoProveedor { get; set; }
        [DataMember(Order = 9)]
        public string numAutorizacion { get; set; }
        [DataMember(Order = 10)]
        public string detalle { get; set; }
        [DataMember(Order = 11)]
        public string subtotal { get; set; }
        [DataMember(Order = 12)]
        public string subTotIvaCero { get; set; }
        [DataMember(Order = 13)]
        public string subTotIva { get; set; }
        [DataMember(Order = 14)]
        public string subTotExcentoIva { get; set; }
        [DataMember(Order = 15)]
        public string valorIVA { get; set; }
        [DataMember(Order = 16)]
        public string valorICE { get; set; }
        [DataMember(Order = 17)]
        public string valorINBPNR { get; set; }
        [DataMember(Order = 18)]
        public string total { get; set; }
        [DataMember(Order = 19)]
        public string numFactura { get; set; }
        [DataMember(Order = 20)]
        public string codigoImp { get; set; }
        [DataMember(Order = 21)]
        public string codigoPorcentajeImp { get; set; }
        [DataMember(Order = 22)]
        public string tarifaImp { get; set; }
    }
}
