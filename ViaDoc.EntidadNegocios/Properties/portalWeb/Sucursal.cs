using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.portalWeb
{

    public class SucursalLista
    {
        public List<Sucursal> objListaSucursal { get; set; }
    }



    public class Sucursal
    {
        public int ciCompania { get; set; }
        public string ciSucursal { get; set; }
        public string direccion { get; set; }
        public string ciEstado { get; set; }
        public string estado { get; set; }

        public Sucursal()
        {
            ciCompania = 0;
            ciSucursal = string.Empty;
            direccion = string.Empty;
            ciEstado = string.Empty;
            estado = string.Empty;
        }
    }
}
