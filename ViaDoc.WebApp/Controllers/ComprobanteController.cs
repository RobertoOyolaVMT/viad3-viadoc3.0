using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaDoc.WebApp.Models;

namespace ViaDoc.WebApp.Controllers
{

    

    public class ComprobanteController : Controller
    {
        MetodosCatalogos metodosCatalogos = new MetodosCatalogos();
        // GET: Comprobante
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PruebasFactura()
        {
            ViewBag.listaCompania = metodosCatalogos.ConsultaCompania();

            return View();
        }
    }
}