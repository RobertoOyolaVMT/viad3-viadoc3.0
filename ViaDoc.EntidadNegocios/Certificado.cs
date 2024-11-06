using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios
{
    public class CertificadoLista
    {
        public List<Certificado> objListaCertificado { get; set; }
    }


    public class Certificado
    {
        public int CiCertificado { get; set; }
        public int CiCompania { get; set; }
        public Guid UiSemilla { get; set; }
        public String TxClave { get; set; }
        public String txKey { get; set; }
        public byte[] ObCertificado { get; set; }
        public DateTime FcDesde { get; set; }
        public DateTime FcHasta { get; set; }
        public String CiEstado { get; set; }
        public string razonSocial { get; set; }
        public string ruc { get; set; }
        public String estado { get; set; }
        public Certificado()
        {
            this.CiCertificado = 0;
            this.CiCompania = 0;
            this.UiSemilla = new Guid();
            this.TxClave = "";
            this.txKey = "";
            this.ObCertificado = null;
            this.FcDesde = new DateTime();
            this.FcHasta = new DateTime();
            this.CiEstado = "";
        }
    }

    public class SucuersalCompania 
    {
        public string secuencialCia { get; set; }
        public string newIdcompañia { get; set; }
    }

    public class SucuersalCompanialista
    {
        public List<SucuersalCompania> ObjSC = new List<SucuersalCompania>();
    }
}
