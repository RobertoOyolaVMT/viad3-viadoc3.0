using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace eSync
{
    public class SensorServicioResponse
    {
        public Boolean ServicioDisponible { get; set; }
        public Boolean TieneExcepcion { get; set; }
        public WebException ExcepcionWeb { get; set; }

        public SensorServicioResponse()
        {
            ServicioDisponible = false;
            TieneExcepcion = false;
            ExcepcionWeb = null;
        }
    }
}
