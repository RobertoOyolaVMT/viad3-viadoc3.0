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

namespace ReportesViaDoc.LogicaReporte
{
    public class ProcesarRideFactura
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
        public RideFactura ProcesarXmlAutorizadoFactura(ref string mensajeError, string xmlDocumentoAutorizado, string fechaHoraAutorizacion, string numeroAutorizacion)
        {
            mensajeError = "";
            RideFactura objRideFactura = new RideFactura();
            System.IO.StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlDocumentoAutorizado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml;

                XmlNodeList CamposAutorizacion = document.SelectNodes("autorizacion");
                XmlNode Informacion = CamposAutorizacion.Item(0);
                if (Informacion != null)
                {
                    objRideFactura.BanderaGeneracionObjeto = true;
                    objRideFactura.NumeroAutorizacion = numeroAutorizacion;
                    objRideFactura.FechaHoraAutorizacion = fechaHoraAutorizacion;

                    string NumeroAutorizacion = Informacion.SelectSingleNode("numeroAutorizacion").InnerText;
                    string FechaAutorizacion = Informacion.SelectSingleNode("fechaAutorizacion").InnerText;
                    string xmlComprobante = Informacion.SelectSingleNode("comprobante").InnerText;
                    document.LoadXml(xmlComprobante);
                    document.WriteTo(xtr);


                    XmlNodeList camposXML_infoTributaria = document.SelectNodes("factura/infoTributaria");
                    XmlNodeList camposXML_infoFactura = document.SelectNodes("factura/infoFactura");
                    XmlNodeList camposXML_detalles = document.SelectNodes("factura/detalles");

                    if (camposXML_infoTributaria.Count > 0 && camposXML_infoFactura.Count > 0 && camposXML_detalles.Count > 0)
                    {
                        XmlNodeList camposXML_infoAdicional = document.SelectNodes("factura/infoAdicional");
                        if (camposXML_infoAdicional.Count > 0)
                        {
                            #region Informacion Adicional de la Factura
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
                            objRideFactura._infosAdicional = listaInfoAdicional;
                            #endregion
                        }

                        #region Informacion tributaria de la factura
                        XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                        objRideFactura._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                        objRideFactura._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                        objRideFactura._infoTributaria.NombreComercial = "";
                        try { objRideFactura._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText; }
                        catch (Exception ex) { objRideFactura._infoTributaria.NombreComercial = ""; }
                        objRideFactura._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                        objRideFactura._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                        objRideFactura._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                        objRideFactura._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                        objRideFactura._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                        objRideFactura._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                        objRideFactura._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;
                        try
                        {
                            objRideFactura._infoTributaria.RegimenMicroempresa = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                        }
                        catch (Exception)
                        {
                            objRideFactura._infoTributaria.RegimenMicroempresa = "";
                        }

                        try
                        {
                            objRideFactura._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                        }
                        catch (Exception)
                        {
                            objRideFactura._infoTributaria.AgenteRetencion = "";
                        }

                        objRideFactura._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;
                        objRideFactura._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                        #endregion
                        #region Informacion de la factura

                        XmlNode tagInfoFactura = camposXML_infoFactura.Item(0);
                        objRideFactura._infoFactura.FechaEmision = tagInfoFactura.SelectSingleNode("fechaEmision").InnerText;
                        try
                        {
                            objRideFactura._infoFactura.DirEstablecimiento = tagInfoFactura.SelectSingleNode("dirEstablecimiento").InnerText; ;
                        }
                        catch (Exception ex)
                        {
                            objRideFactura._infoFactura.DirEstablecimiento = "";
                        };

                        try { objRideFactura._infoFactura.NumeroContribuyenteEspecial = tagInfoFactura.SelectSingleNode("contribuyenteEspecial").InnerText; }
                        catch (Exception ex)
                        {
                            objRideFactura._infoFactura.NumeroContribuyenteEspecial = "";

                        }
                        objRideFactura._infoFactura.ObligadoContabilidad = tagInfoFactura.SelectSingleNode("obligadoContabilidad").InnerText;
                        objRideFactura._infoFactura.TipoIdentificacion = tagInfoFactura.SelectSingleNode("tipoIdentificacionComprador").InnerText;
                        var razonSocialComprador = tagInfoFactura.SelectSingleNode("razonSocialComprador").InnerText;
                        if (razonSocialComprador.Contains("&"))
                        {
                            objRideFactura._infoFactura.RazonSocial = razonSocialComprador.Replace("&","");
                        }
                        else
                        {
                            objRideFactura._infoFactura.RazonSocial = razonSocialComprador;
                        }
                        try { objRideFactura._infoFactura.GuiaRemision = tagInfoFactura.SelectSingleNode("guiaRemision").InnerText; }
                        catch (Exception ex)
                        {
                            objRideFactura._infoFactura.GuiaRemision = "";

                        }
                        objRideFactura._infoFactura.Identificacion = tagInfoFactura.SelectSingleNode("identificacionComprador").InnerText;
                        objRideFactura._infoFactura.TotalSinImpuestos = tagInfoFactura.SelectSingleNode("totalSinImpuestos").InnerText;
                        objRideFactura._infoFactura.TotalDescuento = tagInfoFactura.SelectSingleNode("totalDescuento").InnerText;

                        string xmlTotalConImpuestos = "<totalConImpuestos>" + tagInfoFactura.SelectSingleNode("totalConImpuestos").InnerXml + "</totalConImpuestos>";  //<-- AQUIII

                        objRideFactura._infoFactura.Propina = tagInfoFactura.SelectSingleNode("propina").InnerText;
                        objRideFactura._infoFactura.ImporteTotal = tagInfoFactura.SelectSingleNode("importeTotal").InnerText;
                        objRideFactura._infoFactura.Moneda = tagInfoFactura.SelectSingleNode("moneda").InnerText;
                        string xmlFormaPago = "";
                        try { xmlFormaPago = "<FormaPagos>" + tagInfoFactura.SelectSingleNode("pagos").InnerXml + "</FormaPagos>"; }
                        catch (Exception ex)
                        {
                            xmlFormaPago = "";

                        }
                        #region Totales con impuesto de la factura


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
                        objRideFactura._infoFactura._totalesConImpuesto = listaTotalConImpuesto;
                        #endregion
                        #region Formas de Pago de la Factura

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
                            objRideFactura._infoFactura._formasPago = listaFormaPago;
                        }
                        #endregion

                        #endregion
                        #region Detalle de la Factura
                        List<Detalle> detallesFactura = new List<Detalle>();
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
                                detallesFactura.Add(detalle);
                            }
                        }
                        objRideFactura._detalles = detallesFactura;
                        #endregion
                    }
                }
                else
                {
                    objRideFactura = ProcesarXMLFirmadoFactura(ref mensajeError, xmlDocumentoAutorizado);
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Genera Reade Facturas" +ex.Message);
                objRideFactura.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml autorizado de la Factura. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
            }
            return objRideFactura;
        }

        /// <summary>
        /// Autor: William Jacome Choez(Viamatica S.A.)
        /// Descripcion: Desarmar el xml firmado de la factura para generar un objeto para procesar el byte[] del documento.
        /// Fecha: 08/05/2017
        /// </summary>
        /// <param name="mensajeError">Mensaje del error generado en el proceso</param>
        /// <param name="xmlDocumentoFirmado">Xml firmado de la factura</param>
        /// <returns></returns>
        public RideFactura ProcesarXMLFirmadoFactura(ref string mensajeError, string xmlDocumentoFirmado)
        {
            mensajeError = "";
            RideFactura objRideFactura = new RideFactura();
            System.IO.StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);

            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlDocumentoFirmado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml;

                XmlNodeList camposXML_infoTributaria = document.SelectNodes("factura/infoTributaria");
                XmlNodeList camposXML_infoFactura = document.SelectNodes("factura/infoFactura");
                XmlNodeList camposXML_detalles = document.SelectNodes("factura/detalles");

                if (camposXML_infoTributaria.Count > 0 && camposXML_infoFactura.Count > 0 && camposXML_detalles.Count > 0)
                {
                    objRideFactura.BanderaGeneracionObjeto = true;
                    XmlNodeList camposXML_infoAdicional = document.SelectNodes("factura/infoAdicional");
                    if (camposXML_infoAdicional.Count > 0)
                    {
                        #region Informacion Adicional de la Factura
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
                        objRideFactura._infosAdicional = listaInfoAdicional;
                        #endregion
                    }

                    #region Informacion tributaria de la factura
                    XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                    objRideFactura._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                    objRideFactura._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                    try { objRideFactura._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText; } catch (Exception ex) { objRideFactura._infoTributaria.NombreComercial = ""; }
                    objRideFactura._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                    objRideFactura.NumeroAutorizacion = informacionXML.SelectSingleNode("claveAcceso").InnerText;   //se agrego esto Joseph
                    objRideFactura._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                    objRideFactura._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                    objRideFactura._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                    objRideFactura._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                    objRideFactura._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                    objRideFactura._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                    try
                    {
                        objRideFactura._infoTributaria.RegimenMicroempresa = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                    }
                    catch (Exception)
                    {
                        objRideFactura._infoTributaria.RegimenMicroempresa = "";
                    }

                    try
                    {
                        objRideFactura._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                    }
                    catch (Exception)
                    {
                        objRideFactura._infoTributaria.AgenteRetencion = "";
                    }

                    objRideFactura._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                    #endregion
                    #region Informacion de la facturaS
                    XmlNode tagInfoFactura = camposXML_infoFactura.Item(0);
                    objRideFactura._infoFactura.FechaEmision = tagInfoFactura.SelectSingleNode("fechaEmision").InnerText;
                    objRideFactura._infoFactura.DirEstablecimiento = tagInfoFactura.SelectSingleNode("dirEstablecimiento").InnerText;
                    try { objRideFactura._infoFactura.GuiaRemision = tagInfoFactura.SelectSingleNode("guiaRemision").InnerText; } catch (Exception ex) { objRideFactura._infoFactura.GuiaRemision = ""; }
                    try { objRideFactura._infoFactura.NumeroContribuyenteEspecial = tagInfoFactura.SelectSingleNode("contribuyenteEspecial").InnerText; } catch (Exception ex) { objRideFactura._infoFactura.NumeroContribuyenteEspecial = ""; }

                    objRideFactura._infoFactura.ObligadoContabilidad = tagInfoFactura.SelectSingleNode("obligadoContabilidad").InnerText;
                    objRideFactura._infoFactura.TipoIdentificacion = tagInfoFactura.SelectSingleNode("tipoIdentificacionComprador").InnerText;
                    var razonSocialComprador = tagInfoFactura.SelectSingleNode("razonSocialComprador").InnerText;
                    if (razonSocialComprador.Contains("&"))
                    {
                        objRideFactura._infoFactura.RazonSocial = razonSocialComprador.Replace("&", "");
                    }
                    else
                    {
                        objRideFactura._infoFactura.RazonSocial = razonSocialComprador;
                    }
                    objRideFactura._infoFactura.Identificacion = tagInfoFactura.SelectSingleNode("identificacionComprador").InnerText;
                    objRideFactura._infoFactura.TotalSinImpuestos = tagInfoFactura.SelectSingleNode("totalSinImpuestos").InnerText;
                    objRideFactura._infoFactura.TotalDescuento = tagInfoFactura.SelectSingleNode("totalDescuento").InnerText;

                    //string xmlTotalConImpuestos = tagInfoFactura.SelectSingleNode("totalConImpuestos").InnerXml;
                    string xmlTotalConImpuestos = "<totalConImpuestos>" + tagInfoFactura.SelectSingleNode("totalConImpuestos").InnerXml + "</totalConImpuestos>";

                    objRideFactura._infoFactura.Propina = tagInfoFactura.SelectSingleNode("propina").InnerText;
                    objRideFactura._infoFactura.ImporteTotal = tagInfoFactura.SelectSingleNode("importeTotal").InnerText;
                    objRideFactura._infoFactura.Moneda = tagInfoFactura.SelectSingleNode("moneda").InnerText;
                    string xmlFormaPago = "";
                    try { xmlFormaPago = "<FormaPagos>" + tagInfoFactura.SelectSingleNode("pagos").InnerXml + "</FormaPagos>"; } catch (Exception ex) { xmlFormaPago = ""; }

                    #region Totales con impuesto de la factura
                    document.LoadXml(xmlTotalConImpuestos);
                    document.WriteTo(xtr);
                    //XmlNodeList camposXML_totalConImpuesto = document.SelectNodes("totalImpuesto");
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
                    objRideFactura._infoFactura._totalesConImpuesto = listaTotalConImpuesto;
                    #endregion
                    #region Formas de Pago de la Factura
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
                        objRideFactura._infoFactura._formasPago = listaFormaPago;
                    }
                    #endregion

                    #endregion
                    #region Detalle de la Factura
                    List<Detalle> detallesFactura = new List<Detalle>();
                    foreach (XmlNode tagDetalles in camposXML_detalles)
                    {
                        XmlNodeList nodos = tagDetalles.ChildNodes;
                        foreach (XmlNode nodo in nodos)
                        {
                            Detalle detalle = new Detalle();
                            detalle.CodigoPrincipal = nodo.SelectSingleNode("codigoPrincipal").InnerText;
                            try { detalle.CodigoAuxiliar = nodo.SelectSingleNode("codigoAuxiliar").InnerText; } catch (Exception) { detalle.CodigoAuxiliar = ""; }
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
                            detallesFactura.Add(detalle);
                        }
                    }
                    objRideFactura._detalles = detallesFactura;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                objRideFactura.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml firmado de la Factura. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
            }

            return objRideFactura;

        }

        /// <summary>
        /// Autor: William Jacome Choez(Viamatica .S.A)
        /// Descripcion: Genera el array de byte del RIDE de la Factura
        /// </summary>
        /// <param name="mensajeError"></param>
        /// <param name="objRideFactura"></param>
        /// <param name="configuraciones"></param>
        /// <param name="catalogoSistema"></param>
        /// <returns></returns>
        public Byte[] GenerarRideFactura(ref string mensajeError, RideFactura objRideFactura, List<ConfiguracionReporte> configuraciones, List<CatalogoReporte> catalogoSistema)
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
                List<ConfiguracionReporte> confCompania = configuraciones.FindAll(x => x.RucCompania == objRideFactura._infoTributaria.Ruc);
                if (confCompania != null)
                {
                    #region Origen Datos del RIDE Factura
                    DataTable tblInfoTributaria = new DataTable();
                    DataTable tblInfoFactura = new DataTable();
                    DataTable tblDetalleFactura = new DataTable();
                    DataTable tblInformacionAdicional = new DataTable();
                    DataTable tblInformacionMonetaria = new DataTable();
                    DataTable tblFormaPago = new DataTable();
                    tblInfoTributaria.TableName = "tblInformacionTributaria";
                    tblInfoFactura.TableName = "tblInformacionFactura";
                    tblDetalleFactura.TableName = "tblDetalleFactura";
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
                        new DataColumn("estab",typeof(string)),
                        new DataColumn("nombreComercial",typeof(string)),
                        new DataColumn("ptoEmi",typeof(string)),
                        new DataColumn("razonSocial",typeof(string)),
                        new DataColumn("ruc",typeof(string)),
                        new DataColumn("secuencial",typeof(string)),
                        new DataColumn("tipoEmision",typeof(string))
                    };

                    DataColumn[] cols_tblInfoFactura = new DataColumn[] {
                        new DataColumn("contribuyenteEspecial",typeof(string)),
                        new DataColumn("dirEstablecimiento",typeof(string)),
                        new DataColumn("fechaEmision",typeof(string)),
                        new DataColumn("guiaRemision",typeof(string)),
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
                        new DataColumn("subTotalCero",typeof(string)),
                        new DataColumn("subTotalNoObjetoIva",typeof(string)),
                        new DataColumn("subTotalSinImpuesto",typeof(string)),
                        new DataColumn("subTotalExcentoIva",typeof(string)),
                        new DataColumn("descuento",typeof(string)),
                        new DataColumn("iva",typeof(string)),
                        new DataColumn("INBPNR",typeof(string)),
                        new DataColumn("propina",typeof(string)),
                        new DataColumn("valor",typeof(string)),
                        new DataColumn("ice",typeof(string))
                    };


                    tblInfoTributaria.Columns.AddRange(cols_tblInfoTributaria);
                    tblInfoFactura.Columns.AddRange(cols_tblInfoFactura);
                    tblDetalleFactura.Columns.AddRange(cols_tblDetalleFactura);
                    tblInformacionAdicional.Columns.AddRange(cols_tblInformacionAdicional);
                    tblFormaPago.Columns.AddRange(cols_tblFormaPago);
                    tblInformacionMonetaria.Columns.AddRange(cols_tblInformacionMonetaria);
                    #endregion

                    #region Logo de la Compania
                    pathLogoEmpresa = CatalogoViaDoc.rutaLogoCompania;
                    string rutaImagen = pathLogoEmpresa + objRideFactura._infoTributaria.Ruc.Trim() + ".png";
                    #endregion

                    #region Genera Codigo Barra de la Factura  MEJORAR
                    byte[] imgBar;
                    Code.BarcodeGenerator bgCode128 = new Code.BarcodeGenerator();
                    Code.Convertir cCode = new Code.Convertir();
                    Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                    StringFormat fi = new StringFormat(StringFormatFlags.NoClip);
                    fi.Alignment = StringAlignment.Center;
                    imgBar = RetornarXml.ImageToByte2(Code128Rendering.MakeBarcodeImage(objRideFactura._infoTributaria.ClaveAcceso, 3, true));
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("INgreso nuevo 1");
                    pathCodBarra = CatalogoViaDoc.rutaCodigoBarra + objRideFactura._infoTributaria.ClaveAcceso.Trim();
                    pathRider = CatalogoViaDoc.rutaRide + objRideFactura._infoTributaria.Ruc.Trim() + "\\" + objRideFactura._infoTributaria.CodigoDocumento.Trim();
                    if (!Directory.Exists(pathRider))
                    {
                        Directory.CreateDirectory(pathRider);
                    }
                    File.WriteAllBytes(pathCodBarra + ".jpg", imgBar);
                    #endregion
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("INgreso nuevo 2");

                    #region Configuracion de los separadores de miles de la Factura MEJORAR
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
                                    string[] fechaEmisionDocumento = objRideFactura._infoFactura.FechaEmision.Split('/');
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
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("INgreso nuevo 3");

                    LocalReport localReport = new LocalReport();
                    localReport.ReportPath = CatalogoViaDoc.rutaRidePlantilla + "RideFactura.rdlc";
                    ReportParameter[] parameters = new ReportParameter[10];
                    parameters[0] = new ReportParameter("txFechaAutorizacion", objRideFactura.FechaHoraAutorizacion);
                    parameters[1] = new ReportParameter("txNumeroAutorizacion", objRideFactura._infoTributaria.ClaveAcceso);// Numero autorizacion es el mismo 
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
                        CatalogoReporte tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == objRideFactura._infoTributaria.Ambiente);
                        drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                    }
                    else
                        drInfoTributaria["ambiente"] = objRideFactura._infoTributaria.Ambiente;
                    drInfoTributaria["claveAcceso"] = objRideFactura._infoTributaria.ClaveAcceso;
                    drInfoTributaria["codDoc"] = objRideFactura._infoTributaria.CodigoDocumento;
                    drInfoTributaria["dirMatriz"] = objRideFactura._infoTributaria.DirMatriz;
                    try { drInfoTributaria["agenteRetencion"] = objRideFactura._infoTributaria.AgenteRetencion.Equals(null) ? "" : CatalogoViaDoc.LeyendaAgente.Trim(); } catch { drInfoTributaria["agenteRetencion"] = ""; }
                    try { drInfoTributaria["regimenMicroempresas"] = objRideFactura._infoTributaria.RegimenMicroempresa; } catch { drInfoTributaria["regimenMicroempresas"] = ""; }
                    drInfoTributaria["estab"] = objRideFactura._infoTributaria.Establecimiento;
                    drInfoTributaria["nombreComercial"] = objRideFactura._infoTributaria.NombreComercial;
                    drInfoTributaria["ptoEmi"] = objRideFactura._infoTributaria.PuntoEmision;
                    drInfoTributaria["razonSocial"] = objRideFactura._infoTributaria.RazonSocial;
                    drInfoTributaria["ruc"] = objRideFactura._infoTributaria.Ruc;
                    drInfoTributaria["secuencial"] = objRideFactura._infoTributaria.Secuencial;
                    if (validaEmisionFactura)
                    {
                        CatalogoReporte tipoemision = emisionesFacturacion.Find(x => x.Codigo == objRideFactura._infoTributaria.TipoEmision);
                        drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                    }
                    else
                        drInfoTributaria["tipoEmision"] = objRideFactura._infoTributaria.TipoEmision;
                    tblInfoTributaria.Rows.Add(drInfoTributaria);
                    #endregion
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("INgreso nuevo 6");
                    #region Datos de la Factura
                    DataRow drInfoFactura = tblInfoFactura.NewRow();
                    drInfoFactura["dirEstablecimiento"] = objRideFactura._infoFactura.DirEstablecimiento;
                    drInfoFactura["fechaEmision"] = objRideFactura._infoFactura.FechaEmision;
                    drInfoFactura["guiaRemision"] = "";
                    if (validaNumeroContribuyenteEspecial)
                        drInfoFactura["contribuyenteEspecial"] = objRideFactura._infoFactura.NumeroContribuyenteEspecial;
                    else
                        drInfoFactura["contribuyenteEspecial"] = "";
                    drInfoFactura["identificacionComprador"] = objRideFactura._infoFactura.Identificacion;
                    drInfoFactura["importeTotal"] = objRideFactura._infoFactura.ImporteTotal;
                    drInfoFactura["moneda"] = objRideFactura._infoFactura.Moneda;
                    drInfoFactura["obligadoContabilidad"] = objRideFactura._infoFactura.ObligadoContabilidad;
                    drInfoFactura["propina"] = objRideFactura._infoFactura.Propina;
                    drInfoFactura["razonSocialComprador"] = objRideFactura._infoFactura.RazonSocial;
                    drInfoFactura["tipoIdentificacionComprador"] = objRideFactura._infoFactura.TipoIdentificacion;
                    drInfoFactura["totalDescuento"] = objRideFactura._infoFactura.TotalDescuento;
                    drInfoFactura["totalSinImpuestos"] = objRideFactura._infoFactura.TotalSinImpuestos;
                    tblInfoFactura.Rows.Add(drInfoFactura);
                    #endregion
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("INgreso nuevo 7");
                    #region Detalle de la factura
                    DataRow drDetalle;
                    foreach (Detalle item in objRideFactura._detalles)
                    {
                        drDetalle = tblDetalleFactura.NewRow();
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
                        tblDetalleFactura.Rows.Add(drDetalle);
                    }
                    #endregion
                    #region Informacion Adicional
                    DataRow drInfoAdicional;
                    foreach (InformacionAdicional item in objRideFactura._infosAdicional)
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
                        foreach (FormaPago item in objRideFactura._infoFactura._formasPago)
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
                    dr_tblInformacionMonetaria["subTotalCero"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalExcentoIva"] = "0.00";
                    dr_tblInformacionMonetaria["descuento"] = "0.00";
                    dr_tblInformacionMonetaria["iva"] = "0.00";
                    dr_tblInformacionMonetaria["INBPNR"] = "0.00";
                    dr_tblInformacionMonetaria["propina"] = "0.00";
                    dr_tblInformacionMonetaria["valor"] = "0.00";
                    dr_tblInformacionMonetaria["ice"] = "0.00";
                    foreach (TotalConImpuesto objfactTotalImpuesto in objRideFactura._infoFactura._totalesConImpuesto)
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
                            //decimal irbpnr = Convert.ToDecimal(objfactTotalImcmdpuesto.Valor.Replace('.', ','));
                            //if (validaSeparadorMiles)
                            //    dr_tblInformacionMonetaria["INBPNR"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, irbpnr);
                            //else
                            //    dr_tblInformacionMonetaria["INBPNR"] = "";

                            dr_tblInformacionMonetaria["INBPNR"] = "0.00";
                        }
                    }

                    decimal totalSinImpuesto = Convert.ToDecimal(objRideFactura._infoFactura.TotalSinImpuestos.Replace('.', ','));
                    decimal totalDescuento = Convert.ToDecimal(objRideFactura._infoFactura.TotalDescuento.Replace('.', ','));
                    decimal propina = Convert.ToDecimal(objRideFactura._infoFactura.Propina.Replace('.', ','));
                    decimal importeTotal = Convert.ToDecimal(objRideFactura._infoFactura.ImporteTotal.Replace('.', ','));

                    if (validaSeparadorMiles)
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalSinImpuesto);
                        dr_tblInformacionMonetaria["descuento"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalDescuento);
                        dr_tblInformacionMonetaria["propina"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, propina);
                        dr_tblInformacionMonetaria["valor"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, importeTotal);
                    }
                    else
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = objRideFactura._infoFactura.TotalSinImpuestos;
                        dr_tblInformacionMonetaria["descuento"] = objRideFactura._infoFactura.TotalDescuento;
                        dr_tblInformacionMonetaria["propina"] = objRideFactura._infoFactura.Propina;
                        dr_tblInformacionMonetaria["valor"] = objRideFactura._infoFactura.ImporteTotal;
                    }
                    tblInformacionMonetaria.Rows.Add(dr_tblInformacionMonetaria);
                    #endregion

                    ReportDataSource datos_tblInfoTributaria = new ReportDataSource("tblInformacionTributaria", tblInfoTributaria);
                    ReportDataSource datos_tblInfoFactura = new ReportDataSource("tblInformacionFactura", tblInfoFactura);
                    ReportDataSource datos_tblDetalleFactura = new ReportDataSource("tblDetalleFactura", tblDetalleFactura);
                    ReportDataSource datos_tblInformacionAdicional = new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional);
                    ReportDataSource datos_tblFormaPago = new ReportDataSource("tblFormaPago", tblFormaPago);
                    ReportDataSource datos_tblInformacionMonetaria = new ReportDataSource("tblInformacionMonetaria", tblInformacionMonetaria);

                    localReport.EnableExternalImages = true;
                    localReport.SetParameters(parameters);
                    localReport.DataSources.Add(datos_tblInfoTributaria);
                    localReport.DataSources.Add(datos_tblInfoFactura);
                    localReport.DataSources.Add(datos_tblDetalleFactura);
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
                else
                {
                    mensajeError = "Error al generar del byte[] para elo ride Factura. Mensaje Error: La compania " + objRideFactura._infoTributaria.RazonSocial + " no tiene registrada las configuraciones del reporte";
                    bytePDF = null;
                    //LogReporte.GuardaLog(mensajeError);
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Ride Factura" + ex.Message);
                mensajeError = "Error al generar del byte[] para elo ride Factura. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
                bytePDF = null;
                //LogReporte.GuardaLog(mensajeError);
            }
            return bytePDF;

        }

    }
}