using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ViaDoc.ServicioWcf.modelo
{
    
    public class FacturaResponse: Retorno
    {

        public List<DocumentosProcesados> documentoProcesado { get; set; }

        
        public FacturaResponse()
        {
            documentoProcesado = new List<DocumentosProcesados>();
        }
    }
}