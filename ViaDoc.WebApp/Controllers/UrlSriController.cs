using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class UrlSriController : Controller
    {
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        // GET: UrlSri
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }

            List<CatAmbiente> listAmbiente = new ProcesoCatalogos().ConsultaAmbiente();
            ViewData["listAmbientes"] = listAmbiente;
            return View();
        }

        public JsonResult CargarUrlSris(string txTipoAmbiente)
        {
            UrlSri urlSriObject = new UrlSri();
            int idtipoAmbiente = int.Parse(txTipoAmbiente);
            urlSriObject = new ProcesoConfiguracion().ConsultarUrlSri(idtipoAmbiente, ref codigoRetorno, ref mensajeRetorno);

            return Json(urlSriObject);
        }

        public JsonResult GuardarUrlSris(string txTipoAmbiente, string urlRecepcion, string urlAutorizacion)
        {
            UrlSri urlSriParameter = new UrlSri();
            urlSriParameter.urlRecepcion = urlRecepcion;
            urlSriParameter.urlAutorizacion = urlAutorizacion;
            int idtipoAmbiente = int.Parse(txTipoAmbiente);
            new ProcesoConfiguracion().InsertarUrlSri(idtipoAmbiente, urlSriParameter, ref codigoRetorno, ref mensajeRetorno);

            return Json(urlSriParameter);
        }
    }
}