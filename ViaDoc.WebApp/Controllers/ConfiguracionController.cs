using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaDoc.WebApp.Models;

namespace ViaDoc.WebApp.Controllers
{
    public class ConfiguracionController : Controller
    {
        // GET: Configuracion
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        MetodosCatalogos metodosCatalogos = new MetodosCatalogos();
        MetodosConfiguracion metodosConfiguracion = new MetodosConfiguracion();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Documentos()
        {
            MCatalogos mCatalogos = new MCatalogos();
            ViewBag.listaCompania = metodosCatalogos.ConsultaCompania();
            ViewBag.listaDocumento = metodosCatalogos.ConsultaDocumento();


            return View(mCatalogos);
        }

        [HttpPost]
        public JsonResult ConsultaParametrizacion(int idCompania)
        {
            MParametroResponse respuesta = new MParametroResponse();
            respuesta.data = metodosConfiguracion.ConsultaParametrosEmpresa(idCompania, ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;
            
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarParametrizacion(int idCompania, int idRegistro, string idTipoDocumento, int cantidadFirma, 
                                                 int cantidadCorreo, int cantidadAutorizacion, int reprocesoFirma, 
                                                 int reprocesoCorreo, int reprocesoAutorizacion)
        {
            MRetorno respuesta = new MRetorno();
            MParametro mParametro = new MParametro();
            mParametro.idCompania = idCompania;
            mParametro.idRegistro = idRegistro;
            mParametro.idTipoDocumento = idTipoDocumento;
            mParametro.cantidadFirma = cantidadFirma;
            mParametro.cantidadCorreo = cantidadCorreo;
            mParametro.cantidadAutorizacion = cantidadAutorizacion;
            mParametro.reprocesoFirma = reprocesoFirma;
            mParametro.reprocesoCorreo = reprocesoCorreo;
            mParametro.reprocesoAutorizacion = reprocesoAutorizacion;
            metodosConfiguracion.IngresosParametrosEmpresa(mParametro, ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarParametrizacion(int idRegistro)
        {
            MRetorno respuesta = new MRetorno();
            metodosConfiguracion.EliminarParametrosEmpresa(idRegistro, ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Compania()
        {
            ViewBag.listaCompania = metodosCatalogos.ConsultaCompania();

            return View();
        }

        public ActionResult SMTP()
        {
            return View();
        }

        public ActionResult TiempoServicio()
        {
            ViewBag.listaCompania = metodosCatalogos.ConsultaCompania();
            return View();
        }
    }
}