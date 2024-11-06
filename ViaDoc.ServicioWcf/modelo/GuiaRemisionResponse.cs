using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.ServicioWcf.modelo
{
    public class GuiaRemisionResponse: Retorno
    {
        public List<DocumentosProcesados> documentoProcesado { get; set; }


        public GuiaRemisionResponse()
        {
            documentoProcesado = new List<DocumentosProcesados>();
        }
    }
}