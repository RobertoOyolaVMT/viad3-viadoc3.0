using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.WebApp.Models
{
    public class MCatalogos
    {
        public List<MCompania> listaCompania { get; set; }
        public List<MDocumento> listaDocumento { get; set; }
    }

    public class MCompania
    {
        public int idCompania { get; set; }
        public string nombreComercial { get; set; }
        public string razonSocial { get; set; }
        public string RucCompania { get; set; }
    }

    public class MDocumento
    {
        public string idTipoDocumento { get; set; }
        public string descripcion { get; set; }
    }
}