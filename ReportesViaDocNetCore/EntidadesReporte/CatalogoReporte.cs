using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDocNetCore.EntidadesReporte
{
    public class CatalogoReporte
    {
        public string CodigoReferencia { get; set; }
        public string Codigo { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public CatalogoReporte()
        {
            CodigoReferencia = "";
            Codigo = "";
            Valor = "";
            Descripcion = "";
        }
    }
}
