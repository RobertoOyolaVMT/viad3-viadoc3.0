using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.certificado;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class CertificadoController : Controller
    {

        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        public static HttpPostedFileBase fileBase;


        // GET: Certificado
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }

            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = objCatalogos.ConsultaEmpresa();
            List<CatEstado> listEstados = objCatalogos.ConsultaEstado();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listEstados"] = listEstados;
            return View();
        }


        [HttpPost]
        public ActionResult CargarCertificado(int txIdCompania)
        {
            Certificado objCertificadoParametros = new Certificado();
            objCertificadoParametros.CiCompania = txIdCompania == 0 ? 0 : txIdCompania;
            CertificadoLista certificadoLista = new CertificadoLista();
            certificadoLista.objListaCertificado = new ProcesoConfiguracion().ConsultarCertificado(objCertificadoParametros, ref codigoRetorno,
                ref mensajeRetorno);
            return PartialView("PartialViewCertificado", certificadoLista);
        }

        [HttpPost]
        public ActionResult InsertCertificado(string InsertData)
        {
            CertificadoLista certificadoLista = new CertificadoLista();
            ProcesoCertificado ObjPC = new ProcesoCertificado();
            Certificado Cert1 = new Certificado();
            string opcion = string.Empty;
            string[] Cadena = InsertData.Split('|');
            byte[] iv = null;
            byte[] archivoCertificado = null;
            string pathCertificado = string.Empty;
            string keyUisemilla = string.Empty;
            string IvTextWS = string.Empty;
            string Data = string.Empty;

            try
            {
                pathCertificado = CatalogoViaDoc.RutaCertificado +@"\" + Cadena[4].ToString().Trim();

                if (!Cadena[5].ToString().Trim().Equals(""))
                {

                    DateTime FchActual = DateTime.Now;
                    DateTime FchHasta = Convert.ToDateTime(Cadena[2].ToString().Trim());

                    if (DateTime.Compare(FchActual, FchHasta) > 0)
                    {
                        mensajeRetorno = "SU CERTIFICADO A CADUCADO";
                    }
                    else
                    {
                        bool CertificadoCorrecto = ObjPC.DetallleCertificado(pathCertificado, Cadena[5].ToString().Trim(), ref Cert1);

                        if (CertificadoCorrecto.Equals(true))
                        {
                            /*=======================Convierte el certificado en Byte============================*/
                            archivoCertificado = System.IO.File.ReadAllBytes(pathCertificado);
                            /*===================GENERA LA CLAVE POR MEDIO DEL METODO RIJNDAE=====================*/
                            System.Guid keytmp = Guid.NewGuid();
                            keyUisemilla = ObjPC.RetornaUiSemilla(keytmp);

                            string txtClaveEncrypt = AlgoritmoRijndael.encryptString(Cadena[5].ToString().Trim(), keyUisemilla, ref iv);

                            IvTextWS = Convert.ToBase64String(iv);

                            if (archivoCertificado.Length > 0)
                            {
                                opcion = "1";
                                Data =
                                    Cadena[0].ToString().Trim() + "|" +
                                    keytmp + "|" +
                                    txtClaveEncrypt + "|" +
                                    IvTextWS + "|" +
                                    Cadena[1].ToString().Trim() + "|" +
                                    Cadena[2].ToString().Trim() + "|" +
                                    Cadena[3].ToString().Trim();
                                
                                certificadoLista.objListaCertificado = ObjPC.InsertarCertificado(opcion, Data, archivoCertificado, "", ref codigoRetorno, ref mensajeRetorno);

                                if (mensajeRetorno.Equals("CERTIFICADO GUARDADO EXITOSO"))
                                {
                                    System.IO.File.Delete(pathCertificado);

                                }

                            }
                        }
                    }
                }
                else
                {
                    mensajeRetorno = "INGRESE EL PASSWORD DEL CERTIFICADO";
                }
            }
            catch (Exception ex)
            {
                Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message+" - "+ mensajeRetorno.ToString());
            }

            return base.Json(this.mensajeRetorno);
        }

        [HttpPost]
        public ActionResult ObtenerCert()
        {
            fileBase = Request.Files[0];
            return base.Json("");
        }
        [HttpPost]
        public ActionResult verificaClave(string compdata)
        {

            SucuersalCompanialista ObjSCLista = new SucuersalCompanialista();
            ProcesoCertificado ObjPC = new ProcesoCertificado();
            Certificado Cert1 = new Certificado();
            string[] cadena = compdata.Split('|');
            string pathCertificado = string.Empty;
            string ClaveActivacion = string.Empty;
            string CiCompañia = string.Empty;
            string FcDesde = string.Empty;
            string FcHasta = string.Empty;
            string ruta = CatalogoViaDoc.RutaCertificado + @"\" + cadena[2].ToString().Trim();
            string rutasimple = CatalogoViaDoc.RutaCertificado;

            try
            {

                ObjSCLista.ObjSC = ObjPC.ConsuSecuEmpres("2", cadena[1].ToString().Trim(), ref codigoRetorno, ref mensajeRetorno);

                if (cadena[3].ToString().Trim().Equals("1"))
                {
                    if (!Convert.ToInt32(ObjSCLista.ObjSC[0].secuencialCia).Equals(0))
                    {

                        if (!System.IO.File.Exists(ruta))
                        {
                            fileBase.SaveAs(rutasimple + @"\" + fileBase.FileName);
                        }
                        pathCertificado = CatalogoViaDoc.RutaCertificado + @"\" + cadena[2].ToString().Trim();
                 
                        bool valida = ObjPC.DetallleCertificado(pathCertificado, cadena[0].ToString().Trim(), ref Cert1);

                        if (valida.Equals(true))
                        {
                            FcDesde = Cert1.FcDesde.ToString().Trim();
                            FcHasta = Cert1.FcHasta.ToString().Trim();
                            ClaveActivacion = ObjSCLista.ObjSC[0].newIdcompañia.ToString().Trim();
                            CiCompañia = ObjSCLista.ObjSC[0].secuencialCia.ToString().Trim();

                            mensajeRetorno = "Clave de Certificado Correcta" + "|" + FcDesde + "|" + FcHasta + "|" + ClaveActivacion + "|" + CiCompañia;
                        }
                        else
                        {
                            mensajeRetorno = "Clave del Certifcado Incorrecta" + "|" + FcDesde;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return base.Json(this.mensajeRetorno);
        }

    }
}