using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDocNetCore.EntidadesReporte
{
    public class ConfiguracionReporte
    {
        public int IdConfiguracion { get; set; }
        public string CodigoReferencia { get; set; }
        public string Descripcion { get; set; }
        public int IdCompania { get; set; }
        public string RucCompania { get; set; }
        public string Configuracion1 { get; set; }
        public string Configuracion2 { get; set; }
        public string Configuracion3 { get; set; }
        public string Configuracion4 { get; set; }
        public string Configuracion5 { get; set; }

        public ConfiguracionReporte()
        {
            IdConfiguracion = 0;
            CodigoReferencia = "";
            Descripcion = "";
            IdCompania = 0;
            RucCompania = "";
            Configuracion1 = "";
            Configuracion2 = "";
            Configuracion3 = "";
            Configuracion4 = "";
            Configuracion5 = "";
        }

    }
}
