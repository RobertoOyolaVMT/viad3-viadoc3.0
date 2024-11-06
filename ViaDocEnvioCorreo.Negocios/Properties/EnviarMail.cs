using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ViaDocEnvioCorreo.LogicaNegocios
{
    public class EnviarMail
    {
        SmtpClient mSmtpClient = new SmtpClient();
        MailMessage mMailMessage = new MailMessage();

        public void servidorSTMP(String servidor, Int32 puerto, Boolean ssl, String emailCredencial, String passCredencial)
        {
            mSmtpClient.Host = servidor; //aqui poner el smtp de mexexpress
            mSmtpClient.Port = puerto;
            mSmtpClient.EnableSsl = ssl;
            mSmtpClient.UseDefaultCredentials = false;
            mSmtpClient.Credentials = new System.Net.NetworkCredential(emailCredencial, passCredencial);
        }

        public void adjuntar(String ruta)
        {
            mMailMessage.Attachments.Add(new Attachment(ruta));
        }

        public void adjuntar_Documento(MemoryStream doc, string name)
        {
            mMailMessage.Attachments.Add(new Attachment(doc, name));
        }

        public void llenarEmail(string MailAddressfrom, string to, string bcc, string cc, string Asunto, string body)
        {
            mMailMessage.From = new MailAddress(MailAddressfrom);
            String[] destinatarios = to.Split(',');
            foreach (String email in destinatarios)
            {
                mMailMessage.To.Add(new MailAddress(email));
            }
            if ((bcc != null) && (bcc != string.Empty)) mMailMessage.Bcc.Add(new MailAddress(bcc));
            if ((cc != null) && (cc != string.Empty)) mMailMessage.CC.Add(new MailAddress(cc));

            mMailMessage.Subject = Asunto;
            mMailMessage.Body = body;
            mMailMessage.IsBodyHtml = true;
            mMailMessage.Priority = MailPriority.Normal;
        }

        public void AlternateViews(AlternateView htmlView)
        {
            mMailMessage.AlternateViews.Add(htmlView);
        }

        public bool enviarEmail()
        {
            bool Enviado;
            try
            {

                mSmtpClient.Send(mMailMessage);
                Enviado = true;

            }
            catch (SmtpException smtpex)
            {
                Enviado = false;                
            }
            return Enviado;
        }

        public bool enviarEmail(ref string mensajeRetorno)
        {
            bool Enviado;
            try
            {

                mSmtpClient.Send(mMailMessage);
                Enviado = true;

            }
            catch (SmtpException smtpex)
            {
                Enviado = false;
                mensajeRetorno = smtpex.Message;

            }
            return Enviado;
        }

    }
}
