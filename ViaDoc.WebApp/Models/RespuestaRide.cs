using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaDoc.WebApp.Models
{
    public class RespuestaRide
    {
        public string TipoDoc { set; get; }
        public string Documento { set; get; }
        public string Cod { set; get; }
        public List<ListaDetalles> Detalles { set; get; }
        public string Autorizada { set; get; }

    }

    public class ListaDetalles
    {
        public string CodigoPrincipal { set; get; }
        public string Descripcion { set; get; }
    }
}
