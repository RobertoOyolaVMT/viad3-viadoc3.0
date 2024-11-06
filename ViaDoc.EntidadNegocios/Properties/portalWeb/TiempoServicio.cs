using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.portalWeb
{
    public class TiempoServicio
    {
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public string tipoServicio { get; set; }
    }

    public class TiempoServicioLista
    {
        public List<TiempoServicio> objListaTiempo { get; set; }
    }
}
