using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSync
{
    public class Autorizacion
    {
        public String Estado { get; set; }
        public String NumeroAutorizacion { get; set; }
        public String FechaAutorizacion { get; set; }
        public String Ambiente { get; set; }
        public String Comprobante { get; set; }
        public List<Mensaje> Mensajes { get; set; }

        public Autorizacion()
        {
            Estado = "";
            NumeroAutorizacion = "";
            FechaAutorizacion = "";
            Ambiente = "";
            Comprobante = "";
            Mensajes = null;
        }
    }
}
