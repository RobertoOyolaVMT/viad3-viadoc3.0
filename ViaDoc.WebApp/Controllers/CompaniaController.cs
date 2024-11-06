
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.catalogos;
using ViaDoc.LogicaNegocios.portalweb;

namespace ViaDoc.WebApp.Controllers
{
    public class CompaniaController : Controller
    {
        private int codigoRetorno = 0;
        private string mensajeRetorno = string.Empty;
        public static HttpPostedFileBase fileBase;

        // GET: Compania
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
                razonSocial = "9999",
                RucCompania = ""
            };
            listEmpresas.Add(addTodas);
            listEmpresas.AddRange(objCatalogos.ConsultaEmpresa());
            List<CatAmbiente> listAmbiente = objCatalogos.ConsultaAmbiente();
            List<CatEstado> listEstado = objCatalogos.ConsultaEstado();
            List<CatContabilidad> listContabilidad = objCatalogos.ConsultaContabilidad();

            ViewData["listEmpresas"] = listEmpresas;
            ViewData["listAmbientes"] = listAmbiente;
            ViewData["listEstados"] = listEstado;
            ViewData["listContabilidad"] = listContabilidad;
            return View();
        }


        [HttpPost]
        public ActionResult CargarCompania(string txRazonSocial)
        {
            Compania objCompaniaParametros = new Compania();
            objCompaniaParametros.TxRazonSocial = txRazonSocial;
            CompaniaLista companiaLista = new CompaniaLista();
            companiaLista.objListaCompania = new ProcesoConfiguracion().ConsultaCompanias(objCompaniaParametros, "", ref codigoRetorno, ref mensajeRetorno);
            return PartialView("PartialViewCompania", companiaLista);
        }


        [HttpPost]
        public ActionResult CargarSucursal(int txIdCompania)
        {
            Sucursal objSucursalParametros = new Sucursal();
            objSucursalParametros.ciCompania = txIdCompania;
            SucursalLista companiaLista = new SucursalLista();
            companiaLista.objListaSucursal = new ProcesoConfiguracion().ConsultaSucursal(objSucursalParametros, ref codigoRetorno, ref mensajeRetorno);
            return PartialView("PartialViewSucursal", companiaLista);
        }
        [HttpPost]
        public ActionResult ObtenerUrl()
        {
            try
            {
                fileBase = Request.Files[0];
            }
            catch
            {
                fileBase = null;
            }
            
            return base.Json("");
        }
        [HttpPost]
        public ActionResult GuardaCompania(Compania objCompania ,string blob_url, string tipomodal)
        {
            var objCompaniaParametros = new Compania();
            byte[] imgdata = null;
            var rutalogo = CatalogoViaDoc.rutaLogoCompania + blob_url;
            var nomImg = string.Empty;
            var rutaImgCom = CatalogoViaDoc.rutaLogoCompania + objCompania.TxRuc;
            try
            {
                objCompaniaParametros.TxRuc = objCompania.TxRuc;
                objCompaniaParametros.TxRazonSocial = objCompania.TxRazonSocial;
                objCompaniaParametros.TxNombreComercial = objCompania.TxNombreComercial;
                objCompaniaParametros.TxObligadoContabilidad = objCompania.TxObligadoContabilidad;
                objCompaniaParametros.TxContribuyenteEspecial = objCompania.TxContribuyenteEspecial;
                objCompaniaParametros.TxAgenteRetencion = objCompania.TxAgenteRetencion;
                objCompaniaParametros.TxRegimenMicroempresas = objCompania.TxRegimenMicroempresas;
                objCompaniaParametros.TxContribuyenteRimpe = objCompania.TxContribuyenteRimpe;
                objCompaniaParametros.TipoAmbiente = objCompania.TipoAmbiente;
                objCompaniaParametros.CiEstado = objCompania.CiEstado;
                objCompaniaParametros.TxDireccionMatriz = objCompania.TxDireccionMatriz;
                objCompaniaParametros.UiCompania = objCompania.UiCompania;

                if (blob_url.Trim().Equals(""))
                {
                    imgdata = System.IO.File.ReadAllBytes(CatalogoViaDoc.rutaLogoCompania + objCompania.TxRuc + ".png");
                }
                else if (!blob_url.Trim().Equals(""))
                {

                    System.IO.File.WriteAllBytes(CatalogoViaDoc.rutaLogoCompania + objCompania.TxRuc + ".png", imgdata);

                    if (!rutalogo.Contains(objCompaniaParametros.TxRuc))
                    {
                        System.IO.File.Delete(rutalogo);
                    }

                }
                objCompaniaParametros.LogoCompania = imgdata;
            }
            catch
            {
                if (!rutalogo.Contains(objCompaniaParametros.TxRuc))
                {

                    try
                    {
                        if(System.IO.File.Exists(rutalogo))
                            System.IO.File.Delete(rutalogo);
                            if (System.IO.File.Exists(rutaImgCom))
                                System.IO.File.Delete(rutaImgCom);

                        
                        fileBase.SaveAs(CatalogoViaDoc.rutaLogoCompania + objCompania.TxRuc + ".png");
                    }
                    catch
                    {

                    }
                    
                }
                else
                {
                    try
                    {
                        var fileName = Path.GetFileName(fileBase.FileName);
                        fileBase.SaveAs(CatalogoViaDoc.rutaLogoCompania + objCompania.TxRuc + ".png");
                    }
                    catch
                    {

                    }
                }             
            }
            if (Convert.ToInt32(tipomodal).Equals(0))
            {
                CompaniaLista companiaLista = new CompaniaLista();
                companiaLista.objListaCompania = new ProcesoConfiguracion().InsertarCompanias(objCompaniaParametros, 2, ref codigoRetorno, ref mensajeRetorno);
                mensajeRetorno = "Compañia Creada con Exito";
            }
            else if (Convert.ToInt32(tipomodal).Equals(1))
            {
                CompaniaLista companiaLista = new CompaniaLista();
                companiaLista.objListaCompania = new ProcesoConfiguracion().InsertarCompanias(objCompaniaParametros, 3, ref codigoRetorno, ref mensajeRetorno);
                mensajeRetorno = "Compañia Actualizada con Exito";
            }
            
           
            return base.Json(this.mensajeRetorno);
        }

        public ActionResult InsertaSucursal(string DataSucu)
        {
            string[] Cadena = DataSucu.Split('|');
            DataSet ds = null;
            SucursalLista ObjSucuLista = new SucursalLista();
            ProcesoConfiguracion ObjPC = new ProcesoConfiguracion();
            Sucursal ObjSucursal = new Sucursal();
            ObjSucursal.ciCompania = Convert.ToInt32(Cadena[0].ToString().Trim());
            ObjSucursal.ciSucursal = Cadena[1].ToString().Trim();
            ObjSucursal.direccion = Cadena[2].ToString().Trim();
            ObjSucursal.ciEstado = Cadena[3].ToString().Trim();

            ds = ObjPC.InsertarSucursal(ObjSucursal, ref codigoRetorno, ref mensajeRetorno);


            return base.Json(this.mensajeRetorno);
        }
        public ActionResult EliminarCompañia(string datos)
        {
            string[] cadena = datos.Split('|');
            ProcesoConfiguracion ObjPC = new ProcesoConfiguracion();
            Compania objCompaniaParametros = new Compania();
            objCompaniaParametros.TxRazonSocial = cadena[0];
            objCompaniaParametros.CiEstado = "I";

            ObjPC.EliminarCompanias(objCompaniaParametros, ref codigoRetorno, ref mensajeRetorno);

            mensajeRetorno = "Compañia Eliminada con Exito";

            return base.Json(this.mensajeRetorno);
        }

    }
}