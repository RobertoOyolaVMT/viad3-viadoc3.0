using Microsoft.AspNetCore.Mvc;
using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Services;

namespace ReportesViaDocNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneraRideController : Controller
    {

        private readonly IGeneraRideFactura _generaRideFactura;
        private readonly IGeneraRideCompRetencion _generaRideCompRetencion;
        private readonly IGeneraRideNotaCredito _generaRideNotaCredito;
        private readonly IGeneraRideNotaDebito _generaRideNotaDebito;
        private readonly IGeneraRideLiquidacion _generaRideLiquidacion;
        private readonly IGeneraRideGuiaRemision _generaRideGuiaRemision;
        public GeneraRideController(IGeneraRideFactura generaRideFactura, IGeneraRideCompRetencion generaRideCompRetencion, IGeneraRideNotaCredito generaRideNotaCredito
                                    , IGeneraRideNotaDebito generaRideNotaDebito, IGeneraRideLiquidacion generaRideLiquidacion, IGeneraRideGuiaRemision generaRideGuiaRemision)
        {
            this._generaRideFactura = generaRideFactura;
            this._generaRideCompRetencion = generaRideCompRetencion;
            this._generaRideNotaCredito = generaRideNotaCredito;
            this._generaRideNotaDebito = generaRideNotaDebito;
            this._generaRideLiquidacion = generaRideLiquidacion;
            this._generaRideGuiaRemision = generaRideGuiaRemision;
        }

        [HttpGet]
        [Route("Ridefactura")]
        public async Task<RespuestaRide> Ridefactura(string txClaveAcceso)
        {
            var ridePdf = new RespuestaRide();
            try
            {
                ridePdf = await _generaRideFactura.Ridefactura(txClaveAcceso);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("Ridefactura", "GeneraReporteNetcore", ex.Message, null);
            }

            return ridePdf;
        }

        [HttpGet]
        [Route("RideCompRetencion")]
        public async Task<RespuestaRide> RideCompRetencion(string txClaveAcceso)
        {
            var ridePdf = new RespuestaRide();
            try
            {
                ridePdf = await _generaRideCompRetencion.GeneraRideCompRetencion(txClaveAcceso);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("RideCompRetencion", "GeneraReporteNetcore", ex.Message, null);
            }

            return ridePdf;
        }

        [HttpGet]
        [Route("RideNotaCredito")]
        public async Task<RespuestaRide> RideNotaCredito(string txClaveAcceso)
        {
            var ridePdf = new RespuestaRide();
            try
            {
                ridePdf = await _generaRideNotaCredito.GeneraRideNotaCredito(txClaveAcceso);
            }
            catch (Exception ex)
            {

                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("RideNotaCredito", "GeneraReporteNetcore", ex.Message, null);
            }

            return ridePdf;
        }

        [HttpGet]
        [Route("RideNotaDebito")]
        public async Task<RespuestaRide> RideNotaDebito(string txClaveAcceso)
        {
            var ridePdf = new RespuestaRide();
            try
            {
                ridePdf = await _generaRideNotaDebito.GeneraRideNotaDebito(txClaveAcceso);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("RideNotaDebito", "GeneraReporteNetcore", ex.Message, null);
            }

            return ridePdf;
        }

        [HttpGet]
        [Route("RideLiquidacion")]
        public async Task<RespuestaRide> RideLiquidacion(string txClaveAcceso)
        {
            var ridePdf = new RespuestaRide();
            try
            {
                ridePdf = await _generaRideLiquidacion.GeneraRideLiquidacion(txClaveAcceso);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("RideLiquidacion", "GeneraReporteNetcore", ex.Message, null);
            }

            return ridePdf;
        }

        [HttpGet]
        [Route("RideGuiaRemision")]
        public async Task<RespuestaRide> RideGuiaRemision(string txClaveAcceso)
        {
            var ridePdf = new RespuestaRide();
            try
            {
                ridePdf = await _generaRideGuiaRemision.GeneraRideGuiaRemision(txClaveAcceso);
            }
            catch (Exception ex)
            {

                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("RideGuiaRemision", "GeneraReporteNetcore", ex.Message, null);
            }
            return ridePdf;
        }
    }
}
