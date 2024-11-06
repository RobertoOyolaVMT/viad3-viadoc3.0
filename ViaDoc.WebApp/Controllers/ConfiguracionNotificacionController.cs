using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.WebApp.Models;

namespace ViaDoc.WebApp.Controllers
{
    public class ConfiguracionNotificacionController : Controller
    {
        MetodosConfiguracion metodosConfiguracion = new MetodosConfiguracion();
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;

        public static List<HoraNotificacion> parametrizacionHoras;

        // GET: ConfiguracionNotificacion
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ConsultaParametrizacionHoras()
        {
            HorasNotificacionLista respuesta = new HorasNotificacionLista();
            respuesta.data = metodosConfiguracion.ConsultaParametrosHorasNotificacion(ref codigoRetorno, ref mensajeRetorno);
            parametrizacionHoras = respuesta.data;
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardarParametrizacion(int idRegistro, string horasInicio, string horasFinal)
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

            metodosConfiguracion.IngresosParametrosHorasNotificacion(parametrizacionHoras, ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarParametrizacion(int idRegistro)
        {
            MRetorno respuesta = new MRetorno();

            var idRegistroEliminar = parametrizacionHoras.Find(x => x.idRegistro == idRegistro);
            parametrizacionHoras.Remove(idRegistroEliminar);

            metodosConfiguracion.IngresosParametrosHorasNotificacion(parametrizacionHoras, ref codigoRetorno, ref mensajeRetorno);
            respuesta.codigoRetorno = codigoRetorno;
            respuesta.mensajeRetorno = mensajeRetorno;

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}