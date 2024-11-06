using BarcodeStandard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Models;
using SkiaSharp;
using System.Data;
using System.Globalization;

namespace ReportesViaDocNetCore.Services
{
    public class GeneraRideNotaCreditoServices : IGeneraRideNotaCredito
    {
        private readonly FacturacionElectronicaQaContext _context;
        private readonly ICatalogos _catalogos;

        public GeneraRideNotaCreditoServices(FacturacionElectronicaQaContext context, ICatalogos catalogos)
        {
            this._context = context;
            this._catalogos = catalogos;
        }

        public async Task<RespuestaRide> GeneraRideNotaCredito(string claveAcceso)
        {
            var bytePdf = new RespuestaRide();
            var datosDocumento = new DatosDocumento();
            var catalogoDetalle = new List<DetalleCatalogo>();
            var dataCompania = new Companium();
            var configuracione = new List<ConfiguracionReporte>();
            var catalogoReporte = new List<CatalogoReporte>();

            var notaCredito = new NotaCredito();
            var notaCreditoDetaAdicional = new List<NotaCreditoDetalleAdicional>();
            var notaCreditoDetaImpuesto = new List<NotaCreditoDetalleImpuesto>();

            try
            {
                #region Tbl Catalogo
                datosDocumento = await _catalogos.DatosDocuemntosNotaCredito(claveAcceso);
                dataCompania = await _catalogos.Compania(datosDocumento.IdCompania);
                configuracione = await _catalogos.Configuraciones(datosDocumento.IdCompania);
                catalogoReporte = await _catalogos.CatalogoReportes();
                catalogoDetalle = await _catalogos.DetalleCatalogo();
                #endregion
                #region tbl NotaCredito
                notaCredito = await (from notaCred in _context.NotaCreditos
                                     where (notaCred.TxEstablecimiento + "-" + notaCred.TxPuntoEmision + "-" + notaCred.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                           notaCred.CiCompania.Equals(datosDocumento.IdCompania)
                                     select notaCred).FirstOrDefaultAsync();
                if (notaCredito != null)
                {
                    notaCredito.NotaCreditoDetalles = await (from notaCredDet in _context.NotaCreditoDetalles
                                                             where (notaCredDet.TxEstablecimiento + "-" + notaCredDet.TxPuntoEmision + "-" + notaCredDet.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                                                   notaCredDet.CiCompania.Equals(datosDocumento.IdCompania)
                                                             select notaCredDet).ToListAsync();

                    notaCredito.NotaCreditoInfoAdicionals = await (from notaCredAdi in _context.NotaCreditoInfoAdicionals
                                                                   where (notaCredAdi.TxEstablecimiento + "-" + notaCredAdi.TxPuntoEmision + "-" + notaCredAdi.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                                                         notaCredAdi.CiCompania.Equals(datosDocumento.IdCompania)
                                                                   select notaCredAdi).ToListAsync();

                    notaCredito.NotaCreditoTotalImpuestos = await (from notaCredTotalImp in _context.NotaCreditoTotalImpuestos
                                                                   where (notaCredTotalImp.TxEstablecimiento + "-" + notaCredTotalImp.TxPuntoEmision + "-" + notaCredTotalImp.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                                                         notaCredTotalImp.CiCompania.Equals(datosDocumento.IdCompania)
                                                                   select notaCredTotalImp).ToListAsync();
                }

                notaCreditoDetaAdicional = await (from notaCredTotalImp in _context.NotaCreditoDetalleAdicionals
                                                  where (notaCredTotalImp.TxEstablecimiento + "-" + notaCredTotalImp.TxPuntoEmision + "-" + notaCredTotalImp.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                                        notaCredTotalImp.CiCompania.Equals(datosDocumento.IdCompania)
                                                  select notaCredTotalImp).ToListAsync();

                notaCreditoDetaImpuesto = await (from notaCredTotalImp in _context.NotaCreditoDetalleImpuestos
                                                 where (notaCredTotalImp.TxEstablecimiento + "-" + notaCredTotalImp.TxPuntoEmision + "-" + notaCredTotalImp.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                                       notaCredTotalImp.CiCompania.Equals(datosDocumento.IdCompania)
                                                 select notaCredTotalImp).ToListAsync();
                #endregion

                bytePdf.TipoDoc = "04";
                bytePdf.Documento = ProcesoRideNotaCredito(notaCredito, notaCreditoDetaAdicional, notaCreditoDetaImpuesto,
                                                 catalogoDetalle, configuracione, catalogoReporte, dataCompania);
                bytePdf.Cod = "0000";

            }
            catch (Exception ex)
            {
                bytePdf.TipoDoc = "04";
                bytePdf.Documento = "";
                bytePdf.Cod = "9999";

                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("GeneraRideNotaCredito", "GeneraReporteNetcore", ex.Message, null);
            }
            return bytePdf;
        }

        public string ProcesoRideNotaCredito(NotaCredito notaCredito, List<NotaCreditoDetalleAdicional> notaCreditoDetaAdicional, List<NotaCreditoDetalleImpuesto> notaCreditoDetaImpuesto,
            List<DetalleCatalogo> catalogoDetalle, List<ConfiguracionReporte> confCompania, List<CatalogoReporte> catalogoSistema, Companium dataCompania)
        {
            var ridePdf = string.Empty;
            var etiquetaContribuyenteEspecial = string.Empty;
            var etiquetaTipoIvaGrabadoConPorcentaje = string.Empty;
            var etiquetaTipoIvaGrabadoSiPorcentaje = string.Empty;

            try
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
                        new DataColumn("tipoEmision",typeof(string)),
                    //new DataColumn("regimenGeneral",typeof(string))
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
                        new DataColumn("precioUnitario",typeof(string)),
                    };

                DataColumn[] cols_tblInformacionAdicional = new DataColumn[] {
                        new DataColumn("nombre",typeof(string)),
                        new DataColumn("valor",typeof(string))
                    };


                DataColumn[] cols_tblInformacionMonetaria = new DataColumn[] {
                        new DataColumn("subTotalIva",typeof(string)),
                        new DataColumn("subTotalCero",typeof(string)),
                        new DataColumn("subTotalNoObjetoIva",typeof(string)),
                        new DataColumn("subTotalExcentoIva",typeof(string)),
                        new DataColumn("subTotalSinImpuesto",typeof(string)),
                        new DataColumn("totalDescuento",typeof(string)),
                        new DataColumn("ice",typeof(string)),
                        new DataColumn("iva",typeof(string)),
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
                var pathLogoEmpresa = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Select(x => x.Param3).FirstOrDefault();
                var rutaImagen = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("4")).Select(x => x.Param3).FirstOrDefault() + dataCompania.TxRuc.Trim() + ".png";
                #endregion
                #region Genera Codigo Barra
                var claveAcceso = notaCredito.TxClaveAcceso;
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
                #region Configuracion de los separadores
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
                var tipoIvaGrabado = catalogoSistema.Find(x => x.CodigoReferencia == "IVAGRAB");//Catalogo Tipo de Iva para las nuevas empresas que inician con facturacion electronica
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
                        var fechaSalvaguardiaIva14 = catalogoSistema.Find(x => x.CodigoReferencia == "SALVAGUARD");
                        if (fechaSalvaguardiaIva14 != null)
                        {
                            try
                            {

                                string[] fechaIni = fechaSalvaguardiaIva14.Codigo.Split('/');
                                string[] fechaFin = fechaSalvaguardiaIva14.Valor.Split('/');
                                DateTime fechaInicioSalvaguardia = new DateTime(Convert.ToInt32(fechaIni[2]), Convert.ToInt32(fechaIni[1]), Convert.ToInt32(fechaIni[0]));
                                DateTime fechaFinSalvaguardia = new DateTime(Convert.ToInt32(fechaFin[2]), Convert.ToInt32(fechaFin[1]), Convert.ToInt32(fechaFin[0]));
                                DateTime fechaEmision = Convert.ToDateTime(notaCredito.TxFechaEmision);

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

                drInfoTributaria["claveAcceso"] = notaCredito.TxClaveAcceso;
                drInfoTributaria["codDoc"] = notaCredito.CiTipoDocumento;
                drInfoTributaria["dirMatriz"] = dataCompania.TxDireccionMatriz;
                drInfoTributaria["regimenMicroempresas"] = Convert.ToBoolean(agenteRetencion) ? mensajeaAgenteRetencion : string.Empty;
                drInfoTributaria["agenteRetencion"] = Convert.ToBoolean(regimenMicroempresas) ? mensajeRegimenMicroempresas : string.Empty;
                drInfoTributaria["contribuyenteRimpe"] = Convert.ToBoolean(contribuyenteRimpe) ? mensajeContribuyenteRimpe : string.Empty;
                drInfoTributaria["estab"] = notaCredito.TxEstablecimiento;
                drInfoTributaria["nombreComercial"] = dataCompania.TxNombreComercial;
                drInfoTributaria["ptoEmi"] = notaCredito.TxPuntoEmision;
                drInfoTributaria["razonSocial"] = dataCompania.TxRazonSocial;
                drInfoTributaria["ruc"] = dataCompania.TxRuc;
                drInfoTributaria["secuencial"] = notaCredito.TxSecuencial;

                if (validaEmisionFactura)
                {
                    CatalogoReporte tipoemision = emisionesFacturacion.Find(x => x.Codigo == Convert.ToString(notaCredito.CiTipoEmision))!;
                    drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                }
                else
                    drInfoTributaria["tipoEmision"] = notaCredito.CiTipoEmision;
                tblInfoTributaria.Rows.Add(drInfoTributaria);
                #endregion
                #region Datos de la Nota Credito
                DataRow drInfoNotaCredito = tblInfoNotaCredito.NewRow();
                var documentosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                var documentoSustento = documentosRetencion.Find(x => x.Codigo == notaCredito.CiTipoDocumentoModificado);

                drInfoNotaCredito["codDocModificado"] = documentoSustento.Valor;
                drInfoNotaCredito["obligadoContabilidad"] = dataCompania.TxObligadoContabilidad;
                if (validaNumeroContribuyenteEspecial)
                    drInfoNotaCredito["contribuyenteEspecial"] = dataCompania.TxContribuyenteEspecial;
                else
                    drInfoNotaCredito["contribuyenteEspecial"] = "";

                drInfoNotaCredito["dirEstablecimiento"] = dataCompania.Sucursals.FirstOrDefault(x => x.CiSucursal.Equals(notaCredito.TxEstablecimiento)).TxDireccion;
                drInfoNotaCredito["fechaEmision"] = notaCredito.TxFechaEmision;
                drInfoNotaCredito["fechaEmisionDocSustento"] = notaCredito.TxFechaEmisionDocumentoModificado;
                drInfoNotaCredito["identificacionComprador"] = notaCredito.TxIdentificacionComprador;
                drInfoNotaCredito["moneda"] = notaCredito.TxMoneda;
                drInfoNotaCredito["motivo"] = notaCredito.TxMotivo;
                drInfoNotaCredito["numDocModificado"] = notaCredito.TxNumeroDocumentoModificado;
                drInfoNotaCredito["razonSocialComprador"] = dataCompania.TxRazonSocial;
                drInfoNotaCredito["tipoIdentificacionComprador"] = notaCredito.CiTipoIdentificacionComprador;
                drInfoNotaCredito["totalSinImpuestos"] = notaCredito.QnTotalSinImpuestos;
                drInfoNotaCredito["valorModificacion"] = notaCredito.QnValorModificacion;
                tblInfoNotaCredito.Rows.Add(drInfoNotaCredito);

                #endregion
                #region Detalle de la Nota Credito
                DataRow drDetalle;
                foreach (var item in notaCredito.NotaCreditoDetalles)
                {
                    drDetalle = tblDetalleNotaCredito.NewRow();
                    drDetalle["cantidad"] = item.QnCantidad;
                    drDetalle["codigoAdicional"] = item.TxCodigoAdicional;
                    drDetalle["codigoInterno"] = item.TxCodigoInterno;
                    drDetalle["descripcion"] = item.TxDescripcion;
                    decimal descuento = Convert.ToDecimal(item.QnDescuento.ToString().Replace('.', ','));
                    decimal precioTotalSinImp = Convert.ToDecimal(item.QnPrecioTotalSinImpuesto.ToString().Replace('.', ','));
                    decimal precioUnitario = Convert.ToDecimal(item.QnPrecioUnitario.ToString().Replace('.', ','));
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
                    drDetalle["descuentoEspecifico"] = "0.00";
                    tblDetalleNotaCredito.Rows.Add(drDetalle);
                }
                #endregion
                #region Informacion Adicional
                DataRow drInfoAdicional;
                foreach (var item in notaCredito.NotaCreditoInfoAdicionals)
                {
                    drInfoAdicional = tblInformacionAdicional.NewRow();
                    drInfoAdicional["nombre"] = item.TxNombre;
                    drInfoAdicional["valor"] = item.TxValor;
                    tblInformacionAdicional.Rows.Add(drInfoAdicional);
                }
                #endregion
                #region Monto de la Nota de Credito
                DataRow dr_tblInformacionMonetaria = tblInformacionMonetaria.NewRow();

                foreach (var objfactTotalImpuesto in notaCredito.NotaCreditoTotalImpuestos)
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
                                dr_tblInformacionMonetaria["subTotalCero"] = objfactTotalImpuesto.QnBaseImponible.ToString().ToString();
                        }
                        //else
                        //    dr_tblInformacionMonetaria["subTotalCero"] = "0.00";
                        #endregion
                        #region SubTotal 5
                        if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("5")).Param3) == 0)
                        {
                            decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                            decimal valor = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));
                            var subtotalIvaCinco = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible);
                            var ivaCinco = valor;

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
                            if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.Where(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("3")).Select(x => x.Param3).FirstOrDefault()) == 0)
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
                                decimal total = Convert.ToDecimal(notaCredito.QnTotalSinImpuestos.ToString().Replace('.', ','));
                                decimal baseImponible = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                                decimal valor = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));

                                if (validaSeparadorMiles)
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, baseImponible);
                                    dr_tblInformacionMonetaria["iva"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                                    var subtotalIvaGrab = baseImponible;
                                }
                                else
                                {
                                    dr_tblInformacionMonetaria["subTotalIva"] = objfactTotalImpuesto.QnBaseImponible;
                                    dr_tblInformacionMonetaria["iva"] = objfactTotalImpuesto.QnValor;
                                    var subtotalIvaGrab = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible);
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
                        decimal ice = Convert.ToDecimal(objfactTotalImpuesto.QnBaseImponible.ToString().Replace('.', ','));
                        if (validaSeparadorMiles)
                            dr_tblInformacionMonetaria["ice"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, ice);
                        else
                            dr_tblInformacionMonetaria["ice"] = objfactTotalImpuesto.QnValor;
                    }
                    else
                        dr_tblInformacionMonetaria["ice"] = "0.00";

                    if (String.Compare(objfactTotalImpuesto.TxCodigo.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("9")).Param3) == 0)
                    {
                        decimal irbpnr = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));
                        if (validaSeparadorMiles)
                            dr_tblInformacionMonetaria["irbpnr"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, irbpnr);
                        else
                            dr_tblInformacionMonetaria["irbpnr"] = "0";
                    }
                    else
                        dr_tblInformacionMonetaria["irbpnr"] = "0.00";

                    dr_tblInformacionMonetaria["irbpnr"] = "0.00";
                }

                decimal totalSinImpuesto = Convert.ToDecimal(notaCredito.QnTotalSinImpuestos.ToString().Replace('.', ','));
                decimal totalDescuento = Convert.ToDecimal("0,00");
                decimal valorTotal = Convert.ToDecimal(notaCredito.QnValorModificacion.ToString().Replace('.', ','));

                if (validaSeparadorMiles)
                {
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalSinImpuesto);
                    dr_tblInformacionMonetaria["totalDescuento"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalDescuento);
                    dr_tblInformacionMonetaria["valorTotal"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valorTotal);
                }
                else
                {
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = notaCredito.QnTotalSinImpuestos;
                    dr_tblInformacionMonetaria["totalDescuento"] = "0.00";
                    dr_tblInformacionMonetaria["valorTotal"] = notaCredito.QnValorModificacion;
                }
                tblInformacionMonetaria.Rows.Add(dr_tblInformacionMonetaria);
                #endregion

                #region Reporte RDLC
                etiquetaTipoIvaGrabadoSiPorcentaje = etiquetaTipoIvaGrabadoSiPorcentaje.Contains("%") ? etiquetaTipoIvaGrabadoSiPorcentaje : etiquetaTipoIvaGrabadoSiPorcentaje + " %";

                var rutaCarpeta = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Param3;
                var rutaRide = Path.Combine(rutaCarpeta!, "RideNotaCredito.rdlc");

                var reportDefinition = new FileStream(rutaRide, FileMode.Open);
                using (var localreport = new LocalReport())
                {
                    localreport.LoadReportDefinition(reportDefinition);
                    localreport.EnableExternalImages = true;

                    List<ReportParameter> parameters = new List<ReportParameter>
                    {
                    new ReportParameter("txFechaAutorizacion", notaCredito.TxFechaHoraAutorizacion),
                    new ReportParameter("txNumeroAutorizacion", notaCredito.TxClaveAcceso),
                    new ReportParameter("pathImagenCodBarra", filePath),
                    new ReportParameter("pathLogoCompania", @rutaImagen),
                    new ReportParameter("tarifaIva", etiquetaTipoIvaGrabadoSiPorcentaje),
                    new ReportParameter("etiquetaTarifaIva", etiquetaTipoIvaGrabadoConPorcentaje),
                    new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial)
                    };

                    localreport.SetParameters(parameters);
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionTributaria", tblInfoTributaria));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionNotaCredito", tblInfoNotaCredito));
                    localreport.DataSources.Add(new ReportDataSource("tblDetalle", tblDetalleNotaCredito));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionMonetaria", tblInformacionMonetaria));

                    var result = localreport.Render("PDF");

                    localreport.Refresh();
                    localreport.Dispose();

                    ridePdf = Convert.ToBase64String(result);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("ProcesoRideNotaCredito", "GeneraReporteNetcore", ex.Message, null);
            }
            return ridePdf;
        }
    }
}
