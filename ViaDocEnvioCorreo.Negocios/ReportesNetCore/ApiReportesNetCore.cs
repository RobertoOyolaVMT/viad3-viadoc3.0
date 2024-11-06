using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ViaDocEnvioCorreo.Negocios.procesos
{
    public class ApiReportesNetCore
    {
        public async Task<RespuestaRide> Ride(string claveAcceso, string TipoRide)
        {
            var reportBase64 = new RespuestaRide();
            using (var client = new HttpClient())
            {
                try
                {
                    switch (TipoRide)
                    {
                        case "01":
                            TipoRide = "Ridefactura";
                            break;
                        case "03":
                            TipoRide = "RideLiquidacion";
                            break;
                        case "04":
                            TipoRide = "RideNotaCredito";
                            break;
                        case "05":
                            TipoRide = "RideNotaDebito";
                            break;
                        case "06":
                            TipoRide = "RideGuiaRemision";
                            break;
                        case "07":
                            TipoRide = "RideCompRetencion";
                            break;
                    }

                    var parametro = $"txClaveAcceso={claveAcceso}";
                    var raizUrl = "http://localhost:5157/GeneraRide/";
                    var url = $"{raizUrl}{TipoRide}?{parametro}";

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    reportBase64 = JsonConvert.DeserializeObject<RespuestaRide>(responseBody);

                }
                catch (Exception ex)
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("RideFactura", "ViaDoc.WebApp", ex.Message, null);
                }
            }
            return reportBase64;
        }
    }
}