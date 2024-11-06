using ReportesViaDocNetCore.EntidadesReporte;

namespace ReportesViaDocNetCore.Interfaces
{
    public interface IGeneraRideGuiaRemision
    {
        Task<RespuestaRide> GeneraRideGuiaRemision(string txClaveAcceso);
    }
}
