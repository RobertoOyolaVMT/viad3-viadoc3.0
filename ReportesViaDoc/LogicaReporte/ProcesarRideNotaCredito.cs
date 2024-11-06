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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ViaDoc.Configuraciones;
using ViaDoc.Utilitarios;

namespace ReportesViaDoc.LogicaReporte
{
    public class ProcesarRideNotaCredito
    {

        private string pathCodBarra { get; set; }
        private string pathLogoEmpresa { get; set; }
        private String pathRider { get; set; }

        public RideNotaCredito ProcesarXmlAutorizadoNotaCredito(ref string mensajeError, string xmlDocumentoAutorizado, string fechaHoraAutorizacion, string numeroAutorizacion)
        {
            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("ingresa proceso xml");
            ExcepcionesReportes reporte = new ExcepcionesReportes();


            RideNotaCredito objRideNotaCredito = new RideNotaCredito();
            StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            try
            {
                XmlDocument document = new XmlDocument();
                xmlDocumentoAutorizado = Utilitarios.ReemplazarCaracteresEspeciales(xmlDocumentoAutorizado);
                document.LoadXml(xmlDocumentoAutorizado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("obtiene xml sub0" + mensajeError);
                XmlNodeList CamposAutorizacion = document.SelectNodes("autorizacion");
                XmlNode Informacion = CamposAutorizacion.Item(0);
                if (Informacion != null)
                {

                    objRideNotaCredito.BanderaGeneracionObjeto = true;
                    objRideNotaCredito.NumeroAutorizacion = numeroAutorizacion;
                    objRideNotaCredito.FechaHoraAutorizacion = fechaHoraAutorizacion;
                    string NumeroAutorizacion = Informacion.SelectSingleNode("numeroAutorizacion").InnerText;
                    string FechaAutorizacion = Informacion.SelectSingleNode("fechaAutorizacion").InnerText;
                    string xmlComprobante = Informacion.SelectSingleNode("comprobante").InnerText;
                    document.LoadXml(xmlComprobante);
                    document.WriteTo(xtr);

                    XmlNodeList camposXML_infoTributaria = document.SelectNodes("notaCredito/infoTributaria");
                    XmlNodeList camposXML_infoNotaCredito = document.SelectNodes("notaCredito/infoNotaCredito");
                    XmlNodeList camposXML_detalles = document.SelectNodes("notaCredito/detalles");
                    
                    if (camposXML_infoTributaria.Count > 0 && camposXML_infoNotaCredito.Count > 0 && camposXML_detalles.Count > 0)
                    {
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("1");
                        XmlNodeList camposXML_infoAdicional = document.SelectNodes("notaCredito/infoAdicional");
                        if (camposXML_infoAdicional.Count > 0)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("2");
                            #region Informacion Adicional de la nota de credito
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
                            objRideNotaCredito._infoAdicional = listaInfoAdicional;
                            #endregion
                        }

                        #region Detalle de la Nota Credito
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("3");
                        List<Detalle> detallesFactura = new List<Detalle>();
                        foreach (XmlNode tagDetalles in camposXML_detalles)
                        {
                            XmlNodeList nodos = tagDetalles.ChildNodes;
                            foreach (XmlNode nodo in nodos)
                            {
                                Detalle detalle = new Detalle();
                                detalle.CodigoPrincipal = nodo.SelectSingleNode("codigoInterno").InnerText;
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
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("4");
                                detalle._impuestos = impuestosDetalle;
                                detallesFactura.Add(detalle);
                            }
                        }
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("5");
                        objRideNotaCredito._detalles = detallesFactura;
                        #endregion
                        #region Informacion tributaria de la Nota de credito
                        XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                        objRideNotaCredito._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                        objRideNotaCredito._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                        objRideNotaCredito._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText;
                        objRideNotaCredito._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                        objRideNotaCredito._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                        objRideNotaCredito._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                        objRideNotaCredito._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                        objRideNotaCredito._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                        objRideNotaCredito._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                        objRideNotaCredito._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                        try
                        {
                            objRideNotaCredito._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
                        }
                        catch (Exception)
                        {
                            if (!CatalogoViaDoc.LeyendaRegimen.Equals(""))
                            {
                                try
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("6");
                                    if (!ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").ToString().Trim().Equals(""))
                                    {
                                        string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Split('|');
                                        foreach (string Ruc in Cadena)
                                        {
                                            if (!Ruc.Equals(""))
                                            {
                                                if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("7");
                                                        if (validaLeyenda)
                                                            objRideNotaCredito._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideNotaCredito._infoTributaria.contribuyenteRimpe = "";
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
                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("8");
                                        string[] Cadena = ConfigurationManager.AppSettings.Get("contribuyente_Rimpe").Split('|');
                                        foreach (string Ruc in Cadena)
                                        {
                                            if (!Ruc.Equals(""))
                                            {
                                                if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                                {
                                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("9");
                                                    bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                    if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                    {
                                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("10");
                                                        if (validaLeyenda)
                                                            objRideNotaCredito._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                    }
                                                    else
                                                    {
                                                        objRideNotaCredito._infoTributaria.contribuyenteRimpe = "";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                objRideNotaCredito._infoTributaria.contribuyenteRimpe = "";
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        objRideNotaCredito._infoTributaria.contribuyenteRimpe = "";
                                    }

                                }
                            }
                        }

                        try
                        {
                            if (informacionXML.SelectSingleNode("agenteRetencion").InnerText.Equals("1"))
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("10");
                                if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                                {
                                    string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                                    foreach (string Ruc in Cadena)
                                    {
                                        if (!Ruc.Equals(""))
                                        {
                                            if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("11");
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideNotaCredito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                                }
                                                else
                                                {
                                                    objRideNotaCredito._infoTributaria.AgenteRetencion = " ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("12");
                                objRideNotaCredito._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("13");
                                string[] Cadena = ConfigurationManager.AppSettings.Get("Agente_de_Retención").Split('|');
                                foreach (string Ruc in Cadena)
                                {
                                    if (!Ruc.Equals(""))
                                    {
                                        if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideNotaCredito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objRideNotaCredito._infoTributaria.AgenteRetencion = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        objRideNotaCredito._infoTributaria.AgenteRetencion = "";
                                    }
                                }
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("13");
                            }
                        }
                        try
                        {
                            if (informacionXML.SelectSingleNode("regimenMicroempresas").InnerText.Equals(""))
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("14");
                                if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                                {
                                    string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                                    foreach (string Ruc in Cadena)
                                    {
                                        if (!Ruc.Equals(""))
                                        {
                                            if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideNotaCredito._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaAgente.Trim();
                                                }
                                                else
                                                {
                                                    objRideNotaCredito._infoTributaria.regimenMicroempresas = " ";
                                                }
                                            }
                                        }
                                    }
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("15");
                                }
                            }
                            else
                            {
                                objRideNotaCredito._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!CatalogoViaDoc.LeyendaAgente.Equals(""))
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("16");
                                string[] Cadena = ConfigurationManager.AppSettings.Get("regimen_Microempresas").Split('|');
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("16_1");
                                foreach (string Ruc in Cadena)
                                {
                                    if (!Ruc.Equals(""))
                                    {
                                        if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimen.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaRegimen.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideNotaCredito._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen.Trim();
                                            }
                                            else
                                            {
                                                objRideNotaCredito._infoTributaria.regimenMicroempresas = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        objRideNotaCredito._infoTributaria.regimenMicroempresas = "";
                                    }
                                }
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("17");
                            }
                        }
                        objRideNotaCredito._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                        #endregion
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("18");
                        #region Informacion de la Nota de Credito
                        XmlNode tagInfoNotaCredito = camposXML_infoNotaCredito.Item(0);
                        objRideNotaCredito._infoNotaCredito.FechaEmision = tagInfoNotaCredito.SelectSingleNode("fechaEmision").InnerText;
                        objRideNotaCredito._infoNotaCredito.DirEstablecimiento = tagInfoNotaCredito.SelectSingleNode("dirEstablecimiento").InnerText;
                        try { objRideNotaCredito._infoNotaCredito.NumeroContribuyenteEspecial = tagInfoNotaCredito.SelectSingleNode("contribuyenteEspecial").InnerText; } catch (Exception ex) { objRideNotaCredito._infoNotaCredito.NumeroContribuyenteEspecial = ""; }
                        objRideNotaCredito._infoNotaCredito.ObligadoContabilidad = tagInfoNotaCredito.SelectSingleNode("obligadoContabilidad").InnerText;
                        objRideNotaCredito._infoNotaCredito.TipoIdentificacion = tagInfoNotaCredito.SelectSingleNode("tipoIdentificacionComprador").InnerText;
                        objRideNotaCredito._infoNotaCredito.RazonSocial = tagInfoNotaCredito.SelectSingleNode("razonSocialComprador").InnerText;
                        objRideNotaCredito._infoNotaCredito.Identificacion = tagInfoNotaCredito.SelectSingleNode("identificacionComprador").InnerText;
                        objRideNotaCredito._infoNotaCredito.TotalSinImpuestos = tagInfoNotaCredito.SelectSingleNode("totalSinImpuestos").InnerText;
                        objRideNotaCredito._infoNotaCredito.CodDocModificado = tagInfoNotaCredito.SelectSingleNode("codDocModificado").InnerText;
                        objRideNotaCredito._infoNotaCredito.FechaEmisionDocSustento = tagInfoNotaCredito.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                        objRideNotaCredito._infoNotaCredito.NumDocModificado = tagInfoNotaCredito.SelectSingleNode("numDocModificado").InnerText;
                        objRideNotaCredito._infoNotaCredito.ValorModificacion = tagInfoNotaCredito.SelectSingleNode("valorModificacion").InnerText;
                        objRideNotaCredito._infoNotaCredito.Motivo = tagInfoNotaCredito.SelectSingleNode("motivo").InnerText;

                        string xmlTotalConImpuestos = "<totalConImpuestos>" + tagInfoNotaCredito.SelectSingleNode("totalConImpuestos").InnerXml + "</totalConImpuestos>";  //esto se agrego

                        objRideNotaCredito._infoNotaCredito.Moneda = tagInfoNotaCredito.SelectSingleNode("moneda").InnerText;
                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("18");
                        #region Totales con impuesto de la Nota de Credito
                        document.LoadXml(xmlTotalConImpuestos);
                        document.WriteTo(xtr);
                        XmlNodeList camposXML_totalConImpuesto = document.GetElementsByTagName("totalImpuesto");  //esto se agrego

                        List<TotalConImpuesto> listaTotalConImpuesto = new List<TotalConImpuesto>();
                        foreach (XmlNode item in camposXML_totalConImpuesto)
                        {

                            TotalConImpuesto totalConImp = new TotalConImpuesto();
                            totalConImp.Codigo = item.SelectSingleNode("codigo").InnerText;
                            totalConImp.CodigoPorcentaje = item.SelectSingleNode("codigoPorcentaje").InnerText;
                            totalConImp.BaseImponible = item.SelectSingleNode("baseImponible").InnerText;
                            totalConImp.Valor = item.SelectSingleNode("valor").InnerText;
                            listaTotalConImpuesto.Add(totalConImp);
                        }
                        objRideNotaCredito._infoNotaCredito._totalesConImpuesto = listaTotalConImpuesto;
                        #endregion

                        #endregion

                    }
                }
                else
                    objRideNotaCredito = ProcesarXmlFirmadoNotaCredito(ref mensajeError, xmlDocumentoAutorizado);

            }
            catch (Exception ex)
            {
                objRideNotaCredito.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml autorizado de la Factura. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("mensaje Error  PRocesar Ride" + mensajeError);
            }
            return objRideNotaCredito;
        }

        public RideNotaCredito ProcesarXmlFirmadoNotaCredito(ref string mensajeError, string xmlDocumentoFirmado)
        {
            RideNotaCredito objRideNotaCredito = new RideNotaCredito();
            StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            try
            {
                XmlDocument document = new XmlDocument();
                xmlDocumentoFirmado = Utilitarios.ReemplazarCaracteresEspeciales(xmlDocumentoFirmado);

                XmlNodeList camposXML_infoTributaria = document.SelectNodes("notaCredito/infoTributaria");
                XmlNodeList camposXML_infoNotaCredito = document.SelectNodes("notaCredito/infoNotaCredito");
                XmlNodeList camposXML_detalles = document.SelectNodes("notaCredito/detalles");

                if (camposXML_infoTributaria.Count > 0 && camposXML_infoNotaCredito.Count > 0 && camposXML_detalles.Count > 0)
                {
                    objRideNotaCredito.BanderaGeneracionObjeto = true;
                    XmlNodeList camposXML_infoAdicional = document.SelectNodes("notaCredito/infoAdicional");
                    if (camposXML_infoAdicional.Count > 0)
                    {
                        #region Informacion Adicional de la nota de credito
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
                        objRideNotaCredito._infoAdicional = listaInfoAdicional;
                        #endregion
                    }

                    #region Detalle de la Nota Credito
                    List<Detalle> detallesFactura = new List<Detalle>();
                    foreach (XmlNode tagDetalles in camposXML_detalles)
                    {
                        XmlNodeList nodos = tagDetalles.ChildNodes;
                        foreach (XmlNode nodo in nodos)
                        {
                            Detalle detalle = new Detalle();
                            detalle.CodigoPrincipal = nodo.SelectSingleNode("codigoInterno").InnerText;
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
                    objRideNotaCredito._detalles = detallesFactura;
                    #endregion

                    #region Informacion tributaria de la Nota de Credito
                    XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                    objRideNotaCredito._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                    objRideNotaCredito._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                    objRideNotaCredito._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText;
                    objRideNotaCredito._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                    objRideNotaCredito._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                    objRideNotaCredito._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                    objRideNotaCredito._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                    objRideNotaCredito._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                    objRideNotaCredito._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                    objRideNotaCredito._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                    try
                    {
                        objRideNotaCredito._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
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
                                            if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideNotaCredito._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                }
                                                else
                                                {
                                                    objRideNotaCredito._infoTributaria.regimenMicroempresas = "";
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
                                            if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                            {
                                                bool validaLeyenda = !CatalogoViaDoc.LeyendaRegimenRimpe.Trim().Equals("") ? true : false;
                                                if (CatalogoViaDoc.LeyendaRegimenRimpe.Trim() != null)
                                                {
                                                    if (validaLeyenda)
                                                        objRideNotaCredito._infoTributaria.contribuyenteRimpe = CatalogoViaDoc.LeyendaRegimenRimpe.Trim();
                                                }
                                                else
                                                {
                                                    objRideNotaCredito._infoTributaria.contribuyenteRimpe = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            objRideNotaCredito._infoTributaria.contribuyenteRimpe = "";
                                        }
                                    }
                                }
                                catch
                                {
                                    objRideNotaCredito._infoTributaria.contribuyenteRimpe = "";
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
                                        if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                        {
                                            bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                            if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                            {
                                                if (validaLeyenda)
                                                    objRideNotaCredito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                            }
                                            else
                                            {
                                                objRideNotaCredito._infoTributaria.AgenteRetencion = " ";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            objRideNotaCredito._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
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
                                    if (Convert.ToInt64(objRideNotaCredito._infoTributaria.Ruc.Trim()).Equals(Convert.ToInt64(Ruc)))
                                    {
                                        bool validaLeyenda = !CatalogoViaDoc.LeyendaAgente.Trim().Equals("") ? true : false;
                                        if (CatalogoViaDoc.LeyendaAgente.Trim() != null)
                                        {
                                            if (validaLeyenda)
                                                objRideNotaCredito._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente.Trim();
                                        }
                                        else
                                        {
                                            objRideNotaCredito._infoTributaria.AgenteRetencion = "";
                                        }

                                    }
                                }
                                else
                                {
                                    objRideNotaCredito._infoTributaria.AgenteRetencion = "";
                                }
                            }
                        }
                    }
                    #endregion

                    objRideNotaCredito._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;

                    #region Informacion de la Nota de Credito
                    XmlNode tagInfoNotaCredito = camposXML_infoNotaCredito.Item(0);
                    objRideNotaCredito._infoNotaCredito.FechaEmision = tagInfoNotaCredito.SelectSingleNode("fechaEmision").InnerText;
                    objRideNotaCredito._infoNotaCredito.DirEstablecimiento = tagInfoNotaCredito.SelectSingleNode("dirEstablecimiento").InnerText;
                    try { objRideNotaCredito._infoNotaCredito.NumeroContribuyenteEspecial = tagInfoNotaCredito.SelectSingleNode("contribuyenteEspecial").InnerText; } catch (Exception ex) { objRideNotaCredito._infoNotaCredito.NumeroContribuyenteEspecial = ""; }
                    objRideNotaCredito._infoNotaCredito.ObligadoContabilidad = tagInfoNotaCredito.SelectSingleNode("obligadoContabilidad").InnerText;
                    objRideNotaCredito._infoNotaCredito.TipoIdentificacion = tagInfoNotaCredito.SelectSingleNode("tipoIdentificacionComprador").InnerText;
                    objRideNotaCredito._infoNotaCredito.RazonSocial = tagInfoNotaCredito.SelectSingleNode("razonSocialComprador").InnerText;
                    objRideNotaCredito._infoNotaCredito.Identificacion = tagInfoNotaCredito.SelectSingleNode("identificacionComprador").InnerText;
                    objRideNotaCredito._infoNotaCredito.TotalSinImpuestos = tagInfoNotaCredito.SelectSingleNode("totalSinImpuestos").InnerText;
                    objRideNotaCredito._infoNotaCredito.CodDocModificado = tagInfoNotaCredito.SelectSingleNode("codDocModificado").InnerText;
                    objRideNotaCredito._infoNotaCredito.FechaEmisionDocSustento = tagInfoNotaCredito.SelectSingleNode("fechaEmisionDocSustento").InnerText;
                    objRideNotaCredito._infoNotaCredito.NumDocModificado = tagInfoNotaCredito.SelectSingleNode("numDocModificado").InnerText;
                    objRideNotaCredito._infoNotaCredito.ValorModificacion = tagInfoNotaCredito.SelectSingleNode("valorModificacion").InnerText;
                    objRideNotaCredito._infoNotaCredito.Motivo = tagInfoNotaCredito.SelectSingleNode("motivo").InnerText;
                    string xmlTotalConImpuestos = tagInfoNotaCredito.SelectSingleNode("totalConImpuestos").InnerXml;

                    objRideNotaCredito._infoNotaCredito.Moneda = tagInfoNotaCredito.SelectSingleNode("moneda").InnerText;

                    #region Totales con impuesto de la Nota de Credito
                    document.LoadXml(xmlTotalConImpuestos);
                    document.WriteTo(xtr);
                    XmlNodeList camposXML_totalConImpuesto = document.SelectNodes("totalImpuesto");
                    List<TotalConImpuesto> listaTotalConImpuesto = new List<TotalConImpuesto>();
                    foreach (XmlNode item in camposXML_totalConImpuesto)
                    {
                        TotalConImpuesto totalConImp = new TotalConImpuesto();
                        totalConImp.Codigo = item.SelectSingleNode("codigo").InnerText;
                        totalConImp.CodigoPorcentaje = item.SelectSingleNode("codigoPorcentaje").InnerText;
                        totalConImp.BaseImponible = item.SelectSingleNode("baseImponible").InnerText;
                        totalConImp.Valor = item.SelectSingleNode("valor").InnerText;
                        listaTotalConImpuesto.Add(totalConImp);
                    }
                    objRideNotaCredito._infoNotaCredito._totalesConImpuesto = listaTotalConImpuesto;
                    #endregion
                    #endregion
                }
                else
                    mensajeError = "Error al procesar el xml firmado de la Nota de Credito. Mensaje Error: La estructura del xml esta incompleta.";
            }
            catch (Exception ex)
            {
                objRideNotaCredito.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml firmado de la Nota de Credito. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
            }
            return objRideNotaCredito;
        }


        /// <summary>
        /// Autor: William Jacome Choez (Viamatica S.A.)
        /// Descripcion: Genera el byte[] del documento 
        /// </summary>
        /// <param name="mensajeError">Mensaje de Error del proceso</param>
        /// <param name="objRideNotaCredito">Objeto que representa el xml del documento</param>
        /// <param name="configuraciones">Configuraciones del reporte segun la compañia</param>
        /// <param name="catalogoSistema">Catalogos del sistema de Facturacion Electronica</param>
        /// <returns>Arreglo de byte que representa el ride del documento</returns>
        public Byte[] GenerarRideNotaCredito(ref string mensajeError, RideNotaCredito objRideNotaCredito, List<ConfiguracionReporte> configuraciones, List<CatalogoReporte> catalogoSistema)
        {
            ExcepcionesReportes reporte = new ExcepcionesReportes();

            byte[] bytePDF = null;
            string etiquetaContribuyenteEspecial = "";
            string etiquetaTipoIvaGrabadoConPorcentaje = "";
            string etiquetaTipoIvaGrabadoSiPorcentaje = "";

            try
            {
                List<ConfiguracionReporte> confCompania = configuraciones.FindAll(x => x.RucCompania == objRideNotaCredito._infoTributaria.Ruc);
                if (confCompania != null)
                {
                    #region Origen Datos del RIDE Nota de Credito
                    DataTable tblInfoTributaria = new DataTable();
                    DataTable tblInfoNotaCredito = new DataTable();
                    DataTable tblDetalleNotaCredito = new DataTable();
                    DataTable tblInformacionAdicional = new DataTable();
                    DataTable tblInformacionMonetaria = new DataTable();

                    tblInfoTributaria.TableName = "tblInformacionTributaria";
                    tblInfoNotaCredito.TableName = "tblInformacionNotaCredito";
                    tblDetalleNotaCredito.TableName = "tblDetalle";
                    tblInformacionAdicional.TableName = "tblInformacionAdicional";
                    tblInformacionMonetaria.TableName = "tblInformacionMonetaria";

                    DataColumn[] cols_tblInfoTributaria = new DataColumn[]
                    {
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
                        new DataColumn("tipoEmision",typeof(string))
                    };

                    DataColumn[] cols_tblInfoNotaCredito = new DataColumn[]
                    {
                        new DataColumn("codDocModificado",typeof(string)),
                        new DataColumn("contribuyenteEspecial",typeof(string)),
                        new DataColumn("dirEstablecimiento",typeof(string)),
                        new DataColumn("fechaEmision",typeof(string)),
                        new DataColumn("fechaEmisionDocSustento",typeof(string)),
                        new DataColumn("identificacionComprador",typeof(string)),
                        new DataColumn("moneda",typeof(string)),
                        new DataColumn("motivo",typeof(string)),
                        new DataColumn("numDocModificado",typeof(string)),
                        new DataColumn("obligadoContabilidad",typeof(string)),
                        new DataColumn("rise",typeof(string)),
                        new DataColumn("razonSocialComprador",typeof(string)),
                        new DataColumn("tipoIdentificacionComprador",typeof(string)),
                        new DataColumn("totalSinImpuestos",typeof(string)),
                        new DataColumn("valorModificacion",typeof(string))
                    };

                    DataColumn[] cols_tblDetalleNotaCredito = new DataColumn[] {
                        new DataColumn("cantidad",typeof(string)),
                        new DataColumn("codigoAdicional",typeof(string)),
                        new DataColumn("codigoInterno",typeof(string)),
                        new DataColumn("descripcion",typeof(string)),
                        new DataColumn("descuento",typeof(string)),
                        new DataColumn("descuentoEspecifico",typeof(string)),
                        new DataColumn("precioTotalSinImpuesto",typeof(string)),
                        new DataColumn("precioUnitario",typeof(string))
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
                        new DataColumn("totalDescuento",typeof(string)),
                        new DataColumn("ice",typeof(string)),
                        new DataColumn("iva",typeof(string)),
                        new DataColumn("iva5",typeof(string)),
                        new DataColumn("irbpnr",typeof(string)),
                        new DataColumn("valorTotal",typeof(string))
                    };


                    tblInfoTributaria.Columns.AddRange(cols_tblInfoTributaria);
                    tblInfoNotaCredito.Columns.AddRange(cols_tblInfoNotaCredito);
                    tblDetalleNotaCredito.Columns.AddRange(cols_tblDetalleNotaCredito);
                    tblInformacionAdicional.Columns.AddRange(cols_tblInformacionAdicional);
                    tblInformacionMonetaria.Columns.AddRange(cols_tblInformacionMonetaria);
                    #endregion

                    #region Logo de la Compania
                    pathLogoEmpresa = CatalogoViaDoc.rutaLogoCompania;
                    string rutaImagen = pathLogoEmpresa + objRideNotaCredito._infoTributaria.Ruc.Trim() + ".png";
                    #endregion

                    #region Genera Codigo Barra de la Factura
                    byte[] imgBar;
                    Code.BarcodeGenerator bgCode128 = new Code.BarcodeGenerator();
                    Code.Convertir cCode = new Code.Convertir();
                    Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                    StringFormat fi = new StringFormat(StringFormatFlags.NoClip);
                    fi.Alignment = StringAlignment.Center;
                    imgBar = RetornarXml.ImageToByte2(Code128Rendering.MakeBarcodeImage(objRideNotaCredito._infoTributaria.ClaveAcceso, 3, true));

                    pathCodBarra = CatalogoViaDoc.rutaCodigoBarra + objRideNotaCredito._infoTributaria.ClaveAcceso.Trim();
                    pathRider = CatalogoViaDoc.rutaRide + objRideNotaCredito._infoTributaria.Ruc.Trim() + "\\" + objRideNotaCredito._infoTributaria.CodigoDocumento.Trim();
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
                                    DateTime fechaEmision = Convert.ToDateTime(objRideNotaCredito._infoNotaCredito.FechaEmision);

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
                    localReport.ReportPath = CatalogoViaDoc.rutaRidePlantilla + "RideNotaCredito.rdlc";
                    ReportParameter[] parameters = new ReportParameter[7];
                    parameters[0] = new ReportParameter("txFechaAutorizacion", objRideNotaCredito.FechaHoraAutorizacion);
                    parameters[1] = new ReportParameter("txNumeroAutorizacion", objRideNotaCredito._infoTributaria.ClaveAcceso);
                    parameters[2] = new ReportParameter("pathImagenCodBarra", pathCodBarra + ".jpg");
                    parameters[3] = new ReportParameter("pathLogoCompania", @rutaImagen);
                    parameters[4] = new ReportParameter("tarifaIva", etiquetaTipoIvaGrabadoSiPorcentaje);
                    parameters[5] = new ReportParameter("etiquetaTarifaIva", etiquetaTipoIvaGrabadoConPorcentaje);
                    parameters[6] = new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial);

                    #region Datos Tributarios
                    DataRow drInfoTributaria = tblInfoTributaria.NewRow();
                    if (validaAmbiente)
                    {
                        CatalogoReporte tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == objRideNotaCredito._infoTributaria.Ambiente);
                        drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                    }
                    else
                        drInfoTributaria["ambiente"] = objRideNotaCredito._infoTributaria.Ambiente;


                    drInfoTributaria["claveAcceso"] = objRideNotaCredito._infoTributaria.ClaveAcceso;
                    drInfoTributaria["codDoc"] = objRideNotaCredito._infoTributaria.CodigoDocumento;
                    drInfoTributaria["dirMatriz"] = objRideNotaCredito._infoTributaria.DirMatriz;

                    try { drInfoTributaria["regimenMicroempresas"] = objRideNotaCredito._infoTributaria.regimenMicroempresas; } catch { drInfoTributaria["regimenMicroempresas"] = ""; }
                    try { drInfoTributaria["agenteRetencion"] = objRideNotaCredito._infoTributaria.AgenteRetencion; } catch { drInfoTributaria["agenteRetencion"] = ""; }
                    try { drInfoTributaria["contribuyenteRimpe"] = objRideNotaCredito._infoTributaria.contribuyenteRimpe; } catch { drInfoTributaria["contribuyenteRimpe"] = ""; }
                    //try { drInfoTributaria["regimenGeneral"] = objRideNotaCredito._infoTributaria.regimenGeneral; } catch { drInfoTributaria["regimenGeneral"] = ""; }
                    drInfoTributaria["estab"] = objRideNotaCredito._infoTributaria.Establecimiento;
                    drInfoTributaria["nombreComercial"] = objRideNotaCredito._infoTributaria.NombreComercial;

                    drInfoTributaria["ptoEmi"] = objRideNotaCredito._infoTributaria.PuntoEmision;
                    drInfoTributaria["razonSocial"] = objRideNotaCredito._infoTributaria.RazonSocial;
                    drInfoTributaria["ruc"] = objRideNotaCredito._infoTributaria.Ruc;
                    drInfoTributaria["secuencial"] = objRideNotaCredito._infoTributaria.Secuencial;


                    if (validaEmisionFactura)
                    {
                        CatalogoReporte tipoemision = emisionesFacturacion.Find(x => x.Codigo == objRideNotaCredito._infoTributaria.TipoEmision);
                        drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                    }
                    else
                        drInfoTributaria["tipoEmision"] = objRideNotaCredito._infoTributaria.TipoEmision;
                    tblInfoTributaria.Rows.Add(drInfoTributaria);
                    #endregion
                    #region Datos de la Nota Credito

                    DataRow drInfoNotaCredito = tblInfoNotaCredito.NewRow();
                    List<CatalogoReporte> documentosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                    CatalogoReporte documentoSustento = documentosRetencion.Find(x => x.Codigo == objRideNotaCredito._infoNotaCredito.CodDocModificado);

                    drInfoNotaCredito["codDocModificado"] = documentoSustento.Valor;


                    drInfoNotaCredito["obligadoContabilidad"] = objRideNotaCredito._infoNotaCredito.ObligadoContabilidad;


                    if (validaNumeroContribuyenteEspecial)
                        drInfoNotaCredito["contribuyenteEspecial"] = objRideNotaCredito._infoNotaCredito.NumeroContribuyenteEspecial;
                    else
                        drInfoNotaCredito["contribuyenteEspecial"] = "";



                    drInfoNotaCredito["dirEstablecimiento"] = objRideNotaCredito._infoNotaCredito.DirEstablecimiento;
                    drInfoNotaCredito["fechaEmision"] = objRideNotaCredito._infoNotaCredito.FechaEmision;
                    drInfoNotaCredito["fechaEmisionDocSustento"] = objRideNotaCredito._infoNotaCredito.FechaEmisionDocSustento;
                    drInfoNotaCredito["identificacionComprador"] = objRideNotaCredito._infoNotaCredito.Identificacion;
                    drInfoNotaCredito["moneda"] = objRideNotaCredito._infoNotaCredito.Moneda;
                    drInfoNotaCredito["motivo"] = objRideNotaCredito._infoNotaCredito.Motivo;
                    drInfoNotaCredito["numDocModificado"] = objRideNotaCredito._infoNotaCredito.NumDocModificado;
                    drInfoNotaCredito["razonSocialComprador"] = objRideNotaCredito._infoNotaCredito.RazonSocial;
                    drInfoNotaCredito["tipoIdentificacionComprador"] = objRideNotaCredito._infoNotaCredito.TipoIdentificacion;
                    drInfoNotaCredito["totalSinImpuestos"] = objRideNotaCredito._infoNotaCredito.TotalSinImpuestos;
                    drInfoNotaCredito["valorModificacion"] = objRideNotaCredito._infoNotaCredito.ValorModificacion;
                    tblInfoNotaCredito.Rows.Add(drInfoNotaCredito);

                    #endregion

                    #region Detalle de la Nota Credito
                    DataRow drDetalle;
                    foreach (Detalle item in objRideNotaCredito._detalles)
                    {
                        drDetalle = tblDetalleNotaCredito.NewRow();
                        drDetalle["cantidad"] = item.Cantidad;
                        drDetalle["codigoAdicional"] = item.CodigoAuxiliar;
                        drDetalle["codigoInterno"] = item.CodigoPrincipal;
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
                        drDetalle["descuentoEspecifico"] = "0.00";
                        tblDetalleNotaCredito.Rows.Add(drDetalle);
                    }
                    #endregion

                    #region Informacion Adicional
                    DataRow drInfoAdicional;
                    foreach (InformacionAdicional item in objRideNotaCredito._infoAdicional)
                    {
                        drInfoAdicional = tblInformacionAdicional.NewRow();
                        drInfoAdicional["nombre"] = item.Nombre;
                        drInfoAdicional["valor"] = item.Valor;
                        tblInformacionAdicional.Rows.Add(drInfoAdicional);
                    }
                    #endregion

                    #region Monto de la Nota de Credito
                    DataRow dr_tblInformacionMonetaria = tblInformacionMonetaria.NewRow();

                    dr_tblInformacionMonetaria["subTotalIva"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalIva5"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalCero"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = "0.00";
                    dr_tblInformacionMonetaria["subTotalExcentoIva"] = "0.00";
                    dr_tblInformacionMonetaria["iva"] = "0.00";
                    dr_tblInformacionMonetaria["iva5"] = "0.00";
                    dr_tblInformacionMonetaria["irbpnr"] = "0.00";
                    dr_tblInformacionMonetaria["valorTotal"] = "0.00";
                    dr_tblInformacionMonetaria["ice"] = "0.00";

                    decimal subtotalIvaCinco = 0M;
                    decimal subtotalIvaGrab = 0M;
                    decimal ivaCinco = 0M;

                    foreach (TotalConImpuesto objfactTotalImpuesto in objRideNotaCredito._infoNotaCredito._totalesConImpuesto)
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
                            else
                                //dr_tblInformacionMonetaria["subTotalCero"] = "0.00";
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
                                dr_tblInformacionMonetaria["irbpnr"] = "0";
                        }
                        else
                            dr_tblInformacionMonetaria["irbpnr"] = "0.00";

                        if (String.Compare(objfactTotalImpuesto.CodigoPorcentaje.Trim(), CatalogoViaDoc.ParametrosValorIVA0) == 0)
                            dr_tblInformacionMonetaria["subTotalSinImpuesto"] = objRideNotaCredito._infoNotaCredito.TotalSinImpuestos;
                        else
                            dr_tblInformacionMonetaria["subTotalSinImpuesto"] = "0.00";
                        dr_tblInformacionMonetaria["irbpnr"] = "0.00";
                    }

                    decimal totalSinImpuesto = Convert.ToDecimal(objRideNotaCredito._infoNotaCredito.TotalSinImpuestos.Replace('.', ','));

                    decimal totalDescuento = Convert.ToDecimal("0,00");
                    decimal valorTotal = Convert.ToDecimal(objRideNotaCredito._infoNotaCredito.ValorModificacion.Replace('.', ','));

                    if (validaSeparadorMiles)
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalSinImpuesto);
                        dr_tblInformacionMonetaria["totalDescuento"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalDescuento);
                        dr_tblInformacionMonetaria["valorTotal"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valorTotal);
                    }
                    else
                    {
                        dr_tblInformacionMonetaria["subTotalSinImpuesto"] = objRideNotaCredito._infoNotaCredito.TotalSinImpuestos;
                        dr_tblInformacionMonetaria["totalDescuento"] = "0.00";

                        dr_tblInformacionMonetaria["valorTotal"] = objRideNotaCredito._infoNotaCredito.ValorModificacion;
                    }
                    tblInformacionMonetaria.Rows.Add(dr_tblInformacionMonetaria);
                    #endregion

                    ReportDataSource datos_tblInfoTributaria = new ReportDataSource("tblInformacionTributaria", tblInfoTributaria);
                    ReportDataSource datos_tblInfoFactura = new ReportDataSource("tblInformacionNotaCredito", tblInfoNotaCredito);
                    ReportDataSource datos_tblDetalleFactura = new ReportDataSource("tblDetalle", tblDetalleNotaCredito);
                    ReportDataSource datos_tblInformacionAdicional = new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional);
                    ReportDataSource datos_tblInformacionMonetaria = new ReportDataSource("tblInformacionMonetaria", tblInformacionMonetaria);

                    localReport.EnableExternalImages = true;
                    localReport.SetParameters(parameters);
                    localReport.DataSources.Add(datos_tblInfoTributaria);
                    localReport.DataSources.Add(datos_tblInfoFactura);
                    localReport.DataSources.Add(datos_tblDetalleFactura);
                    localReport.DataSources.Add(datos_tblInformacionAdicional);
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
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Mensaje 0" + "Todo Correcto");

                }
                else
                {
                    mensajeError = "Error al generar del byte[] para elo ride Factura. Mensaje Error: La compania " + objRideNotaCredito._infoTributaria.RazonSocial + " no tiene registrada las configuraciones del reporte";
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Mensaje 1" + mensajeError);
                    bytePDF = null;
                }

            }
            catch (Exception ex)
            {
                mensajeError = "Error al generar del byte[] para elo ride Factura. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
                bytePDF = null;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Mensaje 2" + mensajeError);
            }
            return bytePDF;

        }



    }
}
