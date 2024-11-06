using System;
using System.Linq;
using System.ServiceProcess;

namespace AutorizacionViaDoc.WinServ
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);

            //MetodosWinServ obj = new MetodosWinServ();
            //obj.ProbadorServicio();
            //obj.GenerarAutorizacionRecepcionFactura();
            //obj.GenerarAutorizacionRecepcionCompRecepcion();
            //obj.GenerarAutorizacionRecepcionNotaCredito();
            //obj.ReprocesoDoc();
        }
    }
}