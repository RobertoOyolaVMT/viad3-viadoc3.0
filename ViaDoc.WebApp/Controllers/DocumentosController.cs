using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.Mvc;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;
using ViaDocEnvioCorreo.LogicaNegocios;
using ViaDocEnvioCorreo.Negocios;
using System.Xml.Linq;
using ClosedXML.Excel;
using System.Threading.Tasks;
using java.nio;
using ViaDoc.WebApp.ReportesNetCore;
using ViaDoc.WebApp.Models;
using RespuestaRide = ViaDoc.WebApp.Models.RespuestaRide;
using com.sun.org.apache.bcel.@internal.generic;
using System.Web.Helpers;
using System.Xml;
using ReportesViaDoc.EntidadesReporte;

namespace ViaDoc.WebApp.Controllers
{
    public class DocumentosController : Controller
    {
        ProcesoDocumento objDocumentos = new ProcesoDocumento();
        int codigoRetorno = 0;
        string mensajeRetorno = string.Empty;

        // GET: Documentos
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }

            var objCatalogos = new ProcesoCatalogos();
            var listEmpresas = new List<CatCompania>();
            listEmpresas = objCatalogos.ConsultaEmpresa();
            var listDocumentos = new List<CatDocumento>();
            listDocumentos = objCatalogos.ConsultaDocumento();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listDocumentos"] = listDocumentos;

            return View();
        }

        public async Task<JsonResult> CargarDocumentos(string txtIdEmpresa,
                                            string txtIdTipoDocumento,
                                            string txtNumDocumento,
                                            string txtClaveAcceso,
                                            string txtIdentificacion,
                                            string txtNombre,
                                            string txtAutorizacion,
                                            string txtFechaInicio,
                                            string txtFechaFin)
        {
            var objDocParametros = new Documento();

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

            var objListaDocumento = await Task.Run(() => objDocumentos.ConsultarDocumentosTodos(objDocParametros, ref codigoRetorno, ref mensajeRetorno));
            Session["TblExcel"] = ConvertToDataTable(objListaDocumento);

            var map = new Dictionary<string, dynamic>();
            map.Add("data", objListaDocumento);

            return new JsonResult()
            {
                Data = map,
                MaxJsonLength = 86753090
            };
        }

        public DataTable ConvertToDataTable(List<Documento> lista)
        {
            DataTable Rspuesta = new DataTable("Documentos");
            try
            {
                Rspuesta.Columns.Add("txRazonSocial");
                Rspuesta.Columns.Add("txIdentificacionComprador");
                Rspuesta.Columns.Add("txDescripcion");
                Rspuesta.Columns.Add("txFechaHoraAutorizacion");
                Rspuesta.Columns.Add("txNumeroAutorizacion");
                Rspuesta.Columns.Add("NumeroDocumento");
                Rspuesta.Columns.Add("txClaveAcceso");
                Rspuesta.Columns.Add("txtNameEstado");
                Rspuesta.Columns.Add("Tipo_Emisión");
                Rspuesta.Columns.Add("Subtotal");
                Rspuesta.Columns.Add("TotalIva");
                Rspuesta.Columns.Add("Valor");

                foreach (var item in lista)
                {
                    DataRow tdx = Rspuesta.NewRow();
                    tdx["txRazonSocial"] = item.razonSocial.ToString().Trim();
                    tdx["txIdentificacionComprador"] = item.identificacionComprador.ToString().Trim();
                    tdx["txDescripcion"] = item.descripcion.ToString().Trim();
                    tdx["txFechaHoraAutorizacion"] = item.fechaHoraAutorizacion.ToString().Trim();
                    tdx["txNumeroAutorizacion"] = item.numeroAutorizacion.ToString().Trim();
                    tdx["NumeroDocumento"] = item.NumeroDocumento.ToString().Trim();
                    tdx["txClaveAcceso"] = item.claveAcceso.ToString().Trim();
                    tdx["txtNameEstado"] = item.descripcionEstado.ToString().Trim();
                    tdx["Tipo_Emisión"] = "1".ToString().Trim();
                    tdx["Subtotal"] = item.subtotal.ToString().Trim();
                    tdx["TotalIva"] = item.totaliva.ToString().Trim();
                    tdx["Valor"] = item.valor.ToString().Trim();
                    Rspuesta.Rows.Add(tdx);
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
            return Rspuesta;
        }

        public async Task<ActionResult> DescargarXML(string txtClaveAcceso, string txtTipoDocumento)
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

            string xmlDescargar = await Task.Run(() => objDocumentos.ConsultarXMLDescargar(txtClaveAcceso, txtTipoDocumento, "", ref codigoRetorno, ref mensajeRetorno));

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

        public async Task<ActionResult> ObtieneXMLAutori(string txtClaveAcceso, string txtTipoDocumento, string nuevoDetalle, string codigoSec)
        {
            var json = new RespuestaRide();
            var estado = string.Empty;
            System.IO.StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            var listadetalles = new List<ListaDetalles>();
            var xmlComprobante = string.Empty;
            var tipoDocXml = string.Empty;

            try
            {

                if (Session["nombreUsuario"] == null)
                {
                    return RedirectToAction("IniciarSesion", "Validacion");
                }
                codigoRetorno = 0;
                mensajeRetorno = "";

                if (txtTipoDocumento == "FACTURA")
                {
                    txtTipoDocumento = "Factura"; 
                    tipoDocXml = "factura";
                }
                else if (txtTipoDocumento.Trim() == "LIQUIDACIÓN DE COMPRA")
                {
                    txtTipoDocumento = "Liquidacion";
                    tipoDocXml = "liquidacionCompra";
                }
                else if (txtTipoDocumento == "NOTA DE CREDITO") 
                {
                    txtTipoDocumento = "NotaCredito";
                    tipoDocXml = "notaCredito";
                }   
                else if (txtTipoDocumento == "NOTA DE DEBITO")
                {
                    txtTipoDocumento = "NotaDebito";
                    tipoDocXml = "notaDebito";
                }
                else if (txtTipoDocumento == "GUIA DE REMISION")
                {
                    txtTipoDocumento = "GuiaRemision";
                    tipoDocXml = "guiaRemision";
                }
                else
                {
                    txtTipoDocumento = "CompRetencion";
                    tipoDocXml = "comprobanteRetencion";
                }

                string xmlDescargar = await Task.Run(() => objDocumentos.ConsultarXMLDescargar(txtClaveAcceso, txtTipoDocumento, "", ref codigoRetorno, ref mensajeRetorno));

                XmlDocument document = new XmlDocument();
                XmlDocument documentXML = new XmlDocument();
                document.LoadXml(xmlDescargar.Replace("&lt;br&gt;", ""));
                document.WriteTo(xtr);
                var XMLString = document.InnerXml;

                XmlNodeList CamposAutorizacion = document.SelectNodes("autorizacion");
                XmlNode Informacion = CamposAutorizacion.Item(0);

                if (CamposAutorizacion != null && CamposAutorizacion.Count > 0)
                {
                    xmlComprobante = Informacion.SelectSingleNode("comprobante").InnerText.Replace("&", "&amp;");
                    json.Autorizada = "True";
                }
                else
                {
                    xmlComprobante = XMLString;
                    json.Autorizada = "False";
                }

                documentXML.LoadXml(xmlComprobante);
                documentXML.WriteTo(xtr);

                var tipoDetalles = $"{tipoDocXml}/detalles";

                XmlNodeList camposXML_detalles = documentXML.SelectNodes(tipoDetalles);

                foreach (XmlNode tagDetalles in camposXML_detalles)
                {
                    XmlNodeList nodos = tagDetalles.ChildNodes;
                    foreach (XmlNode nodo in nodos)
                    {

                        if (nodos.Count > 0)
                        {
                            var destalle = new ListaDetalles();
                            var destalle0 = new ListaDetalles();

                            destalle.CodigoPrincipal = nodo.SelectSingleNode("codigoPrincipal").InnerText;
                            destalle.Descripcion = nodo.SelectSingleNode("descripcion").InnerText;

                            if (listadetalles.Count == 0)
                            {
                                destalle0.Descripcion = "SELECCIONE";
                                listadetalles.Add(destalle0);
                            }

                            listadetalles.Add(destalle);

                        }

                        if (!string.IsNullOrEmpty(nuevoDetalle))
                        {
                            if (!string.IsNullOrEmpty(codigoSec))
                            {
                                if (codigoSec.Equals(nodo.SelectSingleNode("codigoPrincipal").InnerText))
                                {
                                    nodo.SelectSingleNode("descripcion").InnerText = nuevoDetalle;
                                    estado = "2";
                                }
                            }
                        }
                    }
                }

                if (json.Autorizada.Equals("True"))
                    Informacion.SelectSingleNode("comprobante").InnerText = documentXML.OuterXml;

                var newXML = document.OuterXml;

                if (estado.Equals("2"))
                    await Task.Run(() => objDocumentos.ConsultarXMLDescargar(txtClaveAcceso, txtTipoDocumento, newXML, ref codigoRetorno, ref mensajeRetorno));


                if (estado.Equals("2"))
                {

                    json.Cod = "111";
                    json.Documento = nuevoDetalle;
                    json.TipoDoc = txtTipoDocumento;

                }
                else
                {
                    json.Cod = "000";
                    json.Detalles = listadetalles;
                    json.TipoDoc = txtTipoDocumento;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return base.Json(json);
        }

        public async Task<ActionResult> DescargarPDF(int ciIdCompania, string txFechaHoraAutorizacion, string txNumeroAutorizacion, string txTipoDocumento, string txNumeroDocumento)
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

                string xmlComprobante = await Task.Run(() => objDocumentos.ConsultarXMLDescargar(txNumeroAutorizacion, txTipoDocumento, "", ref this.codigoRetorno, ref this.mensajeRetorno));

                if (xmlComprobante.Length == 0)
                {
                    mensajeRetorno = "Se presentó un error con la descarga";
                }
                else
                {
                    var objR = new ProcesoGenerarRideWeb();
                    byte[] buffer = objR.GenerarRideDocumentos(ciIdCompania, xmlComprobante, txFechaHoraAutorizacion, txNumeroAutorizacion, tipoDocumento, ref this.codigoRetorno, ref this.mensajeRetorno);

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

        public async Task<ActionResult> DescargarPDFNetCore(int ciIdCompania, string txFechaHoraAutorizacion, string txNumeroAutorizacion, string txTipoDocumento, string txNumeroDocumento)
        {
            codigoRetorno = 0;
            mensajeRetorno = "";

            var generaRide = new ApiReportesNetCore();
            var documentoRes = new RespuestaRide();

            try
            {
                var tipoDocumento = string.Empty;
                if (txTipoDocumento.Trim().Equals("FACTURA"))
                {
                    txTipoDocumento = "Ridefactura";
                    tipoDocumento = "01";
                }
                if (txTipoDocumento.Trim().Equals("LIQUIDACIÓN DE COMPRA"))
                {
                    txTipoDocumento = "RideLiquidacion";
                    tipoDocumento = "03";
                }
                if (txTipoDocumento.Trim().Equals("NOTA DE CREDITO"))
                {
                    txTipoDocumento = "RideNotaCredito";
                    tipoDocumento = "04";
                }
                if (txTipoDocumento.Trim().Equals("NOTA DE DEBITO"))
                {
                    txTipoDocumento = "RideNotaDebito";
                    tipoDocumento = "05";
                }
                if (txTipoDocumento.Trim().Equals("GUIA DE REMISION"))
                {
                    txTipoDocumento = "RideGuiaRemision";
                    tipoDocumento = "06";
                }
                if (txTipoDocumento.Trim().Equals("COMPROBANTE DE RETENCION"))
                {
                    txTipoDocumento = "RideCompRetencion";
                    tipoDocumento = "07";
                }

                documentoRes = await Task.Run(() => generaRide.Ride(txNumeroAutorizacion, txTipoDocumento));

                if (documentoRes.Documento.Length > 0)
                {
                    byte[] buffer = Convert.FromBase64String(documentoRes.Documento);

                    base.Response.Clear();
                    var contentDispositionHeader = string.Empty;

                    if (CatalogoViaDoc.NOMBRE_RIDE.Equals("SI"))
                        contentDispositionHeader = "filename =" + txNumeroAutorizacion + ".pdf";
                    else
                        contentDispositionHeader = "filename =" + txTipoDocumento + txNumeroDocumento + ".pdf";

                    base.Response.AppendHeader("Content-Disposition", contentDispositionHeader);
                    base.Response.AppendHeader("Content-Length", buffer.Length.ToString());
                    base.Response.ContentType = "application/octet-stream";
                    base.Response.BinaryWrite(buffer);
                    base.Response.End();
                }
                this.mensajeRetorno = (this.mensajeRetorno != "") ? "Se presentó un error con la descarga" : "RIDE Descargado";
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

        public async Task<JsonResult> EnviaCorreo(string ciEstado, string ciIdCompania, string txTipoDocumento, string txClaveAcceso, string txEmail)
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
            if (ciEstado.Equals("FI") || ciEstado.Equals("RE") || ciEstado.Equals("ERE"))
            {
                mensajeRetorno = "No se pudo enviar el correo. Verifique que el documento ha sido autorizado";
                return base.Json(this.mensajeRetorno);
            }
            ProcesoEnvioCorreo objCorreo = new ProcesoEnvioCorreo();

            await Task.Run(() => objCorreo.EnvioDocumentosCorreoElectronico(txEmail, "WEB", int.Parse(ciIdCompania), txTipoDocumento, 1, txClaveAcceso, ref mensajeRetorno));

            return base.Json(this.mensajeRetorno);
        }

        public JsonResult EnviaPortal(string ciEstado, string ciIdCompania, string txTipoDocumento, string txClaveAcceso)
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

            objCorreo.EnviarDocumentosPortalWeb("WEB", int.Parse(ciIdCompania), txTipoDocumento, 1, txClaveAcceso, ref this.mensajeRetorno);
            return base.Json(this.mensajeRetorno);
        }

        public ActionResult DescargaExcel()
        {
            try
            {
                XLWorkbook document = new XLWorkbook();
                MemoryStream ms = new MemoryStream();
                DataTable dt = new DataTable();

                if (Session["TblExcel"] != (null))
                {
                    DataTable dtDocumentosConsultados = (DataTable)Session["TblExcel"];

                    if (dtDocumentosConsultados.Rows.Count != 0)

                        //Crea cabeceras del excel

                        dt.Columns.Add("Razón Social", typeof(string));
                    dt.Columns.Add("Identificacion Comprador", typeof(string));
                    dt.Columns.Add("Descripcion", typeof(string));
                    dt.Columns.Add("Numero Documento", typeof(string));
                    dt.Columns.Add("Subtotal", typeof(decimal));
                    dt.Columns.Add("Importe Total", typeof(decimal));
                    dt.Columns.Add("Fecha y Hora de Autorizacion", typeof(string));
                    dt.Columns.Add("Numero de Autorización", typeof(string));
                    dt.Columns.Add("Clave de Acceso", typeof(string));
                    dt.Columns.Add("Estado", typeof(string));
                    dt.Columns.Add("Tipo Emision ", typeof(string));

                    foreach (DataRow Fila in dtDocumentosConsultados.Rows)
                    {
                        // Se llenan las celdas
                        DataRow data = dt.NewRow();

                        data[0] = Fila["txRazonSocial"].ToString().Replace("ñ", "&ntilde;");
                        data[1] = Fila["txIdentificacionComprador"].ToString();
                        data[2] = Fila["txDescripcion"].ToString();
                        data[3] = Fila["NumeroDocumento"].ToString();
                        data[4] = Convert.ToDecimal(Fila["Subtotal"].ToString().Replace(",", "."));
                        data[5] = Convert.ToDecimal(Fila["Valor"].ToString().Replace(",", "."));
                        data[6] = Fila["txFechaHoraAutorizacion"].ToString();
                        data[7] = Fila["txNumeroAutorizacion"].ToString() == "" ? "" : Fila["txNumeroAutorizacion"].ToString();
                        data[8] = Fila["txClaveAcceso"].ToString();
                        data[9] = Fila["txtNameEstado"].ToString();
                        data[10] = Fila["Tipo_Emisión"].ToString();

                        dt.Rows.Add(data);
                    }


                    string filepath = ConfigurationManager.AppSettings.Get("pathGeneraViaDocEcxel").ToString().Trim() + "/ReporteDocumentos.xlsx";

                    if (!System.IO.File.Exists(ConfigurationManager.AppSettings.Get("pathGeneraViaDocEcxel").ToString().Trim()))
                        Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("pathGeneraViaDocEcxel").ToString().Trim());

                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);

                    dt.TableName = "ReporteDocumentos";
                    var hoja = document.Worksheets.Add(dt);
                    hoja.ColumnsUsed().AdjustToContents();
                    var co = 5;
                    var ro = 2;

                    document.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    document.Style.Font.Bold = true;
                    hoja.Style.NumberFormat.Format = "0.00";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filepath + ".xlsx");

                    document.SaveAs(filepath);

                    byte[] byZip = System.IO.File.ReadAllBytes(filepath);
                    string Excel = Convert.ToBase64String(byZip);

                    mensajeRetorno = "DESCARGA EXITOSA" + "|" + "ReporteDocumentos" + ".xlsx" + "|" + Excel;
                    //Session["TblExcel"] = null;
                }
                else
                {
                    mensajeRetorno = "NO SE ENCONTRO DOCUEMTOS" + "|" + "" + "|" + "";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
                mensajeRetorno = ex.Message + "|" + "" + "|" + "";
            }

            var json_return = base.Json(this.mensajeRetorno);
            json_return.MaxJsonLength = Int32.MaxValue;

            return json_return;
        }
    }
}