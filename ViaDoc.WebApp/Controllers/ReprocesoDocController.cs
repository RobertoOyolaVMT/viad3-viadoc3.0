using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class ReprocesoDocController : Controller
    {
        ProcesoReproceso objReproDoc = new ProcesoReproceso();
        private int codigoRetorno;
        private string mensajeRetorno;

        // GET: ReprocesoDoc
        public ActionResult Index()
        {
            if (Session["nombreUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Validacion");
            }

            ProcesoCatalogos objCatalogos = new ProcesoCatalogos();
            List<CatCompania> listEmpresas = new List<CatCompania>();
            CatCompania addTodas = new CatCompania()
            {
                idCompania = 9999,
                nombreComercial = "TODAS",
                razonSocial = "TODAS",
                RucCompania = ""
            };
            listEmpresas.Add(addTodas);
            listEmpresas.AddRange(objCatalogos.ConsultaEmpresa());
            List<CatDocumento> listDocumentos = new List<CatDocumento>();
            listDocumentos = objCatalogos.ConsultaDocumento();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listDocumentos"] = listDocumentos;

            return View();
        }

        public ActionResult ConsultaDoc(string datos)
        {
            string[] cadena = datos.Split('|');
            ListaReproceso objListaRepro = new ListaReproceso();

            objListaRepro.objResproceso = objReproDoc.ConsultaDocError(cadena[0], cadena[1], cadena[4], cadena[2], cadena[3], "", "1", ref codigoRetorno, ref mensajeRetorno);
            Session["TblReproceso"] = ConvertToDataTable(objListaRepro.objResproceso);

            return PartialView("PartialViewReproceso", objListaRepro);
        }

        public ActionResult Reproceso_Uno(string datos)
        {
            ListaReproceso objListaRepro = new ListaReproceso();
            string[] cadena = datos.Split('|');

            string opcion = "";
            opcion = cadena[3].ToString().Trim() == "EFI" ? "2" : cadena[3].ToString().Trim() == "FI" ? "4" : "3";

            objListaRepro.objResproceso = objReproDoc.ConsultaDocError(cadena[0], cadena[1], "", "", "", cadena[2], opcion, ref codigoRetorno, ref mensajeRetorno);

            return base.Json(this.mensajeRetorno);
        }

        public ActionResult ReprocesoTodo(string datos)
        {
            string[] cadena = datos.Split('|');
            ListaReproceso objListaRepro = new ListaReproceso();
            try
            {
                string opcion = "";

                DataTable dtRespuesta = (DataTable)Session["TblReproceso"];
                foreach (DataRow row in dtRespuesta.Rows)
                {

                    opcion = row["ciEstado"].ToString().Trim() == "EFI" ? "2" :
                             row["ciEstado"].ToString().Trim() == "FI" ? "4" : "3";

                    objListaRepro.objResproceso = objReproDoc.ConsultaDocError(cadena[0], cadena[1], "", "", "", row["ClaveAcceso"].ToString().Trim(), opcion, ref codigoRetorno, ref mensajeRetorno);
                }
                mensajeRetorno = "El ciclo de los documentos ya fue reiniciado, por favor espere su reproceso";
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
            return base.Json(this.mensajeRetorno);
        }

        public DataTable ConvertToDataTable(List<ResprocesoMD> lista)
        {
            DataTable Rspuesta = new DataTable("Documentos");
            try
            {
                Rspuesta.Columns.Add("RazonSocial");
                Rspuesta.Columns.Add("TipoDocumento");
                Rspuesta.Columns.Add("NumeroDocumento");
                Rspuesta.Columns.Add("ClaveAcceso");
                Rspuesta.Columns.Add("FechaEmision");
                Rspuesta.Columns.Add("FechaHoraAutorizacion");
                Rspuesta.Columns.Add("Estado");
                Rspuesta.Columns.Add("ciEstado");

                foreach (var item in lista)
                {
                    DataRow tdx = Rspuesta.NewRow();
                    tdx["RazonSocial"] = item.RazonSocial.ToString().Trim();
                    tdx["TipoDocumento"] = item.TipoDocumento.ToString().Trim();
                    tdx["NumeroDocumento"] = item.NumeroDocumento.ToString().Trim();
                    tdx["ClaveAcceso"] = item.ClaveAcceso.ToString().Trim();
                    tdx["FechaEmision"] = item.FechaEmision.ToString().Trim();
                    tdx["FechaHoraAutorizacion"] = item.FechaHoraAutorizacion.ToString().Trim();
                    tdx["Estado"] = item.Estado.ToString().Trim();
                    tdx["ciEstado"] = item.CiEstado.ToString().Trim();
                    Rspuesta.Rows.Add(tdx);
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
            return Rspuesta;
        }
    }
}