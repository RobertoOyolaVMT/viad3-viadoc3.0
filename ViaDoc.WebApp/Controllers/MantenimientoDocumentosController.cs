using System.Collections.Generic;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class MantenimientoDocumentosController : Controller
    {
        ProcesoDocumento objDocumentos = new ProcesoDocumento();
        int codigoRetorno = 0;
        string mensajeRetorno = string.Empty;

        public ActionResult Index()
        {
            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = objCatalogos.ConsultaEmpresa();
            List<CatDocumento> listDocumentos = objCatalogos.ConsultaDocumento();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listDocumentos"] = listDocumentos;

            return View();
        }
    }
}