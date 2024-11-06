using com.sun.security.ntlm;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using ViaDoc.WebApp.Models;

namespace ViaDoc.WebApp.ReportesNetCore
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
                    var parametro = $"txClaveAcceso={claveAcceso}";
                    var raizUrl = ConfigurationManager.AppSettings["UrlBaseReportes"];
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