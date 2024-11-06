using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ReportesViaDoc.EntidadesReporte;
using System.Data;
using System.Globalization;
using Microsoft.Reporting.WinForms;
using System.Configuration;
using System.Drawing;
using GenCode128;
using ViaDoc.Configuraciones;
using ViaDoc.Utilitarios;

namespace ReportesViaDoc.LogicaReporte
{
    public class ProcesarRideCompRetencion
    {

        private string pathCodBarra { get; set; }
        private string pathLogoEmpresa { get; set; }
        private String pathRider { get; set; }


        /// <summary>
        /// Autor: William Jacome Choez (Viamatica S.A)
        /// Descripcion: Crea el objeto que representa el xml para la generacion del ride.
        /// </summary>
        /// <param name="mensajeError">Mensaje de error producido durante el proceso</param>
        /// <param name="xmlDocumentoAutorizado">xml del documento puede estar firmado o autorizado</param>
        /// <param name="fechaHoraAutorizacion">Fecha y hora de la autorizacion del documento</param>
        /// <param name="numeroAutorizacion">Numero de autorizacion del documento</param>
        /// <returns>Objeto con los datos del xml del documento</returns>
        public RideCompRetencion GeneraComprobanteRetencionPDF(ref string mensajeError, string xmlDocumentoAutorizado, string fechaHoraAutorizacion, string numeroAutorizacion)
        {

            RideCompRetencion objRideRetencion = new RideCompRetencion();
            StringWriter stw = new StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            string version = string.Empty;
            try
            {
                XmlDocument document = new XmlDocument();
                xmlDocumentoAutorizado = Utilitarios.ReemplazarCaracteresEspeciales(xmlDocumentoAutorizado);

                document.LoadXml(xmlDocumentoAutorizado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml;

                XmlNodeList CamposAutorizacion = document.SelectNodes("autorizacion");

                XmlNode Informacion = CamposAutorizacion.Item(0);
                if (Informacion != null)
                {
                    objRideRetencion.BanderaGeneracionObjeto = true;
                    objRideRetencion.NumeroAutorizacion = Informacion.SelectSingleNode("numeroAutorizacion").InnerText;
                    objRideRetencion.FechaHoraAutorizacion = Informacion.SelectSingleNode("fechaAutorizacion").InnerText;
                    string xmlComprobante = Informacion.SelectSingleNode("comprobante").InnerText.Replace("&", "&amp;");
                    document.LoadXml(xmlComprobante);
                    document.WriteTo(xtr);

                    var Campos_Retencion = document.SelectNodes("comprobanteRetencion");
                    foreach (XmlNode item in Campos_Retencion)
                    {
                        XmlNodeList comprobantes = item.ChildNodes;
                        XmlAttributeCollection atributoNodo = item.Attributes;
                        version = atributoNodo["version"].InnerText;
                    }

                    XmlNodeList camposXML_infoTributaria = document.SelectNodes("comprobanteRetencion/infoTributaria");
                    XmlNodeList camposXML_infoCompRetencion = document.SelectNodes("comprobanteRetencion/infoCompRetencion");
                    XmlNodeList camposXML_retenciones = document.SelectNodes("comprobanteRetencion/docsSustento/docSustento/retenciones");
                    XmlNodeList composXML_impuesto;
                    if (version == "2.0.0")
                    {
                        composXML_impuesto = document.SelectNodes("comprobanteRetencion/docsSustento");
                    }
                    else
                    {
                        composXML_impuesto = document.SelectNodes("comprobanteRetencion/impuestos");
                    }


                    XmlNodeList camposXML_infoAdicional = document.SelectNodes("comprobanteRetencion/infoAdicional");

                    if (camposXML_infoTributaria.Count > 0 && camposXML_infoCompRetencion.Count > 0 && composXML_impuesto.Count > 0)
                    {
                        if (camposXML_infoAdicional.Count > 0)
                        {
                            #region Informacion Adicional de la Retencion
                            List<InformacionAdicional> listaInfoAdicional = new List<InformacionAdicional>();
                            foreach (XmlNode tagInfoAdicional in camposXML_infoAdicional)
                            {

                                XmlNodeList nodos = tagInfoAdicional.ChildNodes;
                                foreach (XmlNode nodo in nodos)
                                {
                                    InformacionAdicional infoAdicional = new InformacionAdicional();
                                    XmlAttributeCollection atributoNodo = nodo.Attributes;
                                    infoAdicional.Nombre = atributoNodo["nombre"].InnerText;
                                    infoAdicional.Valor = nodo.FirstChild.InnerText;
                                    listaInfoAdicional.Add(infoAdicional);
                                }
                            }
                            objRideRetencion._infosAdicional = listaInfoAdicional;
                            #endregion
                        }

                        #region Informacion tributaria deL Comprobante Retencion
                        XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                        objRideRetencion._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                        objRideRetencion._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                        try { objRideRetencion._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText; } catch (Exception) { objRideRetencion._infoTributaria.NombreComercial = ""; }
                        objRideRetencion._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;
                        objRideRetencion._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                        objRideRetencion._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                        objRideRetencion._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                        objRideRetencion._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                        objRideRetencion._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                        objRideRetencion._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;

                        try
                        {
                            objRideRetencion._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
                        }
                        catch (Exception)
                        {
                            if (!CatalogoViaDoc.LeyendaRegimen.Equals(""))
                            {
                                try
                                {
                                    if (!ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").ToString().Trim().Equals(""))
                                    {
                                        string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Split('|');
                                        foreach (string Ruc in Cadena)
                                        {
                                            if (!Ruc.Equals(""))
                                            {
                                                if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                        {
                                                            objRideRetencion._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                        }
                                                        else
                                                        {
                                                            objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                catch
                                {
                                    try
                                    {
                                        string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Split('|');
                                        foreach (string Ruc in Cadena)
                                        {
                                            if (!Ruc.Equals(""))
                                            {
                                                if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                            objRideRetencion._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                    }

                                }
                            }
                        }

                        try
                        {
                            if (informacionXML.SelectSingleNode("agenteRetencion").InnerText.Equals("1"))
                            {
                                if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                                {
                                    string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                                    foreach (string Ruc in Cadena)
                                    {
                                        if (!Ruc.Equals(""))
                                        {
                                            if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideRetencion._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                                }
                                                else
                                                {
                                                    objRideRetencion._infoTributaria.AgenteRetencion = " ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objRideRetencion._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                            {
                                string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                                foreach (string Ruc in Cadena)
                                {
                                    if (!Ruc.Equals(""))
                                    {
                                        if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideRetencion._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objRideRetencion._infoTributaria.AgenteRetencion = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        objRideRetencion._infoTributaria.AgenteRetencion = "";
                                    }
                                }
                            }
                        }
                        #region Leyenda Contribuyente Rimpe
                        try
                        {
                            objRideRetencion._infoTributaria.contribuyenteRimpe = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                        }
                        catch (Exception)
                        {
                            if (!CatalogoViaDoc.LeyendaRegimen.Equals(""))
                            {
                                try
                                {
                                    if (!ConfigurationManager.AppSettings.Get("regimen_Microempresas").ToString().Trim().Equals(""))
                                    {
                                        string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                                        foreach (string Ruc in Cadena)
                                        {
                                            if (!Ruc.Equals(""))
                                            {
                                                if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                            objRideRetencion._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideRetencion._infoTributaria.regimenMicroempresas = "";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                catch
                                {
                                    try
                                    {
                                        string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                                        foreach (string Ruc in Cadena)
                                        {
                                            if (!Ruc.Equals(""))
                                            {
                                                if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                            objRideRetencion._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideRetencion._infoTributaria.regimenMicroempresas = "";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                objRideRetencion._infoTributaria.regimenMicroempresas = "";
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        objRideRetencion._infoTributaria.regimenMicroempresas = "";
                                    }

                                }
                            }
                        }
                        #endregion

                        //#endregion
                        ////Regimen General
                        #region Leyenda Regimen General
                        //try
                        //{
                        //    if (informacionXML.SelectSingleNode("regimenGeneral").InnerText.Equals("1"))
                        //    {
                        //        if (!CatalogoViaDoc.LeyendaGeneral.Equals(""))
                        //        {
                        //            string[] Cadena = ConfigurationManager.AppSettings.Get("Regimen_General").Split('|');
                        //            foreach (string Ruc in Cadena)
                        //            {
                        //                if (!Ruc.Equals(""))
                        //                {
                        //                    if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                        //                    {
                        //                        bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                        //                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                        //                        {
                        //                            if (validaLeyenda)
                        //                                objRideRetencion._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                        //                        }
                        //                        else
                        //                        {
                        //                            objRideRetencion._infoTributaria.regimenGeneral = " ";
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        objRideRetencion._infoTributaria.regimenGeneral = string.Empty;
                        //    }
                        //}
                        //catch
                        //{
                        //    if (!CatalogoViaDoc.LeyendaGeneral.Equals(""))
                        //    {
                        //        string[] Cadena = ConfigurationManager.AppSettings.Get("Regimen_General").Split('|');
                        //        foreach (string Ruc in Cadena)
                        //        {
                        //            if (!Ruc.Equals(""))
                        //            {
                        //                if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                        //                {
                        //                    bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                        //                    if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                        //                    {
                        //                        if (validaLeyenda)
                        //                            objRideRetencion._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                        //                    }
                        //                    else
                        //                    {
                        //                        objRideRetencion._infoTributaria.regimenGeneral = "";
                        //                    }

                        //                }
                        //            }
                        //            else
                        //            {
                        //                objRideRetencion._infoTributaria.regimenGeneral = "";
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                        #endregion
                        #region Informacion del comprobante de retencion
                        XmlNode infoCompRetencion = camposXML_infoCompRetencion.Item(0);
                        objRideRetencion._infoCompRetencion.FechaEmision = infoCompRetencion.SelectSingleNode("fechaEmision").InnerText;
                        objRideRetencion._infoCompRetencion.DirEstablecimiento = infoCompRetencion.SelectSingleNode("dirEstablecimiento").InnerText;
                        try { objRideRetencion._infoCompRetencion.NumeroContribuyenteEspecial = infoCompRetencion.SelectSingleNode("contribuyenteEspecial").InnerText; } catch (Exception) { objRideRetencion._infoCompRetencion.NumeroContribuyenteEspecial = ""; } // se agrego esta linea.
                        objRideRetencion._infoCompRetencion.ObligadoContabilidad = infoCompRetencion.SelectSingleNode("obligadoContabilidad").InnerText;
                        objRideRetencion._infoCompRetencion.TipoIdentificacion = infoCompRetencion.SelectSingleNode("tipoIdentificacionSujetoRetenido").InnerText;
                        objRideRetencion._infoCompRetencion.RazonSocial = infoCompRetencion.SelectSingleNode("razonSocialSujetoRetenido").InnerText;
                        objRideRetencion._infoCompRetencion.Identificacion = infoCompRetencion.SelectSingleNode("identificacionSujetoRetenido").InnerText;
                        objRideRetencion._infoCompRetencion.PeriodoFiscal = infoCompRetencion.SelectSingleNode("periodoFiscal").InnerText;
                        #endregion

                        #region Impuestos de la retencion(Documento Sustento)
                        List<ImpuestoRetencion> listaImpuestos = new List<ImpuestoRetencion>();
                        if (version == "2.0.0")
                        {
                            foreach (XmlNode tagImpuestos in composXML_impuesto)
                            {
                                foreach (XmlNode tagRetencion in camposXML_retenciones)
                                {
                                    XmlNodeList nodos = tagImpuestos.ChildNodes;
                                    XmlNodeList nodosR = tagRetencion.ChildNodes;
                                    foreach (XmlNode nodo in nodos)
                                    {
                                        foreach (XmlNode nodoR in nodosR)
                                        {
                                            var prueba = nodo.SelectNodes("retenciones");
                                            ImpuestoRetencion impuesto = new ImpuestoRetencion();
                                            //impuesto.Codigo = nodo.SelectSingleNode("codigo").InnerText;
                                            //impuesto.CodigoRetencion = nodo.SelectSingleNode("codigoRetencion").InnerText;
                                            //impuesto.BaseImponible = nodo.SelectSingleNode("baseImponible").InnerText;
                                            //impuesto.PorcentajeRetener = nodo.SelectSingleNode("porcentajeRetener").InnerText;
                                            //impuesto.ValorRetenido = nodo.SelectSingleNode("valorRetenido").InnerText;
                                            impuesto.Codigo = nodoR.SelectSingleNode("codigo").InnerText;
                                            impuesto.CodigoRetencion = nodoR.SelectSingleNode("codigoRetencion").InnerText;
                                            impuesto.BaseImponible = nodoR.SelectSingleNode("baseImponible").InnerText;
                                            impuesto.PorcentajeRetener = nodoR.SelectSingleNode("porcentajeRetener").InnerText;
                                            impuesto.ValorRetenido = nodoR.SelectSingleNode("valorRetenido").InnerText;
                                            impuesto.CodDocSustento = nodo.SelectSingleNode("codDocSustento").InnerText;
                                            impuesto.NumDocSustento = nodo.SelectSingleNode("numDocSustento").InnerText;
                                            impuesto.FechaEmisionDocSustento = nodo.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                                            listaImpuestos.Add(impuesto);
                                        }

                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (XmlNode tagImpuestos in composXML_impuesto)
                            {
                                XmlNodeList nodos = tagImpuestos.ChildNodes;
                                foreach (XmlNode nodo in nodos)
                                {
                                    var prueba = nodo.SelectNodes("retenciones");
                                    ImpuestoRetencion impuesto = new ImpuestoRetencion();
                                    impuesto.Codigo = nodo.SelectSingleNode("codigo").InnerText;
                                    impuesto.CodigoRetencion = nodo.SelectSingleNode("codigoRetencion").InnerText;
                                    impuesto.BaseImponible = nodo.SelectSingleNode("baseImponible").InnerText;
                                    impuesto.PorcentajeRetener = nodo.SelectSingleNode("porcentajeRetener").InnerText;
                                    impuesto.ValorRetenido = nodo.SelectSingleNode("valorRetenido").InnerText;
                                    impuesto.CodDocSustento = nodo.SelectSingleNode("codDocSustento").InnerText;
                                    impuesto.NumDocSustento = nodo.SelectSingleNode("numDocSustento").InnerText;
                                    impuesto.FechaEmisionDocSustento = nodo.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                                    listaImpuestos.Add(impuesto);
                                }
                            }
                        }

                        objRideRetencion._impuestos = listaImpuestos;
                        #endregion

                    }

                }
                else
                    objRideRetencion = ProcesarXmlFirmadoCompRetencion(ref mensajeError, xmlDocumentoAutorizado);

            }
            catch (Exception ex)
            {
                objRideRetencion.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml autorizado de la Retencion. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
                //ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                //LogReporte.GuardaLog(mensajeError);
            }
            return objRideRetencion;
        }

        /// <summary>
        /// Autor: William Jacome Choez (Viamatica S.A)
        /// Descripcion: Crea el objeto que representa el xml para la generacion del ride.
        /// </summary>
        /// <param name="mensajeError">Mensaje de error producido durante el proceso</param>
        /// <param name="xmlDocumentoFirmado">xml del documento firmado</param>
        /// <returns>Objeto con los datos del xml del documento</returns>
        public RideCompRetencion ProcesarXmlFirmadoCompRetencion(ref string mensajeError, string xmlDocumentoFirmado)
        {
            RideCompRetencion objRideRetencion = new RideCompRetencion();
            StringWriter stw = new StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            string version = string.Empty;
            try
            {
                XmlDocument document = new XmlDocument();
                xmlDocumentoFirmado = Utilitarios.ReemplazarCaracteresEspeciales(xmlDocumentoFirmado);

                document.LoadXml(xmlDocumentoFirmado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml.Replace("&", "&amp;");
                var Campos_Retencion = document.SelectNodes("comprobanteRetencion");
                foreach (XmlNode item in Campos_Retencion)
                {
                    XmlNodeList comprobantes = item.ChildNodes;
                    XmlAttributeCollection atributoNodo = item.Attributes;
                    version = atributoNodo["version"].InnerText;
                }

                XmlNodeList camposXML_infoTributaria = document.SelectNodes("comprobanteRetencion/infoTributaria");
                XmlNodeList camposXML_infoCompRetencion = document.SelectNodes("comprobanteRetencion/infoCompRetencion");
                XmlNodeList camposXML_retenciones = document.SelectNodes("comprobanteRetencion/docsSustento/docSustento/retenciones");
                XmlNodeList composXML_impuesto;
                if (version == "2.0.0")
                {
                    composXML_impuesto = document.SelectNodes("comprobanteRetencion/docsSustento");
                }
                else
                {
                    composXML_impuesto = document.SelectNodes("comprobanteRetencion/impuestos");
                }

                if (camposXML_infoTributaria.Count > 0 && camposXML_infoCompRetencion.Count > 0 && composXML_impuesto.Count > 0)
                {
                    objRideRetencion.BanderaGeneracionObjeto = true;
                    XmlNodeList camposXML_infoAdicional = document.SelectNodes("comprobanteRetencion/infoAdicional");
                    if (camposXML_infoAdicional.Count > 0)
                    {
                        #region Informacion Adicional de la Retencion
                        List<InformacionAdicional> listaInfoAdicional = new List<InformacionAdicional>();
                        foreach (XmlNode tagInfoAdicional in camposXML_infoAdicional)
                        {

                            XmlNodeList nodos = tagInfoAdicional.ChildNodes;
                            foreach (XmlNode nodo in nodos)
                            {
                                InformacionAdicional infoAdicional = new InformacionAdicional();
                                XmlAttributeCollection atributoNodo = nodo.Attributes;
                                infoAdicional.Nombre = atributoNodo["nombre"].InnerText;
                                infoAdicional.Valor = nodo.FirstChild.InnerText;
                                listaInfoAdicional.Add(infoAdicional);
                            }
                        }
                        objRideRetencion._infosAdicional = listaInfoAdicional;
                        #endregion
                    }

                    #region Informacion tributaria deL Comprobante Retencion
                    XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                    objRideRetencion._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                    objRideRetencion._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                    objRideRetencion._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                    try { objRideRetencion._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText; } catch (Exception) { objRideRetencion._infoTributaria.NombreComercial = ""; }
                    objRideRetencion._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;
                    try
                    {
                        objRideRetencion._infoTributaria.contribuyenteRimpe = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
                    }
                    catch (Exception)
                    {
                        if (!CatalogoViaDoc.LeyendaRegimen.Equals(""))
                        {
                            try
                            {
                                if (!ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").ToString().Trim().Equals(""))
                                {
                                    string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Split('|');
                                    foreach (string Ruc in Cadena)
                                    {
                                        if (!Ruc.Equals(""))
                                        {
                                            if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                    {
                                                        objRideRetencion._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                                    }

                                                }
                                                else
                                                {
                                                    objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            catch
                            {
                                try
                                {
                                    string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Split('|');
                                    foreach (string Ruc in Cadena)
                                    {
                                        if (!Ruc.Equals(""))
                                        {
                                            if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideRetencion._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                }
                                                else
                                                {
                                                    objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                        }
                                    }
                                }
                                catch
                                {
                                    objRideRetencion._infoTributaria.contribuyenteRimpe = "";
                                }

                            }
                        }
                    }

                    try
                    {
                        if (informacionXML.SelectSingleNode("agenteRetencion").InnerText.Equals("1"))
                        {
                            if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                            {
                                string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                                foreach (string Ruc in Cadena)
                                {
                                    if (!Ruc.Equals(""))
                                    {
                                        if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideRetencion._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objRideRetencion._infoTributaria.AgenteRetencion = " ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objRideRetencion._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                        {
                            string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                            foreach (string Ruc in Cadena)
                            {
                                if (!Ruc.Equals(""))
                                {
                                    if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                objRideRetencion._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                        }
                                        else
                                        {
                                            objRideRetencion._infoTributaria.AgenteRetencion = "";
                                        }

                                    }
                                }
                                else
                                {
                                    objRideRetencion._infoTributaria.AgenteRetencion = "";
                                }
                            }
                        }
                    }
                    //REGIMEN MICROEMPRESAS
                    try
                    {
                        if (informacionXML.SelectSingleNode("regimenMicroempresas").InnerText.Equals(""))
                        {
                            if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                            {
                                string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                                foreach (string Ruc in Cadena)
                                {
                                    if (!Ruc.Equals(""))
                                    {
                                        if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideRetencion._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objRideRetencion._infoTributaria.regimenMicroempresas = " ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objRideRetencion._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                        {
                            string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                            foreach (string Ruc in Cadena)
                            {
                                if (!Ruc.Equals(""))
                                {
                                    if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                objRideRetencion._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                        }
                                        else
                                        {
                                            objRideRetencion._infoTributaria.regimenMicroempresas = "";
                                        }

                                    }
                                }
                                else
                                {
                                    objRideRetencion._infoTributaria.regimenMicroempresas = "";
                                }
                            }
                        }
                    }
                    #endregion
                    #region Regimen General
                    //#region Leyenda Regimen General
                    //try
                    //{
                    //    if (informacionXML.SelectSingleNode("regimenGeneral").InnerText.Equals("1"))
                    //    {
                    //        if (!CatalogoViaDoc.LeyendaGeneral.Equals(""))
                    //        {
                    //            string[] Cadena = ConfigurationManager.AppSettings.Get("Regimen_General").Split('|');
                    //            foreach (string Ruc in Cadena)
                    //            {
                    //                if (!Ruc.Equals(""))
                    //                {
                    //                    if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                    //                    {
                    //                        bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                    //                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                    //                        {
                    //                            if (validaLeyenda)
                    //                                objRideRetencion._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                    //                        }
                    //                        else
                    //                        {
                    //                            objRideRetencion._infoTributaria.regimenGeneral = " ";
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        objRideRetencion._infoTributaria.regimenGeneral = string.Empty;
                    //    }
                    //}
                    //catch
                    //{
                    //    if (!CatalogoViaDoc.LeyendaGeneral.Equals(""))
                    //    {
                    //        string[] Cadena = ConfigurationManager.AppSettings.Get("Regimen_General").Split('|');
                    //        foreach (string Ruc in Cadena)
                    //        {
                    //            if (!Ruc.Equals(""))
                    //            {
                    //                if (Convert.ToInt64(objRideRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                    //                {
                    //                    bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                    //                    if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                    //                    {
                    //                        if (validaLeyenda)
                    //                            objRideRetencion._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                    //                    }
                    //                    else
                    //                    {
                    //                        objRideRetencion._infoTributaria.regimenGeneral = "";
                    //                    }

                    //                }
                    //            }
                    //            else
                    //            {
                    //                objRideRetencion._infoTributaria.regimenGeneral = "";
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    objRideRetencion._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                    objRideRetencion.NumeroAutorizacion = informacionXML.SelectSingleNode("claveAcceso").InnerText;   //Agregado por Joseph
                    objRideRetencion._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                    objRideRetencion._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                    objRideRetencion._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                    objRideRetencion._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                    objRideRetencion._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                    //#endregion
                    #region Informacion del comprobante de retencion  contribuyenteEspecial dirEstablecimiento
                    XmlNode infoCompRetencion = camposXML_infoCompRetencion.Item(0);
                    objRideRetencion._infoCompRetencion.FechaEmision = infoCompRetencion.SelectSingleNode("fechaEmision").InnerText;
                    objRideRetencion._infoCompRetencion.DirEstablecimiento = infoCompRetencion.SelectSingleNode("dirEstablecimiento").InnerText;
                    try { objRideRetencion._infoCompRetencion.NumeroContribuyenteEspecial = infoCompRetencion.SelectSingleNode("contribuyenteEspecial").InnerText; } catch (Exception) { objRideRetencion._infoCompRetencion.NumeroContribuyenteEspecial = ""; } // se agrego esta linea.
                    objRideRetencion._infoCompRetencion.ObligadoContabilidad = infoCompRetencion.SelectSingleNode("obligadoContabilidad").InnerText;
                    objRideRetencion._infoCompRetencion.TipoIdentificacion = infoCompRetencion.SelectSingleNode("tipoIdentificacionSujetoRetenido").InnerText;
                    objRideRetencion._infoCompRetencion.RazonSocial = infoCompRetencion.SelectSingleNode("razonSocialSujetoRetenido").InnerText;
                    objRideRetencion._infoCompRetencion.Identificacion = infoCompRetencion.SelectSingleNode("identificacionSujetoRetenido").InnerText;
                    objRideRetencion._infoCompRetencion.PeriodoFiscal = infoCompRetencion.SelectSingleNode("periodoFiscal").InnerText;
                    #endregion
                    #region Impuestos de la retencion(Documento Sustento)
                    List<ImpuestoRetencion> listaImpuestos = new List<ImpuestoRetencion>();
                    if (version == "2.0.0")
                    {
                        foreach (XmlNode tagImpuestos in composXML_impuesto)
                        {
                            foreach (XmlNode tagRetencion in camposXML_retenciones)
                            {
                                XmlNodeList nodos = tagImpuestos.ChildNodes;
                                XmlNodeList nodosR = tagRetencion.ChildNodes;
                                foreach (XmlNode nodo in nodos)
                                {
                                    foreach (XmlNode nodoR in nodosR)
                                    {
                                        var prueba = nodo.SelectNodes("retenciones");
                                        ImpuestoRetencion impuesto = new ImpuestoRetencion();
                                        //impuesto.Codigo = nodo.SelectSingleNode("codigo").InnerText;
                                        //impuesto.CodigoRetencion = nodo.SelectSingleNode("codigoRetencion").InnerText;
                                        //impuesto.BaseImponible = nodo.SelectSingleNode("baseImponible").InnerText;
                                        //impuesto.PorcentajeRetener = nodo.SelectSingleNode("porcentajeRetener").InnerText;
                                        //impuesto.ValorRetenido = nodo.SelectSingleNode("valorRetenido").InnerText;
                                        impuesto.Codigo = nodoR.SelectSingleNode("codigo").InnerText;
                                        impuesto.CodigoRetencion = nodoR.SelectSingleNode("codigoRetencion").InnerText;
                                        impuesto.BaseImponible = nodoR.SelectSingleNode("baseImponible").InnerText;
                                        impuesto.PorcentajeRetener = nodoR.SelectSingleNode("porcentajeRetener").InnerText;
                                        impuesto.ValorRetenido = nodoR.SelectSingleNode("valorRetenido").InnerText;
                                        impuesto.CodDocSustento = nodo.SelectSingleNode("codDocSustento").InnerText;
                                        impuesto.NumDocSustento = nodo.SelectSingleNode("numDocSustento").InnerText;
                                        impuesto.FechaEmisionDocSustento = nodo.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                                        listaImpuestos.Add(impuesto);
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        foreach (XmlNode tagImpuestos in composXML_impuesto)
                        {
                            XmlNodeList nodos = tagImpuestos.ChildNodes;
                            foreach (XmlNode nodo in nodos)
                            {
                                var prueba = nodo.SelectNodes("retenciones");
                                ImpuestoRetencion impuesto = new ImpuestoRetencion();
                                impuesto.Codigo = nodo.SelectSingleNode("codigo").InnerText;
                                impuesto.CodigoRetencion = nodo.SelectSingleNode("codigoRetencion").InnerText;
                                impuesto.BaseImponible = nodo.SelectSingleNode("baseImponible").InnerText;
                                impuesto.PorcentajeRetener = nodo.SelectSingleNode("porcentajeRetener").InnerText;
                                impuesto.ValorRetenido = nodo.SelectSingleNode("valorRetenido").InnerText;
                                impuesto.CodDocSustento = nodo.SelectSingleNode("codDocSustento").InnerText;
                                impuesto.NumDocSustento = nodo.SelectSingleNode("numDocSustento").InnerText;
                                impuesto.FechaEmisionDocSustento = nodo.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                                listaImpuestos.Add(impuesto);
                            }
                        }
                    }

                    objRideRetencion._impuestos = listaImpuestos;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                objRideRetencion.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml firmado de la Retencion. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;

            }
            return objRideRetencion;
        }

        /// <summary>
        /// Autor: William Jacome Choez (Viamatica S.A.)
        /// Descripcion: Genera el byte[] del documento 
        /// </summary>
        /// <param name="mensajeError">Mensaje de Error del proceso</param>
        /// <param name="objRideCompRetencion">Objeto que representa el xml del documento</param>
        /// <param name="configuraciones">Configuraciones del reporte segun la compañia</param>
        /// <param name="catalogoSistema">Catalogos del sistema de Facturacion Electronica</param>
        /// <returns>Arreglo de byte que representa el ride del documento</returns>
        public Byte[] GenerarRideCompRetencion(ref string mensajeError, RideCompRetencion objRideCompRetencion, List<ConfiguracionReporte> configuraciones, List<CatalogoReporte> catalogoSistema)
        {

            var culturaNorteAmerica = CultureInfo.GetCultureInfo("es-US");
            byte[] bytePDF = null;
            string etiquetaContribuyenteEspecial = "";
            string etiquetaTotalRetenido = "";
            try
            {
                List<ConfiguracionReporte> confCompania = configuraciones.FindAll(x => x.RucCompania == objRideCompRetencion._infoTributaria.Ruc);
                if (confCompania != null)
                {


                    #region Origen Datos del RIDE CompRetencion
                    DataTable tblInfoTributaria = new DataTable();
                    DataTable tblInfoCompRetencion = new DataTable();
                    DataTable tblImpuesto = new DataTable();
                    DataTable tblInformacionAdicional = new DataTable();

                    tblInfoTributaria.TableName = "tblInformacionTributaria";
                    tblInfoCompRetencion.TableName = "tblInformacionCompRetencion";
                    tblImpuesto.TableName = "tblImpuestos";
                    tblInformacionAdicional.TableName = "tblInformacionAdicional";


                    DataColumn[] cols_tblInfoTributaria = new DataColumn[] {
                        new DataColumn("ambiente",typeof(string)),
                        new DataColumn("claveAcceso",typeof(string)),
                        new DataColumn("codDoc",typeof(string)),
                        new DataColumn("dirMartiz",typeof(string)),
                        new DataColumn("estab",typeof(string)),
                        new DataColumn("nombreComercial",typeof(string)),
                        new DataColumn("ptoEmi",typeof(string)),
                        new DataColumn("razonSocial",typeof(string)),
                        new DataColumn("ruc",typeof(string)),
                        new DataColumn("secuencial",typeof(string)),
                        new DataColumn("agenteRetencion",typeof(string)),
                        new DataColumn("regimenMicroempresas",typeof(string)),
                        //new DataColumn("regimenGeneral",typeof(string)),
                        new DataColumn("contribuyenteRimpe",typeof(string)),

                        new DataColumn("tipoEmision",typeof(string))
                    };

                    DataColumn[] cols_tblInfoCompRetencion = new DataColumn[] {
                        new DataColumn("contribuyenteEspecial",typeof(string)),
                        new DataColumn("dirEstablecimiento",typeof(string)),
                        new DataColumn("fechaEmision",typeof(string)),
                        new DataColumn("identificacionSujetoRetenido",typeof(string)),
                        new DataColumn("obligadoContabilidad",typeof(string)),
                        new DataColumn("periodoFiscal",typeof(string)),
                        new DataColumn("razonSocialSujetoRetenido",typeof(string)),
                        new DataColumn("tipoIdentificacionSujetoRetenido",typeof(string)),
                    };

                    DataColumn[] cols_tblImpuesto = new DataColumn[] {
                        new DataColumn("baseImponible",typeof(string)),
                        new DataColumn("codDocSustento",typeof(string)),
                        new DataColumn("codigo",typeof(string)),
                        new DataColumn("codigoRetencion",typeof(string)),
                        new DataColumn("fechaEmisionDocSustento",typeof(string)),
                        new DataColumn("numDocSustento",typeof(string)),
                        new DataColumn("porcentajeRetener",typeof(string)),
                        new DataColumn("valorRetenido",typeof(string)),
                    };

                    DataColumn[] cols_tblInformacionAdicional = new DataColumn[] {
                        new DataColumn("nombre",typeof(string)),
                        new DataColumn("valor",typeof(string))
                    };

                    tblInfoTributaria.Columns.AddRange(cols_tblInfoTributaria);
                    tblInfoCompRetencion.Columns.AddRange(cols_tblInfoCompRetencion);
                    tblImpuesto.Columns.AddRange(cols_tblImpuesto);
                    tblInformacionAdicional.Columns.AddRange(cols_tblInformacionAdicional);
                    #endregion


                    #region Logo de la Compania
                    pathLogoEmpresa = CatalogoViaDoc.rutaLogoCompania;
                    string rutaImagen = pathLogoEmpresa + objRideCompRetencion._infoTributaria.Ruc.Trim() + ".png";
                    #endregion

                    #region Genera Codigo Barra de la Factura
                    byte[] imgBar;
                    Code.BarcodeGenerator bgCode128 = new Code.BarcodeGenerator();
                    Code.Convertir cCode = new Code.Convertir();
                    Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                    StringFormat fi = new StringFormat(StringFormatFlags.NoClip);
                    fi.Alignment = StringAlignment.Center;
                    imgBar = RetornarXml.ImageToByte2(Code128Rendering.MakeBarcodeImage(objRideCompRetencion._infoTributaria.ClaveAcceso, 3, true));


                    pathCodBarra = CatalogoViaDoc.rutaCodigoBarra + objRideCompRetencion._infoTributaria.ClaveAcceso.Trim();
                    pathRider = CatalogoViaDoc.rutaRide + objRideCompRetencion._infoTributaria.Ruc.Trim() + "\\" + objRideCompRetencion._infoTributaria.CodigoDocumento.Trim();
                    if (!Directory.Exists(pathRider))
                    {
                        Directory.CreateDirectory(pathRider);
                    }

                    File.WriteAllBytes(pathCodBarra + ".jpg", imgBar);
                    #endregion

                    #region Configuracion de los separadores de miles de la Factura
                    ConfiguracionReporte confSeparadorMiles = confCompania.Find(x => x.CodigoReferencia == "ACTMIL");
                    string formatoMiles = "";
                    bool validaSeparadorMiles = false;
                    if (confSeparadorMiles != null)
                    {
                        validaSeparadorMiles = Convert.ToBoolean(confSeparadorMiles.Configuracion2);
                        if (validaSeparadorMiles)
                            formatoMiles = confSeparadorMiles.Configuracion1.Trim();
                    }
                    #endregion

                    #region Configuracion para la etiqueta Contribuyente especial
                    bool validaEtiquetaContribuyenteEspecial = false;
                    ConfiguracionReporte confEtiquetaContEspecial = confCompania.Find(x => x.CodigoReferencia == "ETIQCONT");
                    if (confEtiquetaContEspecial != null)
                    {
                        validaEtiquetaContribuyenteEspecial = Convert.ToBoolean(confEtiquetaContEspecial.Configuracion2.Trim());
                        if (validaEtiquetaContribuyenteEspecial)
                            etiquetaContribuyenteEspecial = confEtiquetaContEspecial.Configuracion1;
                    }
                    #endregion

                    #region Configuracion para visualizar el numero de contribuyente especial
                    bool validaNumeroContribuyenteEspecial = false;
                    ConfiguracionReporte confContribuEspecial = confCompania.Find(x => x.CodigoReferencia == "ACTNUMCONT");
                    if (confContribuEspecial != null)
                        validaNumeroContribuyenteEspecial = Convert.ToBoolean(confContribuEspecial.Configuracion2);
                    #endregion

                    #region Configuracion Etiqueta Total de la retencion
                    ConfiguracionReporte confEtiquetaTotalRetenido = confCompania.Find(x => x.CodigoReferencia == "ETIQTOTRET");
                    bool validaEtiquetaTotalRetenido = false;
                    if (confEtiquetaTotalRetenido != null)
                    {
                        validaEtiquetaTotalRetenido = Convert.ToBoolean(confEtiquetaTotalRetenido.Configuracion2);
                        if (validaEtiquetaTotalRetenido)
                            etiquetaTotalRetenido = confEtiquetaTotalRetenido.Configuracion1;
                    }
                    #endregion

                    #region Catalogo Ambiente Retencion
                    List<CatalogoReporte> ambientesFacturacion = catalogoSistema.FindAll(x => x.CodigoReferencia == "AMBIENTE");
                    bool validaAmbiente = false;
                    if (ambientesFacturacion != null)
                        validaAmbiente = true;
                    #endregion

                    #region Catalogo Emisiones Retencion
                    List<CatalogoReporte> emisionesFacturacion = catalogoSistema.FindAll(x => x.CodigoReferencia == "EMISION");
                    bool validaEmisionFactura = false;
                    if (emisionesFacturacion != null)
                        validaEmisionFactura = true;
                    #endregion

                    //#region Configuracion de Leyenda
                    //string leyenda = "";
                    //if (!ConfigurationManager.AppSettings.Get("Agente_de_Retención").Equals(""))
                    //{
                    //    string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                    //    foreach (string Ruc in Cadena)
                    //    {
                    //        if (!Ruc.Equals(""))
                    //        {
                    //            if (Convert.ToInt64(objRideCompRetencion._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                    //            {
                    //                bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                    //                if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                    //                {
                    //                    if (validaLeyenda)
                    //                        leyenda = CatalogoViaDoc.LeyendaAgente.Trim();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    leyenda = "";
                    //}
                    //#endregion


                    LocalReport localReport = new LocalReport();

                    //ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Se cae antes");
                    localReport.ReportPath = CatalogoViaDoc.rutaRidePlantilla + "RideCompRetencion.rdlc";
                    //ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Se cae despues");
                    ReportParameter[] parameters = new ReportParameter[8];
                    parameters[0] = new ReportParameter("txFechaAutorizacion", objRideCompRetencion.FechaHoraAutorizacion);
                    parameters[1] = new ReportParameter("txNumeroAutorizacion", objRideCompRetencion._infoTributaria.ClaveAcceso);
                    parameters[2] = new ReportParameter("pathImagenCodBarra", pathCodBarra + ".jpg");
                    parameters[3] = new ReportParameter("pathLogoCompania", @rutaImagen);
                    parameters[4] = new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial);
                    parameters[5] = new ReportParameter("etiquetaTotalRetenido", etiquetaTotalRetenido);

                    #region Datos Tributarios
                    DataRow drInfoTributaria = tblInfoTributaria.NewRow();
                    if (validaAmbiente)
                    {
                        CatalogoReporte tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == objRideCompRetencion._infoTributaria.Ambiente);
                        drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                    }
                    else
                        drInfoTributaria["ambiente"] = objRideCompRetencion._infoTributaria.Ambiente;
                    drInfoTributaria["claveAcceso"] = objRideCompRetencion._infoTributaria.ClaveAcceso;
                    drInfoTributaria["codDoc"] = objRideCompRetencion._infoTributaria.CodigoDocumento;
                    drInfoTributaria["dirMartiz"] = objRideCompRetencion._infoTributaria.DirMatriz;
                    try { drInfoTributaria["agenteRetencion"] = objRideCompRetencion._infoTributaria.AgenteRetencion; } catch { drInfoTributaria["agenteRetencion"] = ""; }
                    try { drInfoTributaria["regimenMicroempresas"] = objRideCompRetencion._infoTributaria.regimenMicroempresas; } catch { drInfoTributaria["regimenMicroempresas"] = ""; }
                    try { drInfoTributaria["contribuyenteRimpe"] = objRideCompRetencion._infoTributaria.contribuyenteRimpe; } catch { drInfoTributaria["contribuyenteRimpe"] = ""; }
                    //try { drInfoTributaria["regimenGeneral"] = objRideCompRetencion._infoTributaria.regimenGeneral; } catch { drInfoTributaria["regimenGeneral"] = ""; }
                    drInfoTributaria["estab"] = objRideCompRetencion._infoTributaria.Establecimiento;
                    drInfoTributaria["nombreComercial"] = objRideCompRetencion._infoTributaria.NombreComercial;
                    drInfoTributaria["ptoEmi"] = objRideCompRetencion._infoTributaria.PuntoEmision;
                    drInfoTributaria["razonSocial"] = objRideCompRetencion._infoTributaria.RazonSocial;
                    drInfoTributaria["ruc"] = objRideCompRetencion._infoTributaria.Ruc;
                    drInfoTributaria["secuencial"] = objRideCompRetencion._infoTributaria.Secuencial;

                    if (validaEmisionFactura)
                    {
                        CatalogoReporte tipoemision = emisionesFacturacion.Find(x => x.Codigo == objRideCompRetencion._infoTributaria.TipoEmision);
                        drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                    }
                    else
                        drInfoTributaria["tipoEmision"] = objRideCompRetencion._infoTributaria.TipoEmision;
                    tblInfoTributaria.Rows.Add(drInfoTributaria);
                    #endregion
                    #region Datos de la Retencion
                    DataRow drInfoFactura = tblInfoCompRetencion.NewRow();
                    drInfoFactura["dirEstablecimiento"] = objRideCompRetencion._infoCompRetencion.DirEstablecimiento;
                    drInfoFactura["fechaEmision"] = objRideCompRetencion._infoCompRetencion.FechaEmision;
                    if (validaNumeroContribuyenteEspecial)
                        drInfoFactura["contribuyenteEspecial"] = objRideCompRetencion._infoCompRetencion.NumeroContribuyenteEspecial;
                    else
                        drInfoFactura["contribuyenteEspecial"] = "";
                    drInfoFactura["identificacionSujetoRetenido"] = objRideCompRetencion._infoCompRetencion.Identificacion;
                    drInfoFactura["periodoFiscal"] = objRideCompRetencion._infoCompRetencion.PeriodoFiscal;
                    drInfoFactura["razonSocialSujetoRetenido"] = objRideCompRetencion._infoCompRetencion.RazonSocial;
                    drInfoFactura["obligadoContabilidad"] = objRideCompRetencion._infoCompRetencion.ObligadoContabilidad;
                    drInfoFactura["tipoIdentificacionSujetoRetenido"] = objRideCompRetencion._infoCompRetencion.TipoIdentificacion;
                    tblInfoCompRetencion.Rows.Add(drInfoFactura);
                    #endregion
                    #region Informacion Adicional
                    DataRow drInfoAdicional;
                    foreach (InformacionAdicional item in objRideCompRetencion._infosAdicional)
                    {
                        drInfoAdicional = tblInformacionAdicional.NewRow();
                        drInfoAdicional["nombre"] = item.Nombre;
                        drInfoAdicional["valor"] = item.Valor;
                        tblInformacionAdicional.Rows.Add(drInfoAdicional);
                    }
                    #endregion

                    List<CatalogoReporte> documentosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                    List<CatalogoReporte> impuestosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "IMPRETENC");
                    DataRow drImpuesto;
                    decimal totalValorRetenido = 0;
                    foreach (ImpuestoRetencion item in objRideCompRetencion._impuestos)
                    {
                        drImpuesto = tblImpuesto.NewRow();
                        try
                        {
                            CatalogoReporte documentoSustento = documentosRetencion.Find(x => x.Codigo == item.CodDocSustento);
                            CatalogoReporte impuestoRetencion = impuestosRetencion.Find(x => x.Codigo == item.Codigo);
                            drImpuesto["codDocSustento"] = documentoSustento.Valor;
                            drImpuesto["codigo"] = impuestoRetencion.Valor;
                        }
                        catch (Exception ex)
                        {
                            drImpuesto["codDocSustento"] = item.CodDocSustento;
                            drImpuesto["codigo"] = item.Codigo;
                        }
                        decimal baseImponible = Convert.ToDecimal(item.BaseImponible.Replace('.', ','));
                        if (validaSeparadorMiles)
                            drImpuesto["baseImponible"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                        else
                            drImpuesto["baseImponible"] = item.BaseImponible;
                        drImpuesto["codigoRetencion"] = item.CodigoRetencion;
                        drImpuesto["fechaEmisionDocSustento"] = item.FechaEmisionDocSustento;
                        drImpuesto["numDocSustento"] = item.NumDocSustento;
                        drImpuesto["porcentajeRetener"] = item.PorcentajeRetener;
                        decimal valorRetenido = Convert.ToDecimal(item.ValorRetenido.Replace('.', ','));
                        if (validaSeparadorMiles)
                            drImpuesto["valorRetenido"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valorRetenido);
                        else
                            drImpuesto["valorRetenido"] = item.ValorRetenido;

                        totalValorRetenido = totalValorRetenido + decimal.Parse(item.ValorRetenido, culturaNorteAmerica);
                        tblImpuesto.Rows.Add(drImpuesto);
                    }
                    string txtTotalValorRetenido = "0.00";
                    if (validaSeparadorMiles)
                        txtTotalValorRetenido = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalValorRetenido);
                    else
                        txtTotalValorRetenido = decimal.Round(totalValorRetenido, 2).ToString();
                    parameters[6] = new ReportParameter("valorTotalRetenido", txtTotalValorRetenido);
                    parameters[7] = new ReportParameter("ejercicioFiscal", objRideCompRetencion._infoCompRetencion.PeriodoFiscal);

                    ReportDataSource datos_tblInfoTributaria = new ReportDataSource("tblInformacionTributaria", tblInfoTributaria);
                    ReportDataSource datos_tblInfoCompRetencion = new ReportDataSource("tblInformacionCompRetencion", tblInfoCompRetencion);
                    ReportDataSource datos_tblImpuestos = new ReportDataSource("tblImpuesto", tblImpuesto);
                    ReportDataSource datos_tblInformacionAdicional = new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional);

                    localReport.EnableExternalImages = true;
                    localReport.SetParameters(parameters);
                    localReport.DataSources.Add(datos_tblInfoTributaria);
                    localReport.DataSources.Add(datos_tblInfoCompRetencion);
                    localReport.DataSources.Add(datos_tblImpuestos);
                    localReport.DataSources.Add(datos_tblInformacionAdicional);


                    localReport.Refresh();
                    string reportType = "PDF";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    Warning[] warnings;
                    string[] streams;
                    byte[] renderedBytes;

                    renderedBytes = localReport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    bytePDF = renderedBytes;
                    localReport.ReleaseSandboxAppDomain();
                    localReport.Dispose();

                }
                else
                {
                    mensajeError = "Error al generar del byte[] para elo ride de la Retencion. Mensaje Error: La compania " + objRideCompRetencion._infoTributaria.RazonSocial + " no tiene registrada las configuraciones del reporte";
                    bytePDF = null;
                }
            }
            catch (Exception ex)
            {
                mensajeError = "Error al generar del byte[] para elo ride de la Retencion. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
                bytePDF = null;
                //LogReporte.GuardaLog(mensajeError);
            }
            return bytePDF;
        }
    }
}
