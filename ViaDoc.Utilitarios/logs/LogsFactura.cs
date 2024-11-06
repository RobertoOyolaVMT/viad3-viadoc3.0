using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.Utilitarios.logs
{
    public class LogsFactura
    {

        public static void LogsInicioFin(string mensaje)
        {
            try
            {
                if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs"))
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs");
                System.IO.StreamWriter sb = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\MigradorViaDoc" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
                sb.Write("[CustomError]: " + DateTime.Now.ToString() + "\r\n");
                sb.Write("[Metodo]: " + mensaje + "\r\n");
                sb.Write("[CustomError]: FIN \r\n");

                sb.Close();
            }
            catch (Exception ex)
            { }
        }

        public static void grabaLogsException(string Metodo, string service, string Message, string StackTrace)
        {
            try
            {
                if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs"))
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs");
                System.IO.StreamWriter sb = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\MigradorViaDoc" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
                sb.Write("[CustomError]: " + DateTime.Now.ToString() + "\r\n");
                sb.Write("[Metodo]: " + Metodo + "\r\n");
                sb.Write("[Service]: " + service + "\r\n");
                sb.Write("[Message]: " + Message + "\r\n");
                sb.Write("[StackTrace]: " + StackTrace + "\r\n");
                sb.Write("[CustomError]: FIN \r\n");

                sb.Close();
            }
            catch (Exception ex)
            { }
        }

        public static void grabaLogsSeguimiento(string Metodo, string service, string Message)
        {
            try
            {
                if (bool.Parse(ConfigurationManager.AppSettings["Logs"].ToString()))
                {
                    if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs"))
                        System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs");
                    System.IO.StreamWriter sb = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\MigradorViaDoc" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
                    sb.Write("[CustomError]: " + DateTime.Now.ToString() + "\r\n");
                    sb.Write("[Metodo]: " + Metodo + "\r\n");
                    sb.Write("[Service]: " + service + "\r\n");
                    sb.Write("[Message]: " + Message + "\r\n");
                    sb.Write("[CustomError]: FIN \r\n");

                    sb.Close();
                }
            }
            catch (Exception ex)
            { }
        }
    }

    // Hola mmm
}
