using ReportesViaDocNetCore.EntidadesReporte;

namespace ReportesViaDocNetCore.Interfaces
{
    public interface IGeneraRideNotaCredito
    {
        Task<RespuestaRide> GeneraRideNotaCredito(string claveAcceso);
    }
}
