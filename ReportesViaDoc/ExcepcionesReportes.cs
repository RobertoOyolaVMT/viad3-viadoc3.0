using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc
{
    public class ExcepcionesReportes
    {
        public void GuardaReporte(string strExcepcion)
        {

            string archivo = "logReportesNuevos";
            DateTime fecha = DateTime.Now;
            string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").ToString();
            if (!Directory.Exists(@rutaXml))
            {
                Directory.CreateDirectory(@rutaXml);
            }
            TextWriter tw = new StreamWriter(@rutaXml + "\\" + archivo + ".txt", true);
            tw.WriteLine("<" + DateTime.Now.ToString() + "> " + strExcepcion);
            tw.Close();

        }
    }
}
