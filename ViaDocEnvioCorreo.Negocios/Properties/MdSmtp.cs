using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDocEnvioCorreo.Negocios
{
    public class MdSmtp
    {
        public int ciCompania { get; set; }
        public string HostServidor { get; set; }
        public int puerto { get; set; }
        public string EnableSsl { get; set; }
        public string emailCredencial { get; set; }
        public string passCredencial { get; set; }
        public string MailAddressfrom { get; set; }
        public string para { get; set; }
        public string cc { get; set; }
        public string Asunto { get; set; }
        public string urlPortal { get; set; }
        public bool activarNotificacion { get; set; }


    }
}
