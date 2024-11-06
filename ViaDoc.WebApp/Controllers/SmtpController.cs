using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class SmtpController : Controller
    {
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;

        // GET: Smtp
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }

            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = objCatalogos.ConsultaEmpresa();

            ViewData["listEmpresas"] = listEmpresas;
            return View();
        }

        [HttpPost]
        public ActionResult CargarConfiguracionSmtp(string txRazonSocial = "")
        {
            ProcesoConfiguracion objSmtp = new ProcesoConfiguracion();
            Smtp objSmtpParametros = new Smtp();
            objSmtpParametros.RazonSocial = txRazonSocial == "0"? "": txRazonSocial;
            SmtpLista smtpLista = new SmtpLista();
            smtpLista.objListSmtp = objSmtp.ConsultaConfigSmtp(objSmtpParametros, ref codigoRetorno, ref mensajeRetorno);
            if(smtpLista.objListSmtp.Count > 0)
            {
                return PartialView("PartialViewSmtp", smtpLista);
            }
            else
            {
                mensajeRetorno = "NO_EXISTE_REGISTRO";
                return base.Json(this.mensajeRetorno);
            }
        }


        [HttpPost]
        public ActionResult GuardarConfiguracionSmtp(string SmtpData)
        {
            ProcesoConfiguracion objSmtp = new ProcesoConfiguracion();
            Smtp objSmtpParametros = new Smtp();
            string[] Cadena = SmtpData.Split('|');

            try
            {
                objSmtpParametros.CiCompania = Cadena[0].Trim();
                objSmtpParametros.RazonSocial = Cadena[0].Trim();
                objSmtpParametros.HostServidor = Cadena[1].Trim();
                objSmtpParametros.Puerto = Cadena[2].Trim();
                objSmtpParametros.EnableSsl = Cadena[3].Trim();
                objSmtpParametros.EmailCredencial = Cadena[4].Trim();
                objSmtpParametros.PassCredencial = Cadena[5].Trim();
                objSmtpParametros.MailAddressfrom = Cadena[6].Trim();
                objSmtpParametros.Para = Cadena[7].Trim();
                objSmtpParametros.Cc = Cadena[8].Trim();
                objSmtpParametros.Asunto = Cadena[9].Trim();
                objSmtpParametros.ActivarNotificacion = Cadena[10].Trim();
                
                objSmtp.InsertarConfiguracionSmtp(objSmtpParametros, ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
                Utilitarios.logs.LogsFactura.LogsInicioFin(mensajeRetorno);
            }
            
            return base.Json(this.mensajeRetorno);
        }

        [HttpPost]
        public ActionResult EliminarConfiguracionSmtp(string txIdCompania)
        {
            ProcesoConfiguracion objSmtp = new ProcesoConfiguracion();
            Smtp objSmtpParametros = new Smtp();
            objSmtpParametros.CiCompania = txIdCompania;            
            SmtpLista smtpLista = new SmtpLista();
            smtpLista.objListSmtp = objSmtp.ConsultaConfigSmtp(objSmtpParametros, ref codigoRetorno, ref mensajeRetorno);
            return PartialView("ParcialViewSmtp", smtpLista);
        }
    }
}