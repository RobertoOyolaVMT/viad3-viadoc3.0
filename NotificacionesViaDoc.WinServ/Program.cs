using System;
using System.Linq;
using System.ServiceProcess;

namespace NotificacionesViaDoc.WinServ
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

            //MetodosWinServ _enviaCorreo = new MetodosWinServ();
            //_enviaCorreo.EnvioDocumentosCorreoElectronico();
            //_enviaCorreo.EnvioDocumentosPortalWeb();
            //_enviaCorreo.EnvioNotificacionCertificadoCaducado();
            //_enviaCorreo.EnvioDocumentosPortalWeb();
            //_enviaCorreo.EnvioEstadisticaDocumentos();
            //_enviaCorreo.NotificacinDocError();
            //_enviaCorreo.NotificacinDocAtrasados();
            //_enviaCorreo.Reenvio_a_Portal();
        }
    }
}