using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MailKit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using MimeKit.Text;

namespace ViaDocEnvioCorreo.LogicaNegocios
{
    public class EnviarMail
    {
        readonly MimeMessage mimemail = new MimeMessage();
        readonly SmtpClient clientsmtp = new SmtpClient();
        readonly BodyBuilder builder = new BodyBuilder();
        readonly Multipart multi = new Multipart();
   


        public void servidorSTMP(String servidor, Int32 puerto, Boolean ssl, String emailCredencial, String passCredencial)
        {
            try
            {
                if (puerto == 587)
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("conectar puerto 587");
                    clientsmtp.Connect(servidor, puerto, SecureSocketOptions.StartTls);
                }
                else if (puerto == 465)
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("conectar puerto 465");
                    clientsmtp.Connect(servidor, puerto, ssl);
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("conectar otro puerto");
                    clientsmtp.Connect(servidor, puerto, SecureSocketOptions.StartTls);
                }
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("autenticar usuario y contraseña");
                clientsmtp.Authenticate(emailCredencial, passCredencial);
            }
            catch(Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Servidor " + ex.Message);
            }
        }

        public void adjuntar(String ruta)
        {
            builder.Attachments.Add(ruta);
        }

        public void adjuntar_Documento(MemoryStream doc, string name)   
        {
            try
            {
                doc.Position = 0;
                var attachmentPart = new MimePart(MediaTypeNames.Application.Pdf)
                {
                    Content = new MimeContent(doc),
                    ContentId = name,
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = name
                };
                multi.Add(attachmentPart);
            }
            catch(Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Adjuntar Documento " + ex.Message);
            }
        }

        public void llenarEmail(string MailAddressfrom, string to, string bcc, string cc, string Asunto, string body)
        {
            try
            {
                mimemail.From.Add(MailboxAddress.Parse(MailAddressfrom));
                var textPart = new TextPart(TextFormat.Html)
                {
                    Text = body,
                    ContentTransferEncoding = ContentEncoding.Base64,
                };
                multi.Add(textPart);
                if (to != null || to != string.Empty)
                {
                    String[] destinatarios = to.Split(',');
                    foreach (String email in destinatarios)
                    {
                        mimemail.To.Add(MailboxAddress.Parse(email));
                    }
                }

                if ((bcc != null) && (bcc != string.Empty))
                {
                    String[] bccs = bcc.Split(',');
                    foreach (String bc in bccs)
                    {
                        mimemail.Bcc.Add(MailboxAddress.Parse(bc));
                    }
                }

                if ((cc != null) && (cc != string.Empty))
                {
                    String[] copias = cc.Split(',');
                    foreach (String copia in copias)
                    {
                        mimemail.Cc.Add(MailboxAddress.Parse(copia));
                    }
                }
                mimemail.Subject = Asunto;
                mimemail.Body = multi;
                mimemail.Priority = MessagePriority.Urgent;
                mimemail.Importance = MessageImportance.High;
            }
            catch(Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("llenar mail " + ex.Message);
            }
            

        }

        public void AlternateViews(string htmlView)
        {
            var attachmentPart = new MimePart(MediaTypeNames.Image.Jpeg)
            {
                ContentId = htmlView,
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = htmlView
            };
            multi.Add(attachmentPart);
           builder.LinkedResources.Add(attachmentPart); 
        }

        public bool enviarEmail()
        {
            bool Enviado;
            try
            {

                clientsmtp.Send(mimemail);
               
                Enviado = true;

            }
            catch(Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("enviar mail " + ex.Message);
                Enviado = false;                
            }
            return Enviado;
        }

        public bool enviarEmail(ref string mensajeRetorno)
        {
            bool Enviado;
            try
            {

                clientsmtp.Send(mimemail);
                Enviado = true;

            }
            catch(Exception ex )
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("enviar mail " + ex.Message);
                Enviado = false;
                mensajeRetorno = "error de envio";

            }
            return Enviado;
        }

    }
}
