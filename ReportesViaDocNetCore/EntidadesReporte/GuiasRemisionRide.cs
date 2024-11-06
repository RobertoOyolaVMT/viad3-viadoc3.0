using ReportesViaDocNetCore.Models;

namespace ReportesViaDocNetCore.EntidadesReporte
{
    public class GuiasRemisionRide
    {
        public GuiaRemision? guiaRemision { get; set; }
        public List<GuiaRemisionDestinatarioDetalle>? guiaRemisionDestinatarioDetalle { get; set; }
        public List<GuiaRemisionDestinatarioDetalleAdicional>? guiaRemisionDetalleAdicional { get; set; }
    }
}
