using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class EstadisticasController : Controller
    {
        ProcesoDocumento objDocumentos = new ProcesoDocumento();
        int codigoRetorno = 0;
        string mensajeRetorno = string.Empty;
        // GET: Estadisticas
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }

            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = objCatalogos.ConsultaEmpresa();
            List<CatDocumento> listDocumentos = objCatalogos.ConsultaDocumento();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listDocumentos"] = listDocumentos;

            return View();
        }

        public ActionResult ConsultarEstadisticas(string txtIdEmpresa, string txtFechaInicio, string txtFechaFin)
        {
            codigoRetorno = 0;
            mensajeRetorno = "";
            EstadisticasLista listEstadisticas = new EstadisticasLista();
            try
            {

                listEstadisticas.objListEstadisticas = objDocumentos.ConsultarOpcionEstadisticas(txtIdEmpresa, txtFechaInicio, txtFechaFin, ref codigoRetorno, ref mensajeRetorno);

                var estadistica = listEstadisticas.objListEstadisticas.Count == 0 ? null: listEstadisticas;

                return PartialView("PartialViewEstadisticas", estadistica);
            }
            catch (Exception ex)
            {
                Utilitarios.logs.LogsFactura.LogsInicioFin("ConsultarEstadisticasError: " + ex);
                return PartialView("PartialViewEstadisticas", listEstadisticas);
            }
        }

        public ActionResult ConsultarDetalles(string compania, string fecha, string fechaHasta, string tipoDocumento, string ciEstado)
        {
            
            codigoRetorno = 0;
            mensajeRetorno = "";
            EstadisticasDetalleLista listEstadisticas = new EstadisticasDetalleLista();

            listEstadisticas.objListEstadisticasDetalle = objDocumentos.ConsultarOpcionEstadisticasDetalles(compania, fecha, fechaHasta, tipoDocumento, ciEstado, ref codigoRetorno, ref mensajeRetorno);

            return PartialView("PartialViewEstadisticasDetalles", listEstadisticas);
        }
    }
}