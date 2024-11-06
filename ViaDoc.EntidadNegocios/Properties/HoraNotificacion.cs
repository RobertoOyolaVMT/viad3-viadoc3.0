using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios
{

    public class HorasNotificacionLista: Retorno
    {
        public List<HoraNotificacion> data { get; set; }
    }

    public class HoraNotificacion
    {
        public int idRegistro { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }





    }
}
