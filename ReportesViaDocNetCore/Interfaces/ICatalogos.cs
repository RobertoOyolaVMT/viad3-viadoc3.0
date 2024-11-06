using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Models;

namespace ReportesViaDocNetCore.Interfaces
{
    public interface ICatalogos
    {
        Task<DatosDocumento> DatosDocuemntosFactura(string claveAcceso);
        Task<DatosDocumento> DatosDocuemntosCompRetecion(string claveAcceso);
        Task<DatosDocumento> DatosDocuemntosNotaCredito(string claveAcceso);
        Task<DatosDocumento> DatosDocuemntosNotaDebito(string claveAcceso);
        Task<DatosDocumento> DatosDocuemntosLiquidacion(string claveAcceso);
        Task<Companium> Compania(int idCompania);
        Task<List<ConfiguracionReporte>> Configuraciones(int idCompania);
        Task<List<CatalogoReporte>> CatalogoReportes();
        Task<List<DetalleCatalogo>> DetalleCatalogo();
    }
}
