using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class AutorizacionesController : Controller
    {

        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;

        ProcesoDocumento objDocumentos = new ProcesoDocumento();

        // GET: Autorizaciones
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


        [HttpPost]
        public  async Task<ActionResult> CargarDocumentosAutorizar(string txtIdEmpresa,
                                                      string txtIdTipoDocumento,
                                                      string txtNumDocumento)
        {
           
            var objDocParametros = new Documento();

            objDocParametros.razonSocial = txtIdEmpresa;
            objDocParametros.tipoDocumento = txtIdTipoDocumento;
            objDocParametros.NumeroDocumento = txtNumDocumento;

            var objListAutorizar = await Task.Run(() => objDocumentos.ConsultarDocumentosAutorizar(objDocParametros, ref codigoRetorno, ref mensajeRetorno));

            return PartialView("ParcialViewAutorizaciones", objListAutorizar);
        }

        public async Task<JsonResult> AutorizarEnLinea(string txtClaveAcceso,
                                             string txtIdTipoDocumento,
                                             int txtIdCompania)
        {

            var autorizaciones = new ViaDocAutorizacion.LogicaNegocios.MetodosDocumentos();
            await Task.Run(() => autorizaciones.GenerarRecepcionesAutorizacionesWeb(txtClaveAcceso, txtIdCompania, txtIdTipoDocumento, ref codigoRetorno, ref mensajeRetorno));
            if (mensajeRetorno.Trim() == "")
            {
                mensajeRetorno = "Documento autorizado";
            }
            return Json(mensajeRetorno);
        }
    }
}