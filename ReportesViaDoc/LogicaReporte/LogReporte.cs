using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;


namespace ReportesViaDoc.LogicaReporte
{
    public class LogReporte
    {
        public static void GuardaLog1(string strExcepcion)
        {
            DateTime fecha = DateTime.Now;
            bool activar = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ActGuardarExcepcionesFichero"));
            if (activar)
            {
                string archivo = ConfigurationManager.AppSettings.Get("archivoExcepciones");
                string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").ToString() + "\\" + ConfigurationManager.AppSettings.Get("subCarpetaPortal") + "\\" + fecha.ToShortDateString().Replace("/", "-");
                if (!Directory.Exists(rutaXml))
                {
                    Directory.CreateDirectory(rutaXml);
                }
                TextWriter tw = new StreamWriter(rutaXml + "\\" + archivo + ".txt", true);
                tw.WriteLine("<" + DateTime.Now.ToString() + "> " + strExcepcion);
                tw.Close();
            }
        }

    }
}
