using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace AutorizacionViaDoc.WinServ
{
    public class TimerProces
    {
        string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").Trim();
        public void StarServ()
        {
            if (!System.IO.File.Exists(@rutaXml + "\\" + "HoraServAutorizacion.txt"))
            {
                TextWriter tw = new StreamWriter(@rutaXml + "\\HoraServAutorizacion.txt", true);
                tw.WriteLine("07:00-23:00");
                tw.Close();
            }

            try
            {
                String[] ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraServAutorizacion.txt");
                Int32 intHoraSystema = Convert.ToInt32(DateTime.Now.ToString("H:mm").Trim().Replace(":", ""));

                foreach (String strHoraEntre in ArrayStrHorasEjecucion)
                {

                    String[] arrStrHoraEntre = strHoraEntre.Trim().Split('-');
                    Int32 intHoraInicio = Convert.ToInt32(arrStrHoraEntre[0].Trim().Replace(":", ""));
                    Int32 intHoraFin = Convert.ToInt32(arrStrHoraEntre[1].Trim().Replace(":", ""));

                    if (intHoraSystema >= intHoraInicio && intHoraSystema <= intHoraFin)
                    {
                        Timer_Elapsed_Factura();

                        Timer_Elapsed_Liqudiacion();

                        Timer_Elapsed_CompRetencion();

                        Timer_Elapsed_GuiaRemision();

                        Timer_Elapsed_NotaCredito();

                        Timer_Elapsed_NotaDebito();

                        Timer_Elapsed_Reproceso();
                    }
                }
            }
            catch (Exception e)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Rangos de tiempo: " + e.Message);
            }
        }

        private void Timer_Elapsed_Factura()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            try
            {
                metodos.GenerarAutorizacionRecepcionFactura();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion Timer_Elapsed_Factura: " + ex.Message);
            }
        }

        private void Timer_Elapsed_Liqudiacion()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            try
            {
                metodos.GenerarAutorizacionRecepcionLiquidacion();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion Timer_Elapsed_Factura: " + ex.Message);
            }
        }

        private void Timer_Elapsed_CompRetencion()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            try
            {
                metodos.GenerarAutorizacionRecepcionCompRecepcion();

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: Timer_Elapsed_CompRetencion" + ex.Message);
            }
        }

        private void Timer_Elapsed_NotaCredito()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            try
            {
                metodos.GenerarAutorizacionRecepcionNotaCredito();

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: Timer_Elapsed_CompRetencion" + ex.Message);
            }
        }

        private void Timer_Elapsed_NotaDebito()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            try
            {
                metodos.GenerarAutorizacionRecepcionNotaDebito();

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: Timer_Elapsed_CompRetencion" + ex.Message);
            }
        }

        private void Timer_Elapsed_GuiaRemision()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            try
            {
                metodos.GenerarAutorizacionRecepcionGuiaRemision();

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: Timer_Elapsed_CompRetencion" + ex.Message);
            }
        }

        private void Timer_Elapsed_Reproceso()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            try
            {
                metodos.ReprocesoDoc();

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: Timer_Elapsed_CompRetencion" + ex.Message);
            }
        }
    }
}