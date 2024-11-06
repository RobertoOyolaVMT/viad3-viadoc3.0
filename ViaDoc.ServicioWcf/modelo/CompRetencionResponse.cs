using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.ServicioWcf.modelo
{
    public class CompRetencionResponse: Retorno
    {
        public List<DocumentosProcesados> documentoProcesado { get; set; }


        public CompRetencionResponse()
        {
            documentoProcesado = new List<DocumentosProcesados>();
        }
    }
}