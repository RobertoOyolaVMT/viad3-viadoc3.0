using ReportesViaDocNetCore.EntidadesReporte;

namespace ReportesViaDocNetCore.Interfaces
{
    public interface IGeneraRideNotaDebito
    {
        Task<RespuestaRide> GeneraRideNotaDebito(string txClaveAcceso);
    }
}
