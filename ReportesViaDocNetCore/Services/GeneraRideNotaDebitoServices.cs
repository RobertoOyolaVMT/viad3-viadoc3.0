using BarcodeStandard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Models;
using SkiaSharp;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace ReportesViaDocNetCore.Services
{
    public class GeneraRideNotaDebitoServices : IGeneraRideNotaDebito
    {
        private readonly FacturacionElectronicaQaContext _context;
        private readonly ICatalogos _catalogos;

        public GeneraRideNotaDebitoServices(FacturacionElectronicaQaContext context, ICatalogos catalogos)
        {
            this._context = context;
            this._catalogos = catalogos;
        }

        public async Task<RespuestaRide> GeneraRideNotaDebito(string txClaveAcceso)
        {
            var bytePdf = new RespuestaRide();
            var datosDocumento = new DatosDocumento();
            var catalogoDetalle = new List<DetalleCatalogo>();
            var dataCompania = new Companium();
            var configuracione = new List<ConfiguracionReporte>();
            var catalogoReporte = new List<CatalogoReporte>();

            var notaDebito = new NotaDebito();
            var notaDebitoMotivo = new List<NotaDebitoMotivo>();

            try
            {
                #region Tbl Catalogo
                datosDocumento = await _catalogos.DatosDocuemntosNotaDebito(txClaveAcceso);
                dataCompania = await _catalogos.Compania(datosDocumento.IdCompania);
                configuracione = await _catalogos.Configuraciones(datosDocumento.IdCompania);
                catalogoReporte = await _catalogos.CatalogoReportes();
                catalogoDetalle = await _catalogos.DetalleCatalogo();
                #endregion
                #region tbl NotaDebito

                notaDebito = await _context.NotaDebitos.Where(nd => (nd.TxEstablecimiento + "-" + nd.TxPuntoEmision + "-" + nd.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          nd.CiCompania.Equals(datosDocumento.IdCompania)).FirstOrDefaultAsync();

                if (notaDebito != null)
                {
                    notaDebito.NotaDebitoImpuestos = await _context.NotaDebitoImpuestos.Where(nd => (nd.TxEstablecimiento + "-" + nd.TxPuntoEmision + "-" + nd.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          nd.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();

                    notaDebito.NotaDebitoInfoAdicionals = await _context.NotaDebitoInfoAdicionals.Where(nd => (nd.TxEstablecimiento + "-" + nd.TxPuntoEmision + "-" + nd.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          nd.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();
                }

                notaDebitoMotivo = await _context.NotaDebitoMotivos.Where(nd => (nd.TxEstablecimiento + "-" + nd.TxPuntoEmision + "-" + nd.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          nd.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();
                #endregion

                bytePdf.TipoDoc = "05";
                bytePdf.Documento = ProcesoRideNotaDebito(notaDebito, notaDebitoMotivo, catalogoDetalle,
                                                configuracione, catalogoReporte, dataCompania);
                bytePdf.Cod = "0000";

            }
            catch (Exception ex)
            {
                bytePdf.TipoDoc = "05";
                bytePdf.Documento = "";
                bytePdf.Cod = "9999";

                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("GeneraRideNotaDebito", "GeneraReporteNetcore", ex.Message, null);
            }
            return bytePdf;
        }

        public string ProcesoRideNotaDebito(NotaDebito notaDebito, List<NotaDebitoMotivo> notaDebitoMotivo, List<DetalleCatalogo> catalogoDetalle,
                                            List<ConfiguracionReporte> confCompania, List<CatalogoReporte> catalogoSistema, Companium dataCompania)
        {
            var ridePdf = string.Empty;
            var etiquetaContribuyenteEspecial = "";
            var etiquetaTipoIvaGrabadoConPorcentaje = "";
            var etiquetaTipoIvaGrabadoSiPorcentaje = "";

            try
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
                        new DataColumn("subTotalCero",typeof(string)),
                        new DataColumn("subTotalNoObjetoIva",typeof(string)),
                        new DataColumn("subTotalExcentoIva",typeof(string)),
                        new DataColumn("subTotalSinImpuesto",typeof(string)),
                        new DataColumn("ice",typeof(string)),
                        new DataColumn("iva",typeof(string)),
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
                var pathLogoEmpresa = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Select(x => x.Param3).FirstOrDefault();
                var rutaImagen = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("4")).Select(x => x.Param3).FirstOrDefault() + dataCompania.TxRuc.Trim() + ".png";
                #endregion
                #region Genera Codigo Barra
                var claveAcceso = notaDebito.TxClaveAcceso;
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
                var confEtiquetaContEspecial = confCompania.Find(x => x.CodigoReferencia == "ETIQCONT");
                if (confEtiquetaContEspecial != null)
                {
                    validaEtiquetaContribuyenteEspecial = Convert.ToBoolean(confEtiquetaContEspecial.Configuracion2.Trim());
                    if (validaEtiquetaContribuyenteEspecial)
                        etiquetaContribuyenteEspecial = confEtiquetaContEspecial.Configuracion1;
                }
                #endregion
                #region Configuracion para visualizar el numero de contribuyente especial
                bool validaNumeroContribuyenteEspecial = false;
                var confContribuEspecial = confCompania.Find(x => x.CodigoReferencia == "ACTNUMCONT");
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
                                DateTime fechaEmision = Convert.ToDateTime(notaDebito.TxFechaEmision);

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

                var mensajeaAgenteRetencion = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("1")).Param3;
                var mensajeRegimenMicroempresas = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("2")).Param3;

                DataRow drInfoTributaria = tblInfoTributaria.NewRow();
                if (validaAmbiente)
                {
                    var tipoAmbiente = ambientesFacturacion.Find(x => x.Codigo == Convert.ToString(dataCompania.CiTipoAmbiente));
                    drInfoTributaria["ambiente"] = tipoAmbiente.Valor;
                }
                else
                    drInfoTributaria["ambiente"] = dataCompania.CiTipoAmbiente;
                drInfoTributaria["claveAcceso"] = notaDebito.TxClaveAcceso;
                drInfoTributaria["codDoc"] = notaDebito.CiTipoDocumento;
                drInfoTributaria["dirMatriz"] = dataCompania.TxDireccionMatriz;
                drInfoTributaria["regimenMicroempresas"] = Convert.ToBoolean(agenteRetencion) ? mensajeaAgenteRetencion : string.Empty;
                drInfoTributaria["agenteRetencion"] = Convert.ToBoolean(regimenMicroempresas) ? mensajeRegimenMicroempresas : string.Empty;
                drInfoTributaria["estab"] = notaDebito.TxEstablecimiento;
                drInfoTributaria["nombreComercial"] = dataCompania.TxNombreComercial;
                drInfoTributaria["ptoEmi"] = notaDebito.TxPuntoEmision;
                drInfoTributaria["razonSocial"] = dataCompania.TxRazonSocial;
                drInfoTributaria["ruc"] = dataCompania.TxRuc;
                drInfoTributaria["secuencial"] = notaDebito.TxSecuencial;
                if (validaEmisionFactura)
                {
                    var tipoemision = emisionesFacturacion.Find(x => x.Codigo == Convert.ToString(notaDebito.CiTipoEmision));
                    drInfoTributaria["tipoEmision"] = tipoemision.Valor;
                }
                else
                    drInfoTributaria["tipoEmision"] = notaDebito.CiTipoEmision;
                tblInfoTributaria.Rows.Add(drInfoTributaria);
                #endregion
                #region Datos de la Nota Debito
                DataRow drInfoNotaDebito = tblInfoNotaDebito.NewRow();
                drInfoNotaDebito["fechaEmision"] = notaDebito.TxFechaEmision;
                drInfoNotaDebito["dirEstablecimiento"] = dataCompania.Sucursals.FirstOrDefault(x => x.CiSucursal.Equals(notaDebito.TxEstablecimiento)).TxDireccion;
                drInfoNotaDebito["tipoIdentificacionComprador"] = notaDebito.CiTipoIdentificacionComprador;
                drInfoNotaDebito["razonSocialComprador"] = dataCompania.TxRazonSocial;
                drInfoNotaDebito["identificacionComprador"] = notaDebito.TxIdentificacionComprador;
                if (validaNumeroContribuyenteEspecial)
                    drInfoNotaDebito["contribuyenteEspecial"] = dataCompania.TxContribuyenteEspecial;
                else
                    drInfoNotaDebito["contribuyenteEspecial"] = "";
                drInfoNotaDebito["obligadoContabilidad"] = dataCompania.TxObligadoContabilidad!.Equals("S") ? "SI" : "NO";
                List<CatalogoReporte> documentosRetencion = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                CatalogoReporte documentoSustento = documentosRetencion.Find(x => x.Codigo == notaDebito.CiTipoDocumentoModificado);
                drInfoNotaDebito["codDocModificado"] = documentoSustento.Valor;
                drInfoNotaDebito["numDocModificado"] = notaDebito.TxNumeroDocumentoModificado;
                drInfoNotaDebito["fechaEmisionDocSustento"] = notaDebito.TxFechaEmisionDocumentoModificado;
                drInfoNotaDebito["totalSinImpuestos"] = notaDebito.QnTotalSinImpuestos;
                drInfoNotaDebito["valorTotal"] = notaDebito.QnValorTotal;
                tblInfoNotaDebito.Rows.Add(drInfoNotaDebito);
                #endregion
                #region Motivo de la Nota Debito
                DataRow drMotivo;
                foreach (var item in notaDebitoMotivo)
                {
                    drMotivo = tblMotivos.NewRow();
                    drMotivo["razon"] = item.TxRazon;
                    decimal valor = Convert.ToDecimal(item.QnValor.ToString().Replace('.', ','));
                    if (validaSeparadorMiles)
                        drMotivo["valor"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valor);
                    else
                        drMotivo["valor"] = item.QnValor;
                    tblMotivos.Rows.Add(drMotivo);
                }
                #endregion
                #region Informacion Adicional
                DataRow drInfoAdicional;
                foreach (var item in notaDebito.NotaDebitoInfoAdicionals)
                {
                    drInfoAdicional = tblInformacionAdicional.NewRow();
                    drInfoAdicional["nombre"] = item.TxNombre;
                    drInfoAdicional["valor"] = item.TxValor;
                    tblInformacionAdicional.Rows.Add(drInfoAdicional);
                }
                #endregion
                #region Monto de la Nota debito
                DataRow dr_tblInformacionMonetaria = tblInformacionMonetaria.NewRow();
                foreach (var objfactTotalImpuesto in notaDebito.NotaDebitoImpuestos)
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
                        else
                            dr_tblInformacionMonetaria["subTotalCero"] = "0.00";
                        #endregion
                        #region SubTotal Iva 12
                        if (String.Compare(objfactTotalImpuesto.TxCodigoPorcentaje.Trim(), catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(22) && x.Param1.Equals("6")).Param3) == 0)
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
                                dr_tblInformacionMonetaria["subTotalIva"] = "0.00";
                                dr_tblInformacionMonetaria["iva"] = "0.00";
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
                        decimal irbpnr = Convert.ToDecimal(objfactTotalImpuesto.QnValor.ToString().Replace('.', ','));
                        if (validaSeparadorMiles)
                            dr_tblInformacionMonetaria["irbpnr"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, irbpnr);
                        else
                            dr_tblInformacionMonetaria["irbpnr"] = "0.00";
                    }
                    else
                        dr_tblInformacionMonetaria["irbpnr"] = "0.00";

                    dr_tblInformacionMonetaria["irbpnr"] = "0.00";
                }

                decimal totalSinImpuesto = Convert.ToDecimal(notaDebito.QnTotalSinImpuestos.ToString().Replace('.', ','));
                decimal valorTotal = Convert.ToDecimal(notaDebito.QnValorTotal.ToString().Replace('.', ','));
                if (validaSeparadorMiles)
                {
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, totalSinImpuesto);
                    dr_tblInformacionMonetaria["valorTotal"] = String.Format(CultureInfo.InvariantCulture, formatoMiles, valorTotal);
                }
                else
                {
                    dr_tblInformacionMonetaria["subTotalSinImpuesto"] = notaDebito.QnTotalSinImpuestos;
                    dr_tblInformacionMonetaria["valorTotal"] = notaDebito.QnValorTotal;
                }
                tblInformacionMonetaria.Rows.Add(dr_tblInformacionMonetaria);
                #endregion

                #region Reporte RDLC
                etiquetaTipoIvaGrabadoSiPorcentaje = etiquetaTipoIvaGrabadoSiPorcentaje.Contains("%") ? etiquetaTipoIvaGrabadoSiPorcentaje : etiquetaTipoIvaGrabadoSiPorcentaje + " %";

                var rutaCarpeta = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Param3;
                var rutaRide = Path.Combine(rutaCarpeta!, "RideNotaDebito.rdlc");

                var reportDefinition = new FileStream(rutaRide, FileMode.Open);
                using (var localreport = new LocalReport())
                {
                    localreport.LoadReportDefinition(reportDefinition);
                    localreport.EnableExternalImages = true;

                    List<ReportParameter> parameters = new List<ReportParameter>
                    {
                    new ReportParameter("txFechaAutorizacion", notaDebito.TxFechaHoraAutorizacion),
                    new ReportParameter("txNumeroAutorizacion", notaDebito.TxClaveAcceso),
                    new ReportParameter("pathImagenCodBarra", filePath),
                    new ReportParameter("pathLogoCompania", @rutaImagen),
                    new ReportParameter("tarifaIva", etiquetaTipoIvaGrabadoSiPorcentaje),
                    new ReportParameter("etiquetaTarifaIva", etiquetaTipoIvaGrabadoConPorcentaje),
                    new ReportParameter("campoNumContribuyenteEspecial", etiquetaContribuyenteEspecial)
                    };

                    localreport.SetParameters(parameters);
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionTributaria", tblInfoTributaria));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionNotaDebito", tblInfoNotaDebito));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionMonetaria", tblInformacionMonetaria));
                    localreport.DataSources.Add(new ReportDataSource("tblInformacionAdicional", tblInformacionAdicional));
                    localreport.DataSources.Add(new ReportDataSource("tblMotivos", tblMotivos));

                    var result = localreport.Render("PDF");

                    localreport.Refresh();
                    localreport.Dispose();

                    ridePdf = Convert.ToBase64String(result);
                }
                #endregion

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("ProcesoRideNotaDebito", "GeneraReporteNetcore", ex.Message, null);
            }
            return ridePdf;
        }
    }
}
