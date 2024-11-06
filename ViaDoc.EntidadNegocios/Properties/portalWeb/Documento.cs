using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.portalWeb
{
    public class Documento
    {
        public string razonSocial { get; set; }
        public string claveAcceso { get; set; }
        public string descripcion { get; set; }
        public string numeroAutorizacion { get; set; }
        public string tipoDocumento { get; set; }
        public string nameEstado { get; set; }
        public string identificacionComprador { get; set; }
        public string razonSocialComprador { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public string NumeroDocumento { get; set; }
        public string filtroFechaDH { get; set; }
        public string documentoAutorizado { get; set; }
        public string fechaHoraAutorizacion { get; set; }
        public string descripcionEstado { get; set; }
        public string descripcionEmision { get; set; }
        public string idEstado { get; set; }
        public string idCompania { get; set; }
        public string email { get; set; }
        public string fechaEmision { get; set; }
        public string valor { get; set; }
        public string enviarMail { get; set; }
        public string enviarPortal { get; set; }
        public string pdf { get; set; }
        public string xml { get; set; }
    }

    public class DocumentosLista
    {
        public List<Documento> objListaDocumento = new List<Documento>();
    }

    public class Estadisticas
    {
        public string documento { get; set; }
        public string sinProcesar { get; set; }
        public string firmados { get; set; }
        public string errorFirma { get; set; }
        public string errorRecepcion { get; set; }
        public string enProceso { get; set; }
        public string errorAutorizacion { get; set; }
        public string autorizado { get; set; }
        public string noEnviadoCliente { get; set; }
        public string noEnviadoPortal { get; set; }
        public string total { get; set; }

    }

    public class EstadisticasLista
    {
        public List<Estadisticas> objListEstadisticas { get; set; }
    }

    public class EstadisticasDetalle
    {
        public string NumDocumento { get; set; }
        public string TxMensajeError { get; set; }
        public string txClaveAcceso { get; set; }
    }

    public class EstadisticasDetalleLista
    {
        public List<EstadisticasDetalle> objListEstadisticasDetalle { get; set; }
    }


    public class Autorizar
    {
        public string NumeroDocumento { get; set; }
        public string descripcion { get; set; }
        public string tipoDocumento { get; set; }
        public string identificacionComprador { get; set; }
        public string claveAcceso { get; set; }
        public string idCompania { get; set; }
    }

    public class AutorizarLista
    {
        public List<Autorizar> objListAutorizar { get; set; }
    }
}
