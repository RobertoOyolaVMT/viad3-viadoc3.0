using GenCode128;
using Microsoft.Reporting.WebForms;
using ReportesViaDoc.EntidadesReporte;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ViaDoc.Configuraciones;
using ViaDoc.Utilitarios;

namespace ReportesViaDoc.LogicaReporte
{
    class ProcesarRideLiquidacion
    {
        private string pathCodBarra { get; set; }
        private string pathLogoEmpresa { get; set; }
        private String pathRider { get; set; }


        /// <summary>
        /// Autor: William Jacome Choez(Viamatica S.A.)
        /// Descripcion: Desarmar el xml autorizado de la factura para generar un objeto para procesar el byte[] del documento.
        /// Fecha: 08/05/2017
        /// </summary>
        /// <param name="mensajeError">Mensaje del error generado durante el proceso</param>
        /// <param name="xmlDocumentoAutorizado">XML autorizado de la Factura</param>
        /// <param name="fechaHoraAutorizacion">fecha de autorizacion de la Factura</param>
        /// <param name="numeroAutorizacion">numero de autorizacion de la Factura</param>
        /// <returns></returns>
        /// 

        public RideLiquidacion ProcesarXmlAutorizadoLiquidacion(ref string mensajeError, string xmlDocumentoAutorizado, string fechaHoraAutorizacion, string numeroAutorizacion)
        {
            mensajeError = "";
            RideLiquidacion objLiqui = new RideLiquidacion();
            System.IO.StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);

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
                    objLiqui.BanderaGeneracionObjeto = true;
                    objLiqui.NumeroAutorizacion = numeroAutorizacion;
                    objLiqui.FechaHoraAutorizacion = fechaHoraAutorizacion;

                    string NumeroAutorizacion = Informacion.SelectSingleNode("numeroAutorizacion").InnerText;
                    string FechaAutorizacion = Informacion.SelectSingleNode("fechaAutorizacion").InnerText;
                    string xmlComprobante = Informacion.SelectSingleNode("comprobante").InnerText.Replace("&", "&amp;");
                    document.LoadXml(xmlComprobante);
                    document.WriteTo(xtr);

                    XmlNodeList camposXML_infoTributaria = document.SelectNodes("liquidacionCompra/infoTributaria");
                    XmlNodeList camposXML_infoFactura = document.SelectNodes("liquidacionCompra/infoLiquidacionCompra");
                    XmlNodeList camposXML_detalles = document.SelectNodes("liquidacionCompra/detalles");

                    if (camposXML_infoTributaria.Count > 0 && camposXML_infoFactura.Count > 0 && camposXML_detalles.Count > 0)
                    {
                        XmlNodeList camposXML_infoAdicional = document.SelectNodes("liquidacionCompra/infoAdicional");
                        if (camposXML_infoAdicional.Count > 0)
                        {
                            #region Informacion Adicional de la Liquidacion
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
                            objLiqui._infosAdicional = listaInfoAdicional;
                            #endregion
                        }
                        #region Informacion tributaria de la Liqudacion
                        XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                        objLiqui._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                        objLiqui._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                        try { objLiqui._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText; }
                        catch (Exception ex) { objLiqui._infoTributaria.NombreComercial = ""; }
                        objLiqui._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                        objLiqui._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                        objLiqui._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                        objLiqui._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                        objLiqui._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                        objLiqui._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                        objLiqui._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                        try
                        {
                            objLiqui._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
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
                                                if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                            objLiqui._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objLiqui._infoTributaria.contribuyenteRimpe = "";
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
                                                if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        if (validaLeyenda)
                                                            objLiqui._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objLiqui._infoTributaria.contribuyenteRimpe = "";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                objLiqui._infoTributaria.contribuyenteRimpe = "";
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        objLiqui._infoTributaria.contribuyenteRimpe = "";
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
                                            if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objLiqui._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                                }
                                                else
                                                {
                                                    objLiqui._infoTributaria.AgenteRetencion = " ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objLiqui._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
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
                                        if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objLiqui._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objLiqui._infoTributaria.AgenteRetencion = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        objLiqui._infoTributaria.AgenteRetencion = "";
                                    }
                                }
                            }
                        }
                        #endregion
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
                                            if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objLiqui._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaAgente.Trim();
                                                }
                                                else
                                                {
                                                    objLiqui._infoTributaria.regimenMicroempresas = " ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objLiqui._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                            {
                                try
                                {
                                    string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                                    foreach (string Ruc in Cadena)
                                    {
                                        if (!Ruc.Equals(""))
                                        {
                                            if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objLiqui._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                                }
                                                else
                                                {
                                                    objLiqui._infoTributaria.regimenMicroempresas = "";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            objLiqui._infoTributaria.regimenMicroempresas = "";
                                        }
                                    }
                                }
                                catch
                                {
                                    objLiqui._infoTributaria.regimenMicroempresas = "";
                                }
                            }
                        }
                        //Leyenda Regimen General
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
                        //                    if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                        //                    {
                        //                        bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                        //                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                        //                        {
                        //                            if (validaLeyenda)
                        //                                objLiqui._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                        //                        }
                        //                        else
                        //                        {
                        //                            objLiqui._infoTributaria.regimenGeneral = " ";
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        objLiqui._infoTributaria.regimenGeneral = string.Empty;
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
                        //                if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                        //                {
                        //                    bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                        //                    if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                        //                    {
                        //                        if (validaLeyenda)
                        //                            objLiqui._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                        //                    }
                        //                    else
                        //                    {
                        //                        objLiqui._infoTributaria.regimenGeneral = "";
                        //                    }

                        //                }
                        //            }
                        //            else
                        //            {
                        //                objLiqui._infoTributaria.regimenGeneral = "";
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        objLiqui._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                        // #endregion



                        #region Infromacion De La Liquidaicon
                        XmlNode tagInfoLiquidacion = camposXML_infoFactura.Item(0);
                        objLiqui._infoLiquidacion.FechaEmision = tagInfoLiquidacion.SelectSingleNode("fechaEmision").InnerText;
                        objLiqui._infoLiquidacion.DirEstablecimiento = tagInfoLiquidacion.SelectSingleNode("dirEstablecimiento").InnerText;
                        try { objLiqui._infoLiquidacion.NumeroContribuyenteEspecial = tagInfoLiquidacion.SelectSingleNode("contribuyenteEspecial").InnerText; }
                        catch (Exception ex) { objLiqui._infoLiquidacion.NumeroContribuyenteEspecial = ""; }
                        objLiqui._infoLiquidacion.ObligadoContabilidad = tagInfoLiquidacion.SelectSingleNode("obligadoContabilidad").InnerText;
                        objLiqui._infoLiquidacion.TipoIdentificacion = tagInfoLiquidacion.SelectSingleNode("tipoIdentificacionProveedor").InnerText;
                        objLiqui._infoLiquidacion.RazonSocial = tagInfoLiquidacion.SelectSingleNode("razonSocialProveedor").InnerText;
                        try { objLiqui._infoLiquidacion.GuiaRemision = tagInfoLiquidacion.SelectSingleNode("guiaRemision").InnerText; }
                        catch (Exception ex) { objLiqui._infoLiquidacion.GuiaRemision = ""; }
                        objLiqui._infoLiquidacion.Identificacion = tagInfoLiquidacion.SelectSingleNode("identificacionProveedor").InnerText;
                        objLiqui._infoLiquidacion.TotalSinImpuestos = tagInfoLiquidacion.SelectSingleNode("totalSinImpuestos").InnerText;
                        objLiqui._infoLiquidacion.TotalDescuento = tagInfoLiquidacion.SelectSingleNode("totalDescuento").InnerText;

                        string xmlTotalConImpuestos = "<totalConImpuestos>" + tagInfoLiquidacion.SelectSingleNode("totalConImpuestos").InnerXml + "</totalConImpuestos>";  //<-- AQUIII

                        objLiqui._infoLiquidacion.ImporteTotal = tagInfoLiquidacion.SelectSingleNode("importeTotal").InnerText;
                        objLiqui._infoLiquidacion.Moneda = tagInfoLiquidacion.SelectSingleNode("moneda").InnerText;
                        string xmlFormaPago = "";
                        try { xmlFormaPago = "<FormaPagos>" + tagInfoLiquidacion.SelectSingleNode("pagos").InnerXml + "</FormaPagos>"; }
                        catch (Exception ex) { xmlFormaPago = ""; }

                        #region Totales con impuesto de la Liquidacion

                        document.LoadXml(xmlTotalConImpuestos);
                        document.WriteTo(xtr);
                        XmlNodeList camposXML_totalConImpuesto = document.GetElementsByTagName("totalImpuesto");
                        List<TotalConImpuesto> listaTotalConImpuesto = new List<TotalConImpuesto>();
                        foreach (XmlNode item in camposXML_totalConImpuesto)
                        {
                            TotalConImpuesto totalConImp = new TotalConImpuesto();
                            totalConImp.Codigo = item.SelectSingleNode("codigo").InnerText;
                            totalConImp.CodigoPorcentaje = item.SelectSingleNode("codigoPorcentaje").InnerText;
                            totalConImp.BaseImponible = item.SelectSingleNode("baseImponible").InnerText;
                            totalConImp.Tarifa = item.SelectSingleNode("tarifa").InnerText;
                            totalConImp.Valor = item.SelectSingleNode("valor").InnerText;
                            listaTotalConImpuesto.Add(totalConImp);
                        }
                        objLiqui._infoLiquidacion._totalesConImpuesto = listaTotalConImpuesto;
                        #endregion

                        #region Formas de Pago de la Liquidacion

                        if (xmlFormaPago != "")
                        {
                            document.LoadXml(xmlFormaPago);
                            document.WriteTo(xtr);
                            XmlNodeList camposXML_formasPago = document.GetElementsByTagName("pago");
                            List<FormaPago> listaFormaPago = new List<FormaPago>();
                            foreach (XmlNode itemFormaPago in camposXML_formasPago)
                            {
                                FormaPago formaPago = new FormaPago();
                                formaPago.CodigoFormaPago = itemFormaPago.SelectSingleNode("formaPago").InnerText;
                                formaPago.Total = itemFormaPago.SelectSingleNode("total").InnerText;
                                formaPago.Plazo = itemFormaPago.SelectSingleNode("plazo").InnerText;
                                formaPago.UnidadTiempo = itemFormaPago.SelectSingleNode("unidadTiempo").InnerText;
                                listaFormaPago.Add(formaPago);
                            }
                            objLiqui._infoLiquidacion._formasPago = listaFormaPago;
                        }
                        #endregion

                        #endregion

                        #region Detalle de la Liqudacion
                        List<Detalle> detallesLiqudiacion = new List<Detalle>();
                        foreach (XmlNode tagDetalles in camposXML_detalles)
                        {
                            XmlNodeList nodos = tagDetalles.ChildNodes;
                            foreach (XmlNode nodo in nodos)
                            {
                                Detalle detalle = new Detalle();
                                detalle.CodigoPrincipal = nodo.SelectSingleNode("codigoPrincipal").InnerText;
                                detalle.CodigoAuxiliar = "";
                                try { detalle.CodigoAuxiliar = nodo.SelectSingleNode("codigoAuxiliar").InnerText; } catch (Exception ex) { detalle.CodigoAuxiliar = ""; }

                                detalle.Descripcion = nodo.SelectSingleNode("descripcion").InnerText;
                                detalle.Cantidad = nodo.SelectSingleNode("cantidad").InnerText;
                                detalle.PrecioUnitario = nodo.SelectSingleNode("precioUnitario").InnerText;
                                detalle.Descuento = nodo.SelectSingleNode("descuento").InnerText;
                                detalle.PrecioTotalSinImpuesto = nodo.SelectSingleNode("precioTotalSinImpuesto").InnerText;
                                string xmlImpuestosDetalle = nodo.SelectSingleNode("impuestos").InnerXml;
                                document.LoadXml(xmlImpuestosDetalle);
                                document.WriteTo(xtr);
                                XmlNodeList camposXML_impuestos = document.SelectNodes("impuesto");
                                List<Impuesto> impuestosDetalle = new List<Impuesto>();
                                foreach (XmlNode nodoImpuesto in camposXML_impuestos)
                                {
                                    Impuesto impuesto = new Impuesto();
                                    impuesto.Codigo = nodoImpuesto.SelectSingleNode("codigo").InnerText;
                                    impuesto.CodigoPorcentaje = nodoImpuesto.SelectSingleNode("codigoPorcentaje").InnerText;
                                    impuesto.Tarifa = nodoImpuesto.SelectSingleNode("tarifa").InnerText;
                                    impuesto.BaseImponible = nodoImpuesto.SelectSingleNode("baseImponible").InnerText;
                                    impuesto.Valor = nodoImpuesto.SelectSingleNode("valor").InnerText;
                                    impuestosDetalle.Add(impuesto);
                                }
                                detalle._impuestos = impuestosDetalle;
                                detallesLiqudiacion.Add(detalle);
                            }
                        }
                        objLiqui._detalles = detallesLiqudiacion;
                        #endregion
                    }
                }
                else
                {
                    objLiqui = ProcesarXMLFirmadoLiqudiacion(ref mensajeError, xmlDocumentoAutorizado);
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
                objLiqui.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml firmado de la Factura. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
            }

            return objLiqui;
        }

        public RideLiquidacion ProcesarXMLFirmadoLiqudiacion(ref string mensajeError, string xmlDocumentoFirmado)
        {
            mensajeError = "";
            RideLiquidacion objLiqui = new RideLiquidacion();
            System.IO.StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);

            try
            {
                XmlDocument document = new XmlDocument();
                xmlDocumentoFirmado = Utilitarios.ReemplazarCaracteresEspeciales(xmlDocumentoFirmado);
                document.LoadXml(xmlDocumentoFirmado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml;

                XmlNodeList camposXML_infoTributaria = document.SelectNodes("liquidacionCompra/infoTributaria");
                XmlNodeList camposXML_infoFactura = document.SelectNodes("liquidacionCompra/infoLiquidacionCompra");
                XmlNodeList camposXML_detalles = document.SelectNodes("liquidacionCompra/detalles");

                if (camposXML_infoTributaria.Count > 0 && camposXML_infoFactura.Count > 0 && camposXML_detalles.Count > 0)
                {
                    objLiqui.BanderaGeneracionObjeto = true;
                    XmlNodeList camposXML_infoAdicional = document.SelectNodes("liquidacionCompra/infoAdicional");
                    if (camposXML_infoAdicional.Count > 0)
                    {
                        #region Informacion Adicional de la Liquidacion
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
                        objLiqui._infosAdicional = listaInfoAdicional;
                        #endregion
                    }
                    #region Informacion tributaria de la Liqudacion
                    XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                    objLiqui._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                    objLiqui._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                    objLiqui._infoTributaria.NombreComercial = "";
                    try { objLiqui._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText; }
                    catch (Exception ex) { objLiqui._infoTributaria.NombreComercial = ""; }
                    objLiqui._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                    objLiqui._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                    objLiqui._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                    objLiqui._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                    objLiqui._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                    objLiqui._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                    objLiqui._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                    try
                    {
                        objLiqui._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
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
                                            if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objLiqui._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                }
                                                else
                                                {
                                                    objLiqui._infoTributaria.contribuyenteRimpe = "";
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
                                            if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objLiqui._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                }
                                                else
                                                {
                                                    objLiqui._infoTributaria.contribuyenteRimpe = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            objLiqui._infoTributaria.contribuyenteRimpe = "";
                                        }
                                    }
                                }
                                catch
                                {
                                    objLiqui._infoTributaria.contribuyenteRimpe = "";
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
                                        if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objLiqui._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objLiqui._infoTributaria.AgenteRetencion = " ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objLiqui._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
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
                                    if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                objLiqui._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                        }
                                        else
                                        {
                                            objLiqui._infoTributaria.AgenteRetencion = "";
                                        }

                                    }
                                }
                                else
                                {
                                    objLiqui._infoTributaria.AgenteRetencion = "";
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
                                string[] Cadena = ConfigurationManager.AppSettings.Get("Regimen_Microempresa").Split('|');
                                foreach (string Ruc in Cadena)
                                {
                                    if (!Ruc.Equals(""))
                                    {
                                        if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objLiqui._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objLiqui._infoTributaria.regimenMicroempresas = " ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objLiqui._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                        {
                            string[] Cadena = ConfigurationManager.AppSettings.Get("Regimen_Microempresa").Split('|');
                            foreach (string Ruc in Cadena)
                            {
                                if (!Ruc.Equals(""))
                                {
                                    if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                objLiqui._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                        }
                                        else
                                        {
                                            objLiqui._infoTributaria.regimenMicroempresas = "";
                                        }

                                    }
                                }
                                else
                                {
                                    objLiqui._infoTributaria.regimenMicroempresas = "";
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
                    //                    if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                    //                    {
                    //                        bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                    //                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                    //                        {
                    //                            if (validaLeyenda)
                    //                                objLiqui._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                    //                        }
                    //                        else
                    //                        {
                    //                            objLiqui._infoTributaria.regimenGeneral = " ";
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        objLiqui._infoTributaria.regimenGeneral = string.Empty;
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
                    //                if (Convert.ToInt64(objLiqui._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                    //                {
                    //                    bool validaLeyenda = !CatalogoViaDoc.LeyendaGeneral.Trim().Equals("") ? true : false;
                    //                    if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                    //                    {
                    //                        if (validaLeyenda)
                    //                            objLiqui._infoTributaria.regimenGeneral = CatalogoViaDoc.LeyendaGeneral.Trim();
                    //                    }
                    //                    else
                    //                    {
                    //                        objLiqui._infoTributaria.regimenGeneral = "";
                    //                    }

                    //                }
                    //            }
                    //            else
                    //            {
                    //                objLiqui._infoTributaria.regimenGeneral = "";
                    //            }
                    //        }
                    //    }
                    //}
                    //#endregion
                    objLiqui._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                    // #endregion
                    objLiqui._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                    #endregion

                    #region Infromacion De La Liquidaicon
                    XmlNode tagInfoLiquidacion = camposXML_infoFactura.Item(0);
                    objLiqui._infoLiquidacion.FechaEmision = tagInfoLiquidacion.SelectSingleNode("fechaEmision").InnerText;
                    objLiqui._infoLiquidacion.DirEstablecimiento = tagInfoLiquidacion.SelectSingleNode("dirEstablecimiento").InnerText;
                    try { objLiqui._infoLiquidacion.NumeroContribuyenteEspecial = tagInfoLiquidacion.SelectSingleNode("contribuyenteEspecial").InnerText; }
                    catch (Exception ex) { objLiqui._infoLiquidacion.NumeroContribuyenteEspecial = ""; }
                    objLiqui._infoLiquidacion.ObligadoContabilidad = tagInfoLiquidacion.SelectSingleNode("obligadoContabilidad").InnerText;
                    objLiqui._infoLiquidacion.TipoIdentificacion = tagInfoLiquidacion.SelectSingleNode("tipoIdentificacionProveedor").InnerText;
                    objLiqui._infoLiquidacion.RazonSocial = tagInfoLiquidacion.SelectSingleNode("razonSocialProveedor").InnerText;
                    objLiqui._infoLiquidacion.Identificacion = tagInfoLiquidacion.SelectSingleNode("identificacionProveedor").InnerText;
                    objLiqui._infoLiquidacion.TotalSinImpuestos = tagInfoLiquidacion.SelectSingleNode("totalSinImpuestos").InnerText;
                    objLiqui._infoLiquidacion.TotalDescuento = tagInfoLiquidacion.SelectSingleNode("totalDescuento").InnerText;

                    string xmlTotalConImpuestos = "<totalConImpuestos>" + tagInfoLiquidacion.SelectSingleNode("totalConImpuestos").InnerXml + "</totalConImpuestos>";  //<-- AQUIII

                    objLiqui._infoLiquidacion.ImporteTotal = tagInfoLiquidacion.SelectSingleNode("importeTotal").InnerText;
                    objLiqui._infoLiquidacion.Moneda = tagInfoLiquidacion.SelectSingleNode("moneda").InnerText;
                    string xmlFormaPago = "";
                    try { xmlFormaPago = "<FormaPagos>" + tagInfoLiquidacion.SelectSingleNode("pagos").InnerXml + "</FormaPagos>"; }
                    catch (Exception ex) { xmlFormaPago = ""; }

                    #region Totales con impuesto de la Liquidacion

                    document.LoadXml(xmlTotalConImpuestos);
                    document.WriteTo(xtr);
                    XmlNodeList camposXML_totalConImpuesto = document.GetElementsByTagName("totalImpuesto");
                    List<TotalConImpuesto> listaTotalConImpuesto = new List<TotalConImpuesto>();
                    foreach (XmlNode item in camposXML_totalConImpuesto)
                    {
                        TotalConImpuesto totalConImp = new TotalConImpuesto();
                        totalConImp.Codigo = item.SelectSingleNode("codigo").InnerText;
                        totalConImp.CodigoPorcentaje = item.SelectSingleNode("codigoPorcentaje").InnerText;
                        totalConImp.BaseImponible = item.SelectSingleNode("baseImponible").InnerText;
                        totalConImp.Tarifa = item.SelectSingleNode("tarifa").InnerText;
                        totalConImp.Valor = item.SelectSingleNode("valor").InnerText;
                        listaTotalConImpuesto.Add(totalConImp);
                    }
                    objLiqui._infoLiquidacion._totalesConImpuesto = listaTotalConImpuesto;
                    #endregion

                    #region Formas de Pago de la Liquidacion

                    if (xmlFormaPago != "")
                    {
                        document.LoadXml(xmlFormaPago);
                        document.WriteTo(xtr);
                        XmlNodeList camposXML_formasPago = document.GetElementsByTagName("pago");
                        List<FormaPago> listaFormaPago = new List<FormaPago>();
                        foreach (XmlNode itemFormaPago in camposXML_formasPago)
                        {
                            FormaPago formaPago = new FormaPago();
                            formaPago.CodigoFormaPago = itemFormaPago.SelectSingleNode("formaPago").InnerText;
                            formaPago.Total = itemFormaPago.SelectSingleNode("total").InnerText;
                            formaPago.Plazo = itemFormaPago.SelectSingleNode("plazo").InnerText;
                            formaPago.UnidadTiempo = itemFormaPago.SelectSingleNode("unidadTiempo").InnerText;
                            listaFormaPago.Add(formaPago);
                        }
                        objLiqui._infoLiquidacion._formasPago = listaFormaPago;
                    }
                    #endregion

                    #endregion

                    #region Detalle de la Liqudacion
                    List<Detalle> detallesLiqudiacion = new List<Detalle>();
                    foreach (XmlNode tagDetalles in camposXML_detalles)
                    {
                        XmlNodeList nodos = tagDetalles.ChildNodes;
                        foreach (XmlNode nodo in nodos)
                        {
                            Detalle detalle = new Detalle();
                            detalle.CodigoPrincipal = nodo.SelectSingleNode("codigoPrincipal").InnerText;
                            detalle.CodigoAuxiliar = "";
                            try { detalle.CodigoAuxiliar = nodo.SelectSingleNode("codigoAuxiliar").InnerText; } catch (Exception ex) { detalle.CodigoAuxiliar = ""; }

                            detalle.Descripcion = nodo.SelectSingleNode("descripcion").InnerText;
                            detalle.Cantidad = nodo.SelectSingleNode("cantidad").InnerText;
                            detalle.PrecioUnitario = nodo.SelectSingleNode("precioUnitario").InnerText;
                            detalle.Descuento = nodo.SelectSingleNode("descuento").InnerText;
                            detalle.PrecioTotalSinImpuesto = nodo.SelectSingleNode("precioTotalSinImpuesto").InnerText;
                            string xmlImpuestosDetalle = nodo.SelectSingleNode("impuestos").InnerXml;
                            document.LoadXml(xmlImpuestosDetalle);
                            document.WriteTo(xtr);
                            XmlNodeList camposXML_impuestos = document.SelectNodes("impuesto");
                            List<Impuesto> impuestosDetalle = new List<Impuesto>();
                            foreach (XmlNode nodoImpuesto in camposXML_impuestos)
                            {
                                Impuesto impuesto = new Impuesto();
                                impuesto.Codigo = nodoImpuesto.SelectSingleNode("codigo").InnerText;
                                impuesto.CodigoPorcentaje = nodoImpuesto.SelectSingleNode("codigoPorcentaje").InnerText;
                                impuesto.Tarifa = nodoImpuesto.SelectSingleNode("tarifa").InnerText;
                                impuesto.BaseImponible = nodoImpuesto.SelectSingleNode("baseImponible").InnerText;
                                impuesto.Valor = nodoImpuesto.SelectSingleNode("valor").InnerText;
                                impuestosDetalle.Add(impuesto);
                            }
                            detalle._impuestos = impuestosDetalle;
                            detallesLiqudiacion.Add(detalle);
                        }
                    }
                    objLiqui._detalles = detallesLiqudiacion;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
                objLiqui.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml firmado de la Factura. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
            }

            return objLiqui;
        }

        /// <summary>
        /// Autor: William Jacome Choez(Viamatica .S.A)
        /// Descripcion: Genera el array de byte del RIDE de la Factura
        /// </summary>
        /// <param name="mensajeError"></param>
        /// <param name="objRideLiquidacion"></param>
        /// <param name="configuraciones"></param>
        /// <param name="catalogoSistema"></param>
        /// <returns></returns>
        public Byte[] GenerarRideLiquidaicon(ref string mensajeError, RideLiquidacion objRideLiquidacion, List<ConfiguracionReporte> configuraciones, List<CatalogoReporte> catalogoSistema)
        {
            byte[] bytePDF = null;
            string etiquetaContribuyenteEspecial = "";
            string etiquetaTipoIvaGrabadoConPorcentaje = "";
            string etiquetaTipoIvaGrabadoSiPorcentaje = "";
            string validaCamposNegritasInfoAdicional = "0";
            string palabraClaveFechaVencimiento = "";
            string palabraClaveFechaCorte = "";

            try
            {
                List<ConfiguracionReporte> confCompania = configuraciones.FindAll(x => x.RucCompania == objRideLiquidacion._infoTributaria.Ruc);
                if (confCompania != null)
                {
                    #region Origen Datos del RIDE Liquidacion
                    DataTable tblInfoTributaria = new DataTable();
                    DataTable tblInfoLiquidacion = new DataTable();
                    DataTable tblDetalleLiquidacion = new DataTable();
                    DataTable tblInformacionAdicional = new DataTable();
                    DataTable tblInformacionMonetaria = new DataTable();
                    DataTable tblFormaPago = new DataTable();
                    tblInfoTributaria.TableName = "tblInformacionTributaria";
                    tblInfoLiquidacion.TableName = "tblInformacionFactura";
                    tblDetalleLiquidacion.TableName = "tblDetalleFactura";
                    tblInformacionAdicional.TableName = "tblInformacionAdicional";
                    tblFormaPago.TableName = "tblFormaPago";
                    tblInformacionMonetaria.TableName = "tblInformacionMonetaria";

                    DataColumn[] cols_tblInfoTributaria = new DataColumn[] {
                        new DataColumn("ambiente",typeof(string)),
                        new DataColumn("claveAcceso",typeof(string)),
                        new DataColumn("codDoc",typeof(string)),
                        new DataColumn("dirMatriz",typeof(string)),
                        new DataColumn("regimenMicroempresas",typeof(string)),
                        new DataColumn("agenteRetencion",typeof(string)),
                        new DataColumn("contribuyenteRimpe", typeof(string)),
                        new DataColumn("estab",typeof(string)),
                        new DataColumn("nombreComercial",typeof(string)),
                        new DataColumn("ptoEmi",typeof(string)),
                        new DataColumn("razonSocial",typeof(string)),
                        new DataColumn("ruc",typeof(string)),
                        new DataColumn("secuencial",typeof(string)),
                        new DataColumn("tipoEmision",typeof(string)),
                        //new DataColumn("regimenGeneral", typeof(string))
                    };

                    DataColumn[] cols_tblInfoFactura = new DataColumn[] {
                        new DataColumn("contribuyenteEspecial",typeof(string)),
                        new DataColumn("dirEstablecimiento",typeof(string)),
                        new DataColumn("fechaEmision",typeof(string)),
                        //new DataColumn("guiaRemision",typeof(string)),
                        new DataColumn("identificacionComprador",typeof(string)),
                        new DataColumn("importeTotal",typeof(string)),
                        new DataColumn("moneda",typeof(string)),
                        new DataColumn("obligadoContabilidad",typeof(string)),
                        new DataColumn("propina",typeof(string)),
                        new DataColumn("razonSocialComprador",typeof(string)),
                        new DataColumn("tipoIdentificacionComprador",typeof(string)),
                        new DataColumn("totalDescuento",typeof(string)),
                        new DataColumn("totalSinImpuestos",typeof(string)),
                    };

                    DataColumn[] cols_tblDetalleFactura = new DataColumn[] {
                        new DataColumn("cantidad",typeof(string)),
                        new DataColumn("codigoAuxiliar",typeof(string)),
                        new DataColumn("codigoPrincipal",typeof(string)),
                        new DataColumn("descripcion",typeof(string)),
                        new DataColumn("descuento",typeof(string)),
                        new DataColumn("precioTotalSinImpuesto",typeof(string)),
                        new DataColumn("precioUnitario",typeof(string)),
                    };

                    DataColumn[] cols_tblInformacionAdicional = new DataColumn[] {
                        new DataColumn("nombre",typeof(string)),
                        new DataColumn("valor",typeof(string))
                    };

                    DataColumn[] cols_tblFormaPago = new DataColumn[] {
                        new DataColumn("formaPago",typeof(string)),
                        new DataColumn("total",typeof(string)),
                        new DataColumn("plazo",typeof(string)),
                        new DataColumn("unidadTiempo",typeof(string)),
                    };

                    DataColumn[] cols_tblInformacionMonetaria = new DataColumn[] {
                        new DataColumn("subTotalIva",typeof(string)),
                        new DataColumn("subTotalIva5",typeof(string)),
                        new DataColumn("subTotalCero",typeof(string)),
                        new DataColumn("subTotalNoObjetoIva",typeof(string)),
                        new DataColumn("subTotalSinImpuesto",typeof(string)),
                        new DataColumn("subTotalExcentoIva",typeof(string)),
                        new DataColumn("descuento",typeof(string)),
                        new DataColumn("iva",typeof(string)),
                        new DataColumn("iva5",typeof(string)),
                        new DataColumn("INBPNR",typeof(string)),
                        new DataColumn("propina",typeof(string)),
                        new DataColumn("valor",typeof(string)),
                        new DataColumn("ice",typeof(string))
                    };


                    tblInfoTributaria.Columns.AddRange(cols_tblInfoTributaria);
                    tblInfoLiquidacion.Columns.AddRange(cols_tblInfoFactura);
                    tblDetalleLiquidacion.Columns.AddRange(cols_tblDetalleFactura);
                    tblInformacionAdicional.Columns.AddRange(cols_tblInformacionAdicional);
                    tblFormaPago.Columns.AddRange(cols_tblFormaPago);
                    tblInformacionMonetaria.Columns.AddRange(cols_tblInformacionMonetaria);
                    #endregion
                    #region Logo de la Compania
                    pathLogoEmpresa = CatalogoViaDoc.rutaLogoCompania;
                    string rutaImagen = pathLogoEmpresa + objRideLiquidacion._infoTributaria.Ruc.ToString().Trim() + ".png";
                    #endregion
                    #region Genera Codigo Barra de la Liquidacion  MEJORAR
                    byte[] imgBar;
                    Code.BarcodeGenerator bgCode128 = new Code.BarcodeGenerator();
                    Code.Convertir cCode = new Code.Convertir();
                    Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                    StringFormat fi = new StringFormat(StringFormatFlags.NoClip);
                    fi.Alignment = StringAlignment.Center;
                    imgBar = RetornarXml.ImageToByte2(Code128Rendering.MakeBarcodeImage(objRideLiquidacion._infoTributaria.ClaveAcceso, 3, true));

                    pathCodBarra = CatalogoViaDoc.rutaCodigoBarra + objRideLiquidacion._infoTributaria.ClaveAcceso.Trim();
                    pathRider = CatalogoViaDoc.rutaRide + objRideLiquidacion._infoTributaria.Ruc.Trim() + "\\" + objRideLiquidacion._infoTributaria.CodigoDocumento.Trim();
                    if (!Directory.Exists(pathRider))
                    {
                        Directory.CreateDirectory(pathRider);
                    }
                    File.WriteAllBytes(pathCodBarra + ".jpg", imgBar);
                    #endregion

                    #region Configuracion de los separadores de miles de la Liquidacion MEJORAR
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
                    #region Configuracion para agregar la forma de pago en el ride
                    bool validaAgregacionFormaPago = false;
                    ConfiguracionReporte confFormaPago = confCompania.Find(x => x.CodigoReferencia == "ACTFORMPAG");
                    if (confFormaPago != null)
                        validaAgregacionFormaPago = Convert.ToBoolean(confFormaPago.Configuracion2.Trim());
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

                    ConfiguracionReporte confContribuEspecial = confCompania.Find(x => x.CodigoReferencia == "ACTNUMCONT");
                    bool validaNumeroContribuyenteEspecial = false;
                    if (confContribuEspecial != null)
                        validaNumeroContribuyenteEspecial = Convert.ToBoolean(confContribuEspecial.Configuracion2);
                    #endregion

                    #region Configuracion para colocar en negritas datos de la seccion de Informacion Adicional
                    ConfiguracionReporte confActivarNegrita = confCompania.Find(x => x.CodigoReferencia == "NEGRIFOADD");
                    if (confActivarNegrita != null)
                    {
                        bool activarNegrita = Convert.ToBoolean(confActivarNegrita.Configuracion2.Trim());
                        if (activarNegrita)
                        {
                            validaCamposNegritasInfoAdicional = "1";
                            List<ConfiguracionReporte> valores = confCompania.FindAll(x => x.CodigoReferencia == "PALCLAVE");
                            palabraClaveFechaVencimiento = valores[0].Configuracion1;
                            palabraClaveFechaCorte = valores[1].Configuracion1;
                        }
                    }

                    #endregion Lista de campos para ubicarlo en negrita

                    #region Catalogo Forma de Pago
                    List<CatalogoReporte> listadoFormasPago = catalogoSistema.FindAll(x => x.CodigoReferencia == "FORMAPAGO");
                    bool validaFormaPago = false;
                    if (listadoFormasPago != null)
                        validaFormaPago = true;
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
                                    string[] fechaEmisionDocumento = objRideLiquidacion._infoLiquidacion.FechaEmision.Split('/');
                                    DateTime fechaInicioSalvaguardia = new DateTime(Convert.ToInt32(fechaIni[2]), Convert.ToInt32(fechaIni[1]), Convert.ToInt32(fechaIni[0]));
                                    DateTime fechaFinSalvaguardia = new DateTime(Convert.ToInt32(fechaFin[2]), Convert.ToInt32(fechaFin[1]), Convert.ToInt32(fechaFin[0]));
                                    DateTime fechaEmision = new DateTime(Convert.ToInt32(fechaEmisionDocumento[2]), Convert.ToInt32(fechaEmisionDocumento[1]), Convert.ToInt32(fechaEmisionDocumento[0]));

                                    if (fechaEmision.Year <= fechaFinSalvaguardia.Year && fechaEmision.Year >= fechaInicioSalvaguardia.Year)
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
                    localReport.ReportPath = CatalogoViaDoc.rutaRidePlantilla + "RideLiquidacion.rdlc";
                    ReportParameter[] parameters = new ReportParameter[10];
                    parameters[0] = new ReportParameter("txFechaAutorizacion", objRideLiquidacion.FechaHoraAutorizacion);
                    parameters[1] = new ReportParameter("txNumeroAutorizacion", objRideLiquidacion._infoTributaria.ClaveAcceso);// Numero autorizacion es el mismo 
                    parameters[2] = new ReportParameter("pathImagenCodBarra", pathCodBarra + ".jpg");
                    parameters[3] = new ReportParameter("pathLogoCompania", @rutaImagen);
                    parameters[4] = new ReportParameter("tarifaIva", etiquetaTipoIvaGrabadoSiPorcentaje);
                    parameters[5] = new ReportParameter("etiquetaTarifaIva", etiquetaTipoIvaGrabadoConPorcentaje);
                    parameters[6] = new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial);
                    parameters[7] = new ReportParameter("banderaNegrita", validaCamposNegritasInfoAdicional);
                    parameters[8] = new ReportParameter("fechaVencimiento", palabraClaveFechaVencimiento);
                    parameters[9] = new ReportParameter("fechaCorte", palabraClaveFechaCorte);

                    #region Datos Tributarios
                    DataRow drInfoTributaria = tblInfoTributaria.NewRow();
                    if (validaAmbiente)
                    {
                        CatalogoReporte tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == objRideLiquidacion._infoTributaria.Ambiente);
                        drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                    }
                    else
                        drInfoTributaria["ambiente"] = objRideLiquidacion._infoTributaria.Ambiente;
                    drInfoTributaria["claveAcceso"] = objRideLiquidacion._infoTributaria.ClaveAcceso;
                    drInfoTributaria["codDoc"] = objRideLiquidacion._infoTributaria.CodigoDocumento;
                    drInfoTributaria["dirMatriz"] = objRideLiquidacion._infoTributaria.DirMatriz;
                    try { drInfoTributaria["regimenMicroempresas"] = objRideLiquidacion._infoTributaria.regimenMicroempresas; } catch { drInfoTributaria["regimenMicroempresas"] = ""; }
                    try { drInfoTributaria["agenteRetencion"] = objRideLiquidacion._infoTributaria.AgenteRetencion; } catch { drInfoTributaria["agenteRetencion"] = ""; }
                    try { drInfoTributaria["contribuyenteRimpe"] = objRideLiquidacion._infoTributaria.contribuyenteRimpe; } catch { drInfoTributaria["contribuyenteRimpe"] = ""; }
                    // try { drInfoTributaria["regimenGeneral"] = objRideLiquidacion._infoTributaria.regimenGeneral; } catch { drInfoTributaria["regimenGeneral"] = ""; }
                    drInfoTributaria["estab"] = objRideLiquidacion._infoTributaria.Establecimiento;
                    drInfoTributaria["nombreComercial"] = objRideLiquidacion._infoTributaria.NombreComercial;
                    drInfoTributaria["ptoEmi"] = objRideLiquidacion._infoTributaria.PuntoEmision;
                    drInfoTributaria["razonSocial"] = objRideLiquidacion._infoTributaria.RazonSocial;
                    drInfoTributaria["ruc"] = objRideLiquidacion._infoTributaria.Ruc;
                    drInfoTributaria["secuencial"] = objRideLiquidacion._infoTributaria.Secuencial;
                    if (validaEmisionFactura)
                    {
                        CatalogoReporte tipoemision = emisionesFacturacion.Find(x => x.Codigo == objRideLiquidacion._infoTributaria.TipoEmision);
                        drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                    }
                    else
                        drInfoTributaria["tipoEmision"] = objRideLiquidacion._infoTributaria.TipoEmision;
                    tblInfoTributaria.Rows.Add(drInfoTributaria);
                    #endregion
                    #region Datos de la Liquidacion
                    DataRow drInfoLiquidacion = tblInfoLiquidacion.NewRow();
                    drInfoLiquidacion["dirEstablecimiento"] = objRideLiquidacion._infoLiquidacion.DirEstablecimiento;
                    drInfoLiquidacion["fechaEmision"] = objRideLiquidacion._infoLiquidacion.FechaEmision;
                    if (validaNumeroContribuyenteEspecial)
                        drInfoLiquidacion["contribuyenteEspecial"] = objRideLiquidacion._infoLiquidacion.NumeroContribuyenteEspecial;
                    else
                        drInfoLiquidacion["contribuyenteEspecial"] = "";
                    drInfoLiquidacion["identificacionComprador"] = objRideLiquidacion._infoLiquidacion.Identificacion;
                    drInfoLiquidacion["importeTotal"] = objRideLiquidacion._infoLiquidacion.ImporteTotal;
                    drInfoLiquidacion["moneda"] = objRideLiquidacion._infoLiquidacion.Moneda;
                    drInfoLiquidacion["obligadoContabilidad"] = objRideLiquidacion._infoLiquidacion.ObligadoContabilidad;
                    drInfoLiquidacion["razonSocialComprador"] = objRideLiquidacion._infoLiquidacion.RazonSocial;
                    drInfoLiquidacion["tipoIdentificacionComprador"] = objRideLiquidacion._infoLiquidacion.TipoIdentificacion;
                    drInfoLiquidacion["totalDescuento"] = objRideLiquidacion._infoLiquidacion.TotalDescuento;
                    drInfoLiquidacion["totalSinImpuestos"] = objRideLiquidacion._infoLiquidacion.TotalSinImpuestos;
                    tblInfoLiquidacion.Rows.Add(drInfoLiquidacion);
                    #endregion
                    #region Detalle de la Liquidacion
                    DataRow drDetalle;
                    foreach (Detalle item in objRideLiquidacion._detalles)
                    {
                        drDetalle = tblDetalleLiquidacion.NewRow();
                        drDetalle["cantidad"] = item.Cantidad;
                        drDetalle["codigoAuxiliar"] = item.CodigoAuxiliar;
                        drDetalle["codigoPrincipal"] = item.CodigoPrincipal;
                        drDetalle["descripcion"] = item.Descripcion;
                        decimal descuento = Convert.ToDecimal(item.Descuento.Replace('.', ','));
                        decimal precioTotalSinImp = Convert.ToDecimal(item.PrecioTotalSinImpuesto.Replace('.', ','));
                        decimal precioUnitario = Convert.ToDecimal(item.PrecioUnitario.Replace('.', ','));
                        if (validaSeparadorMiles)
                        {
                            drDetalle["descuento"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, descuento);
                            drDetalle["precioTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, precioTotalSinImp);
                            drDetalle["precioUnitario"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, precioUnitario);
                        }
                        else
                        {
                            drDetalle["descuento"] = item.Descuento;
                            drDetalle["precioTotalSinImpuesto"] = item.PrecioTotalSinImpuesto;
                            drDetalle["precioUnitario"] = item.PrecioUnitario;
                        }
                        tblDetalleLiquidacion.Rows.Add(drDetalle);
                    }
                    #endregion
                    #region Informacion Adicional
                    DataRow drInfoAdicional;
                    foreach (InformacionAdicional item in objRideLiquidacion._infosAdicional)
                    {
                        drInfoAdicional = tblInformacionAdicional.NewRow();
                        drInfoAdicional["nombre"] = item.Nombre;
                        drInfoAdicional["valor"] = item.Valor;
                        tblInformacionAdicional.Rows.Add(drInfoAdicional);
                    }
                    #endregion
                    #region Forma de Pago
                    DataRow drFormaPago;
                    if (validaAgregacionFormaPago)
                    {
                        #region Cuando la compañia tiene activada la forma de pago
                        foreach (FormaPago item in objRideLiquidacion._infoLiquidacion._formasPago)
                        {
                            drFormaPago = tblFormaPago.NewRow();
                            decimal total = Convert.ToDecimal(item.Total.Replace('.', ','));
                            if (validaFormaPago)
                            {
                                CatalogoReporte objFormaPago = listadoFormasPago.Find(x => x.Codigo == item.CodigoFormaPago);
                                drFormaPago["formaPago"] = objFormaPago.Valor;
                            }
                            else
                                drFormaPago["formaPago"] = item.CodigoFormaPago;
                            drFormaPago["plazo"] = item.Plazo;
                            if (validaSeparadorMiles)
                                drFormaPago["total"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, total);
                            else
                                drFormaPago["total"] = item.Total;
                            drFormaPago["unidadTiempo"] = item.UnidadTiempo;
                            tblFormaPago.Rows.Add(drFormaPago);
                        }
                        #endregion
                    }
                    #endregion

                    #region Monto de la Factura

                    DataRow dr_tblInformacionMonetaria = tblInformacionMonetaria.NewRow();

                    dr_tblInformacionMonetaria["subTotalIva"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalIva5"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalCero"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalExcentoIva"] = "0.00";
                    dr_tblInformacionMonetaria["descuento"] = "0.00";
                    dr_tblInformacionMonetaria["iva"] = "0.00";
                    dr_tblInformacionMonetaria["iva5"] = "0.00";
                    dr_tblInformacionMonetaria["INBPNR"] = "0.00";
                    dr_tblInformacionMonetaria["propina"] = "0.00";
                    dr_tblInformacionMonetaria["valor"] = "0.00";
                    dr_tblInformacionMonetaria["ice"] = "0.00";

                    foreach (TotalConImpuesto objfactTotalImpuesto in objRideLiquidacion._infoLiquidacion._totalesConImpuesto)
                    {
                        if (String.Compare(objfactTotalImpuesto.Codigo.Trim(), CatalogoViaDoc.ParametrosValorIva) == 0)
                        {
                            #region IMPUESTOS_IVA
                            #region SubTotal Cero
                            if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorIVA0) == 0 ||
                                String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorExentoIVA) == 0)
                            {

                                decimal subTotalCero = Convert.ToDecimal(objfactTotalImpuesto.BaseImponible.Replace('.', ','));
                                if (validaSeparadorMiles)
                                    dr_tblInformacionMonetaria["subTotalCero"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalCero);
                                else
                                    dr_tblInformacionMonetaria["subTotalCero"] = objfactTotalImpuesto.BaseImponible.ToString();
                            }
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
                                dr_tblInformacionMonetaria["INBPNR"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, irbpnr);
                            else
                                dr_tblInformacionMonetaria["INBPNR"] = objfactTotalImpuesto.Valor;
                        }
                    }

                    decimal totalSinImpuesto = Convert.ToDecimal(objRideLiquidacion._infoLiquidacion.TotalSinImpuestos.Replace('.', ','));
                    decimal totalDescuento = Convert.ToDecimal(objRideLiquidacion._infoLiquidacion.TotalDescuento.Replace('.', ','));
                    decimal importeTotal = Convert.ToDecimal(objRideLiquidacion._infoLiquidacion.ImporteTotal.Replace('.', ','));

                    if (validaSeparadorMiles)
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalSinImpuesto);
                        dr_tblInformacionMonetaria["descuento"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalDescuento);
                        dr_tblInformacionMonetaria["valor"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, importeTotal);
                    }
                    else
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = objRideLiquidacion._infoLiquidacion.TotalSinImpuestos;
                        dr_tblInformacionMonetaria["descuento"] = objRideLiquidacion._infoLiquidacion.TotalDescuento;
                        dr_tblInformacionMonetaria["valor"] = objRideLiquidacion._infoLiquidacion.ImporteTotal;
                    }
                    tblInformacionMonetaria.Rows.Add(dr_tblInformacionMonetaria);
                    #endregion

                    ReportDataSource datos_tblInfoTributaria = new ReportDataSource("tblInformacionTributaria", tblInfoTributaria);
                    ReportDataSource datos_tblInfoLiquidacion = new ReportDataSource("tblInformacionFactura", tblInfoLiquidacion);
                    ReportDataSource datos_tblDetalleLiquidacion = new ReportDataSource("tblDetalleFactura", tblDetalleLiquidacion);
                    ReportDataSource datos_tblInformacionAdicional = new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional);
                    ReportDataSource datos_tblFormaPago = new ReportDataSource("tblFormaPago", tblFormaPago);
                    ReportDataSource datos_tblInformacionMonetaria = new ReportDataSource("tblInformacionMonetaria", tblInformacionMonetaria);

                    localReport.EnableExternalImages = true;
                    localReport.SetParameters(parameters);
                    localReport.DataSources.Add(datos_tblInfoTributaria);
                    localReport.DataSources.Add(datos_tblInfoLiquidacion);
                    localReport.DataSources.Add(datos_tblDetalleLiquidacion);
                    localReport.DataSources.Add(datos_tblInformacionAdicional);
                    localReport.DataSources.Add(datos_tblFormaPago);
                    localReport.DataSources.Add(datos_tblInformacionMonetaria);

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
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
                mensajeError = "Error al generar del byte[] para elo ride Factura. Mensaje Error: La compania " + objRideLiquidacion._infoTributaria.RazonSocial + " no tiene registrada las configuraciones del reporte";
                bytePDF = null;
            }

            return bytePDF;
        }
    }
}
