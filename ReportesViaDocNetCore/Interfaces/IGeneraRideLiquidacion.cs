using ReportesViaDocNetCore.EntidadesReporte;

namespace ReportesViaDocNetCore.Interfaces
{
    public interface IGeneraRideLiquidacion
    {
        Task<RespuestaRide> GeneraRideLiquidacion(string txClaveAcceso);
    }
}
