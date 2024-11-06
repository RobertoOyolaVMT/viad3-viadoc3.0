using System;
using System.Linq;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class ProcesoController : Controller
    {
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;

        // GET: Proceso
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CargarTiempoProceso(string txTipoServicio)
        {
            TiempoServicio objTiempoParametros = new TiempoServicio();
            objTiempoParametros.tipoServicio = txTipoServicio;
            TiempoServicioLista TiempoListaLista = new TiempoServicioLista();
            TiempoListaLista.objListaTiempo = new ProcesoConfiguracion().ConsultarHorasProceso(objTiempoParametros, ref codigoRetorno, ref mensajeRetorno);
            return PartialView("PartialViewTiempoProceso", TiempoListaLista);
        }
    }
}