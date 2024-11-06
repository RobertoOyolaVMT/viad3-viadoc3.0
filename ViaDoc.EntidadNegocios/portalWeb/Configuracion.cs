using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.portalWeb
{
    public class Configuracion
    {

    }

   

    public class Parametros
    {
        public int idRegistro { get; set; }
        public string idTipoDocumento { get; set; }
        public int idCompania { get; set; }
        public string nombreComercial { get; set; }
        public string descripcion { get; set; }
        public int cantidadFirma { get; set; }
        public int cantidadAutorizacion { get; set; }
        public int cantidadCorreo { get; set; }
        public int reprocesoFirma { get; set; }
        public int reprocesoAutorizacion { get; set; }
        public int reprocesoCorreo { get; set; }
        public string estado { get; set; }
    }
}
