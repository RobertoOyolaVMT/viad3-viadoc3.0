using System;
using System.Linq;
using System.ServiceProcess;

namespace FirmaViadoc.WinServ
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
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
            //obj.GenerarFirmaElectronicaFactura();
            //obj.GenerarFirmaElectronicaNotaCredito();
            //obj.GenerarFirmaElectronicaNotaDebito();
            //obj.GenerarFirmaElectronicaCompRetencion();
        }
    }
}