using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSync
{
    public class Mensaje
    {
        public String Identificador { get; set; }
        public String MensajeRespuesta { get; set; }
        public String InformacionAdicional { get; set; }
        public String Tipo { get; set; }

        public Mensaje()
        {
            Identificador = "";
            MensajeRespuesta = "";
            InformacionAdicional = "";
            Tipo = "";
        }
    }
}
