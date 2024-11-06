using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.portalweb;
using ViaDoc.WebApp.Models;

namespace ViaDoc.WebApp.Controllers
{
    public class DocumentoController : Controller
    {
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        MetodosCatalogos metodosCatalogos = new MetodosCatalogos();
        MetodosConfiguracion metodosConfiguracion = new MetodosConfiguracion();
        MetodosDocumentos metodosDocumentos = new MetodosDocumentos();
        ProcesoDocumento objDocumentos = new ProcesoDocumento();

        // GET: Documento
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ConsultaTodos()
        {
            ViewBag.listaCompania = metodosCatalogos.ConsultaCompania();
            ViewBag.listaDocumento = metodosCatalogos.ConsultaDocumento();
            return View();
        }

        [HttpPost]
        public JsonResult ConsultarDocumentos(string razonSocial, string claveAcceso, string numeroAutorizacion,
                                              string numeroDocumento, string identificacionComprador,
                                              string tipoDocumento, string fechaDesde, string fechaHasta,
                                              string razonSocialComprador, string filtroFechaDH)
        {
            MDocumentoResponse respuesta = new MDocumentoResponse();
            respuesta.data = metodosDocumentos.ConsultaTodosDocumentos(razonSocial, claveAcceso, numeroAutorizacion,
                                               numeroDocumento, identificacionComprador, tipoDocumento, fechaDesde,
                                               fechaHasta, razonSocialComprador, filtroFechaDH, 
                                               ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ConsultaHistorico()
        {
            return View();
        }

        public ActionResult Estadistica()
        {
            return View();
        }

        public ActionResult GenerarRide()
        {
            return View();
        }

        //public ActionResult AutorizarPendiente()
        //{
        //    ViewBag.listaCompania = metodosCatalogos.ConsultaCompania();
        //    ViewBag.listaDocumento = metodosCatalogos.ConsultaDocumento();
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult CargarDocumentosAutorizar(string txtIdEmpresa,
        //                                              string txtIdTipoDocumento,
        //                                              string txtNumDocumento)
        //{
        //    Documento objDocParametros = new Documento();

        //    objDocParametros.razonSocial = txtIdEmpresa;
        //    objDocParametros.tipoDocumento = txtIdTipoDocumento;
        //    objDocParametros.NumeroDocumento = txtNumDocumento;
        //    AutorizarLista objListDocumentos = new AutorizarLista();
        //    objListDocumentos.objListAutorizar = objDocumentos.ConsultarDocumentosAutorizar(objDocParametros, ref codigoRetorno, ref mensajeRetorno);

        //    return PartialView("PartialViewAutorizar", objListDocumentos);
        //}

        public ActionResult AutorizarEnLinea(string txtClaveAcceso,
                                            string txtIdTipoDocumento)
        {

            //ViaDoc. 
            return View();
        }

        public ActionResult EnvioMasivo()
        {
            return View();
        }

        
    }
}