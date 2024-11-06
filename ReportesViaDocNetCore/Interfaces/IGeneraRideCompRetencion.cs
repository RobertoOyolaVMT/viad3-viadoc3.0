using ReportesViaDocNetCore.EntidadesReporte;

namespace ReportesViaDocNetCore.Interfaces
{
    public interface IGeneraRideCompRetencion
    {
        Task<RespuestaRide> GeneraRideCompRetencion(string claveAcceso);
    }
}
