using GenCode128;
using Microsoft.Reporting.WinForms;
using ReportesViaDoc.EntidadesReporte;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using ViaDoc.Configuraciones;
using ViaDoc.Utilitarios;

namespace ReportesViaDoc.LogicaReporte
{
    public class ProcesarRideNotaDebito
    {


        private string pathCodBarra { get; set; }
        private string pathLogoEmpresa { get; set; }
        private String pathRider { get; set; }

        /// <summary>
        /// Autor: William Jacome Choez (Viamatica)
        /// Descripcion: Descompone el xml del documento y genera el objeto ride de Nota de Debito
        /// </summary>
        /// <param name="mensajeError">Mensaje de error generado en el proceso</param>
        /// <param name="xmlDocumentoAutorizado">xml del documento</param>
        /// <param name="fechaHoraAutorizacion">fecha de autorizacion del documento</param>
        /// <param name="numeroAutorizacion">numero de autorizacion del documento</param>
        /// <returns></returns>
        public RideNotaDebito ProcesarXmlAutorizadoNotaCredito(ref string mensajeError, string xmlDocumentoAutorizado, string fechaHoraAutorizacion, string numeroAutorizacion)
        {
            RideNotaDebito objRideNotaDebito = new RideNotaDebito();
            StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            try
            {
                ExcepcionesReportes reporte = new ExcepcionesReportes();

                XmlDocument document = new XmlDocument();
                xmlDocumentoAutorizado = Utilitarios.ReemplazarCaracteresEspeciales(xmlDocumentoAutorizado);
                document.LoadXml(xmlDocumentoAutorizado);
                string XMLString = document.InnerXml;


                XmlNodeList CamposAutorizacion = document.SelectNodes("autorizacion");
                XmlNode Informacion = CamposAutorizacion.Item(0);
                if (Informacion != null)
                {
                    objRideNotaDebito.BanderaGeneracionObjeto = true;
                    objRideNotaDebito.NumeroAutorizacion = numeroAutorizacion;
                    objRideNotaDebito.FechaHoraAutorizacion = fechaHoraAutorizacion;
                    string NumeroAutorizacion = Informacion.SelectSingleNode("numeroAutorizacion").InnerText;
                    string FechaAutorizacion = Informacion.SelectSingleNode("fechaAutorizacion").InnerText;
                    string xmlComprobante = Informacion.SelectSingleNode("comprobante").InnerText.Replace("&", "&amp;");
                    document.LoadXml(xmlComprobante);
                    document.WriteTo(xtr);


                    XmlNodeList camposXML_infoTributaria = document.SelectNodes("notaDebito/infoTributaria");
                    XmlNodeList camposXML_infoNotaDebito = document.SelectNodes("notaDebito/infoNotaDebito");
                    XmlNodeList camposXML_motivos = document.SelectNodes("notaDebito/motivos");

                    if (camposXML_infoTributaria.Count > 0 && camposXML_infoNotaDebito.Count > 0 && camposXML_motivos.Count > 0)
                    {
                        XmlNodeList camposXML_infoAdicional = document.SelectNodes("notaDebito/infoAdicional");
                        if (camposXML_infoAdicional.Count > 0)
                        {
                            #region Informacion Adicional de la nota de debito
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
                            objRideNotaDebito._infoAdicional = listaInfoAdicional;

                            #endregion
                        }

                        #region Motivo de la Nota Debito
                        List<Motivo> motivosNotaDebito = new List<Motivo>();


                        foreach (XmlNode tagmotivos in camposXML_motivos)
                        {
                            XmlNodeList nodos = tagmotivos.ChildNodes;
                            foreach (XmlNode nodo in nodos)
                            {
                                Motivo motivo = new Motivo();
                                motivo.Razon = nodo.SelectSingleNode("razon").InnerText;
                                motivo.Valor = nodo.SelectSingleNode("valor").InnerText;
                                motivosNotaDebito.Add(motivo);
                            }
                        }
                        objRideNotaDebito._motivos = motivosNotaDebito;


                        #endregion

                        #region Informacion tributaria de la Nota de debito
                        XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                        objRideNotaDebito._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                        objRideNotaDebito._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                        objRideNotaDebito._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText;
                        objRideNotaDebito._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                        objRideNotaDebito._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                        objRideNotaDebito._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                        objRideNotaDebito._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                        objRideNotaDebito._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                        objRideNotaDebito._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                        objRideNotaDebito._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                        try
                        {
                            objRideNotaDebito._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
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
                                                if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                            objRideNotaDebito._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideNotaDebito._infoTributaria.contribuyenteRimpe = "";
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
                                                if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                            objRideNotaDebito._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideNotaDebito._infoTributaria.contribuyenteRimpe = "";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                objRideNotaDebito._infoTributaria.contribuyenteRimpe = "";
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        objRideNotaDebito._infoTributaria.contribuyenteRimpe = "";
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
                                            if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideNotaDebito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                                }
                                                else
                                                {
                                                    objRideNotaDebito._infoTributaria.AgenteRetencion = " ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objRideNotaDebito._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
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
                                        if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideNotaDebito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objRideNotaDebito._infoTributaria.AgenteRetencion = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        objRideNotaDebito._infoTributaria.AgenteRetencion = "";
                                    }
                                }
                            }
                        }
                        try
                        {
                            if (informacionXML.SelectSingleNode("regimenMicroempresas").InnerText.Equals(""))
                            {
                                if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                                {
                                    string[] Cadena = ConfigurationManager.AppSettings.Get("Regimen_Microempresas").Split('|');
                                    foreach (string Ruc in Cadena)
                                    {
                                        if (!Ruc.Equals(""))
                                        {
                                            if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideNotaDebito._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaAgente.Trim();
                                                }
                                                else
                                                {
                                                    objRideNotaDebito._infoTributaria.regimenMicroempresas = " ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objRideNotaDebito._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
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
                                        if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideNotaDebito._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                            }
                                            else
                                            {
                                                objRideNotaDebito._infoTributaria.regimenMicroempresas = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        objRideNotaDebito._infoTributaria.regimenMicroempresas = "";
                                    }
                                }
                            }
                        }
                        ////Leyenda Regimen General
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
                        //                    if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                        //                    {
                        //                        bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                        //                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                        //                        {
                        //                            if (validaLeyenda)
                        //                                objRideNotaDebito._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                        //                        }
                        //                        else
                        //                        {
                        //                            objRideNotaDebito._infoTributaria.regimenGeneral = " ";
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        objRideNotaDebito._infoTributaria.regimenGeneral = string.Empty;
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
                        //                if (Convert.ToInt64(objRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                        //                {
                        //                    bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                        //                    if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                        //                    {
                        //                        if (validaLeyenda)
                        //                            objRideNotaDebito._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                        //                    }
                        //                    else
                        //                    {
                        //                        objRideNotaDebito._infoTributaria.regimenGeneral = "";
                        //                    }

                        //                }
                        //            }
                        //            else
                        //            {
                        //                objRideNotaDebito._infoTributaria.regimenGeneral = "";
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        objRideNotaDebito._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                        #endregion

                        #region Informacion de la Nota de Debito
                        XmlNode tagInfoNotaDebito = camposXML_infoNotaDebito.Item(0);
                        objRideNotaDebito._infoNotaDebito.FechaEmision = tagInfoNotaDebito.SelectSingleNode("fechaEmision").InnerText;
                        objRideNotaDebito._infoNotaDebito.DirEstablecimiento = tagInfoNotaDebito.SelectSingleNode("dirEstablecimiento").InnerText;
                        objRideNotaDebito._infoNotaDebito.TipoIdentificacion = tagInfoNotaDebito.SelectSingleNode("tipoIdentificacionComprador").InnerText;
                        objRideNotaDebito._infoNotaDebito.RazonSocial = tagInfoNotaDebito.SelectSingleNode("razonSocialComprador").InnerText;
                        objRideNotaDebito._infoNotaDebito.Identificacion = tagInfoNotaDebito.SelectSingleNode("identificacionComprador").InnerText;
                        try { objRideNotaDebito._infoNotaDebito.NumeroContribuyenteEspecial = tagInfoNotaDebito.SelectSingleNode("contribuyenteEspecial").InnerText; } catch (Exception ex) { objRideNotaDebito._infoNotaDebito.NumeroContribuyenteEspecial = ""; }
                        try { objRideNotaDebito._infoNotaDebito.ObligadoContabilidad = tagInfoNotaDebito.SelectSingleNode("obligadoContabilidad").InnerText; } catch { objRideNotaDebito._infoNotaDebito.ObligadoContabilidad = ""; }
                        objRideNotaDebito._infoNotaDebito.CodDocModificado = tagInfoNotaDebito.SelectSingleNode("codDocModificado").InnerText;
                        objRideNotaDebito._infoNotaDebito.NumDocModificado = tagInfoNotaDebito.SelectSingleNode("numDocModificado").InnerText;
                        objRideNotaDebito._infoNotaDebito.FechaEmisionDocSustento = tagInfoNotaDebito.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                        objRideNotaDebito._infoNotaDebito.TotalSinImpuestos = tagInfoNotaDebito.SelectSingleNode("totalSinImpuestos").InnerText;
                        objRideNotaDebito._infoNotaDebito.ValorTotal = tagInfoNotaDebito.SelectSingleNode("valorTotal").InnerText;
                        string xmlImpuesto = tagInfoNotaDebito.SelectSingleNode("impuestos").InnerXml;
                        #region Impuestos de la Nota de Debito
                        document.LoadXml(xmlImpuesto);
                        document.WriteTo(xtr);
                        XmlNodeList camposXML_Impuesto = document.SelectNodes("impuesto");
                        List<Impuesto> impuestosNotaDebito = new List<Impuesto>();
                        foreach (XmlNode nodoImpuesto in camposXML_Impuesto)
                        {
                            Impuesto impuesto = new Impuesto();
                            impuesto.Codigo = nodoImpuesto.SelectSingleNode("codigo").InnerText;
                            impuesto.CodigoPorcentaje = nodoImpuesto.SelectSingleNode("codigoPorcentaje").InnerText;
                            impuesto.Tarifa = nodoImpuesto.SelectSingleNode("tarifa").InnerText;
                            impuesto.BaseImponible = nodoImpuesto.SelectSingleNode("baseImponible").InnerText;
                            impuesto.Valor = nodoImpuesto.SelectSingleNode("valor").InnerText;
                            impuestosNotaDebito.Add(impuesto);
                        }
                        objRideNotaDebito._infoNotaDebito._impuestos = impuestosNotaDebito;
                        #endregion
                        #endregion
                    }
                }
                else
                    objRideNotaDebito = ProcesarXmlFirmaNotaDebito(ref mensajeError, xmlDocumentoAutorizado);
            }
            catch (Exception ex)
            {
                objRideNotaDebito.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml autorizado de la Nota Debito. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
            }
            return objRideNotaDebito;
        }

        /// <summary>
        /// Autor: William Jacome Choez (Viamatica)
        /// Descripcion: Descompone el xml del documento y genera el objeto ride de Nota de Debito
        /// </summary>
        /// <param name="mensajeError">Mensaje de error generado en el proceso</param>
        /// <param name="xmlDocumentoFirmado">xml del documento</param>

        public RideNotaDebito ProcesarXmlFirmaNotaDebito(ref string mensajeError, string xmlDocumentoFirmado)
        {
            RideNotaDebito ObjRideNotaDebito = new RideNotaDebito();
            StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            try
            {
                //ExcepcionesReportes reporte = new ExcepcionesReportes();
                XmlDocument document = new XmlDocument();
                xmlDocumentoFirmado = Utilitarios.ReemplazarCaracteresEspeciales(xmlDocumentoFirmado);
                document.LoadXml(xmlDocumentoFirmado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml;

                ObjRideNotaDebito.BanderaGeneracionObjeto = true;

                XmlNodeList camposXML_infoTributaria = document.SelectNodes("notaDebito/infoTributaria");
                XmlNodeList camposXML_infoNotaDebito = document.SelectNodes("notaDebito/infoNotaDebito");
                XmlNodeList camposXML_motivos = document.SelectNodes("notaDebito/motivos");

                if (camposXML_infoTributaria.Count > 0 && camposXML_infoNotaDebito.Count > 0 && camposXML_motivos.Count > 0)
                {
                    XmlNodeList camposXML_infoAdicional = document.SelectNodes("notaDebito/infoAdicional");
                    if (camposXML_infoAdicional.Count > 0)
                    {
                        #region Informacion Adicional de la nota de debito
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
                        ObjRideNotaDebito._infoAdicional = listaInfoAdicional;

                        #endregion
                    }

                    #region Motivo de la Nota Debito
                    List<Motivo> motivosNotaDebito = new List<Motivo>();


                    foreach (XmlNode tagmotivos in camposXML_motivos)
                    {
                        XmlNodeList nodos = tagmotivos.ChildNodes;
                        foreach (XmlNode nodo in nodos)
                        {
                            Motivo motivo = new Motivo();
                            motivo.Razon = nodo.SelectSingleNode("razon").InnerText;
                            motivo.Valor = nodo.SelectSingleNode("valor").InnerText;
                            motivosNotaDebito.Add(motivo);
                        }
                    }
                    ObjRideNotaDebito._motivos = motivosNotaDebito;


                    #endregion

                    #region Informacion tributaria de la Nota de debito
                    XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                    ObjRideNotaDebito._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                    ObjRideNotaDebito._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                    ObjRideNotaDebito._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText;
                    ObjRideNotaDebito._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                    ObjRideNotaDebito._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                    ObjRideNotaDebito._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                    ObjRideNotaDebito._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                    ObjRideNotaDebito._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                    ObjRideNotaDebito._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                    ObjRideNotaDebito._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;
                    try
                    {
                        ObjRideNotaDebito._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
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
                                            if (Convert.ToInt64(ObjRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        ObjRideNotaDebito._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                                }
                                                else
                                                {
                                                    ObjRideNotaDebito._infoTributaria.regimenMicroempresas = "";
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
                                            if (Convert.ToInt64(ObjRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        ObjRideNotaDebito._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                                }
                                                else
                                                {
                                                    ObjRideNotaDebito._infoTributaria.regimenMicroempresas = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ObjRideNotaDebito._infoTributaria.regimenMicroempresas = "";
                                        }
                                    }
                                }
                                catch
                                {
                                    ObjRideNotaDebito._infoTributaria.regimenMicroempresas = "";
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
                                        if (Convert.ToInt64(ObjRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    ObjRideNotaDebito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                ObjRideNotaDebito._infoTributaria.AgenteRetencion = " ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ObjRideNotaDebito._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
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
                                    if (Convert.ToInt64(ObjRideNotaDebito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                ObjRideNotaDebito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                        }
                                        else
                                        {
                                            ObjRideNotaDebito._infoTributaria.AgenteRetencion = "";
                                        }

                                    }
                                }
                                else
                                {
                                    ObjRideNotaDebito._infoTributaria.AgenteRetencion = "";
                                }
                            }
                        }
                    }
                    //try
                    //{
                    //    ObjRideNotaDebito._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
                    //}
                    //catch (Exception)
                    //{
                    //    ObjRideNotaDebito._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen;
                    //}

                    //try
                    //{
                    //    ObjRideNotaDebito._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                    //}
                    //catch (Exception)
                    //{
                    //    ObjRideNotaDebito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente;
                    //}

                    ObjRideNotaDebito._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                    #endregion

                    #region Informacion de la Nota de Debito
                    XmlNode tagInfoNotaDebito = camposXML_infoNotaDebito.Item(0);
                    ObjRideNotaDebito._infoNotaDebito.FechaEmision = tagInfoNotaDebito.SelectSingleNode("fechaEmision").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.DirEstablecimiento = tagInfoNotaDebito.SelectSingleNode("dirEstablecimiento").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.TipoIdentificacion = tagInfoNotaDebito.SelectSingleNode("tipoIdentificacionComprador").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.RazonSocial = tagInfoNotaDebito.SelectSingleNode("razonSocialComprador").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.Identificacion = tagInfoNotaDebito.SelectSingleNode("identificacionComprador").InnerText;
                    try { ObjRideNotaDebito._infoNotaDebito.NumeroContribuyenteEspecial = tagInfoNotaDebito.SelectSingleNode("contribuyenteEspecial").InnerText; } catch (Exception ex) { ObjRideNotaDebito._infoNotaDebito.NumeroContribuyenteEspecial = ""; }
                    try { ObjRideNotaDebito._infoNotaDebito.ObligadoContabilidad = tagInfoNotaDebito.SelectSingleNode("obligadoContabilidad").InnerText; } catch { ObjRideNotaDebito._infoNotaDebito.ObligadoContabilidad = ""; }
                    ObjRideNotaDebito._infoNotaDebito.CodDocModificado = tagInfoNotaDebito.SelectSingleNode("codDocModificado").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.NumDocModificado = tagInfoNotaDebito.SelectSingleNode("numDocModificado").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.FechaEmisionDocSustento = tagInfoNotaDebito.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.TotalSinImpuestos = tagInfoNotaDebito.SelectSingleNode("totalSinImpuestos").InnerText;
                    ObjRideNotaDebito._infoNotaDebito.ValorTotal = tagInfoNotaDebito.SelectSingleNode("valorTotal").InnerText;
                    string xmlImpuesto = tagInfoNotaDebito.SelectSingleNode("impuestos").InnerXml;
                    #region Impuestos de la Nota de Debito
                    document.LoadXml(xmlImpuesto);
                    document.WriteTo(xtr);
                    XmlNodeList camposXML_Impuesto = document.SelectNodes("impuesto");
                    List<Impuesto> impuestosNotaDebito = new List<Impuesto>();
                    foreach (XmlNode nodoImpuesto in camposXML_Impuesto)
                    {
                        Impuesto impuesto = new Impuesto();
                        impuesto.Codigo = nodoImpuesto.SelectSingleNode("codigo").InnerText;
                        impuesto.CodigoPorcentaje = nodoImpuesto.SelectSingleNode("codigoPorcentaje").InnerText;
                        impuesto.Tarifa = nodoImpuesto.SelectSingleNode("tarifa").InnerText;
                        impuesto.BaseImponible = nodoImpuesto.SelectSingleNode("baseImponible").InnerText;
                        impuesto.Valor = nodoImpuesto.SelectSingleNode("valor").InnerText;
                        impuestosNotaDebito.Add(impuesto);
                    }
                    ObjRideNotaDebito._infoNotaDebito._impuestos = impuestosNotaDebito;
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ObjRideNotaDebito.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml frima de la Nota Debito. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
            }

            return ObjRideNotaDebito;
        }

        /// <summary>
        /// Autor: William Jacome Choez (Viamatica S.A.)
        /// Descripcion: Genera el byte[] del documento 
        /// </summary>
        /// <param name="mensajeError">Mensaje de Error del proceso</param>
        /// <param name="objRideNotaDebito">Objeto que representa el xml del documento</param>
        /// <param name="configuraciones">Configuraciones del reporte segun la compañia</param>
        /// <param name="catalogoSistema">Catalogos del sistema de Facturacion Electronica</param>
        /// <returns>Arreglo de byte que representa el ride del documento</returns>
        public byte[] GenerarRideNotaDebito(ref string mensajeError, RideNotaDebito objRideNotaDebito, List<ConfiguracionReporte> configuraciones, List<CatalogoReporte> catalogoSistema)
        {
            byte[] bytePDF = null;
            string etiquetaContribuyenteEspecial = "";
            string etiquetaTipoIvaGrabadoConPorcentaje = "";
            string etiquetaTipoIvaGrabadoSiPorcentaje = "";
            try
            {
                List<ConfiguracionReporte> confCompania = configuraciones.FindAll(x => x.RucCompania == objRideNotaDebito._infoTributaria.Ruc);
                if (confCompania != null)
                {
                    #region Origen Datos del RIDE Nota de Debito
                    DataTable tblInfoTributaria = new DataTable();
                    DataTable tblInfoNotaDebito = new DataTable();
                    DataTable tblInformacionMonetaria = new DataTable();
                    DataTable tblInformacionAdicional = new DataTable();
                    DataTable tblMotivos = new DataTable();

                    tblInfoTributaria.TableName = "tblInformacionTributaria";
                    tblInfoNotaDebito.TableName = "tblInformacionNotaDebito";
                    tblInformacionMonetaria.TableName = "tblInformacionMonetaria";
                    tblInformacionAdicional.TableName = "tblInformacionAdicional";
                    tblMotivos.TableName = "tblMotivos";

                    DataColumn[] cols_tblInfoTributaria = new DataColumn[]
                    {
                        new DataColumn("ambiente",typeof(string)),
                        new DataColumn("tipoEmision",typeof(string)),
                        new DataColumn("razonSocial",typeof(string)),
                        new DataColumn("nombreComercial",typeof(string)),
                        new DataColumn("ruc",typeof(string)),
                        new DataColumn("claveAcceso",typeof(string)),
                        new DataColumn("codDoc",typeof(string)),
                        new DataColumn("estab",typeof(string)),
                        new DataColumn("ptoEmi",typeof(string)),
                        new DataColumn("secuencial",typeof(string)),
                        new DataColumn("dirMatriz",typeof(string)),
                        new DataColumn("regimenMicroempresas",typeof(string)),
                        new DataColumn("agenteRetencion",typeof(string))
                    };

                    DataColumn[] cols_tblInfoNotaDebito = new DataColumn[]
                    {
                        new DataColumn("fechaEmision",typeof(string)),
                        new DataColumn("dirEstablecimiento",typeof(string)),
                        new DataColumn("tipoIdentificacionComprador",typeof(string)),
                        new DataColumn("razonSocialComprador",typeof(string)),
                        new DataColumn("identificacionComprador",typeof(string)),
                        new DataColumn("contribuyenteEspecial",typeof(string)),
                        new DataColumn("obligadoContabilidad",typeof(string)),
                        new DataColumn("codDocModificado",typeof(string)),
                        new DataColumn("numDocModificado",typeof(string)),
                        new DataColumn("fechaEmisionDocSustento",typeof(string)),
                        new DataColumn("totalSinImpuestos",typeof(string)),
                        new DataColumn("valorTotal",typeof(string))
                    };

                    DataColumn[] cols_tblMotivos = new DataColumn[] {
                        new DataColumn("razon",typeof(string)),
                        new DataColumn("valor",typeof(string))
                    };

                    DataColumn[] cols_tblInformacionAdicional = new DataColumn[] {
                        new DataColumn("nombre",typeof(string)),
                        new DataColumn("valor",typeof(string))
                    };


                    DataColumn[] cols_tblInformacionMonetaria = new DataColumn[] {
                        new DataColumn("subTotalIva",typeof(string)),
                        new DataColumn("subTotalIva5",typeof(string)),
                        new DataColumn("subTotalCero",typeof(string)),
                        new DataColumn("subTotalNoObjetoIva",typeof(string)),
                        new DataColumn("subTotalExcentoIva",typeof(string)),
                        new DataColumn("subTotalSinImpuesto",typeof(string)),
                        new DataColumn("ice",typeof(string)),
                        new DataColumn("iva",typeof(string)),
                        new DataColumn("iva5",typeof(string)),
                        new DataColumn("irbpnr",typeof(string)),
                        new DataColumn("valorTotal",typeof(string))
                    };


                    tblInfoTributaria.Columns.AddRange(cols_tblInfoTributaria);
                    tblInfoNotaDebito.Columns.AddRange(cols_tblInfoNotaDebito);
                    tblInformacionMonetaria.Columns.AddRange(cols_tblInformacionMonetaria);
                    tblInformacionAdicional.Columns.AddRange(cols_tblInformacionAdicional);
                    tblMotivos.Columns.AddRange(cols_tblMotivos);
                    #endregion

                    #region Logo de la Compania
                    pathLogoEmpresa = CatalogoViaDoc.rutaLogoCompania;
                    string rutaImagen = pathLogoEmpresa + objRideNotaDebito._infoTributaria.Ruc.Trim() + ".png";
                    #endregion

                    #region Genera Codigo Barra de la Factura
                    byte[] imgBar;
                    Code.BarcodeGenerator bgCode128 = new Code.BarcodeGenerator();
                    Code.Convertir cCode = new Code.Convertir();
                    Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                    StringFormat fi = new StringFormat(StringFormatFlags.NoClip);
                    fi.Alignment = StringAlignment.Center;
                    imgBar = RetornarXml.ImageToByte2(Code128Rendering.MakeBarcodeImage(objRideNotaDebito._infoTributaria.ClaveAcceso, 3, true));

                    pathCodBarra = CatalogoViaDoc.rutaCodigoBarra + objRideNotaDebito._infoTributaria.ClaveAcceso.Trim();
                    pathRider = CatalogoViaDoc.rutaRide + objRideNotaDebito._infoTributaria.Ruc.Trim() + "\\" + objRideNotaDebito._infoTributaria.CodigoDocumento.Trim();
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
                    #region Catalogo Ambiente Facturacion
                    List<CatalogoReporte> ambientesFacturacion = catalogoSistema.FindAll(x => x.CodigoReferencia == "AMBIENTE");
                    bool validaAmbiente = false;
                    if (ambientesFacturacion != null)
                        validaAmbiente = true;
                    #endregion
                    #region Catalogo Emisiones Facturacion
                    List<CatalogoReporte> emisionesFacturacion = catalogoSistema.FindAll(x => x.CodigoReferencia == "EMISION");
                    bool validaEmisionFactura = false;
                    if (emisionesFacturacion != null)
                        validaEmisionFactura = true;
                    #endregion

                    #region Validacion de la Etiqueta del tipo de Iva Grabado
                    CatalogoReporte tipoIvaGrabado = catalogoSistema.Find(x => x.CodigoReferencia == "IVAGRAB");//Catalogo Tipo de Iva para las nuevas empresas que inician con facturacion electronica
                    bool validaTipoIvaNuevasEmpresas = false;
                    if (tipoIvaGrabado != null)
                    {
                        validaTipoIvaNuevasEmpresas = Convert.ToBoolean(tipoIvaGrabado.Valor.Trim());
                        if (validaTipoIvaNuevasEmpresas)
                        {
                            etiquetaTipoIvaGrabadoSiPorcentaje = tipoIvaGrabado.Codigo;
                            etiquetaTipoIvaGrabadoConPorcentaje = "IVA " + tipoIvaGrabado.Codigo + "%";
                        }
                        else
                        {
                            CatalogoReporte fechaSalvaguardiaIva14 = catalogoSistema.Find(x => x.CodigoReferencia == "SALVAGUARD");
                            if (fechaSalvaguardiaIva14 != null)
                            {
                                try
                                {

                                    string[] fechaIni = fechaSalvaguardiaIva14.Codigo.Split('/');
                                    string[] fechaFin = fechaSalvaguardiaIva14.Valor.Split('/');
                                    DateTime fechaInicioSalvaguardia = new DateTime(Convert.ToInt32(fechaIni[2]), Convert.ToInt32(fechaIni[1]), Convert.ToInt32(fechaIni[0]));
                                    DateTime fechaFinSalvaguardia = new DateTime(Convert.ToInt32(fechaFin[2]), Convert.ToInt32(fechaFin[1]), Convert.ToInt32(fechaFin[0]));
                                    DateTime fechaEmision = Convert.ToDateTime(objRideNotaDebito._infoNotaDebito.FechaEmision);

                                    if (fechaEmision.Year <= fechaFinSalvaguardia.Year && fechaEmision.Year >= fechaInicioSalvaguardia.Year)//Valida que el año este dentro del rango de las salvanguardia
                                    {
                                        CatalogoReporte IVASalvaguardia = catalogoSistema.Find(x => x.CodigoReferencia == "IVASALVAGU");
                                        if (fechaEmision.Year == fechaFinSalvaguardia.Year)
                                        {
                                            if (fechaEmision.Month <= fechaFinSalvaguardia.Month)
                                            {
                                                etiquetaTipoIvaGrabadoSiPorcentaje = IVASalvaguardia.Codigo;
                                                etiquetaTipoIvaGrabadoConPorcentaje = IVASalvaguardia.Valor;

                                            }
                                            else
                                            {
                                                etiquetaTipoIvaGrabadoSiPorcentaje = tipoIvaGrabado.Codigo;
                                                etiquetaTipoIvaGrabadoConPorcentaje = "IVA " + tipoIvaGrabado.Codigo + "%";
                                            }
                                        }
                                        else
                                        {
                                            if (fechaEmision.Month >= fechaInicioSalvaguardia.Month)
                                            {
                                                etiquetaTipoIvaGrabadoSiPorcentaje = IVASalvaguardia.Codigo;
                                                etiquetaTipoIvaGrabadoConPorcentaje = IVASalvaguardia.Valor;
                                            }
                                            else
                                            {
                                                etiquetaTipoIvaGrabadoSiPorcentaje = tipoIvaGrabado.Codigo;
                                                etiquetaTipoIvaGrabadoConPorcentaje = "IVA " + tipoIvaGrabado.Codigo + "%";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        etiquetaTipoIvaGrabadoSiPorcentaje = tipoIvaGrabado.Codigo;
                                        etiquetaTipoIvaGrabadoConPorcentaje = "IVA " + tipoIvaGrabado.Codigo + "%";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    etiquetaTipoIvaGrabadoSiPorcentaje = "";
                                    etiquetaTipoIvaGrabadoConPorcentaje = "";
                                }
                            }
                        }
                    }
                    #endregion
                    etiquetaTipoIvaGrabadoSiPorcentaje = etiquetaTipoIvaGrabadoSiPorcentaje.Contains("%") ? etiquetaTipoIvaGrabadoSiPorcentaje : etiquetaTipoIvaGrabadoSiPorcentaje + " %";

                    LocalReport localReport = new LocalReport();
                    localReport.ReportPath = CatalogoViaDoc.rutaRidePlantilla + "RideNotaDebito.rdlc";
                    ReportParameter[] parameters = new ReportParameter[7];
                    parameters[0] = new ReportParameter("txFechaAutorizacion", objRideNotaDebito.FechaHoraAutorizacion);
                    parameters[1] = new ReportParameter("txNumeroAutorizacion", objRideNotaDebito._infoTributaria.ClaveAcceso);
                    parameters[2] = new ReportParameter("pathImagenCodBarra", pathCodBarra + ".jpg");
                    parameters[3] = new ReportParameter("pathLogoCompania", @rutaImagen);
                    parameters[4] = new ReportParameter("tarifaIva", etiquetaTipoIvaGrabadoSiPorcentaje);
                    parameters[5] = new ReportParameter("etiquetaTarifaIva", etiquetaTipoIvaGrabadoConPorcentaje);
                    parameters[6] = new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial);
                    
                    #region Datos Tributarios
                    DataRow drInfoTributaria = tblInfoTributaria.NewRow();
                    if (validaAmbiente)
                    {
                        CatalogoReporte tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == objRideNotaDebito._infoTributaria.Ambiente);
                        drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                    }
                    else
                        drInfoTributaria["ambiente"] = objRideNotaDebito._infoTributaria.Ambiente;
                    drInfoTributaria["claveAcceso"] = objRideNotaDebito._infoTributaria.ClaveAcceso;
                    drInfoTributaria["codDoc"] = objRideNotaDebito._infoTributaria.CodigoDocumento;
                    drInfoTributaria["dirMatriz"] = objRideNotaDebito._infoTributaria.DirMatriz;
                    try { drInfoTributaria["regimenMicroempresas"] = objRideNotaDebito._infoTributaria.regimenMicroempresas; } catch { drInfoTributaria["regimenMicroempresas"] = ""; }
                    try { drInfoTributaria["agenteRetencion"] = objRideNotaDebito._infoTributaria.AgenteRetencion; } catch { drInfoTributaria["agenteRetencion"] = ""; } 
                    drInfoTributaria["estab"] = objRideNotaDebito._infoTributaria.Establecimiento;
                    drInfoTributaria["nombreComercial"] = objRideNotaDebito._infoTributaria.NombreComercial;
                    drInfoTributaria["ptoEmi"] = objRideNotaDebito._infoTributaria.PuntoEmision;
                    drInfoTributaria["razonSocial"] = objRideNotaDebito._infoTributaria.RazonSocial;
                    drInfoTributaria["ruc"] = objRideNotaDebito._infoTributaria.Ruc;
                    drInfoTributaria["secuencial"] = objRideNotaDebito._infoTributaria.Secuencial;
                    if (validaEmisionFactura)
                    {
                        CatalogoReporte tipoemision = emisionesFacturacion.Find(x => x.Codigo == objRideNotaDebito._infoTributaria.TipoEmision);
                        drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                    }
                    else
                        drInfoTributaria["tipoEmision"] = objRideNotaDebito._infoTributaria.TipoEmision;
                    tblInfoTributaria.Rows.Add(drInfoTributaria);
                    #endregion
                    #region Datos de la Nota Debito
                    DataRow drInfoNotaDebito = tblInfoNotaDebito.NewRow();
                    drInfoNotaDebito["fechaEmision"] = objRideNotaDebito._infoNotaDebito.FechaEmision;
                    drInfoNotaDebito["dirEstablecimiento"] = objRideNotaDebito._infoNotaDebito.DirEstablecimiento;
                    drInfoNotaDebito["tipoIdentificacionComprador"] = objRideNotaDebito._infoNotaDebito.TipoIdentificacion;
                    drInfoNotaDebito["razonSocialComprador"] = objRideNotaDebito._infoNotaDebito.RazonSocial;
                    drInfoNotaDebito["identificacionComprador"] = objRideNotaDebito._infoNotaDebito.Identificacion;
                    if (validaNumeroContribuyenteEspecial)
                        drInfoNotaDebito["contribuyenteEspecial"] = objRideNotaDebito._infoNotaDebito.NumeroContribuyenteEspecial;
                    else
                        drInfoNotaDebito["contribuyenteEspecial"] = "";
                    drInfoNotaDebito["obligadoContabilidad"] = objRideNotaDebito._infoNotaDebito.ObligadoContabilidad;
                    List<CatalogoReporte> documentosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                    CatalogoReporte documentoSustento = documentosRetencion.Find(x => x.Codigo == objRideNotaDebito._infoNotaDebito.CodDocModificado);
                    drInfoNotaDebito["codDocModificado"] = documentoSustento.Valor;
                    drInfoNotaDebito["numDocModificado"] = objRideNotaDebito._infoNotaDebito.NumDocModificado;
                    drInfoNotaDebito["fechaEmisionDocSustento"] = objRideNotaDebito._infoNotaDebito.FechaEmisionDocSustento;
                    drInfoNotaDebito["totalSinImpuestos"] = objRideNotaDebito._infoNotaDebito.TotalSinImpuestos;
                    drInfoNotaDebito["valorTotal"] = objRideNotaDebito._infoNotaDebito.ValorTotal;
                    tblInfoNotaDebito.Rows.Add(drInfoNotaDebito);

                    #endregion
                    #region Motivo de la Nota Debito
                    DataRow drMotivo;
                    foreach (Motivo item in objRideNotaDebito._motivos)
                    {
                        drMotivo = tblMotivos.NewRow();
                        drMotivo["razon"] = item.Razon;
                        decimal valor = Convert.ToDecimal(item.Valor.Replace('.', ','));
                        if (validaSeparadorMiles)
                            drMotivo["valor"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                        else
                            drMotivo["valor"] = item.Valor;
                        tblMotivos.Rows.Add(drMotivo);
                    }
                    #endregion
                    #region Informacion Adicional
                    DataRow drInfoAdicional;
                    foreach (InformacionAdicional item in objRideNotaDebito._infoAdicional)
                    {
                        drInfoAdicional = tblInformacionAdicional.NewRow();
                        drInfoAdicional["nombre"] = item.Nombre;
                        drInfoAdicional["valor"] = item.Valor;
                        tblInformacionAdicional.Rows.Add(drInfoAdicional);
                    }
                    #endregion
                    #region Monto de la Nota debito
                    DataRow dr_tblInformacionMonetaria = tblInformacionMonetaria.NewRow();
                    foreach (Impuesto objfactTotalImpuesto in objRideNotaDebito._infoNotaDebito._impuestos)
                    {
                        if (String.Compare(objfactTotalImpuesto.Codigo.Trim(), CatalogoViaDoc.ParametrosValorIva) == 0)
                        {
                            #region IMPUESTOS_IVA
                            #region SubTotal Cero
                            if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorIVA0) == 0)
                            {
                                decimal subTotalCero = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                if (validaSeparadorMiles)
                                    dr_tblInformacionMonetaria["subTotalCero"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalCero);
                                else
                                    dr_tblInformacionMonetaria["subTotalCero"] = objfactTotalImpuesto.BaseImponible.ToString();
                            }
                            else
                                dr_tblInformacionMonetaria["subTotalCero"] = "0.00";
                            #endregion

                            #region SubTotal Iva 5
                            if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorIVA5) == 0)
                            {
                                decimal baseCinco = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                decimal valor = Convert.ToDecimal(objfactTotalImpuesto.Valor.Replace('.', ','));
                                if (validaSeparadorMiles)
                                {

                                    dr_tblInformacionMonetaria["subTotalIva5"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseCinco);
                                    dr_tblInformacionMonetaria["iva5"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                                }
                                else
                                {
                                    dr_tblInformacionMonetaria["subTotalIva5"] = objfactTotalImpuesto.BaseImponible;
                                    dr_tblInformacionMonetaria["iva5"] = objfactTotalImpuesto.Valor;
                                }
                            }
                            else
                            {
                                dr_tblInformacionMonetaria["subTotalIva5"] = "0.00";
                                dr_tblInformacionMonetaria["iva5"] = "0.00";
                            }
                            #endregion

                            #region SubTotal Iva 12
                            if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorIVA12) == 0)
                            {
                                decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                decimal valor = Convert.ToDecimal(objfactTotalImpuesto.Valor.Replace('.', ','));
                                if (validaSeparadorMiles)
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                                    dr_tblInformacionMonetaria["iva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                                }
                                else
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = objfactTotalImpuesto.BaseImponible;
                                    dr_tblInformacionMonetaria["iva"] = objfactTotalImpuesto.Valor;
                                }

                            }
                            else
                            {
                                #region SubTotal 14
                                if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorIVA14) == 0)
                                {
                                    decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                    decimal valor = Convert.ToDecimal(objfactTotalImpuesto.Valor.Replace('.', ','));
                                    if (validaSeparadorMiles)
                                    {
                                        dr_tblInformacionMonetaria["subTotalIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                                        dr_tblInformacionMonetaria["iva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                                    }
                                    else
                                    {
                                        dr_tblInformacionMonetaria["subTotalIva"] = objfactTotalImpuesto.BaseImponible;
                                        dr_tblInformacionMonetaria["iva"] = objfactTotalImpuesto.Valor;
                                    }
                                }
                                else if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorIVA15) == 0)
                                {
                                    decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                    decimal valor = Convert.ToDecimal(objfactTotalImpuesto.Valor.Replace('.', ','));
                                    if (validaSeparadorMiles)
                                    {
                                        dr_tblInformacionMonetaria["subTotalIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                                        dr_tblInformacionMonetaria["iva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                                    }
                                    else
                                    {
                                        dr_tblInformacionMonetaria["subTotalIva"] = objfactTotalImpuesto.BaseImponible;
                                        dr_tblInformacionMonetaria["iva"] = objfactTotalImpuesto.Valor;
                                    }
                                }
                                else
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = "0.00";
                                    dr_tblInformacionMonetaria["iva"] = "0.00";
                                }
                                #endregion
                            }
                            #endregion

                            #region No objeto de IVA
                            if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorObjetoImpuesto) == 0)
                            {
                                decimal subTotalNoObjetoIva = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                if (validaSeparadorMiles)
                                    dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalNoObjetoIva);
                                else
                                    dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = objfactTotalImpuesto.BaseImponible.ToString();
                            }
                            else
                                dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = "0.00";
                            #endregion
                            #region Excento de Iva
                            if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorExentoIVA) == 0)
                            {
                                decimal excdntoIva = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                if (validaSeparadorMiles)
                                    dr_tblInformacionMonetaria["subTotalExcentoIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, excdntoIva);
                                else
                                    dr_tblInformacionMonetaria["subTotalExcentoIva"] = objfactTotalImpuesto.BaseImponible.ToString();
                            }
                            else
                                dr_tblInformacionMonetaria["subTotalExcentoIva"] = "0.00";
                            #endregion
                            #endregion IMPUESTOS_IVA
                        }

                        if (String.Compare(objfactTotalImpuesto.Codigo.Trim(), CatalogoViaDoc.ParametrosValorICE) == 0)
                        {
                            decimal ice = Convert.ToDecimal(objfactTotalImpuesto.Valor.Replace('.', ','));
                            if (validaSeparadorMiles)
                                dr_tblInformacionMonetaria["ice"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, ice);
                            else
                                dr_tblInformacionMonetaria["ice"] = objfactTotalImpuesto.Valor;
                        }
                        else
                            dr_tblInformacionMonetaria["ice"] = "0.00";

                        if (String.Compare(objfactTotalImpuesto.Codigo.Trim(), CatalogoViaDoc.ParametrosValorIRBPNR) == 0)
                        {
                            decimal irbpnr = Convert.ToDecimal(objfactTotalImpuesto.Valor.Replace('.', ','));
                            if (validaSeparadorMiles)
                                dr_tblInformacionMonetaria["irbpnr"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, irbpnr);
                            else
                                dr_tblInformacionMonetaria["irbpnr"] = "0.00";
                        }
                        else
                            dr_tblInformacionMonetaria["irbpnr"] = "0.00";

                        dr_tblInformacionMonetaria["irbpnr"] = "0.00";
                    }

                    decimal totalSinImpuesto = Convert.ToDecimal(objRideNotaDebito._infoNotaDebito.TotalSinImpuestos.Replace('.', ','));
                    decimal valorTotal = Convert.ToDecimal(objRideNotaDebito._infoNotaDebito.ValorTotal.Replace('.', ','));
                    if (validaSeparadorMiles)
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalSinImpuesto);
                        dr_tblInformacionMonetaria["valorTotal"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valorTotal);
                    }
                    else
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = objRideNotaDebito._infoNotaDebito.TotalSinImpuestos;
                        dr_tblInformacionMonetaria["valorTotal"] = objRideNotaDebito._infoNotaDebito.ValorTotal;
                    }
                    tblInformacionMonetaria.Rows.Add(dr_tblInformacionMonetaria);
                    #endregion

                    ReportDataSource datos_tblInfoTributaria = new ReportDataSource("tblInformacionTributaria", tblInfoTributaria);
                    ReportDataSource datos_tblInfoNotaDebito = new ReportDataSource("tblInformacionNotaDebito", tblInfoNotaDebito);
                    ReportDataSource datos_tblinfomacionMonetaria = new ReportDataSource("tblInformacionMonetaria", tblInformacionMonetaria);
                    ReportDataSource datos_tblInformacionAdicional = new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional);
                    ReportDataSource datos_tblMotivo = new ReportDataSource("tblMotivos", tblMotivos);

                    localReport.EnableExternalImages = true;
                    localReport.SetParameters(parameters);
                    localReport.DataSources.Add(datos_tblInformacionAdicional);
                    localReport.DataSources.Add(datos_tblMotivo);
                    localReport.DataSources.Add(datos_tblInfoTributaria);
                    localReport.DataSources.Add(datos_tblInfoNotaDebito);
                    localReport.DataSources.Add(datos_tblinfomacionMonetaria);

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
                    mensajeError = "Error al generar del byte[] para el ride Nota de Debito. Mensaje Error: La compania " + objRideNotaDebito._infoTributaria.RazonSocial + " no tiene registrada las configuraciones del reporte";
                    bytePDF = null;
                }

            }
            catch (Exception ex)
            {
                mensajeError = "Error al generar del byte[] para elo ride Nota de Debito. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
                bytePDF = null;
            }
            return bytePDF;

        }
    }
}
