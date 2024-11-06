using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos;

namespace NotificacionesViaDoc.WinServ
{
    public class TimeProces
    {


        DocumentoAD _metodosDocumentos = new DocumentoAD();
        string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").Trim();

        public void StarServ()
        {
            if (!System.IO.File.Exists(@rutaXml + "\\" + "HoraServCorreo.txt"))
            {
                TextWriter tw = new StreamWriter(@rutaXml + "\\HoraServCorreo.txt", true);
                tw.WriteLine("07:00-23:00");
                tw.Close();
            }

            try
            {
                String[] ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraServCorreo.txt");
                Int32 intHoraSystema = Convert.ToInt32(DateTime.Now.ToString("H:mm").Trim().Replace(":", ""));

                foreach (String strHoraEntre in ArrayStrHorasEjecucion)
                {
                    String[] arrStrHoraEntre = strHoraEntre.Trim().Split('-');
                    Int32 intHoraInicio = Convert.ToInt32(arrStrHoraEntre[0].Trim().Replace(":", ""));
                    Int32 intHoraFin = Convert.ToInt32(arrStrHoraEntre[1].Trim().Replace(":", ""));

                    if (intHoraSystema >= intHoraInicio && intHoraSystema <= intHoraFin)
                    {

                        Timer_EnvioCorreo();

                        Timer_EnvioPortal();

                        Timer_Notificacin();

                        Timer_Estadisticas();

                        Timer_NotificacinDocError();

                        Timer_NotificacinDocAtrasados();
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
        }

        private void Timer_EnvioCorreo()
        {
            MetodosWinServ _enviaCorreo = new MetodosWinServ();
            try
            {
                _enviaCorreo.EnvioDocumentosCorreoElectronico();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion_Timer_Elapsed: " + ex.Message);
            }
        }


        private void Timer_EnvioPortal()
        {
            MetodosWinServ _enviaPortal = new MetodosWinServ();
            try
            {
                _enviaPortal.EnvioDocumentosPortalWeb();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion_Timer_Elapsed: " + ex.Message);
            }
        }

        private void Timer_Notificacin()
        {
            MetodosWinServ _enviaNotifi = new MetodosWinServ();
            try
            {
                _enviaNotifi.EnvioNotificacionCertificadoCaducado();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Timer_Notificacin: " + ex.Message);
            }
        }

        private void Timer_Estadisticas()
        {
            MetodosWinServ _enviaNotifi = new MetodosWinServ();
            try
            {
                _enviaNotifi.EnvioEstadisticaDocumentos();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Timer_Notificacin: " + ex.Message);
            }
        }

        private void Timer_NotificacinDocError()
        {
            MetodosWinServ _enviaNotifi = new MetodosWinServ();
            try
            {
                _enviaNotifi.NotificacinDocError();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Timer_Notificacin: " + ex.Message);
            }
        }

        private void Timer_NotificacinDocAtrasados()
        {
            MetodosWinServ _enviaNotifi = new MetodosWinServ();
            try
            {
                _enviaNotifi.NotificacinDocAtrasados();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Timer_Notificacin: " + ex.Message);
            }
        }

        private void Timer_Reenvio_a_Portal()
        {
            MetodosWinServ _enviaNotifi = new MetodosWinServ();
            try
            {
                _enviaNotifi.Reenvio_a_Portal();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Timer_Notificacin: " + ex.Message);
            }
        }
    }
}
