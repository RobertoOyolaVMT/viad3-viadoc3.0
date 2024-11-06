
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;
using ViaDocEnvioCorreo.Negocios;

namespace ViaDoc.WebApp.Controllers
{
    public class GenerarPDFZipController : Controller
    {
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        private bool exito;

        // GET: GenerarPDFZip
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }

            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = new List<CatCompania>();
            listEmpresas = objCatalogos.ConsultaEmpresa();
            List<CatDocumento> listDocumentos = new List<CatDocumento>();
            listDocumentos = objCatalogos.ConsultaDocumento();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listDocumentos"] = listDocumentos;
            return View();
        }

        public ActionResult GenerePDF(string Data)
        {
            ProcesoGenererPDF GeneraRide = new ProcesoGenererPDF();
            ProcesoGenerarRideWeb objR = new ProcesoGenerarRideWeb();

            byte[] Ride = null;
            byte[] byZip = null;
            DataSet ds = null;
            string fecha = string.Empty;
            string rutaTmp = string.Empty;
            string filepath = string.Empty;
            string rutaZip = string.Empty;
            string nombreDocumento = string.Empty;
            string nameDoc = string.Empty;
            Boolean valTipName = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("RideNameZip").Trim());

            string[] Parametros = Data.Split('|');
            string codEmpresa = Parametros[0].Trim();
            string codDocumento = Parametros[1].Trim();
            string fechaDesde = Parametros[2].Trim();
            string fechaHasta = Parametros[3].Trim();

            try
            {
                if (codDocumento.Equals("01"))
                    nombreDocumento = "Factura";
                else if (codDocumento.Equals("03"))
                    nombreDocumento = "Liquidacion";
                else if (codDocumento.Equals("04"))
                    nombreDocumento = "NotaCredito";
                else if (codDocumento.Equals("05"))
                    nombreDocumento = "NotaDebito";
                else if (codDocumento.Equals("06"))
                    nombreDocumento = "GuiaRemision";
                else if (codDocumento.Equals("07"))
                    nombreDocumento = "CompRetencion";

                ds = GeneraRide.ConsultaGenraPDF(1, codEmpresa, nombreDocumento, fechaDesde, fechaHasta, ref codigoRetorno, ref mensajeRetorno);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow datos in ds.Tables[0].Rows)
                    {
                        string ClaveAcceso = datos["txClaveAcceso"].ToString().Trim();
                        string numeroDocumento = datos["NumeroDocumento"].ToString().Trim();
                        string FechaAutorización = datos["txFechaHoraAutorizacion"].ToString().Trim();
                        string NumeroAutorización = datos["txClaveAcceso"].ToString().Trim();
                        string razonSocial = datos["RazonSocial"].ToString().Trim();
                        string xmldoc = datos["xmlDocumentoAutorizado"].ToString().Trim();

                        DateTime fechaActual = DateTime.Today;
                        fecha = fechaActual.ToString("dd/MM/yyyy");

                        rutaTmp = ConfigurationManager.AppSettings.Get("pathGeneraRIDE").Trim() + nombreDocumento + "/" + fecha + "tmp";
                        filepath = rutaTmp;
                        rutaZip = ConfigurationManager.AppSettings.Get("pathGeneraRIDE").Trim() + nombreDocumento + "\\" + fecha;

                        #region Proceso de generacion del Ride de los documentos
                        if (!System.IO.Directory.Exists(filepath))
                        {
                            System.IO.Directory.CreateDirectory(filepath);
                            System.IO.Directory.CreateDirectory(rutaZip);
                        }

                        Ride = objR.GenerarRideDocumentos(Convert.ToInt32(codEmpresa), xmldoc, FechaAutorización, ClaveAcceso, codDocumento, ref this.codigoRetorno, ref this.mensajeRetorno);

                        nameDoc = valTipName == true ? numeroDocumento : razonSocial.Replace("/","-").Replace("?","-") + '-' + numeroDocumento;

                        filepath += "/" + nombreDocumento + "-" + nameDoc + ".pdf";

                        using (System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Create))
                        {
                            fs.Write(Ride, 0, Ride.Length);
                            fs.Flush();
                            fs.Close();
                        }
                        exito = true;
                        #endregion

                    }
                    if (exito)
                    {
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Ingresa despues del Bool exito");
                        rutaZip += "\\" + nombreDocumento + ".zip";
                        if (System.IO.File.Exists(rutaZip))
                        {
                            System.IO.File.Delete(rutaZip);
                        }
                        ZipFile.CreateFromDirectory(rutaTmp, rutaZip);
                        Directory.Delete(rutaTmp, true);

                        byZip = System.IO.File.ReadAllBytes(rutaZip);
                        string Zip64 = Convert.ToBase64String(byZip);

                        mensajeRetorno = "DESCARGA EXITOSA" + "|" + nombreDocumento + ".zip" + "|" + Zip64;
                    }
                }
                else
                {
                    mensajeRetorno = "NO SE ENCONTRO DOCUMENTOS" + "|" + "" + "|" + "";
                }
            }

            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error: " + ex.Message);
                mensajeRetorno = "Se generó un imprevisto, por favor comuníquese con el departamento de sistemas." + "|" + "" + "|" + "";
            }

            var json_return = base.Json(this.mensajeRetorno);
            json_return.MaxJsonLength = Int32.MaxValue;

            return json_return;
        }

        public ActionResult GenereXML(string Data)
        {
            ProcesoGenererPDF GeneraRide = new ProcesoGenererPDF();

            byte[] xmll = null;
            byte[] byZip = null;
            DataSet ds = null;
            string fecha = string.Empty;
            string rutaTmp = string.Empty;
            string filepath = string.Empty;
            string rutaZip = string.Empty;
            string nombreDocumento = string.Empty;

            string[] Parametros = Data.Split('|');
            string codEmpresa = Parametros[0].Trim();
            string codDocumento = Parametros[1].Trim();
            string fechaDesde = Parametros[2].Trim();
            string fechaHasta = Parametros[3].Trim();


            if (codDocumento.Equals("01"))
                nombreDocumento = "Factura";
            else if (codDocumento.Equals("03"))
                nombreDocumento = "Liquidacion";
            else if (codDocumento.Equals("04"))
                nombreDocumento = "NotaCredito";
            else if (codDocumento.Equals("05"))
                nombreDocumento = "NotaDebito";
            else if (codDocumento.Equals("06"))
                nombreDocumento = "GuiaRemision";
            else if (codDocumento.Equals("07"))
                nombreDocumento = "CompRetencion";

            ds = GeneraRide.ConsultaGenraPDF(2, codEmpresa, nombreDocumento, fechaDesde, fechaHasta, ref codigoRetorno, ref mensajeRetorno);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow datos in ds.Tables[0].Rows)
                {
                    string ClaveAcceso = datos["txClaveAcceso"].ToString().Trim();
                    String xmldoc = datos["xmlDocumentoAutorizado"].ToString().Trim();
                    string numeroDocumento = datos["NumeroDocumento"].ToString().Trim();

                    DateTime fechaActual = DateTime.Today;
                    fecha = fechaActual.ToString().Substring(0, 10).Replace("/", "-"); ;

                    rutaTmp = ConfigurationManager.AppSettings.Get("pathGeneraRIDE").Trim() + nombreDocumento + "/" + fecha + "tmp";//Nombre del directorio donde se va a crear los archivos temporalmente
                    filepath = rutaTmp;//SE concatena el nombre del archivo para guardarlo en la ruta temporal
                    rutaZip = ConfigurationManager.AppSettings.Get("pathGeneraRIDE").Trim() + nombreDocumento + "\\" + fecha;//Ruta de la carpeta donde estan guardado los pdf para comprimirlo

                    //Si la ruta o existe la creo
                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                        System.IO.Directory.CreateDirectory(rutaZip);
                    }

                    //Si no es Guia De Remision
                    #region Crea XML Para ser descargado

                    //Crea pdf y teeransforma a byte
                    xmll = System.Text.Encoding.ASCII.GetBytes(xmldoc); //Convierto xml a byte
                    filepath += "/" + nombreDocumento + "-" + numeroDocumento + ".xml";
                    //Guardo el Byte en la Ruta De los RIDE
                    System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Create);
                    fs.Write(xmll, 0, xmll.Length);
                    fs.Flush();
                    fs.Close();
                    exito = true;
                    #endregion
                }
                if (exito)
                {
                    rutaZip += "\\" + nombreDocumento + ".zip";
                    if (System.IO.File.Exists(rutaZip))
                    {
                        System.IO.File.Delete(rutaZip);
                    }
                    ZipFile.CreateFromDirectory(rutaTmp, rutaZip);
                    Directory.Delete(rutaTmp, true);

                    byZip = System.IO.File.ReadAllBytes(rutaZip);
                    string Zip64 = Convert.ToBase64String(byZip);

                    mensajeRetorno = "DESCARGA EXITOSA" + "|" + nombreDocumento + ".zip" + "|" + Zip64;
                }
            }
            else
            {
                mensajeRetorno = "NO SE ENCONTRO DOCUEMTOS" + "|" + "" + "|" + "";
            }

            var json_return = base.Json(this.mensajeRetorno);
            json_return.MaxJsonLength = Int32.MaxValue;

            return json_return;
        }
    }
}