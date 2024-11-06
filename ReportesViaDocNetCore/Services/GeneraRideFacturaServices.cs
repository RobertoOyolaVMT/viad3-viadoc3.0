using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Models;
using System.Data;
using System.Globalization;
using BarcodeStandard;
using SkiaSharp;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace ReportesViaDocNetCore.Services
{
    public class GeneraRideFacturaServices : IGeneraRideFactura
    {
        private readonly FacturacionElectronicaQaContext _context;
        private readonly IConfiguration _configuracion;
        private readonly ICatalogos _catalogos;

        public GeneraRideFacturaServices(FacturacionElectronicaQaContext context, IConfiguration configuracion, ICatalogos catalogos)
        {
            this._context = context;
            this._configuracion = configuracion;
            this._catalogos = catalogos;
        }

        public async Task<RespuestaRide> Ridefactura(string txClaveAcceso)
        {
            var bytePdf = new RespuestaRide();
            var datosDocumento = new DatosDocumento();
            var catalogoDetalle = new List<DetalleCatalogo>();
            var dataCompania = new Companium();
            var configuracione = new List<ConfiguracionReporte>();
            var catalogoReporte = new List<CatalogoReporte>();

            var facCab = new Factura1();
            var facturaDetalleImpuestos = new List<FacturaDetalleImpuesto>();
            var facturaDetalleFormaPagos1 = new List<FacturaDetalleFormaPago1>();
            var facturaReembolsoGastos = new List<FacturaReembolsoGasto>();

            try
            {
                #region Tbl Catalogo
                datosDocumento = await _catalogos.DatosDocuemntosFactura(txClaveAcceso);
                dataCompania = await _catalogos.Compania(datosDocumento.IdCompania);
                configuracione = await _catalogos.Configuraciones(datosDocumento.IdCompania);
                catalogoReporte = await _catalogos.CatalogoReportes();
                catalogoDetalle = await _catalogos.DetalleCatalogo();
                #endregion

                #region Consulta Factura
                facCab = await _context.Facturas1.Where(f => (f.TxEstablecimiento + "-" + f.TxPuntoEmision + "-" + f.TxSecuencial).Equals(datosDocumento.NumDoc)
                                                                                    && f.CiCompania.Equals(datosDocumento.IdCompania)).FirstOrDefaultAsync();

                if (facCab != null)
                {
                    facCab.FacturaDetalle1s = await _context.FacturaDetalles1.Where(f => (f.TxEstablecimiento + "-" + f.TxPuntoEmision + "-" + f.TxSecuencial).Equals(datosDocumento.NumDoc)
                                                                                    && f.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();

                    facCab.FacturaInfoAdicionals = await _context.FacturaInfoAdicionals.Where(f => (f.TxEstablecimiento + "-" + f.TxPuntoEmision + "-" + f.TxSecuencial).Equals(datosDocumento.NumDoc)
                                                                                    && f.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();

                    facCab.FacturaTotalImpuestos = await _context.FacturaTotalImpuestos.Where(f => (f.TxEstablecimiento + "-" + f.TxPuntoEmision + "-" + f.TxSecuencial).Equals(datosDocumento.NumDoc)
                                                                                    && f.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();
                }

                facturaDetalleImpuestos = await _context.FacturaDetalleImpuestos.Where(f => (f.TxEstablecimiento + "-" + f.TxPuntoEmision + "-" + f.TxSecuencial).Equals(datosDocumento.NumDoc)
                                                                                    && f.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();

                facturaDetalleFormaPagos1 = await _context.FacturaDetalleFormaPagos1.Where(f => (f.TxEstablecimiento + "-" + f.TxPuntoEmision + "-" + f.TxSecuencial).Equals(datosDocumento.NumDoc)
                                                                                    && f.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();

                facturaReembolsoGastos = await _context.FacturaReembolsoGastos.Where(f => (f.TxEstablecimiento + "-" + f.TxPuntoEmision + "-" + f.TxSecuencial).Equals(datosDocumento.NumDoc)
                                                                                    && f.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();
                #endregion

                bytePdf.TipoDoc = "01";
                bytePdf.Documento = ProcesoRideFactura(facCab!, facturaDetalleImpuestos, facturaDetalleFormaPagos1, facturaReembolsoGastos,
                                             catalogoDetalle, configuracione, catalogoReporte, dataCompania);
                bytePdf.Cod = "0000";

            }
            catch (Exception ex)
            {
                bytePdf.TipoDoc = "01";
                bytePdf.Documento = "";
                bytePdf.Cod = "9999";

                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("Ridefactura", "GeneraReporteNetcore", ex.Message, null);
            }

            return bytePdf;
        }

        public string ProcesoRideFactura(Factura1 objRideFactura, List<FacturaDetalleImpuesto> facturaDetalleImpuestos, List<FacturaDetalleFormaPago1> facturaDetalleFormaPagos1,
                                            List<FacturaReembolsoGasto> facturaReembolsoGastos, List<DetalleCatalogo> catalogoDetalle, List<ConfiguracionReporte> confCompania, List<CatalogoReporte> catalogoSistema, Companium dataCompania)
        {
            var ridePdf = string.Empty;

            var etiquetaContribuyenteEspecial = string.Empty;
            var etiquetaTipoIvaGrabadoConPorcentaje = string.Empty;
            var etiquetaTipoIvaGrabadoSiPorcentaje = string.Empty;
            var etiquetaTipoIvaGrabadoConPorcentaje5 = string.Empty;
            var etiquetaTipoIvaGrabadoSiPorcentaje5 = string.Empty;
            var validaCamposNegritasInfoAdicional = "0";
            var palabraClaveFechaVencimiento = string.Empty;
            var palabraClaveFechaCorte = string.Empty;
            try
            {

                #region Origen Datos del RIDE Factura
                DataTable tblInfoTributaria = new DataTable();
                DataTable tblInfoFactura = new DataTable();
                DataTable tblDetalleFactura = new DataTable();
                DataTable tblInformacionAdicional = new DataTable();
                DataTable tblInformacionMonetaria = new DataTable();
                DataTable tblFormaPago = new DataTable();
                DataTable tblReembolsoGasto = new DataTable();

                tblInfoTributaria.TableName = "tblInformacionTributaria";
                tblInfoFactura.TableName = "tblInformacionFactura";
                tblDetalleFactura.TableName = "tblDetalleFactura";
                tblInformacionAdicional.TableName = "tblInformacionAdicional";
                tblFormaPago.TableName = "tblFormaPago";
                tblInformacionMonetaria.TableName = "tblInformacionMonetaria";
                tblReembolsoGasto.TableName = "tblReembolsoGasto";

                DataColumn[] cols_tblInfoTributaria = new DataColumn[] {
                        new DataColumn("ambiente",typeof(string)),
                        new DataColumn("claveAcceso",typeof(string)),
                        new DataColumn("codDoc",typeof(string)),
                        new DataColumn("dirMatriz",typeof(string)),
                        new DataColumn("regimenGeneral",typeof(string)),
                        new DataColumn("regimenMicroempresas",typeof(string)),
                        new DataColumn("agenteRetencion",typeof(string)),
                        new DataColumn("contribuyenteRimpe",typeof(string)),
                        //new DataColumn("regimenGeneral",typeof(string)),
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

                DataColumn[] cols_tblReembolsosGastos = new DataColumn[] {
                        new DataColumn("tipoDocumento",typeof(string)),
                        new DataColumn("numeroDocumento",typeof(string)),
                        new DataColumn("fechaEmision",typeof(string)),
                        new DataColumn("identificacionProveedor",typeof(string)),
                        new DataColumn("autorizacion",typeof(string)),
                        new DataColumn("subTotalNoIva",typeof(string)),
                        new DataColumn("subTotalIvaCero",typeof(string)),
                        new DataColumn("subTotalIva",typeof(string)),
                        new DataColumn("subTotalIva5",typeof(string)),
                        new DataColumn("subTotalExcentoIva",typeof(string)),
                        new DataColumn("impuestoIva",typeof(string)),
                        new DataColumn("impuestoIva5",typeof(string)),
                        new DataColumn("impuestoIce",typeof(string)),
                        new DataColumn("impuestoIRBPNR",typeof(string)),
                        new DataColumn("valorTotal",typeof(string)),
                        new DataColumn("subTotalTrama",typeof(string)),
                        new DataColumn("impuestosTrama",typeof(string)),
                        new DataColumn("detalle",typeof(string)),
                        new DataColumn("valorBase",typeof(string))
                    };


                tblInfoTributaria.Columns.AddRange(cols_tblInfoTributaria);
                tblInfoFactura.Columns.AddRange(cols_tblInfoFactura);
                tblDetalleFactura.Columns.AddRange(cols_tblDetalleFactura);
                tblInformacionAdicional.Columns.AddRange(cols_tblInformacionAdicional);
                tblFormaPago.Columns.AddRange(cols_tblFormaPago);
                tblInformacionMonetaria.Columns.AddRange(cols_tblInformacionMonetaria);
                tblReembolsoGasto.Columns.AddRange(cols_tblReembolsosGastos);
                #endregion
                #region Logo de la Compania
                var pathLogoEmpresa = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Select(x => x.Param3).FirstOrDefault();
                var rutaImagen = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("4")).Select(x => x.Param3).FirstOrDefault() + dataCompania.TxRuc.Trim() + ".png";
                #endregion
                #region Genera Codigo Barra de la Factura
                var claveAcceso = objRideFactura.TxClaveAcceso;
                var raizRutaCodeBar = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("3")).Select(x => x.Param3).FirstOrDefault();
                var filePath = Path.Combine(raizRutaCodeBar!, $"{claveAcceso}.jpg");

                Barcode barcode = new Barcode();
                barcode.IncludeLabel = false;
                SKImage codigoDeBarras = barcode.Encode(BarcodeStandard.Type.Code128, claveAcceso, SKColors.Black, SKColors.White, 700, 100);
                using (var data = codigoDeBarras.Encode(SKEncodedImageFormat.Jpeg, 100))
                using (var stream = File.OpenWrite(filePath))
                {
                    data.SaveTo(stream);
                }
                #endregion
                #region Configuracion de los separadores de miles de la Factura MEJORAR
                var confSeparadorMiles = confCompania.Find(x => x.CodigoReferencia == "ACTMIL");
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
                var confFormaPago = confCompania.Find(x => x.CodigoReferencia == "ACTFORMPAG");
                if (confFormaPago != null)
                    validaAgregacionFormaPago = Convert.ToBoolean(confFormaPago.Configuracion2.Trim());
                #endregion
                #region Configuracion para la etiqueta Contribuyente especial
                bool validaEtiquetaContribuyenteEspecial = false;
                var confEtiquetaContEspecial = confCompania.Find(x => x.CodigoReferencia == "ETIQCONT");
                if (confEtiquetaContEspecial != null)
                {
                    validaEtiquetaContribuyenteEspecial = Convert.ToBoolean(confEtiquetaContEspecial.Configuracion2.Trim());
                    if (validaEtiquetaContribuyenteEspecial)
                        etiquetaContribuyenteEspecial = confEtiquetaContEspecial.Configuracion1;
                }
                #endregion
                #region Configuracion para visualizar el numero de contribuyente especial
                var confContribuEspecial = confCompania.Find(x => x.CodigoReferencia == "ACTNUMCONT");
                bool validaNumeroContribuyenteEspecial = false;
                if (confContribuEspecial != null)
                    validaNumeroContribuyenteEspecial = Convert.ToBoolean(confContribuEspecial.Configuracion2);
                #endregion
                #region Configuracion para colocar en negritas datos de la seccion de Informacion Adicional
                var confActivarNegrita = confCompania.Find(x => x.CodigoReferencia == "NEGRIFOADD");
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
                #endregion
                #region Catalogo Forma de Pago
                var listadoFormasPago = catalogoSistema.FindAll(x => x.CodigoReferencia == "FORMAPAGO");
                bool validaFormaPago = false;
                if (listadoFormasPago != null)
                    validaFormaPago = true;
                #endregion
                #region Catalogo Ambiente Facturacion
                var ambientesFacturacion = catalogoSistema.FindAll(x => x.CodigoReferencia == "AMBIENTE");
                bool validaAmbiente = false;
                if (ambientesFacturacion != null)
                    validaAmbiente = true;
                #endregion
                #region Catalogo Emisiones Facturacion
                var emisionesFacturacion = catalogoSistema.FindAll(x => x.CodigoReferencia == "EMISION");
                bool validaEmisionFactura = false;
                if (emisionesFacturacion != null)
                    validaEmisionFactura = true;
                #endregion
                #region Validacion de la Etiqueta del tipo de Iva Grabado
                var tipoIvaGrabado = catalogoSistema.Find(x => x.CodigoReferencia == "IVAGRAB")!;//Catalogo Tipo de Iva para las nuevas empresas que inician con facturacion electronica
                var tipoIvaGrabado5 = catalogoSistema.Find(x => x.CodigoReferencia == "IVACONSTR")!;//Catalogo Tipo de Iva para las nuevas empresas que inician con facturacion electronica

                bool validaTipoIvaNuevasEmpresas = false;
                if (tipoIvaGrabado5 != null)
                {
                    etiquetaTipoIvaGrabadoSiPorcentaje5 = tipoIvaGrabado5.Codigo;
                    etiquetaTipoIvaGrabadoConPorcentaje5 = "IVA " + tipoIvaGrabado5.Codigo + "%";
                }
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
                        var fechaSalvaguardiaIva14 = catalogoSistema.Find(x => x.CodigoReferencia == "SALVAGUARD")!;
                        if (fechaSalvaguardiaIva14 != null)
                        {
                            try
                            {

                                string[] fechaIni = fechaSalvaguardiaIva14.Codigo.Split('/');
                                string[] fechaFin = fechaSalvaguardiaIva14.Valor.Split('/');
                                string[] fechaEmisionDocumento = objRideFactura.TxFechaEmision.Split('/');
                                DateTime fechaInicioSalvaguardia = new DateTime(Convert.ToInt32(fechaIni[2]), Convert.ToInt32(fechaIni[1]), Convert.ToInt32(fechaIni[0]));
                                DateTime fechaFinSalvaguardia = new DateTime(Convert.ToInt32(fechaFin[2]), Convert.ToInt32(fechaFin[1]), Convert.ToInt32(fechaFin[0]));
                                DateTime fechaEmision = new DateTime(Convert.ToInt32(fechaEmisionDocumento[2]), Convert.ToInt32(fechaEmisionDocumento[1]), Convert.ToInt32(fechaEmisionDocumento[0]));

                                if (fechaEmision.Year <= fechaFinSalvaguardia.Year && fechaEmision.Year >= fechaInicioSalvaguardia.Year)
                                {
                                    var IVASalvaguardia = catalogoSistema.Find(x => x.CodigoReferencia == "IVASALVAGU")!;
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
                            catch
                            {
                                etiquetaTipoIvaGrabadoSiPorcentaje = "";
                                etiquetaTipoIvaGrabadoConPorcentaje = "";
                            }
                        }
                    }
                }
                #endregion
                #region Datos Tributarios

                var agenteRetencion = dataCompania.TxAgenteRetencion;
                var regimenMicroempresas = dataCompania.TxRegimenMicroempresas;
                var contribuyenteRimpe = dataCompania.TxContribuyenteRimpe;

                var mensajeaAgenteRetencion = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("1")).Param3;
                var mensajeRegimenMicroempresas = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("2")).Param3;
                var mensajeContribuyenteRimpe = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("3")).Param3;

                DataRow drInfoTributaria = tblInfoTributaria.NewRow();

                if (validaAmbiente)
                {
                    var tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == Convert.ToString(dataCompania.CiTipoAmbiente))!;
                    drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                }
                else
                    drInfoTributaria["ambiente"] = Convert.ToString(dataCompania.CiTipoAmbiente);
                drInfoTributaria["claveAcceso"] = objRideFactura.TxClaveAcceso;
                drInfoTributaria["codDoc"] = objRideFactura.CiTipoDocumento;
                drInfoTributaria["dirMatriz"] = dataCompania.TxDireccionMatriz;
                drInfoTributaria["agenteRetencion"] = Convert.ToBoolean(agenteRetencion) ? mensajeaAgenteRetencion : string.Empty;
                drInfoTributaria["regimenMicroempresas"] = Convert.ToBoolean(regimenMicroempresas) ? mensajeRegimenMicroempresas : string.Empty;
                drInfoTributaria["contribuyenteRimpe"] = Convert.ToBoolean(contribuyenteRimpe) ? mensajeContribuyenteRimpe : string.Empty;
                //try { drInfoTributaria["regimenGeneral"] = objRideFactura._infoTributaria.regimenGeneral; } catch { drInfoTributaria["regimenGeneral"] = ""; }
                drInfoTributaria["estab"] = objRideFactura.TxEstablecimiento;
                drInfoTributaria["nombreComercial"] = dataCompania.TxNombreComercial;
                drInfoTributaria["ptoEmi"] = objRideFactura.TxPuntoEmision;
                drInfoTributaria["razonSocial"] = dataCompania.TxRazonSocial;
                drInfoTributaria["ruc"] = dataCompania.TxRuc;
                drInfoTributaria["secuencial"] = objRideFactura.TxSecuencial;
                if (validaEmisionFactura)
                {
                    var tipoemision = emisionesFacturacion.Find(x => x.Codigo == Convert.ToString(objRideFactura.CiTipoEmision))!;
                    drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                }
                else
                    drInfoTributaria["tipoEmision"] = objRideFactura.CiTipoEmision;
                tblInfoTributaria.Rows.Add(drInfoTributaria);
                #endregion
                #region Datos de la Factura
                DataRow drInfoFactura = tblInfoFactura.NewRow();
                drInfoFactura["dirEstablecimiento"] = dataCompania.Sucursals.FirstOrDefault(x => x.CiSucursal.Equals(objRideFactura.TxEstablecimiento)).TxDireccion;
                drInfoFactura["fechaEmision"] = objRideFactura.TxFechaEmision;
                drInfoFactura["guiaRemision"] = "";
                if (validaNumeroContribuyenteEspecial)
                    drInfoFactura["contribuyenteEspecial"] = dataCompania.TxContribuyenteEspecial;
                else
                    drInfoFactura["contribuyenteEspecial"] = "";
                drInfoFactura["identificacionComprador"] = objRideFactura.TxIdentificacionComprador;
                drInfoFactura["importeTotal"] = objRideFactura.QnImporteTotal;
                drInfoFactura["moneda"] = objRideFactura.TxMoneda;
                drInfoFactura["obligadoContabilidad"] = dataCompania.TxObligadoContabilidad!.Equals("S") ? "SI" : "NO";
                drInfoFactura["propina"] = objRideFactura.QnPropina;
                drInfoFactura["razonSocialComprador"] = dataCompania.TxRazonSocial;
                drInfoFactura["tipoIdentificacionComprador"] = objRideFactura.CiTipoIdentificacionComprador;
                drInfoFactura["totalDescuento"] = objRideFactura.QnTotalDescuento;
                drInfoFactura["totalSinImpuestos"] = objRideFactura.QnTotalSinImpuestos;
                tblInfoFactura.Rows.Add(drInfoFactura);
                #endregion
                #region Detalle de la factura
                DataRow drDetalle;
                foreach (var item in objRideFactura.FacturaDetalle1s)
                {
                    drDetalle = tblDetalleFactura.NewRow();
                    drDetalle["cantidad"] = item.QnCantidad;
                    drDetalle["codigoAuxiliar"] = item.TxCodigoAuxiliar;
                    drDetalle["codigoPrincipal"] = item.TxCodigoPrincipal;
                    drDetalle["descripcion"] = item.TxDescripcion;
                    decimal descuento = Convert.ToDecimal(item.QnDescuento.ToString().Replace('.', ','));
                    decimal precioTotalSinImp = Convert.ToDecimal(item.QnPrecioTotalSinImpuesto.ToString().Replace('.', ','));
                    decimal precioUnitario = Convert.ToDecimal(item.QnPrecioUnitario.ToString()!.Replace('.', ','));
                    if (validaSeparadorMiles)
                    {
                        drDetalle["descuento"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, descuento);
                        drDetalle["precioTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, precioTotalSinImp);
                        drDetalle["precioUnitario"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, precioUnitario);
                    }
                    else
                    {
                        drDetalle["descuento"] = item.QnDescuento;
                        drDetalle["precioTotalSinImpuesto"] = item.QnPrecioTotalSinImpuesto;
                        drDetalle["precioUnitario"] = item.QnPrecioUnitario;
                    }
                    tblDetalleFactura.Rows.Add(drDetalle);
                }
                #endregion
                #region Informacion Adicional
                DataRow drInfoAdicional;
                foreach (var item in objRideFactura.FacturaInfoAdicionals)
                {
                    drInfoAdicional = tblInformacionAdicional.NewRow();
                    drInfoAdicional["nombre"] = item.TxNombre;
                    drInfoAdicional["valor"] = item.TxValor;
                    tblInformacionAdicional.Rows.Add(drInfoAdicional);
                }
                #endregion
                #region Forma de Pago
                DataRow drFormaPago;
                if (validaAgregacionFormaPago)
                {
                    #region Cuando la compañia tiene activada la forma de pago
                    foreach (var item in facturaDetalleFormaPagos1)
                    {
                        drFormaPago = tblFormaPago.NewRow();
                        decimal total = Convert.ToDecimal(item.QnTotal.ToString()!.Replace('.', ','));
                        if (validaFormaPago)
                        {
                            var objFormaPago = listadoFormasPago.Find(x => x.Codigo == item.TxFormaPago);
                            drFormaPago["formaPago"] = objFormaPago.Valor;
                        }
                        else
                            drFormaPago["formaPago"] = item.TxFormaPago;
                        drFormaPago["plazo"] = item.TxPlazo;
                        if (validaSeparadorMiles)
                            drFormaPago["total"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, total);
                        else
                            drFormaPago["total"] = item.QnTotal;
                        drFormaPago["unidadTiempo"] = item.TxUnidadTiempo;
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

                decimal subtotalIvaCinco = 0M;
                decimal subtotalIvaGrab = 0M;
                decimal ivaCinco = 0M;
                foreach (var objfactTotalImpuesto in objRideFactura.FacturaTotalImpuestos)
                {
                    if (String.Compare(objfactTotalImpuesto.TxCodigo.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("2")).Param3) == 0)
                    {
                        #region IMPUESTOS_IVA
                        #region SubTotal Cero
                        if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("1")).Param3) == 0)
                        {

                            decimal subTotalCero = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                            if (validaSeparadorMiles)
                                dr_tblInformacionMonetaria["subTotalCero"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalCero);
                            else
                                dr_tblInformacionMonetaria["subTotalCero"] = objfactTotalImpuesto.QnBaseImponible.ToString();
                        }
                        #endregion
                        #region SubTotal 5
                        if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("5")).Param3) == 0)
                        {
                            decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                            decimal valor = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));
                            subtotalIvaCinco = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible);
                            ivaCinco = valor;

                            if (validaSeparadorMiles)
                            {

                                dr_tblInformacionMonetaria["subTotalIva5"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                                dr_tblInformacionMonetaria["iva5"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                            }
                            else
                            {
                                dr_tblInformacionMonetaria["subTotalIva5"] = objfactTotalImpuesto.QnBaseImponible;
                                dr_tblInformacionMonetaria["iva5"] = objfactTotalImpuesto.QnValor;
                            }
                        }
                        else
                        {
                            #region SubTotal 14
                            if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("3")).Param3) == 0)
                            {
                                decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                                decimal valor = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));
                                if (validaSeparadorMiles)
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                                    dr_tblInformacionMonetaria["iva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                                }
                                else
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = objfactTotalImpuesto.QnBaseImponible;
                                    dr_tblInformacionMonetaria["iva"] = objfactTotalImpuesto.QnValor;
                                }
                            }

                            else
                            {
                                #region SubTotal Iva 5,8,12,15,13
                                decimal total = Convert.ToDecimal(objRideFactura.QnTotalSinImpuestos.ToString().Replace('.', ','));
                                decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                                decimal valor = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));

                                if (validaSeparadorMiles)
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                                    dr_tblInformacionMonetaria["iva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                                    subtotalIvaGrab = baseImponible;
                                }
                                else
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = objfactTotalImpuesto.QnBaseImponible;
                                    dr_tblInformacionMonetaria["iva"] = objfactTotalImpuesto.QnValor;
                                    subtotalIvaGrab = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible);
                                }
                                #endregion
                            }
                            #endregion

                        }
                        #endregion
                        #region No objeto de IVA
                        if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("6")).Param3) == 0)
                        {
                            decimal subTotalNoObjetoIva = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                            if (validaSeparadorMiles)
                                dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalNoObjetoIva);
                            else
                                dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = objfactTotalImpuesto.QnBaseImponible.ToString();
                        }
                        else
                            dr_tblInformacionMonetaria["subTotalNoObjetoIva"] = "0.00";
                        #endregion
                        #region Excento de Iva
                        if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("7")).Param3) == 0)
                        {
                            decimal excdntoIva = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                            if (validaSeparadorMiles)
                                dr_tblInformacionMonetaria["subTotalExcentoIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, excdntoIva);
                            else
                                dr_tblInformacionMonetaria["subTotalExcentoIva"] = objfactTotalImpuesto.QnBaseImponible.ToString();
                        }
                        else
                            dr_tblInformacionMonetaria["subTotalExcentoIva"] = "0.00";
                        #endregion
                        #endregion IMPUESTOS_IVA
                    }

                    if (String.Compare(objfactTotalImpuesto.TxCodigo.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("8")).Param3) == 0)
                    {
                        decimal ice = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));
                        if (validaSeparadorMiles)
                            dr_tblInformacionMonetaria["ice"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, ice);
                        else
                            dr_tblInformacionMonetaria["ice"] = objfactTotalImpuesto.QnValor;
                    }
                    else
                        dr_tblInformacionMonetaria["ice"] = "0.00";

                    if (String.Compare(objfactTotalImpuesto.TxCodigo.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("9")).Param3) == 0)
                    {
                        dr_tblInformacionMonetaria["INBPNR"] = "0.00";
                    }
                }

                if (validaSeparadorMiles)
                {
                    decimal total = Convert.ToDecimal(objRideFactura.QnTotalSinImpuestos.ToString().Replace('.', ','));
                    dr_tblInformacionMonetaria["subTotalCero"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, total - subtotalIvaGrab - subtotalIvaCinco);

                }
                else
                {
                    decimal totalformat = (Convert.ToDecimal(objRideFactura.QnTotalSinImpuestos) - subtotalIvaGrab - subtotalIvaCinco);
                    dr_tblInformacionMonetaria["subTotalCero"] = totalformat.ToString();
                }

                decimal totalSinImpuesto = Convert.ToDecimal(objRideFactura.QnTotalSinImpuestos.ToString().Replace('.', ','));
                decimal totalDescuento = Convert.ToDecimal(objRideFactura.QnTotalDescuento.ToString().Replace('.', ','));
                decimal propina = Convert.ToDecimal(objRideFactura.QnPropina.ToString().Replace('.', ','));
                decimal importeTotal = Convert.ToDecimal(objRideFactura.QnImporteTotal.ToString().Replace('.', ','));

                if (validaSeparadorMiles)
                {
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalSinImpuesto);
                    dr_tblInformacionMonetaria["descuento"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalDescuento);
                    dr_tblInformacionMonetaria["propina"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, propina);
                    dr_tblInformacionMonetaria["valor"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, importeTotal);
                }
                else
                {
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = objRideFactura.QnTotalSinImpuestos;
                    dr_tblInformacionMonetaria["descuento"] = objRideFactura.QnTotalDescuento;
                    dr_tblInformacionMonetaria["propina"] = objRideFactura.QnPropina;
                    dr_tblInformacionMonetaria["valor"] = objRideFactura.QnImporteTotal;
                }
                tblInformacionMonetaria.Rows.Add(dr_tblInformacionMonetaria);
                #endregion
                #region  Reembolso De gastos
                if (facturaReembolsoGastos.Count != 0)
                {
                    DataRow drReembolso;
                    foreach (var item in facturaReembolsoGastos)
                    {
                        drReembolso = tblReembolsoGasto.NewRow();
                        drReembolso["tipoDocumento"] = item.CiTipoDocumento;
                        drReembolso["numeroDocumento"] = item.TxNumDocumento;
                        drReembolso["fechaEmision"] = item.TxFechaEmision;
                        drReembolso["identificacionProveedor"] = item.TxIdProveedor;
                        drReembolso["autorizacion"] = item.TxClaveAcceso;
                        drReembolso["detalle"] = item.Detalle;
                        drReembolso["valorBase"] = item.ValorBase;
                        decimal subTotalNoIva = Convert.ToDecimal(item.SubTotNoIva.ToString()!.Replace('.', ','));
                        decimal subTotalIvaCero = Convert.ToDecimal(item.SubTotIvaCero.ToString()!.Replace('.', ','));
                        decimal subTotalIva = Convert.ToDecimal(item.SubTotIva.ToString()!.Replace('.', ','));
                        decimal subTotalIva5 = Convert.ToDecimal("0.00"); //Convert.ToDecimal((item.SubTotalIva5 == null ? "0.00" : item.SubTotalIva5).Replace('.', ','));
                        decimal subTotalExcentoIva = Convert.ToDecimal(item.SubTotExcentoIva.ToString()!.Replace('.', ','));
                        decimal impuestoIva = Convert.ToDecimal(item.ImpIva.ToString()!.Replace('.', ','));
                        decimal impuestoIva5 = Convert.ToDecimal("0.00"); //Convert.ToDecimal((item.ImpuestoIva5 == null ? "0.00" : item.ImpuestoIva5).Replace('.', ','));
                        decimal impuestoIce = Convert.ToDecimal(item.ImpIce.ToString()!.Replace('.', ','));
                        decimal impuestoIRBPNR = Convert.ToDecimal(item.ImpIrbpnr.ToString()!.Replace('.', ','));
                        decimal valTotal = Convert.ToDecimal(item.ValTotal.ToString()!.Replace('.', ','));
                        if (validaSeparadorMiles)
                        {
                            drReembolso["subTotalNoIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalNoIva);
                            drReembolso["subTotalIvaCero"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalIvaCero);
                            drReembolso["subTotalIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalIva);
                            drReembolso["SubTotalIva5"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalIva5);
                            drReembolso["subTotalExcentoIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, subTotalExcentoIva);
                            drReembolso["impuestoIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, impuestoIva);
                            drReembolso["ImpuestoIva5"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, impuestoIva5);
                            drReembolso["impuestoIce"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, impuestoIce);
                            drReembolso["impuestoIRBPNR"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, impuestoIRBPNR);
                            drReembolso["valorTotal"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valTotal);
                        }
                        else
                        {
                            drReembolso["subTotalNoIva"] = item.SubTotNoIva;
                            drReembolso["subTotalIvaCero"] = item.SubTotIvaCero;
                            drReembolso["subTotalIva"] = item.SubTotIva;
                            drReembolso["subTotalIva5"] = item.SubTotIva;
                            drReembolso["subTotalExcentoIva"] = item.SubTotExcentoIva;
                            drReembolso["impuestoIva"] = item.ImpIva;
                            drReembolso["impuestoIva5"] = item.ImpIva;
                            drReembolso["impuestoIce"] = item.ImpIce;
                            drReembolso["impuestoIRBPNR"] = item.ImpIrbpnr;
                            drReembolso["valorTotal"] = item.ValTotal;
                        }
                        drReembolso["subTotalTrama"] = "No Iva: " + drReembolso["subTotalNoIva"] + "\n\rIva 0%: " + drReembolso["subTotalIvaCero"] + "\n\rIva: " + drReembolso["subTotalIva"] + "\n\rIva 5%: " + drReembolso["subTotalIva5"] + "\n\rExcento Iva: " + drReembolso["subTotalExcentoIva"];
                        drReembolso["impuestosTrama"] = "IVA: " + drReembolso["impuestoIva"] + "\n\rICE: " + drReembolso["impuestoIce"] + "\n\rIRBPNR: " + drReembolso["impuestoIRBPNR"];
                        tblReembolsoGasto.Rows.Add(drReembolso);
                    }
                }
                #endregion

                #region Genere RDLC
                etiquetaTipoIvaGrabadoSiPorcentaje = etiquetaTipoIvaGrabadoSiPorcentaje.Contains("%") ? etiquetaTipoIvaGrabadoSiPorcentaje : etiquetaTipoIvaGrabadoSiPorcentaje + " %";

                var leyendaFactura = "El adquirente y obligado al pago del servicio brindado por la Bolsa de Valores de Guayaquil S. A. BVG, en adelante BVG, declara expresamente su aceptación a la presente factura. Declara así mismo, que los fondos con los que realizará los pagos de esta factura, tienen un origen lícito y por ende no provienen de actividades ilícitas y prohibidas por la Ley, eximiendo a la BVG de toda responsabilidad civil, penal, o administrativa si la declaración contenida en esta cláusula fuere falsa.";

                var rutaCarpeta = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Param3;
                var rutaRide = Path.Combine(rutaCarpeta!, "RideFactura.rdlc");

                var reportDefinition = new FileStream(rutaRide, FileMode.Open);
                using (var localreport = new LocalReport())
                {
                    localreport.LoadReportDefinition(reportDefinition);

                    localreport.EnableExternalImages = true;

                    List<ReportParameter> parameters = new List<ReportParameter>
                    {
                        new ReportParameter("txFechaAutorizacion", objRideFactura.TxFechaHoraAutorizacion),
                        new ReportParameter("txNumeroAutorizacion", objRideFactura.TxClaveAcceso),
                        new ReportParameter("pathImagenCodBarra", filePath),
                        new ReportParameter("pathLogoCompania", @rutaImagen),
                        new ReportParameter("tarifaIva", etiquetaTipoIvaGrabadoSiPorcentaje),
                        new ReportParameter("etiquetaTarifaIva", etiquetaTipoIvaGrabadoConPorcentaje),
                        new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial),
                        new ReportParameter("banderaNegrita", validaCamposNegritasInfoAdicional),
                        new ReportParameter("fechaVencimiento", palabraClaveFechaVencimiento),
                        new ReportParameter("fechaCorte", palabraClaveFechaCorte)
                    };

                    localreport.SetParameters(parameters);
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionTributaria", tblInfoTributaria));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionFactura", tblInfoFactura));
                    localreport.DataSources.Add(new ReportDataSource("tblDetalleFactura", tblDetalleFactura));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional));
                    localreport.DataSources.Add(new ReportDataSource("tblFormaPago", tblFormaPago));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionMonetaria", tblInformacionMonetaria));
                    localreport.DataSources.Add(new ReportDataSource("tblReembolsoGasto", tblReembolsoGasto));
                    var result = localreport.Render("PDF");

                    localreport.Refresh();
                    localreport.Dispose();

                    ridePdf = Convert.ToBase64String(result);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("ProcesoRideFactura", "GeneraReporteNetcore", ex.Message, null);
            }
            return ridePdf;
        }
    }
}
