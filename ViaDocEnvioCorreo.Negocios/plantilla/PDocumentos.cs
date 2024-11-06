using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.Configuraciones;

namespace ViaDocEnvioCorreo.Negocios.plantilla
{
    public class PDocumentos
    {
        public string GenerarCorreoDocumentos(string rucCompania, string urlPortal, string razonSocial,string MailAddressfrom,
            string razonSocialComprador)
        {
            byte[] imgbyte = null;
            string imgBase64 = string.Empty;
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantilla);
            string imagenpath = CatalogoViaDoc.rutaLogoCompania + rucCompania.Trim() + ".png";

            imgbyte = File.ReadAllBytes(imagenpath);
            imgBase64 = Convert.ToBase64String(imgbyte);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_URLCOMPANIA]", urlPortal);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_CORREO]", MailAddressfrom);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", imgBase64);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_RUCIMAGENES]", imgBase64);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.COMPRADOR]", razonSocialComprador);
            return plantillaHtml;
        }

        public string GenerarCorreoEstadisticaDiaria(string rucCompania, string razonSocial, string fechaEmision, string estadisticaDiaria)
        {
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaEstadistica);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            plantillaHtml = plantillaHtml.Replace("[FECHA.EMISIONESTADISTICA]", fechaEmision);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_RUCIMAGENES]", rucCompania);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.ESTADISTICA]", estadisticaDiaria);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", $"cid: {rucCompania}");

            return plantillaHtml;
        }

        public string GenerarCorreoCertificadoDigital(string rucCompania, string razonSocial, string fechaDesde, string fechaHasta, 
            string descripcionCorreo)
        {
            //string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaCertificado);
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaCertificado);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.FECHADESDECERTIFICADO]", fechaDesde);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.FECHAHASTACERTIFICADO]", fechaHasta);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_RUCIMAGENES]", rucCompania);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.DESCRIPCION]", descripcionCorreo);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", $"cid: {rucCompania}");

            //[NOTIFICACION.DESCRIPCION]
            return plantillaHtml;
        }

        public string GenerarCorreoNotificionError(string rucCompania, string razonSocial, string descripcionCorreo)
        {
            //string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaCertificado);
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaNotificacionError);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.DESCRIPCION]", descripcionCorreo);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", $"cid: {rucCompania}");

            //[NOTIFICACION.DESCRIPCION]
            return plantillaHtml;
        }

        public string GenerarCorreoNotificionAtrasadas(string rucCompania, string razonSocial, string descripcionCorreo)
        {
            //string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaCertificado);
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaNotificacionError);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.DESCRIPCION]", descripcionCorreo);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", $"cid: {rucCompania}");

            //[NOTIFICACION.DESCRIPCION]
            return plantillaHtml;
        }
    }
}
