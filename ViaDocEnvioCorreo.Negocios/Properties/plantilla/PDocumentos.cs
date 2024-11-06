using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantilla);
            string imagenesCorreo = "cid:" + rucCompania;
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_URLCOMPANIA]", urlPortal);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_CORREO]", MailAddressfrom);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", imagenesCorreo);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_RUCIMAGENES]", rucCompania);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.COMPRADOR]", razonSocialComprador);
       
            //[[RazonSocialComprador]]
            return plantillaHtml;
        }

        public string GenerarCorreoEstadisticaDiaria(string rucCompania, string razonSocial, string fechaEmision, string estadisticaDiaria)
        {
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaEstadistica);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            
            plantillaHtml = plantillaHtml.Replace("[FECHA.EMISIONESTADISTICA]", fechaEmision);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_RUCIMAGENES]", rucCompania);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.ESTADISTICA]", estadisticaDiaria);
            string imagenesCorreo = "cid:" + rucCompania;
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", imagenesCorreo);

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
            string imagenesCorreo = "cid:" + rucCompania;
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", imagenesCorreo);

            //[NOTIFICACION.DESCRIPCION]
            return plantillaHtml;
        }

        public string GenerarCorreoNotificionError(string rucCompania, string razonSocial, string descripcionCorreo)
        {
            //string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaCertificado);
            string plantillaHtml = File.ReadAllText(CatalogoViaDoc.rutaPlantillaNotificacionError);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_NOMBRECOMPANIA]", razonSocial);
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION.DESCRIPCION]", descripcionCorreo);
            string imagenesCorreo = "cid:" + rucCompania;
            plantillaHtml = plantillaHtml.Replace("[NOTIFICACION_IMAGENES]", imagenesCorreo);

            //[NOTIFICACION.DESCRIPCION]
            return plantillaHtml;
        }
    }
}
