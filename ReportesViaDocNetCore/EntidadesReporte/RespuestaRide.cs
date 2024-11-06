using Microsoft.AspNetCore.Authentication;
using System.Buffers.Text;

namespace ReportesViaDocNetCore.EntidadesReporte
{
    public class RespuestaRide
    {
        public string TipoDoc { set; get; }
        public string Documento { set; get; }
        public string Cod { set; get; }
    }
}
