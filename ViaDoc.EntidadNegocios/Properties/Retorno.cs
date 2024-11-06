using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios
{
    public class Retorno
    {
        public int codigoRetorno { get; set; }
        public string mensajeRetorno { get; set; }

        public Retorno()
        {
            codigoRetorno = 0;
            mensajeRetorno = string.Empty;
        }
    }
}
