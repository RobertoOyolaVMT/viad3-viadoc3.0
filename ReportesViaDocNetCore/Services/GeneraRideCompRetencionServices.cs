using BarcodeStandard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Models;
using SkiaSharp;
using System.Data;
using System.Globalization;

namespace ReportesViaDocNetCore.Services
{
    public class GeneraRideCompRetencionServices : IGeneraRideCompRetencion
    {
        private readonly FacturacionElectronicaQaContext _context;
        private readonly ICatalogos _catalogos;

        public GeneraRideCompRetencionServices(FacturacionElectronicaQaContext context, ICatalogos catalogos)
        {
            this._context = context;
            this._catalogos = catalogos;
        }

        public async Task<RespuestaRide> GeneraRideCompRetencion(string txClaveAcceso)
        {
            var bytePdf = new RespuestaRide();
            var datosDocumento = new DatosDocumento();
            var catalogoDetalle = new List<DetalleCatalogo>();
            var dataCompania = new Companium();
            var configuracione = new List<ConfiguracionReporte>();
            var catalogoReporte = new List<CatalogoReporte>();

            var compRetencion = new CompRetencion();
            var compReteDetalle = new List<CompRetencionDetalle>();
            var compReteDocSus = new List<CompRetencionDocSustento>();
            var compReteFromPago = new List<CompRetencionFormaPago>();

            try
            {
                #region Tbl Catalogo
                datosDocumento = await _catalogos.DatosDocuemntosCompRetecion(txClaveAcceso);
                dataCompania = await _catalogos.Compania(datosDocumento.IdCompania);
                configuracione = await _catalogos.Configuraciones(datosDocumento.IdCompania);
                catalogoReporte = await _catalogos.CatalogoReportes();
                catalogoDetalle = await _catalogos.DetalleCatalogo();
                #endregion

                #region tblCompRetencion
                compRetencion = await (from comp in _context.CompRetencions
                                       where (comp.TxEstablecimiento + "-" + comp.TxPuntoEmision + "-" + comp.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                             comp.CiCompania.Equals(datosDocumento.IdCompania)
                                       select comp).FirstOrDefaultAsync();



                if (compRetencion != null)
                {

                    compRetencion.CompRetencionInfoAdicionals = await (from compInfo in _context.CompRetencionInfoAdicionals
                                                                       where (compInfo.TxEstablecimiento + "-" + compInfo.TxPuntoEmision + "-" + compInfo.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                                                             compInfo.CiCompania.Equals(datosDocumento.IdCompania)
                                                                       select compInfo).ToListAsync();
                }

                compReteDetalle = await (from reteDetalle in _context.CompRetencionDetalles
                                         where (reteDetalle.TxEstablecimiento + "-" + reteDetalle.TxPuntoEmision + "-" + reteDetalle.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                               reteDetalle.CiCompania.Equals(datosDocumento.IdCompania)
                                         select reteDetalle).ToListAsync();

                compReteDocSus = await (from docSus in _context.CompRetencionDocSustentos
                                        where (docSus.TxEstablecimiento + "-" + docSus.TxPuntoEmision + "-" + docSus.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                              docSus.CiCompania.Equals(datosDocumento.IdCompania)
                                        select docSus).ToListAsync();

                compReteFromPago = await (from docFormPago in _context.CompRetencionFormaPagos
                                          where (docFormPago.TxEstablecimiento + "-" + docFormPago.TxPuntoEmision + "-" + docFormPago.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                                docFormPago.CiCompania.Equals(datosDocumento.IdCompania)
                                          select docFormPago).ToListAsync();
                #endregion

                bytePdf.TipoDoc = "07";
                bytePdf.Documento = ProcesoRideCompRetencion(compRetencion, compReteDetalle, compReteDocSus, compReteFromPago, configuracione, catalogoReporte, catalogoDetalle, dataCompania);
                bytePdf.Cod = "0000";

            }
            catch (Exception ex)
            {
                bytePdf.TipoDoc = "07";
                bytePdf.Documento = "";
                bytePdf.Cod = "9999";

                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("GeneraRideCompRetencion", "GeneraReporteNetcore", ex.Message, null);
            }
            return bytePdf;
        }

        public string ProcesoRideCompRetencion(CompRetencion compRetencion, List<CompRetencionDetalle> compReteDetalle, List<CompRetencionDocSustento> compReteDocSus, List<CompRetencionFormaPago> compReteFromPago,
            List<ConfiguracionReporte> confCompania, List<CatalogoReporte> catalogoSistema, List<DetalleCatalogo> catalogoDetalle, Companium dataCompania)
        {
            var ridePdf = string.Empty;
            var etiquetaContribuyenteEspecial = string.Empty;
            var etiquetaTotalRetenido = string.Empty;
            try
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
                var pathLogoEmpresa = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Select(x => x.Param3).FirstOrDefault();
                var rutaImagen = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("4")).Select(x => x.Param3).FirstOrDefault() + dataCompania.TxRuc.Trim() + ".png";
                #endregion
                #region Genera Codigo Barra
                var claveAcceso = compRetencion.TxClaveAcceso;
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
                #region Configuracion de los separadores de miles
                ConfiguracionReporte confSeparadorMiles = confCompania.Find(x => x.CodigoReferencia == "ACTMIL")!;
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
                ConfiguracionReporte confEtiquetaTotalRetenido = confCompania.Find(x => x.CodigoReferencia == "ETIQTOTRET")!;
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
                #region Datos Tributarios
                var agenteRetencion = dataCompania.TxAgenteRetencion;
                var regimenMicroempresas = dataCompania.TxRegimenMicroempresas;
                var contribuyenteRimpe = dataCompania.TxContribuyenteRimpe;

                var mensajeaAgenteRetencion = catalogoDetalle.Where(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("1")).Select(x => x.Param3).FirstOrDefault();
                var mensajeRegimenMicroempresas = catalogoDetalle.Where(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("2")).Select(x => x.Param3).FirstOrDefault();
                var mensajeContribuyenteRimpe = catalogoDetalle.Where(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("3")).Select(x => x.Param3).FirstOrDefault();

                DataRow drInfoTributaria = tblInfoTributaria.NewRow();
                if (validaAmbiente)
                {
                    CatalogoReporte tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == Convert.ToString(dataCompania.CiTipoAmbiente))!;
                    drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                }
                else
                    drInfoTributaria["ambiente"] = dataCompania.CiTipoAmbiente;
                drInfoTributaria["claveAcceso"] = compRetencion.TxClaveAcceso;
                drInfoTributaria["codDoc"] = compRetencion.CiTipoDocumento;
                drInfoTributaria["dirMartiz"] = dataCompania.TxDireccionMatriz;
                drInfoTributaria["agenteRetencion"] = Convert.ToBoolean(agenteRetencion) ? mensajeaAgenteRetencion : string.Empty;
                drInfoTributaria["regimenMicroempresas"] = Convert.ToBoolean(regimenMicroempresas) ? mensajeRegimenMicroempresas : string.Empty;
                drInfoTributaria["contribuyenteRimpe"] = Convert.ToBoolean(contribuyenteRimpe) ? mensajeContribuyenteRimpe : string.Empty;
                //try { drInfoTributaria["regimenGeneral"] = objRideCompRetencion._infoTributaria.regimenGeneral; } catch { drInfoTributaria["regimenGeneral"] = ""; }
                drInfoTributaria["estab"] = compRetencion.TxEstablecimiento;
                drInfoTributaria["nombreComercial"] = dataCompania.TxNombreComercial;
                drInfoTributaria["ptoEmi"] = compRetencion.TxPuntoEmision;
                drInfoTributaria["razonSocial"] = dataCompania.TxRazonSocial;
                drInfoTributaria["ruc"] = dataCompania.TxRuc;
                drInfoTributaria["secuencial"] = compRetencion.TxSecuencial;

                if (validaEmisionFactura)
                {
                    CatalogoReporte tipoemision = emisionesFacturacion.Find(x => x.Codigo == Convert.ToString(compRetencion.CiTipoEmision))!;
                    drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                }
                else
                    drInfoTributaria["tipoEmision"] = compRetencion.CiTipoEmision;
                tblInfoTributaria.Rows.Add(drInfoTributaria);
                #endregion
                #region Datos de la Retencion
                DataRow drInfoFactura = tblInfoCompRetencion.NewRow();
                drInfoFactura["dirEstablecimiento"] = dataCompania.Sucursals.Where(x => x.CiSucursal.Equals(compRetencion.TxEstablecimiento)).Select(x => x.TxDireccion).FirstOrDefault();
                drInfoFactura["fechaEmision"] = compRetencion.TxFechaEmision;
                if (validaNumeroContribuyenteEspecial)
                    drInfoFactura["contribuyenteEspecial"] = dataCompania.TxContribuyenteEspecial;
                else
                    drInfoFactura["contribuyenteEspecial"] = "";
                drInfoFactura["identificacionSujetoRetenido"] = compRetencion.TxIdentificacionSujetoRetenido;
                drInfoFactura["periodoFiscal"] = compRetencion.TxPeriodoFiscal;
                drInfoFactura["razonSocialSujetoRetenido"] = dataCompania.TxRazonSocial;
                drInfoFactura["obligadoContabilidad"] = dataCompania.TxObligadoContabilidad!.Equals("S")?"SI":"NO";
                drInfoFactura["tipoIdentificacionSujetoRetenido"] = compRetencion.CiTipoIdentificacionSujetoRetenido;
                tblInfoCompRetencion.Rows.Add(drInfoFactura);
                #endregion
                #region Informacion Adicional
                DataRow drInfoAdicional;
                foreach (var item in compRetencion.CompRetencionInfoAdicionals)
                {
                    drInfoAdicional = tblInformacionAdicional.NewRow();
                    drInfoAdicional["nombre"] = item.TxNombre;
                    drInfoAdicional["valor"] = item.TxValor;
                    tblInformacionAdicional.Rows.Add(drInfoAdicional);
                }
                #endregion
                #region Detalle e Impuesto
                List<CatalogoReporte> documentosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                List<CatalogoReporte> impuestosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "IMPRETENC");
                DataRow drImpuesto;
                decimal totalValorRetenido = Convert.ToDecimal("0.00");
                foreach (var item in compReteDetalle)
                {
                    drImpuesto = tblImpuesto.NewRow();

                    try
                    {
                        CatalogoReporte documentoSustento = documentosRetencion.Find(x => x.Codigo == item.TxCodDocSustento)!;
                        CatalogoReporte impuestoRetencion = impuestosRetencion.Find(x => x.Codigo == compReteDocSus.FirstOrDefault()!.TxCodImpuestoDocSustento)!;
                        drImpuesto["codDocSustento"] = documentoSustento.Valor;
                        drImpuesto["codigo"] = impuestoRetencion.Valor;
                    }
                    catch (Exception ex)
                    {
                        drImpuesto["codDocSustento"] = item.TxCodDocSustento;
                        drImpuesto["codigo"] = compReteDocSus.FirstOrDefault()!.TxCodImpuestoDocSustento;
                    }
                    decimal baseImponible = Convert.ToDecimal(compReteDocSus.FirstOrDefault()!.TxBaseImponible.Replace('.', ','));
                    if (validaSeparadorMiles)
                        drImpuesto["baseImponible"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                    else
                        drImpuesto["baseImponible"] = compReteDocSus.FirstOrDefault()!.TxBaseImponible;
                    drImpuesto["codigoRetencion"] = item.TxCodRetencion;
                    drImpuesto["fechaEmisionDocSustento"] = item.TxFechaEmisionDocSustento;
                    drImpuesto["numDocSustento"] = item.TxNumDocSustento;
                    drImpuesto["porcentajeRetener"] = item.QnPorcentajeRetener;
                    decimal valorRetenido = Convert.ToDecimal(item.QnValorRetenido.ToString().Replace('.', ','));
                    if (validaSeparadorMiles)
                        drImpuesto["valorRetenido"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valorRetenido);
                    else
                        drImpuesto["valorRetenido"] = item.QnValorRetenido;

                    totalValorRetenido = totalValorRetenido + Convert.ToDecimal(item.QnValorRetenido.ToString().Replace('.', ','));
                    tblImpuesto.Rows.Add(drImpuesto);
                }
                var txtTotalValorRetenido = "0.00";
                if (validaSeparadorMiles)
                    txtTotalValorRetenido = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalValorRetenido);
                else
                    txtTotalValorRetenido = decimal.Round(totalValorRetenido, 2).ToString();
                #endregion

                #region Genrea Informe RDLC
                var rutaCarpeta = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Param3;
                var rutaRide = Path.Combine(rutaCarpeta!, "RideCompRetencion.rdlc");

                var reportDefinition = new FileStream(rutaRide, FileMode.Open);
                using (var localreport = new LocalReport())
                {
                    localreport.LoadReportDefinition(reportDefinition);                    
                    localreport.EnableExternalImages = true;

                    List<ReportParameter> parameters = new List<ReportParameter>
                    {
                    new ReportParameter("txFechaAutorizacion", compRetencion.TxFechaHoraAutorizacion),
                    new ReportParameter("txNumeroAutorizacion", compRetencion.TxClaveAcceso),
                    new ReportParameter("pathImagenCodBarra", filePath),
                    new ReportParameter("pathLogoCompania", @rutaImagen),
                    new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial),
                    new ReportParameter("etiquetaTotalRetenido", etiquetaTotalRetenido),
                    new ReportParameter("valorTotalRetenido", txtTotalValorRetenido),
                    new ReportParameter("ejercicioFiscal", compRetencion.TxPeriodoFiscal)
                    };

                    localreport.SetParameters(parameters);
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionTributaria", tblInfoTributaria));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionCompRetencion", tblInfoCompRetencion));
                    localreport.DataSources.Add(new ReportDataSource("tblImpuesto", tblImpuesto));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional));

                    var result = localreport.Render("PDF");

                    localreport.Refresh();
                    localreport.Dispose();

                    ridePdf = Convert.ToBase64String(result);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("ProcesoRideCompRetencion", "GeneraReporteNetcore", ex.Message, null);
            }
            return ridePdf;
        }
    }
}
