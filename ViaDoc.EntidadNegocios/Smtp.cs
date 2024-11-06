using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios
{

    public class SmtpLista
    {
        public List<Smtp> objListSmtp { get; set; }
    }

    public class Smtp
    {
        public string CiCompania { get; set; }
        public string RazonSocial { get; set; }
        public string RucCompania { get; set; }
        public string HostServidor { get; set; }
        public string Puerto { get; set; }
        public string EnableSsl { get; set; }
        public string EmailCredencial { get; set; }
        public string PassCredencial { get; set; }
        public string MailAddressfrom { get; set; }
        public string Para { get; set; }
        public string Cc { get; set; }
        public string Asunto { get; set; }
        public string UrlPortal { get; set; }
        public string UrlCompania { get; set; }
        public string ActivarNotificacion { get; set; }

        public Smtp()
        {
            this.CiCompania = "";
            this.HostServidor = "";
            this.Puerto = "";
            this.EnableSsl = "";
            this.EmailCredencial = "";
            this.PassCredencial = "";
            this.MailAddressfrom = "";
            this.Para = "";
            this.Cc = "";
            this.Asunto = "";
            this.UrlPortal = "";
            this.RazonSocial = "";
            this.RucCompania = "";
            this.UrlCompania = string.Empty;
            this.ActivarNotificacion = string.Empty;
        }
    }
}
