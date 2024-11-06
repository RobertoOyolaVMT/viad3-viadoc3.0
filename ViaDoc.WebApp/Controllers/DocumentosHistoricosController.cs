using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;
using ViaDocEnvioCorreo.LogicaNegocios;
using ViaDocEnvioCorreo.Negocios;

namespace ViaDoc.WebApp.Controllers
{
    public class DocumentosHistoricosController : Controller
    {
        ProcesoDocumento objDocumentos = new ProcesoDocumento();
        int codigoRetorno = 0;
        string mensajeRetorno = string.Empty;

        public ActionResult Index()
        {
            String empresaHistorico = ConfigurationManager.AppSettings["empresasHistorico"].ToString().Split('|')[0];
            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = new List<CatCompania>();
            listEmpresas = objCatalogos.ConsultaEmpresaHistorico(empresaHistorico);
            List<CatDocumento> listDocumentos = new List<CatDocumento>();
            listDocumentos = objCatalogos.ConsultaDocumento();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listDocumentos"] = listDocumentos;

            return View();
        }

        public JsonResult RecargarEmpresas(string empresaHistorico)
        {
            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = new List<CatCompania>();
            listEmpresas = objCatalogos.ConsultaEmpresaHistorico(empresaHistorico);

            return Json(listEmpresas);
        }

        public JsonResult CargarDocumentos(string empresaHistorico,
                                            string txtIdEmpresa,
                                            string txtIdTipoDocumento,
                                            string txtNumDocumento,
                                            string txtClaveAcceso,
                                            string txtIdentificacion,
                                            string txtNombre,
                                            string txtAutorizacion,
                                            string txtFechaInicio,
                                            string txtFechaFin)
        {
            Documento objDocParametros = new Documento();

            objDocParametros.razonSocial = txtIdEmpresa;
            objDocParametros.tipoDocumento = txtIdTipoDocumento;
            objDocParametros.NumeroDocumento = txtNumDocumento;
            objDocParametros.claveAcceso = txtClaveAcceso;
            objDocParametros.identificacionComprador = txtIdentificacion;
            objDocParametros.razonSocialComprador = txtNombre;
            objDocParametros.filtroFechaDH = txtAutorizacion;
            objDocParametros.fechaDesde = txtFechaInicio;
            objDocParametros.fechaHasta = txtFechaFin;
            objDocParametros.numeroAutorizacion = "";

            DocumentosLista objListDocumentos = new DocumentosLista();
            objListDocumentos.objListaDocumento = objDocumentos.ConsultarDocumentosTodosHistoricos(empresaHistorico, objDocParametros, ref codigoRetorno, ref mensajeRetorno);

            var map = new Dictionary<string, dynamic>();
            map.Add("data", objListDocumentos.objListaDocumento);

            return new JsonResult()
            {
                Data = map,
                MaxJsonLength = 86753090
            };
        }

        public ActionResult DescargarXML(string empresaHistorico, string txtClaveAcceso, string txtTipoDocumento)
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }
            codigoRetorno = 0;
            mensajeRetorno = "";

            if (txtTipoDocumento == "FACTURA")
                txtTipoDocumento = "Factura";
            else if (txtTipoDocumento.Trim() == "LIQUIDACIÓN DE COMPRA")
                txtTipoDocumento = "Liquidacion";
            else if (txtTipoDocumento == "NOTA DE CREDITO")
                txtTipoDocumento = "NotaCredito";
            else if (txtTipoDocumento == "NOTA DE DEBITO")
                txtTipoDocumento = "NotaDebito";
            else if (txtTipoDocumento == "GUIA DE REMISION")
                txtTipoDocumento = "GuiaRemision";
            else
                txtTipoDocumento = "CompRetencion";

            string xmlDescargar = objDocumentos.ConsultarXMLDescargarHistoricos(empresaHistorico, txtClaveAcceso, txtTipoDocumento, ref codigoRetorno, ref mensajeRetorno);

            if (xmlDescargar != null && xmlDescargar != "")
            {
                Byte[] xmll = System.Text.Encoding.ASCII.GetBytes(xmlDescargar); //Convierto xml a byte

                //Procesos para descargar XML
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "filename =" + txtClaveAcceso + ".xml");
                Response.AppendHeader("Content-Length", xmll.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(xmll);
                Response.End();
            }

            if (mensajeRetorno == "")
                mensajeRetorno = "XML Descargado";
            else
                mensajeRetorno = "Se presentó un error con la descarga";


            return RedirectToAction("Index");
        }

        public ActionResult DescargarPDF(string empresaHistorico, int ciIdCompania, string txFechaHoraAutorizacion, string txNumeroAutorizacion, string txTipoDocumento, string txNumeroDocumento)
        {
            codigoRetorno = 0;
            mensajeRetorno = "";

            try
            {
                string tipoDocumento = "";
                if (txTipoDocumento.Trim().Equals("FACTURA"))
                {
                    txTipoDocumento = "Factura";
                    tipoDocumento = "01";
                }
                if (txTipoDocumento.Trim().Equals("LIQUIDACIÓN DE COMPRA"))
                {
                    txTipoDocumento = "Liquidacion";
                    tipoDocumento = "03";
                }
                if (txTipoDocumento.Trim().Equals("NOTA DE CREDITO"))
                {
                    txTipoDocumento = "NotaCredito";
                    tipoDocumento = "04";
                }
                if (txTipoDocumento.Trim().Equals("NOTA DE DEBITO"))
                {
                    txTipoDocumento = "NotaDebito";
                    tipoDocumento = "05";
                }
                if (txTipoDocumento.Trim().Equals("GUIA DE REMISION"))
                {
                    txTipoDocumento = "GuiaRemision";
                    tipoDocumento = "06";
                }
                if (txTipoDocumento.Trim().Equals("COMPROBANTE DE RETENCION"))
                {
                    txTipoDocumento = "CompRetencion";
                    tipoDocumento = "07";
                }

                string xmlComprobante = objDocumentos.ConsultarXMLDescargarHistoricos(empresaHistorico, txNumeroAutorizacion, txTipoDocumento, ref this.codigoRetorno, ref this.mensajeRetorno);

                if (xmlComprobante.Equals(""))
                {
                    mensajeRetorno = "Se presentó un error con la descarga";
                }
                else
                {
                    var objR = new ProcesoGenerarRideWeb();

                    byte[] buffer = objR.GenerarRideDocumentosHistorico(empresaHistorico, ciIdCompania, xmlComprobante, txFechaHoraAutorizacion, txNumeroAutorizacion, tipoDocumento, ref this.codigoRetorno, ref this.mensajeRetorno);

                    if (buffer != null)
                    {
                        base.Response.Clear();

                        if (CatalogoViaDoc.NOMBRE_RIDE.Equals("SI"))
                            base.Response.AppendHeader("Content-Disposition", "filename =" + txNumeroAutorizacion + ".pdf");
                        else
                            base.Response.AppendHeader("Content-Disposition", "filename =" + txTipoDocumento + txNumeroDocumento + ".pdf");

                        base.Response.AppendHeader("Content-Length", buffer.Length.ToString());
                        base.Response.ContentType = "application/octet-stream";
                        base.Response.BinaryWrite(buffer);
                        base.Response.End();
                    }

                    this.mensajeRetorno = (this.mensajeRetorno != "") ? "Se present\x00f3 un error con la descarga" : "RIDE Descargado";
                }
                ViewData["mensajeError"] = mensajeRetorno;
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return RedirectToAction("Index");
        }

        private string GetXmlString()
        {
            var xDocument = XDocument.Load(@"C:\Users\Viamatica SA\Documents\nc.xml");
            // convert the xml into string
            string xml = xDocument.ToString();
            return xml;
        }

        public JsonResult EnviaCorreo(string empresaHistorico, string ciEstado, string ciIdCompania, string txTipoDocumento, string txClaveAcceso, string txEmail)
        {
            codigoRetorno = 0;
            mensajeRetorno = "";

            if (txTipoDocumento.Equals("FACTURA"))
            {
                txTipoDocumento = "01";
            }
            else if (txTipoDocumento.Equals("LIQUIDACIÓN DE COMPRA"))
            {
                txTipoDocumento = "03";
            }
            else if (txTipoDocumento.Equals("NOTA DE CREDITO"))
            {
                txTipoDocumento = "04";
            }
            else if (txTipoDocumento.Equals("NOTA DE DEBITO"))
            {
                txTipoDocumento = "05";
            }
            else if (txTipoDocumento.Equals("GUIA DE REMISION"))
            {
                txTipoDocumento = "06";
            }
            else
            {
                txTipoDocumento = "07";
            }

            ProcesoEnvioCorreo objCorreo = new ProcesoEnvioCorreo();

            objCorreo.EnvioDocumentosCorreoElectronicoHistorico(empresaHistorico, txEmail, "WEB", int.Parse(ciIdCompania), txTipoDocumento, 1, txClaveAcceso, ref mensajeRetorno);

            return base.Json(this.mensajeRetorno);
        }

        public JsonResult EnviaPortal(string empresaHistorico, string ciEstado, string ciIdCompania, string txTipoDocumento, string txClaveAcceso)
        {
            this.codigoRetorno = 0;
            this.mensajeRetorno = "";

            if (txTipoDocumento.Equals("FACTURA"))
            {
                txTipoDocumento = "01";
            }
            else if (txTipoDocumento.Equals("LIQUIDACIÓN DE COMPRA"))
            {
                txTipoDocumento = "03";
            }
            else if (txTipoDocumento.Equals("NOTA DE CREDITO"))
            {
                txTipoDocumento = "04";
            }
            else if (txTipoDocumento.Equals("NOTA DE DEBITO"))
            {
                txTipoDocumento = "05";
            }
            else if (txTipoDocumento.Equals("GUIA DE REMISION"))
            {
                txTipoDocumento = "06";
            }
            else
            {
                txTipoDocumento = "07";
            }

            ProcesoEnvioCorreo objCorreo = new ProcesoEnvioCorreo();

            objCorreo.EnviarDocumentosPortalWebHistorico(empresaHistorico, "WEB", int.Parse(ciIdCompania), txTipoDocumento, 1, txClaveAcceso, ref this.mensajeRetorno);
            return base.Json(this.mensajeRetorno);
        }

    }
}
