using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSync
{
    public class Comprobante
    {
        public String ClaveAcceso { get; set; }
        public List<Mensaje> Mensajes { get; set; }

        public Comprobante()
        {
            ClaveAcceso = "";
            Mensajes = null;
        }
    }
}
