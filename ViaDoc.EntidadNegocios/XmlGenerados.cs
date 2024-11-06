using System;
using System.Linq;

namespace ViaDoc.EntidadNegocios
{
    public class XmlGenerados
    {
        public int Identity { get; set; }
        public int CiCompania { get; set; }
        public int CiContingenciaDet { get; set; }
        public String CiTipoEmision { get; set; }
        public String CiTipoDocumento { get; set; }
        public String NameXml { get; set; }
        public String XmlComprobante { get; set; }
        public String ClaveAcceso { get; set; }
        public String TxNumeroAutorizacion { get; set; }
        public String txFechaHoraAutorizacion { get; set; }
        public String XmlEstado { get; set; }
        public String MensajeError { get; set; }
        public Byte[] Rider { get; set; }
        public Byte[] xmlComprobanteByte { get; set; }
        public String MailCliente { get; set; }
        public String ciEstadoEnvioPortal { get; set; }
        public String txCodError { get; set; }
        //Agregado
        public String txTarifa { get; set; }
        //Agregado Email
        public String Email { get; set; }
        public String IdentificacionComprador { get; set; }
        public String RazonSocialComprador { get; set; }
        public int ciNumeroIntento { get; set; }
        public string rucCompania { get; set; }
        public string numeroDocumento { get; set; }
        public string fechaEmision { get; set; }



        public XmlGenerados()
        {
            this.Identity = 0;
            this.CiCompania = 0;
            this.CiTipoDocumento = string.Empty;
            this.NameXml = string.Empty;
            this.XmlComprobante = string.Empty;
            this.TxNumeroAutorizacion = string.Empty;
            this.txFechaHoraAutorizacion = string.Empty;
            this.XmlEstado = string.Empty;
            this.CiContingenciaDet = 0;
            this.MensajeError = string.Empty;
            this.Rider = null;
            this.xmlComprobanteByte = null;
            this.MailCliente = null;
            this.txCodError = "0";
            this.txTarifa = "0";
            this.Email = string.Empty;
            this.IdentificacionComprador = string.Empty;
            this.RazonSocialComprador = string.Empty;
            this.ciNumeroIntento = 0;
            this.rucCompania = string.Empty;
            this.numeroDocumento = string.Empty;
            this.fechaEmision = string.Empty;
        }
    }
}