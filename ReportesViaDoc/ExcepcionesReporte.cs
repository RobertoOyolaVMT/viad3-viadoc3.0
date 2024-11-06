using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace ReportesViaDoc
{
    public class ExcepcionesReporte
    {

        public void GuardaReporte(string strExcepcion)
        {

            string archivo = "logReportesNuevos";
            DateTime fecha = DateTime.Now;
            string rutaXml = ConfigurationManager.AppSettings.Get("NOTIFICACION.pathPrincipal").ToString();
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
