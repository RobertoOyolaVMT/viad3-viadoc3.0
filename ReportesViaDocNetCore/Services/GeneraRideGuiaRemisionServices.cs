using BarcodeStandard;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using ReportesViaDocNetCore.EntidadesReporte;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Models;
using SkiaSharp;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Services
{
    public class GeneraRideGuiaRemisionServices : IGeneraRideGuiaRemision
    {
        private readonly FacturacionElectronicaQaContext _context;
        private readonly ICatalogos _catalogos;

        public GeneraRideGuiaRemisionServices(FacturacionElectronicaQaContext context, ICatalogos catalogos)
        {
            this._context = context;
            this._catalogos = catalogos;
        }

        public async Task<RespuestaRide> GeneraRideGuiaRemision(string txClaveAcceso)
        {
            var bytePdf = new RespuestaRide();
            var datosDocumento = new DatosDocumento();
            var catalogoDetalle = new List<DetalleCatalogo>();
            var dataCompania = new Companium();
            var configuracione = new List<ConfiguracionReporte>();
            var catalogoReporte = new List<CatalogoReporte>();

            var guiaRemisionRide = new GuiasRemisionRide();

            try
            {
                #region Tbl Catalogo
                datosDocumento = await _catalogos.DatosDocuemntosNotaDebito(txClaveAcceso);
                dataCompania = await _catalogos.Compania(datosDocumento.IdCompania);
                configuracione = await _catalogos.Configuraciones(datosDocumento.IdCompania);
                catalogoReporte = await _catalogos.CatalogoReportes();
                catalogoDetalle = await _catalogos.DetalleCatalogo();
                #endregion

                #region tbl GuiaRemision
                guiaRemisionRide.guiaRemision = await _context.GuiaRemisions.Where(gr => (gr.TxEstablecimiento + "-" + gr.TxPuntoEmision + "-" + gr.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          gr.CiCompania.Equals(datosDocumento.IdCompania)).FirstOrDefaultAsync();
                if (guiaRemisionRide.guiaRemision != null)
                {
                    guiaRemisionRide.guiaRemision.GuiaRemisionDestinatarios = await _context.GuiaRemisionDestinatarios.Where(gr => (gr.TxEstablecimiento + "-" + gr.TxPuntoEmision + "-" + gr.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          gr.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();

                    guiaRemisionRide.guiaRemision.GuiaRemisionInfoAdicionals = await _context.GuiaRemisionInfoAdicionals.Where(gr => (gr.TxEstablecimiento + "-" + gr.TxPuntoEmision + "-" + gr.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          gr.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();
                }
                guiaRemisionRide.guiaRemisionDetalleAdicional = await _context.GuiaRemisionDestinatarioDetalleAdicionals.Where(gr => (gr.TxEstablecimiento + "-" + gr.TxPuntoEmision + "-" + gr.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          gr.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();

                guiaRemisionRide.guiaRemisionDestinatarioDetalle = await _context.GuiaRemisionDestinatarioDetalles.Where(gr => (gr.TxEstablecimiento + "-" + gr.TxPuntoEmision + "-" + gr.TxSecuencial).Equals(datosDocumento.NumDoc) &&
                                          gr.CiCompania.Equals(datosDocumento.IdCompania)).ToListAsync();
                #endregion

                bytePdf.TipoDoc = "07";
                bytePdf.Documento = ProcesoRideGuiaRemision(guiaRemisionRide,  catalogoDetalle, configuracione, catalogoReporte, dataCompania);
                bytePdf.Cod = "0000";
            }
            catch (Exception ex)
            {
                bytePdf.TipoDoc = "07";
                bytePdf.Documento = "";
                bytePdf.Cod = "9999";

                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("GeneraRideGuiaRemision", "GeneraReporteNetcore", ex.Message, null);
            }
            return bytePdf;
        }

        public string ProcesoRideGuiaRemision(GuiasRemisionRide guiaRemisionRide, List<DetalleCatalogo> catalogoDetalle, List<ConfiguracionReporte> confCompania, List<CatalogoReporte> catalogoSistema, Companium dataCompania)
        {
            var ridePdf = string.Empty;
            var etiquetaContribuyenteEspecial = string.Empty;
            try
            {
                #region Configuracion de los separadores de miles de la Guia de remision
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
                #region Catalogo Ambiente Guia Remision
                var ambientesGuia = catalogoSistema.FindAll(x => x.CodigoReferencia == "AMBIENTE");
                bool validaAmbiente = false;
                if (ambientesGuia != null)
                    validaAmbiente = true;
                #endregion
                #region Catalogo Emisiones Guia Remision
                var emisionesGui = catalogoSistema.FindAll(x => x.CodigoReferencia == "EMISION");
                bool validaEmisionFactura = false;
                if (emisionesGui != null)
                    validaEmisionFactura = true;
                #endregion
                #region Logo de la Compania
                var pathLogoEmpresa = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("2")).Select(x => x.Param3).FirstOrDefault();
                var rutaImagen = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("4")).Select(x => x.Param3).FirstOrDefault() + dataCompania.TxRuc.Trim() + ".png";
                var imagenNombre = dataCompania.TxRuc.Trim();
                var nuevaRuta = string.Empty;
                if (!Directory.Exists(rutaImagen))
                {
                    rutaImagen = pathLogoEmpresa + imagenNombre + ".png";
                }
                #endregion
                #region Genera Codigo Barra 
                var claveAcceso = guiaRemisionRide.guiaRemision!.TxClaveAcceso;
                var raizRutaCodeBar = catalogoDetalle.Where(x => x.CiCatalogo.Equals(21) && x.Param1.Equals("3")).Select(x => x.Param3).FirstOrDefault();
                var filePath = Path.Combine(raizRutaCodeBar!, $"{claveAcceso}.jpg");

                BarcodeStandard.Barcode barcode = new BarcodeStandard.Barcode();
                barcode.IncludeLabel = false;
                SKImage codigoDeBarras = barcode.Encode(BarcodeStandard.Type.Code128, claveAcceso, SKColors.Black, SKColors.White, 700, 100);
                using (var data = codigoDeBarras.Encode(SKEncodedImageFormat.Jpeg, 100))
                using (var stream = File.OpenWrite(filePath))
                {
                    data.SaveTo(stream);
                }
                #endregion

                #region Construye Reporte
                MemoryStream memoryStream = new MemoryStream();
                Document documentoPdf = new Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                PdfWriter writer = PdfWriter.GetInstance(documentoPdf, memoryStream);
                documentoPdf.Open();
                PdfContentByte cb = writer.DirectContent;

                float x_infoCompania = 30f; // de derecha a izquierda
                float y_infoCompania = 580f; // de abajo a arriba
                float w_infoCompania = 273f; // ancho
                float h_infoCompania = 120f; // altura de abaja hacia arriba 120
                cb.Rectangle(x_infoCompania, y_infoCompania, w_infoCompania, h_infoCompania);
                cb.Stroke();

                EscribirTextoReporte(cb, dataCompania.TxRazonSocial, true, 11, 35, 682);
                PdfPTable tbTributario = new PdfPTable(2);
                tbTributario.WidthPercentage = 50;
                float[] widthstbTributario = new float[] { 50f, 218f }; //Declaramos un array con los tamaños de nuestras columnas deben de coincidir con el tamaño de columna
                tbTributario.SetTotalWidth(widthstbTributario);
                PdfPCell celdaDireccion = EscribirCeldaReporte("Direccion Matriz:", 1, 1, 9, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", true);
                tbTributario.AddCell(celdaDireccion);
                PdfPCell celda_Dieccion = EscribirCeldaReporte(dataCompania.TxDireccionMatriz, 1, 1, 8, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", false);
                tbTributario.AddCell(celda_Dieccion);
                PdfPCell celdaSucursal = EscribirCeldaReporte("Direccion Sucursal:", 1, 1, 9, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", true);
                tbTributario.AddCell(celdaSucursal);
                PdfPCell celda_Sucursal = EscribirCeldaReporte(dataCompania.Sucursals.FirstOrDefault(x =>x.CiSucursal.Equals(guiaRemisionRide.guiaRemision.TxEstablecimiento)).TxDireccion!, 1, 1, 8, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", false);
                tbTributario.AddCell(celda_Sucursal);
                tbTributario.WriteSelectedRows(0, -1, 35, 699 - 20, cb);

                if (!validaNumeroContribuyenteEspecial)
                {
                    EscribirTextoReporte(cb, "Contribuyente Especial Nro:", true, 9, 35, 625);
                    EscribirTextoReporte(cb, dataCompania.TxContribuyenteEspecial, false, 9, 180, 612);
                }

                EscribirTextoReporte(cb, "OBLIGADO A LLEVAR CONTABILIDAD:", true, 9, 35, 610);
                EscribirTextoReporte(cb, dataCompania.TxObligadoContabilidad, false, 9, 210, 610);

                var agenteRetencion = dataCompania.TxAgenteRetencion;
                var regimenMicroempresas = dataCompania.TxRegimenMicroempresas;
                var contribuyenteRimpe = dataCompania.TxContribuyenteRimpe;

                var mensajeaAgenteRetencion = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("1")).Param3;
                var mensajeRegimenMicroempresas = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("2")).Param3;
                var mensajeContribuyenteRimpe = catalogoDetalle.FirstOrDefault(x => x.CiCatalogo.Equals(23) && x.Param1.Equals("3")).Param3;

                EscribirTextoReporte(cb, Convert.ToBoolean(regimenMicroempresas) ? mensajeRegimenMicroempresas : string.Empty, false, 9, 35, 598);
                EscribirTextoReporte(cb, Convert.ToBoolean(agenteRetencion) ? mensajeaAgenteRetencion : string.Empty, false, 9, 35, 586);

                iTextSharp.text.Image imgpfd = iTextSharp.text.Image.GetInstance(@rutaImagen);

                imgpfd.SetAbsolutePosition(40, 725); //Posicion en el eje carteciano de X y Y; VALOR DE Y MAS ALTO LA IMAGEN SUBE; X MAS ALTO LA IMAGEN VA A LA DERECHA WJ
                imgpfd.ScaleAbsolute(250, 80);//Ancho y altura de la imagen
                documentoPdf.Add(imgpfd); // Agrega la imagen al documento------------ok

                #region Zona de la Informacion tributaria de la Guia de Remision
                float x_infoTributaria = 310f; // de derecha a izquierda
                float y_infoTributaria = 580f; // de abajo a arriba
                float w_infoTributaria = 250; // ancho
                float h_infoTributaria = 230; // altura de abaja hacia arriba, mientra mayor sea el valor de h mas alto crecera de abaja hacia arriba
                cb.Rectangle(x_infoTributaria, y_infoTributaria, w_infoTributaria, h_infoTributaria);
                cb.Stroke();

                EscribirTextoReporte(cb, "RUC.:", true, 9, 320, 793);
                EscribirTextoReporte(cb, dataCompania.TxRuc, false, 9, 357, 793);
                EscribirTextoReporte(cb, "GUIA  DE  REMISIÓN", true, 12, 320, 776);
                EscribirTextoReporte(cb, "No.", true, 9, 320, 762);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxEstablecimiento + "-" + guiaRemisionRide.guiaRemision.TxPuntoEmision + "-" + guiaRemisionRide.guiaRemision.TxSecuencial, false, 9, 354, 762);
                EscribirTextoReporte(cb, "NUMERO DE AUTORIZACION", true, 9, 320, 748);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxNumeroAutorizacion, false, 8, 320, 735);
                EscribirTextoReporte(cb, "FECHA Y HORA", true, 9, 320, 720);
                EscribirTextoReporte(cb, "DE", true, 9, 320, 710);
                EscribirTextoReporte(cb, "AUTORIZACION", true, 9, 320, 700);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxFechaHoraAutorizacion!, false, 9, 398, 710);
                EscribirTextoReporte(cb, "AMBIENTE:", true, 9, 320, 685);

                if (validaAmbiente)
                {  //aquii se cae-------->
                    CatalogoReporte tipoAmbiente = ambientesGuia.Find(x => x.Codigo == dataCompania.CiTipoAmbiente.ToString());
                    EscribirTextoReporte(cb, tipoAmbiente.Valor, false, 8, 373, 685);
                }
                else
                    EscribirTextoReporte(cb, dataCompania.CiTipoAmbiente.ToString(), false, 8, 373, 685);
                EscribirTextoReporte(cb, "EMISION:", true, 9, 320, 670);
                if (validaEmisionFactura)
                {
                    CatalogoReporte tipoemision = emisionesGui.Find(x => x.Codigo == guiaRemisionRide.guiaRemision.CiTipoEmision.ToString());
                    EscribirTextoReporte(cb, tipoemision.Valor, false, 8, 373, 670);
                }
                else
                    EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.CiTipoEmision.ToString(), true, 9, 373, 670);
                EscribirTextoReporte(cb, "CLAVE DE ACCESO", true, 9, 320, 655);
                string rutaCodigoBarra = filePath;
                iTextSharp.text.Image imgCodigoBarra = iTextSharp.text.Image.GetInstance(@rutaCodigoBarra);
                imgCodigoBarra.SetAbsolutePosition(317, 612); //Posicion en el eje carteciano de X y Y; VALOR DE Y MAS ALTO LA IMAGEN SUBE; X MAS LATO LA IMAGE VA A LA DERECHA WJ
                imgCodigoBarra.ScaleAbsolute(240, 40);//Ancho y altura de la imagen
                documentoPdf.Add(imgCodigoBarra); // Agrega la imagen al documento
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxClaveAcceso, false, 8, 320, 600);
                #endregion

                #region Zona de la informacion del transportista
                float x_infoTransportista = 30f; // de derecha a izquierda
                float y_infoTransportista = 500f; // de abajo a arriba
                float w_infoTransportista = 532f; // ancho
                float h_infoTransportista = 70f; // altura de abaja hacia arriba
                cb.Rectangle(x_infoTransportista, y_infoTransportista, w_infoTransportista, h_infoTransportista);
                cb.Stroke();

                EscribirTextoReporte(cb, "Identificación (Transportista):", true, 9, 35, 555);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxRucTransportista, false, 9, 170, 555);
                EscribirTextoReporte(cb, "Razón Social / Nombres y Apellidos:", true, 9, 35, 543);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxRazonSocialTransportista, false, 9, 210, 543);
                EscribirTextoReporte(cb, "Placa:", true, 9, 35, 531);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxPlaca, false, 9, 75, 531);
                EscribirTextoReporte(cb, "Punto de Partida:", true, 9, 35, 519);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxDireccionPartida, false, 9, 125, 519);
                EscribirTextoReporte(cb, "Fecha Inicio Transporte:", true, 9, 35, 507);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxFechaIniTransporte, false, 9, 145, 507);
                EscribirTextoReporte(cb, "Fecha fin Transporte:", true, 9, 315, 507);
                EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxFechaFinTransporte, false, 9, 430, 507);
                #endregion

                #region Zona detalle del detinatario
                float x_infoDestinatario_lineaSuperior = 30f;
                float w_infoDestinatario_lineaSuperior = 560f;
                float y_infoDestinatario_lineaSuperior = 495f;
                float y_infoDestinatario_lineaInferior = 330f;
                List<CatalogoReporte> documentos = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                int cntDetalleDestinatario = 0;
                bool cantidadDetalles = false;
                foreach (var destinatario in guiaRemisionRide.guiaRemision.GuiaRemisionDestinatarios)
                {


                    EscribirTextoReporte(cb, "Comprobante de venta:", true, 9, 35, 480);
                    CatalogoReporte documento = documentos.Find(x => x.Codigo == destinatario.CiTipoDocumentoSustento);
                    if (documento != null)
                        EscribirTextoReporte(cb, documento.Valor, false, 9, 150, 480);
                    else
                        EscribirTextoReporte(cb, destinatario.CiTipoDocumentoSustento, false, 9, 150, 480);

                    EscribirTextoReporte(cb, destinatario.TxNumeroDocumentoSustento, false, 9, 310, 480);
                    EscribirTextoReporte(cb, "Fecha Emisión:", true, 9, 415, 480);
                    EscribirTextoReporte(cb, destinatario.TxFechaEmisionDocumentoSustento, false, 9, 490, 480);
                    EscribirTextoReporte(cb, "Número de Autorización:", true, 9, 35, 468);
                    EscribirTextoReporte(cb, destinatario.TxNumeroAutorizacionDocumentoSustento, false, 9, 170, 468);
                    EscribirTextoReporte(cb, "Motivo Traslado:", true, 9, 35, 448);
                    EscribirTextoReporte(cb, destinatario.TxMotivoTraslado, false, 9, 120, 448);
                    EscribirTextoReporte(cb, "Destino (Punto de llegada): ", true, 9, 35, 436);
                    EscribirTextoReporte(cb, destinatario.TxDireccionDestinatario, false, 9, 155, 436);
                    EscribirTextoReporte(cb, "Identificación (Destinatario):", true, 9, 35, 424);
                    EscribirTextoReporte(cb, destinatario.TxIdentificacionDestinatario, false, 9, 165, 424);
                    EscribirTextoReporte(cb, "Razón Social/Nombres Apellidos:", true, 9, 35, 412);
                    EscribirTextoReporte(cb, destinatario.TxRazonSocialDestinatario, false, 9, 195, 412);
                    EscribirTextoReporte(cb, "Documento Aduanero:", true, 9, 35, 400);
                    EscribirTextoReporte(cb, "", false, 9, 150, 400);
                    EscribirTextoReporte(cb, "Código Establecimiento Destino:", true, 9, 35, 388);
                    EscribirTextoReporte(cb, "", true, 9, 190, 388);
                    EscribirTextoReporte(cb, "Ruta:", true, 9, 35, 376);
                    EscribirTextoReporte(cb, destinatario.TxRuta, false, 9, 85, 376);
                    EscribirTextoReporte(cb, "Identificación (Transportista):", true, 9, 35, 364);
                    EscribirTextoReporte(cb, guiaRemisionRide.guiaRemision.TxRucTransportista, false, 9, 185, 364);

                    PdfPTable tblDetalleDestinatario = new PdfPTable(3);
                    tblDetalleDestinatario.WidthPercentage = 80;
                    float[] widths = new float[] { 50f, 370f, 90f }; //Declaramos un array con los tamaños de nuestras columnas deben de coincidir con el tamaño de columna
                    tblDetalleDestinatario.SetTotalWidth(widths);
                    PdfPCell celdaCab_Cantidad = EscribirCeldaReporte("Cantidad", 1, 1, 9, Element.ALIGN_CENTER, 5, BaseColor.BLACK, "White", true);
                    PdfPCell celdaCab_Descripcion = EscribirCeldaReporte("Descripción", 1, 1, 9, Element.ALIGN_CENTER, 5, BaseColor.BLACK, "White", true);
                    PdfPCell celdaCab_CodigoPrincipal = EscribirCeldaReporte("Código Principal", 1, 1, 9, Element.ALIGN_CENTER, 5, BaseColor.BLACK, "White", true);

                    tblDetalleDestinatario.AddCell(celdaCab_Cantidad);
                    tblDetalleDestinatario.AddCell(celdaCab_Descripcion);
                    tblDetalleDestinatario.AddCell(celdaCab_CodigoPrincipal);

                    cntDetalleDestinatario = guiaRemisionRide.guiaRemisionDestinatarioDetalle!.Count;
                    foreach (var detalle in guiaRemisionRide.guiaRemisionDestinatarioDetalle!)
                    {
                        PdfPCell celda_Cantidad = EscribirCeldaReporte(detalle.QnCantidad.ToString(), 1, 1, 8, Element.ALIGN_CENTER, 5, BaseColor.BLACK, "White", false);
                        PdfPCell celda_Descripcion = EscribirCeldaReporte(detalle.TxDescripcion!, 1, 1, 8, Element.ALIGN_JUSTIFIED, 5, BaseColor.BLACK, "White", false);
                        PdfPCell celda_CodigoPrincipal = EscribirCeldaReporte(detalle.TxCodigoInterno, 1, 1, 8, Element.ALIGN_LEFT, 5, BaseColor.BLACK, "White", false);

                        tblDetalleDestinatario.AddCell(celda_Cantidad);
                        tblDetalleDestinatario.AddCell(celda_Descripcion);
                        tblDetalleDestinatario.AddCell(celda_CodigoPrincipal);
                    }
                    int cantDetalle = guiaRemisionRide.guiaRemision.GuiaRemisionDestinatarios.Count;
                    tblDetalleDestinatario.WriteSelectedRows(0, -1, 41, 350, cb);

                    PdfContentByte linea;
                    linea = writer.DirectContent;
                    linea.SetLineWidth(1);// 'configurando el ancho de linea
                    #region Line superior zona de informacion destina y datos del documento sustento
                    linea.MoveTo(x_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaSuperior);// 'MoveTo indica el punto de inicio
                    linea.LineTo(w_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaSuperior);// 'LineTo indica hacia donde se dibuja la linea 
                    linea.Stroke();// 'traza la linea actual y se puede iniciar una nueva
                    #endregion

                    #region Line inferior zona de informacion destina y datos del documento sustento
                    y_infoDestinatario_lineaInferior = y_infoDestinatario_lineaInferior - (cntDetalleDestinatario * 12);
                    linea.MoveTo(x_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaInferior);// 'MoveTo indica el punto de inicio
                    linea.LineTo(w_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaInferior);// 'LineTo indica hacia donde se dibuja la linea 
                    linea.Stroke();// 'traza la linea actual y se puede iniciar una nueva
                    #endregion

                    #region Line latera izquierda de la zona de informacion destina y datos del documento sustento
                    linea.MoveTo(x_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaSuperior);// 'MoveTo indica el punto de inicio
                    linea.LineTo(x_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaInferior);// 'LineTo indica hacia donde se dibuja la linea 
                    linea.Stroke();// 'traza la linea actual y se puede iniciar una nueva
                    #endregion

                    #region Line latera izquierda de la zona de informacion destina y datos del documento sustento
                    linea.MoveTo(w_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaSuperior);// 'MoveTo indica el punto de inicio
                    linea.LineTo(w_infoDestinatario_lineaSuperior, y_infoDestinatario_lineaInferior);// 'LineTo indica hacia donde se dibuja la linea 
                    linea.Stroke();// 'traza la linea actual y se puede iniciar una nueva
                    #endregion

                    if (cntDetalleDestinatario > 24)
                        documentoPdf.NewPage();
                }
                #endregion

                PdfPTable tblInformacionAdicional = new PdfPTable(2);
                tblInformacionAdicional.WidthPercentage = 50;
                float[] widthsInfoAdicional = new float[] { 150f, 200f }; //Declaramos un array con los tamaños de nuestras columnas deben de coincidir con el tamaño de columna
                tblInformacionAdicional.SetTotalWidth(widthsInfoAdicional);
                PdfPCell celdaCab_InformacionAdicional = EscribirCeldaReporte("Información Adicional", 2, 1, 9, Element.ALIGN_CENTER, 5, BaseColor.BLACK, "White", true);
                tblInformacionAdicional.AddCell(celdaCab_InformacionAdicional);
                foreach (var informacionAdicional in guiaRemisionRide.guiaRemision.GuiaRemisionInfoAdicionals)
                {
                    PdfPCell celda_Nombre = EscribirCeldaReporte(informacionAdicional.TxNombre!, 1, 1, 8, Element.ALIGN_LEFT, 5, BaseColor.BLACK, "White", false);
                    PdfPCell celda_Valor = EscribirCeldaReporte(informacionAdicional.TxValor!, 1, 1, 8, Element.ALIGN_JUSTIFIED, 5, BaseColor.BLACK, "White", false);
                    tblInformacionAdicional.AddCell(celda_Nombre);
                    tblInformacionAdicional.AddCell(celda_Valor);

                }
                if (cntDetalleDestinatario == 25)
                    tblInformacionAdicional.WriteSelectedRows(0, -1, 100, 800 - 20, cb);  //modificarlo
                else
                    tblInformacionAdicional.WriteSelectedRows(0, -1, 100, y_infoDestinatario_lineaInferior - 20, cb);  //modificarlo

                //Cerramos el Documento 
                documentoPdf.Close();
                //documentoPdf.NewPage();
                writer.Flush();
                #endregion

                Stream stream1 = new MemoryStream(memoryStream.ToArray());

                ridePdf = Convert.ToBase64String(ToArray(stream1));

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.grabaLogsException("ProcesoRideGuiaRemision", "GeneraReporteNetcore", ex.Message, null);
            }

            return ridePdf;
        }

        private void EscribirTextoReporte(PdfContentByte cb, string texto, bool titulo, int tamanyoLetra, float ejeX, float ejeY)
        {
            cb.BeginText();
            if (titulo)
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false), tamanyoLetra);
            else
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false), tamanyoLetra);
            cb.SetTextMatrix(ejeX, ejeY);
            cb.ShowText(texto);
            cb.EndText();
        }

        private PdfPCell EscribirCeldaReporte(string TextoCelda, int collspan, int rowspan, float tamanyoLetra, int alignment, int borde, BaseColor colorTexto, string colorCelda, bool cabecera)
        {
            //Creamos un Objeto de tipo PdfPCell en el cual agregamos directo nuestro texto como vera utilizo Paragraph con un tipo de letra en mi caso fijo pero si lo desean pueden enviarlo como parametro, un tamaño y un color de letra que se envian como parametro
            PdfPCell CeldaPDF;
            if (cabecera)
            {
                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, tamanyoLetra);
                CeldaPDF = new PdfPCell(new Paragraph(TextoCelda, boldFont));
            }
            else
            {

                CeldaPDF = new PdfPCell(new Paragraph(TextoCelda, FontFactory.GetFont("Calibri", tamanyoLetra, colorTexto)));
            }

            //varias combinancaciones para poner el borde deseado
            #region "borde"

            if (borde == 0)
            {
                CeldaPDF.Border = 0;
            }
            else
            {
                if (borde == 1)
                {
                    CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                }
                else
                {
                    if (borde == 2)
                    {
                        CeldaPDF.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                    }
                    else
                    {
                        if (borde == 3)
                        {
                            CeldaPDF.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        }
                        else
                        {
                            if (borde == 4)
                            {
                                CeldaPDF.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;

                            }
                            else
                            {
                                if (borde == 5)
                                {
                                    CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                                }
                                else
                                {
                                    if (borde == 6)
                                    {
                                        CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;

                                    }
                                    else
                                    {
                                        if (borde == 7)
                                        {
                                            CeldaPDF.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;

                                        }
                                        else
                                        {
                                            if (borde == 8)
                                            {
                                                CeldaPDF.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;

                                            }
                                            else
                                            {
                                                if (borde == 9)
                                                {
                                                    CeldaPDF.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            //Indicamos que queremos un color de fondo, el parametro enviado es un strign donde vendra nuestro color por default si no lo indican es blanco
            CeldaPDF.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml(colorCelda));
            //En caso de querer poner un collspan indicamos el numero de columnas que abarcara
            CeldaPDF.Colspan = collspan;
            //Lo mismo si es un rowspan, indicaremos el numero de filar que abarcara
            CeldaPDF.Rowspan = rowspan;
            //Indicamos la alineacion horizontal
            CeldaPDF.HorizontalAlignment = alignment;
            //Indicamos la alineacion vertical
            CeldaPDF.VerticalAlignment = alignment;
            //Regresamos nuestra celda ya formateada
            return CeldaPDF;
        }

        private byte[] ToArray(Stream stream)
        {
            byte[] byteArray = new byte[stream.Length];
            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Lectura que nos podria devolver entre cero y byteArray.Length
                int n = stream.Read(byteArray, numBytesRead, byteArray.Length);
                // Cuando llega al final
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;

            }
            return byteArray;

        }
    }
}
