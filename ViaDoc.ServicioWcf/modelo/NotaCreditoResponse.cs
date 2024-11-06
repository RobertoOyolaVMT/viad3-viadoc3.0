using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.ServicioWcf.modelo
{
    public class NotaCreditoResponse: Retorno
    {
        public List<DocumentosProcesados> documentoProcesado { get; set; }


        public NotaCreditoResponse()
        {
            documentoProcesado = new List<DocumentosProcesados>();
        }
    }
}