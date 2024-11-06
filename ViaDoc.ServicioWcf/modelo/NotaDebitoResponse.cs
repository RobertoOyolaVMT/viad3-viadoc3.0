using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.ServicioWcf.modelo
{
    public class NotaDebitoResponse: Retorno
    {
        public List<DocumentosProcesados> documentoProcesado { get; set; }


        public NotaDebitoResponse()
        {
            documentoProcesado = new List<DocumentosProcesados>();
        }
    }
}