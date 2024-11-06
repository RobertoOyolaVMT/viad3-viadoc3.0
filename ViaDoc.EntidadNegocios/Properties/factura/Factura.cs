using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.factura
{

    [DataContract]
    public class Factura
    {
        [DataMember (Order =1)]
        public int idFactura { get; set; }
        [DataMember(Order = 2)]
        public int compania { get; set; }
        [DataMember(Order = 3)]
        public string establecimiento { get; set; }
        [DataMember(Order = 4)]
        public string puntoEmision { get; set; }
        [DataMember(Order = 5)]
        public string secuencial { get; set; }
        [DataMember(Order = 6)]
        public string fechaEmision { get; set; }
        [DataMember(Order = 7)]
        public string claveAcceso { get; set; }
        [DataMember(Order = 8)]
        public int tipoEmision { get; set; }
        [DataMember(Order = 9)]
        public string tipoDocumento { get; set; }
        [DataMember(Order = 10)]
        public string direccionEstablecimiento { get; set; }
        [DataMember(Order = 11)]
        public string tipoIdentificacionComprador { get; set; }
        [DataMember(Order = 12)]
        public string guiaRemision { get; set; }
        [DataMember(Order = 13)]
        public string razonSocialComprador { get; set; }
        [DataMember(Order = 14)]
        public string identificacionComprador { get; set; }
        [DataMember(Order = 15)]
        public decimal totalSinImpuestos { get; set; }
        [DataMember(Order = 16)]
        public decimal totalDescuento { get; set; }
        [DataMember(Order = 17)]
        public decimal propina { get; set; }
        [DataMember(Order = 18)]
        public decimal importeTotal { get; set; }
        [DataMember(Order = 19)]
        public string moneda { get; set; }
        [DataMember(Order = 20)]
        public int contingenciaDet { get; set; }
        [DataMember(Order = 21)]
        public string email { get; set; }
        [DataMember(Order = 22)]
        public string estado { get; set; }
        [DataMember(Order = 23)]
        public string ambiente { get; set; }
        [DataMember(Order = 24)]
        public string xmlDocumentoAutorizado { get; set; }
        [DataMember(Order = 25)]
        public string codigoNumerico { get; set; }
        [DataMember(Order = 26)]
        public string ruc { get; set; }

        [DataMember]
        public List<FacturaDetalle> detalleFactura { get; set; }
        [DataMember]
        public List<FacturaTotalImpuesto> totalImpuesto { get; set; }
        [DataMember]
        public List<FacturaDetalleFormaPago> formaPago { get; set; }
        [DataMember]
        public List<FacturaInfoAdicional> infoAdicional { get; set; }
        public Factura()
        {
            detalleFactura = new List<FacturaDetalle>();
            totalImpuesto = new List<FacturaTotalImpuesto>();
            formaPago = new List<FacturaDetalleFormaPago>();
            infoAdicional = new List<FacturaInfoAdicional>();
        }
    }
}
