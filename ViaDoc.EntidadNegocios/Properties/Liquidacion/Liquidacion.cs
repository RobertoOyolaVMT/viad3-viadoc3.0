using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.Liquidacion
{
    [DataContract]
    public class Liquidacion
    {
        [DataMember(Order = 1)]
        public int idLiquidacion { get; set; }
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
        public string tipoIdentificacionProvedor { get; set; }
        [DataMember(Order = 13)]
        public string razonSocialProvedor { get; set; }
        [DataMember(Order = 14)]
        public string identificacionProvedor { get; set; }
        [DataMember(Order = 15)]
        public decimal totalSinImpuestos { get; set; }
        [DataMember(Order = 16)]
        public decimal totalDescuento { get; set; }
        [DataMember(Order = 17)]
        public string CodDocReembolso { get; set; }
        [DataMember(Order = 18)]
        public decimal totalComprobantesReembolso { get; set; }
        [DataMember(Order = 19)]
        public decimal totalBaseImponibleReembolso { get; set; }
        [DataMember(Order = 20)]
        public decimal totalImpuestoReembolso { get; set; }
        [DataMember(Order = 21)]
        public decimal importeTotal { get; set; }
        [DataMember(Order = 22)]
        public string moneda { get; set; }
        [DataMember(Order = 23)]
        public int contingenciaDet { get; set; }
        [DataMember(Order = 24)]
        public string email { get; set; }
        [DataMember(Order = 25)]
        public string estado { get; set; }
        [DataMember(Order = 26)]
        public string ambiente { get; set; }
        [DataMember(Order = 27)]
        public string xmlDocumentoAutorizado { get; set; }
        [DataMember(Order = 28)]
        public string codigoNumerico { get; set; }
        [DataMember(Order = 29)]
        public string ruc { get; set; }

        [DataMember]
        public List<LiquidacionDetalle> Liquidaciondetalle { get; set; }
        [DataMember]
        public List<LiquidacionTotalImpuesto> LiquidaciontotalImpuesto { get; set; }
        [DataMember]
        public List<LiquidacionDetalleFormaPago> LiquidacionformaPago { get; set; }
        [DataMember]
        public List<LiquidacionInfoAdicional> LiquidacioninfoAdicional { get; set; }   
        [DataMember]
        public List<LiquidacionReembolso> LiquidacionReembolso { get; set; }

        public Liquidacion()
        {
            Liquidaciondetalle = new List<LiquidacionDetalle>();
            LiquidaciontotalImpuesto = new List<LiquidacionTotalImpuesto>();
            LiquidacionformaPago = new List<LiquidacionDetalleFormaPago>();
            LiquidacioninfoAdicional = new List<LiquidacionInfoAdicional>();
            LiquidacionReembolso = new List<LiquidacionReembolso>();
        }
    }
}
