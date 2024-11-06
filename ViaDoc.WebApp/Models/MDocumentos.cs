using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.WebApp.Models
{
    public class MDocumentos
    {
        public int codigoRetorno { get; set; }
        public string mensajeRetorno { get; set; }
        public List<MParametro> data { get; set; }

        public MDocumentos()
        {

        }
    }

    public class MParametroResponse
    {
        public int codigoRetorno { get; set; }
        public string mensajeRetorno { get; set; }
        public List<MParametro> data { get; set; }

        public MParametroResponse()
        {
            data = new List<MParametro>();
        }
    }


    public class MParametro
    {
        public int idRegistro { get; set; }
        public int idCompania { get; set; }
        public string nombreComercial { get; set; }
        public string idTipoDocumento { get; set; }
        public string descripcion { get; set; }
        public int cantidadFirma { get; set; }
        public int cantidadAutorizacion { get; set; }
        public int cantidadCorreo { get; set; }
        public int reprocesoFirma { get; set; }
        public int reprocesoAutorizacion { get; set; }
        public int reprocesoCorreo { get; set; }
        public string estado { get; set; }
    }


    public class MDocumentoResponse
    {
        public int codigoRetorno { get; set; }
        public string mensajeRetorno { get; set; }
        public List<MDocumentoTodos> data { get; set; }

        public MDocumentoResponse()
        {
            data = new List<MDocumentoTodos>();
        }
    }

    public class MDocumentoTodos
    {
        public string razonSocial { get; set; }
        public string claveAcceso { get; set; }
        public string descripcion { get; set; }
        public string numeroAutorizacion { get; set; }
        public string tipoDocumento { get; set; }
        public string identificacionComprador { get; set; }
        public string razonSocialComprador { get; set; }
        public string NumeroDocumento { get; set; }
        public string documentoAutorizado { get; set; }
        public string fechaHoraAutorizacion { get; set; }
        public string descripcionEstado { get; set; }
        public string descripcionEmision { get; set; }
        public string idEstado { get; set; }
        public string idCompania { get; set; }
    }
}