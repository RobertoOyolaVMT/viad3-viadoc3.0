using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.ServicioWcf.modelo
{
    public class LiquidacionResponse : Retorno
    {
        public List<DocumentosProcesados> documentoProcesado { get; set; }

        public LiquidacionResponse ()
        {
            documentoProcesado = new List<DocumentosProcesados>();
        }

    }
}