using ReportesViaDocNetCore.Models;

namespace ReportesViaDocNetCore.EntidadesReporte
{
    public class LiquidacionAgrupado
    {
        public Liquidacion? liquidacion {get; set;}
        public List<LiquidacionDetalle>? liquidacionDetalle { get; set;}
        public List<LiquidacionDetalleAdicional>? liquidacionDetalleAdic { get; set;}
        public List<LiquidacionDetalleFormaPago>? liquidacionDetalleFormPago { get; set;}
        public List<LiquidacionDetalleImpuesto>? liquidacionDetalleImpues { get; set;}
        public List<LiquidacionInfoAdicional>? liquidacionInfoAdic { get; set;}
        public List<LiquidacionReembolso>? liquidacionReembolso { get; set;}
        public List<LiquidacionTotalImpuesto>? liquidacionTotalImpues { get; set;}
    }
}
