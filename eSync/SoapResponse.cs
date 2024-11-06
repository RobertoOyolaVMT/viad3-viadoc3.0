using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSync
{
    public class SoapResponse
    {
        public String RespuestaSoap { get; set; }
        public Boolean TieneExcepcion { get; set; }
        public Exception Excepcion { get; set; }

        public SoapResponse()
        {
            RespuestaSoap = "";
            TieneExcepcion = false;
            Excepcion = null;
        }


    }
}
