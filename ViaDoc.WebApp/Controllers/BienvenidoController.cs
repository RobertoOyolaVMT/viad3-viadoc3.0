using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ViaDoc.LogicaNegocios.portalweb;
using static ViaDoc.EntidadNegocios.portalWeb.PorInicio;

namespace ViaDoc.WebApp.Controllers
{
    public class BienvenidoController : Controller
    {
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        // GET: Bienvenido
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }
            Portalinicio ObjIniPortal = new Portalinicio();
            ListInicio ObjListaIni = new ListInicio();
            ObjListaIni.objComp = ObjIniPortal.Compania(3, ref codigoRetorno, ref mensajeRetorno);
            ObjListaIni.objDoc = ObjIniPortal.Document(4, ref codigoRetorno, ref mensajeRetorno);
            return View(ObjListaIni);
        }
        
        public ActionResult ReporteAyer()
        {
            Portalinicio Inicio = new Portalinicio();
            DataSet dsRespuest = null;
            DataTable RepAyer = null;
            string Autorizado = string.Empty;
            string NoAutorizado = string.Empty;
            string Recepcion = string.Empty;
            string ErrorRecepcion = string.Empty;
            string NoCorreo = string.Empty;
            string NoPortal = string.Empty;

            try
            {
                dsRespuest = Inicio.Reporte(1,ref codigoRetorno, ref mensajeRetorno);

                if (!dsRespuest.Equals(null))
                {
                    RepAyer = dsRespuest.Tables[0];

                    foreach(DataRow item in RepAyer.Rows)
                    {
                        Autorizado = item["Factura"].ToString();
                        NoAutorizado = item["NotaCredito"].ToString();
                        Recepcion = item["NotaDebito"].ToString();
                        ErrorRecepcion  = item["GuiaRemision"].ToString();
                        NoCorreo = item["CompRetencion"].ToString();
                        NoPortal = item["Liquidacion"].ToString();
                    }
                }
                
                mensajeRetorno = Autorizado + "|" + NoAutorizado + "|" + Recepcion + "|" + ErrorRecepcion + "|" + NoCorreo + "|" + NoPortal;
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return base.Json(this.mensajeRetorno);
        }

        public ActionResult ReporteActual()
        {
            Portalinicio Inicio = new Portalinicio();
            DataSet dsRespuest = null;
            DataTable RepAyer = null;
            string Autorizado = string.Empty;
            string NoAutorizado = string.Empty;
            string Recepcion = string.Empty;
            string ErrorRecepcion = string.Empty;
            string NoCorreo = string.Empty;
            string NoPortal = string.Empty;

            try
            {
                dsRespuest = Inicio.Reporte(2, ref codigoRetorno, ref mensajeRetorno);

                if (!dsRespuest.Equals(null))
                {
                    RepAyer = dsRespuest.Tables[0];

                    foreach (DataRow item in RepAyer.Rows)
                    {
                        Autorizado = item["Factura"].ToString();
                        NoAutorizado = item["NotaCredito"].ToString();
                        Recepcion = item["NotaDebito"].ToString();
                        ErrorRecepcion = item["GuiaRemision"].ToString();
                        NoCorreo = item["CompRetencion"].ToString();
                        NoPortal = item["Liquidacion"].ToString();
                    }
                }
               mensajeRetorno = Autorizado + "|" + NoAutorizado + "|" + Recepcion + "|" + ErrorRecepcion + "|" + NoCorreo + "|" + NoPortal;
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return base.Json(this.mensajeRetorno);
        }
    }
}