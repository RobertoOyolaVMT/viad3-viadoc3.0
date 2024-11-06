using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDocEnvioCorreo.Negocios
{
    class Mappeo
    {
        public List<MdSmtp> MpSmtp(DataTable DtSmtp)
        {
            List<MdSmtp> MpSmtp = new List<MdSmtp>();

            for(var i = 0; i< DtSmtp.Rows.Count; i++)
            {
                MdSmtp Msto = new MdSmtp();
                Msto.ciCompania = Convert.ToInt32(DtSmtp.Rows[i]["ciCompania"].ToString().Trim());
                Msto.HostServidor = DtSmtp.Rows[i]["HostServidor"].ToString().Trim();
                Msto.puerto = Convert.ToInt32(DtSmtp.Rows[i]["puerto"].ToString().Trim());
                Msto.EnableSsl = DtSmtp.Rows[i]["EnableSsl"].ToString().Trim();
                Msto.emailCredencial = DtSmtp.Rows[i]["emailCredencial"].ToString().Trim();
                Msto.passCredencial = DtSmtp.Rows[i]["passCredencial"].ToString().Trim();
                Msto.MailAddressfrom = DtSmtp.Rows[i]["MailAddressfrom"].ToString().Trim();
                Msto.para = DtSmtp.Rows[i]["para"].ToString().Trim();
                Msto.cc = DtSmtp.Rows[i]["cc"].ToString().Trim();
                Msto.Asunto = DtSmtp.Rows[i]["Asunto"].ToString().Trim();
                Msto.urlPortal = DtSmtp.Rows[i]["urlPortal"].ToString().Trim();
                Msto.activarNotificacion = Convert.ToBoolean(DtSmtp.Rows[i]["activarNotificacion"].ToString().Trim());

                MpSmtp.Add(Msto);
            }

            return MpSmtp;
        }
    }
}
