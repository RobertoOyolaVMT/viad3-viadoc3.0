using System.Configuration;
using System.IO;

namespace ViaDoc.Configuraciones
{
    public class CatalogoViaDoc
    {
        #region Tipo Documento
        public static string DocumentoFactura => ConfigurationManager.AppSettings["DOCUMENTO.FACTURA"];
        public static string DocumentoLiquidacion => ConfigurationManager.AppSettings["DOCUMENTO.LIQUIDACION"];
        public static string DocumentoCompRetencion => ConfigurationManager.AppSettings["DOCUMENTO.COMPRETENCION"];
        public static string DocumentoNotaCredito => ConfigurationManager.AppSettings["DOCUMENTO.NOTACREDITO"];
        public static string DocumentoNotaDebito => ConfigurationManager.AppSettings["DOCUMENTO.NOTADEBITO"];
        public static string DocumentoGuiaRemision => ConfigurationManager.AppSettings["DOCUMENTO.GUIAREMISION"];
        #endregion Tipo Documento


        #region Estado Documento
        public static string DocEstadoInicial => ConfigurationManager.AppSettings["ESTADO.DOCINICIAL"];
        public static string DocEstadoFirmado => ConfigurationManager.AppSettings["ESTADO.FIRMADO"];
        public static string DocEstadoGenerado => ConfigurationManager.AppSettings["ESTADO.GENERADO"];
        public static string DocEstadoEFirmado => ConfigurationManager.AppSettings["ESTADO.EFIRMADO"];
        public static string DocEstadoRecibido => ConfigurationManager.AppSettings["ESTADO.RECIBIDO"];
        public static string DocEstadoAutorizado => ConfigurationManager.AppSettings["ESTADO.AUTORIZADO"];
        public static string DocEstadoNoAutorizado => ConfigurationManager.AppSettings["ESTADO.NAUTORIZADO"];
        public static string DocEstadoRAutorizado => ConfigurationManager.AppSettings["ESTADO.RAUTORIZADO"];
        public static string DocEstadoEAutorizado => ConfigurationManager.AppSettings["ESTADO.EAUTORIZADO"];
        public static string DocEstadoERecibido => ConfigurationManager.AppSettings["ESTADO.ERECIBIDO"];
        public static string DocEstadoEnviado => ConfigurationManager.AppSettings["ESTADO.ENVIADO"];
        public static string DocEstadoErrorEnEnvio => ConfigurationManager.AppSettings["ESTADO.ERROR_EN_ENVIO"];
        public static string DocEstadoEnviadoClientePortal => ConfigurationManager.AppSettings["ESTADO.ENVIADO_CLIENTE_PORTAL"];
        public static string DocEstadoNoEnviado => ConfigurationManager.AppSettings["ESTADO.NO_ENVIADO"];
        public static string DocEnviadoPortal => ConfigurationManager.AppSettings["NOTIFICACION.ENVIOPORTAL"];
        public static string DocComprobanteEnviadoPortal => ConfigurationManager.AppSettings["NOTIFICACION.ComprobanteEnviadoPortal"];
        public static string DocComprobanteNoEnviadoPortalEmail => ConfigurationManager.AppSettings["NOTIFICACION.NOENVIOPORTAL"];

        #endregion Estado Documento

        #region Proceso Documento
        public static string TiempoProceso => ConfigurationManager.AppSettings["PROCESO.TIMER_PROCESO"];
        public static string rutaPrincipal => ConfigurationManager.AppSettings["PROCESO.RUTA_PRINCIPAL"];
        public static string rutaPlantilla => ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLA"];
        public static string rutaPlantillaEstadistica => ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLAESTADISTICA"];
        public static string rutaPlantillaCertificado=> ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLACERTIFICADO"];
        public static string rutaPlantillaNotificacionError=> ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLANOTiFiCACIONERROR"];
        
        public static string rutaPlantillaFactura => ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLA_FACTURA"];
        public static string rutaPlantillaCompRetencion => ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLA_COMPRETENCION"];
        public static string rutaPlantillaNotaCredito => ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLA_NOTACREDITO"];
        public static string rutaPlantillaNotaDedito => ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLA_NOTADEDITO"];
        public static string rutaPlantillaGuiaRemision => ConfigurationManager.AppSettings["PROCESO.RUTA_PLANTILLA_GUIAREMISION"];
        public static string rutaLogoCompania => ConfigurationManager.AppSettings["NOTIFICACION.pathLogosCompania"];
        public static string rutaCodigoBarra => ConfigurationManager.AppSettings["PROCESO.RUTA_CODIGOBARRA"];
        public static string activarNotificacionDiario => ConfigurationManager.AppSettings["PROCESO.ACTIVAR_NOTIFICACION_DIARIA"];
        public static string rutaRide => ConfigurationManager.AppSettings["PROCESO.RUTA_RIDE"];
        public static string rutaHorasServicios => ConfigurationManager.AppSettings["PROCESO.RUTA_SERVICIO"];
        public static string rutaRidePlantilla => ConfigurationManager.AppSettings["PROCESO.RUTA_RIDE_PLANTILLA"];
        public static string LeyendaFac => ConfigurationManager.AppSettings["PROCESO.LEYENDAFACTURA"];
        public static string LeyendaAgente => ConfigurationManager.AppSettings["PROCESO.AGENTE.LEYENDA"];
        public static string LeyendaRegimen => ConfigurationManager.AppSettings["PROCESO.REGIMEN.LEYENDA"];
        public static string NOMBRE_RIDE => ConfigurationManager.AppSettings["NOMBRE.RIDE"];




        //PROCESO.ACTIVAR_NOTIFICACION_DIARIA
        #endregion Proceso Documento

        #region Parametros Notificaciones
        public static string ParametrosValorRenta => ConfigurationManager.AppSettings["NOTIFICACION.RENTA"];
        public static string ParametrosValorIva => ConfigurationManager.AppSettings["NOTIFICACION.IVA"];
        public static string ParametrosValorISD => ConfigurationManager.AppSettings["NOTIFICACION.ISD"];

        public static string ParametrosValorIVA0 => ConfigurationManager.AppSettings["NOTIFICACION.0%"];
        public static string ParametrosValorIVA12 => ConfigurationManager.AppSettings["NOTIFICACION.12%"];
        public static string ParametrosValorIVA14 => ConfigurationManager.AppSettings["NOTIFICACION.14%"];
        public static string ParametrosValorObjetoImpuesto => ConfigurationManager.AppSettings["NOTIFICACION.NO_OBJETO_DE_IMPUESTO"];
        public static string ParametrosValorExentoIVA => ConfigurationManager.AppSettings["NOTIFICACION.EXENTO_DE_IVA"];

        public static string ParametrosValorICE => ConfigurationManager.AppSettings["NOTIFICACION.ICE"];
        public static string ParametrosValorIRBPNR => ConfigurationManager.AppSettings["NOTIFICACION.IRBPNR"];
        public static string UrlPortal => ConfigurationManager.AppSettings["NOTIFICACION.UrlWsPortalFactElectronnica"];
        public static string TiempoRequest => ConfigurationManager.AppSettings["NOTIFICACION.TIMER_ESPERA_SOLICTUD"];
        public static string RazonSocial => ConfigurationManager.AppSettings["RAZONSOCIAL"];


        #endregion Parametros Notificaciones

        #region Ruta Certificado
        public static string RutaCertificado => ConfigurationManager.AppSettings["PROCESO.RUTA_CERTIFICADO"];
        #endregion

        #region Rutas TimerExec
        public static string FirmaTimerExec => ConfigurationManager.AppSettings["TimerExec.FIRMA"];
        public static string AutorizacionTimerExec => ConfigurationManager.AppSettings["TimerExec.AUTORIZACION"];
        public static string CorreoTimerExec => ConfigurationManager.AppSettings["TimerExec.CORREO"];

        #endregion

        public static void Catalogos()
        {
            //string ruta = ConfigurationManager.AppSettings["pathJsonFactura"];
            //string _requestFactura = File.ReadAllText(ruta);
            string FileVirtual = "~" + "CatalogosViaDoc.json";
            string ruta = ConfigurationManager.AppSettings["JSONCATALOGOS"];
            string _requestJsonCatalogos = File.ReadAllText(ruta);
        }

    }
}
