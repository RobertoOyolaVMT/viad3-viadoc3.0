using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios
{
    public class Catalogo
    {

    }

    public class CatCompania
    {
        public int idCompania { get; set; }
        public string nombreComercial { get; set; }
        public string razonSocial { get; set; }
        public string RucCompania { get; set; }
    }

    public class CatDocumento
    {
        public string idTipoDocumento { get; set; }
        public string descripcion { get; set; }
    }

    public class CatAmbiente
    {
        public string idTipoAmbiente { get; set; }
        public string descripcion { get; set; }
    }

    public class CatEstado
    {
        public string idEstado { get; set; }
        public string descripcion { get; set; }
    }

    public class CatContabilidad
    {
        public string idContabilidad { get; set; }
        public string descripcion { get; set; }
    }


    public class CatTipoProceso
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }
}
