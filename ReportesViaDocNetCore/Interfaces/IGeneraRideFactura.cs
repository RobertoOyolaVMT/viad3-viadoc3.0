using ReportesViaDocNetCore.EntidadesReporte;

namespace ReportesViaDocNetCore.Interfaces
{
    public interface IGeneraRideFactura
    {
        Task<RespuestaRide> Ridefactura(string txClaveAcceso);
    }
}
