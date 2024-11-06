using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.WebApp.Models;

namespace ViaDoc.WebApp.Controllers
{
    public class ConfiguracionReprocesoController : Controller
    {
        // GET: ConfiguracionReproceso
        MetodosConfiguracion metodosConfiguracion = new MetodosConfiguracion();
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;

        public static List<HoraNotificacion> parametrizacionHoras;

        // GET: ConfiguracionNotificacion
        public ActionResult Index()
        {
            //    List<CatTipoProceso> lista = new List<CatTipoProceso>();

            //    CatTipoProceso p1 = new CatTipoProceso();
            //    p1.id = 1;
            //    p1.descripcion = "Proceso Firma";
            //    lista.Add(p1);

            //    CatTipoProceso p2 = new CatTipoProceso();
            //    p2.id = 2;
            //    p2.descripcion = "Proceso Autorización";
            //    lista.Add(p2);

            //    CatTipoProceso p3 = new CatTipoProceso();
            //    p3.id = 3;
            //    p3.descripcion = "Proceso Correo y Portal";
            //    lista.Add(p3);

            //    ViewData["listTipoProceso"] = lista;
            return View();
        }


        [HttpPost]
        public JsonResult ConsultaParametrizacionHoras(int tipoProceso)
        {
            HorasNotificacionLista respuesta = new HorasNotificacionLista();
            respuesta.data = metodosConfiguracion.ConsultaParametrosHorasReproceso(tipoProceso, ref codigoRetorno, ref mensajeRetorno);
            parametrizacionHoras = respuesta.data;
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardarParametrizacion(int idRegistro, int tipoProceso, string horasInicio, string horasFinal)
        {
            MRetorno respuesta = new MRetorno();
            MParametro mParametro = new MParametro();

            if (idRegistro.Equals(0))
            {
                int maximo = parametrizacionHoras.Count == 0 ? 0 : parametrizacionHoras.Max(x => x.idRegistro);

                maximo++;
                parametrizacionHoras.Add(new HoraNotificacion()
                {
                    idRegistro = maximo,
                    HoraInicio = horasInicio,
                    HoraFin = horasFinal
                });
            }
            else
            {
                for (int i = 0; i < parametrizacionHoras.Count; i++)
                {
                    if (parametrizacionHoras[i].idRegistro == idRegistro)
                    {
                        parametrizacionHoras[i].HoraInicio = horasInicio;
                        parametrizacionHoras[i].HoraFin = horasFinal;
                    }
                }
            }

            metodosConfiguracion.IngresosParametrosHorasReproceso(tipoProceso, parametrizacionHoras, ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarParametrizacion(int idRegistro, int tipoProceso)
        {
            MRetorno respuesta = new MRetorno();

            var idRegistroEliminar = parametrizacionHoras.Find(x => x.idRegistro == idRegistro);
            parametrizacionHoras.Remove(idRegistroEliminar);

            metodosConfiguracion.IngresosParametrosHorasReproceso(tipoProceso, parametrizacionHoras, ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}