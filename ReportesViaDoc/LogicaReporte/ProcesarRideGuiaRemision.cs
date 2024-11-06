using GenCode128;
using iTextSharp.text;
//Clases necesarias de iTextSharp
using iTextSharp.text.pdf;
using ReportesViaDoc.EntidadesReporte;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Xml;
using ViaDoc.Configuraciones;

namespace ReportesViaDoc.LogicaReporte
{
    public class ProcesarRideGuiaRemision
    {
        ExcepcionesReportes reporte = new ExcepcionesReportes();
        public Byte[] GenerarRideGuiaRemision(ref string mensajeError, RideGuiaRemision objRideGuiaRemision, List<ConfiguracionReporte> configuraciones, List<CatalogoReporte> catalogoSistema)
        {
            byte[] array2;
            string etiquetaContribuyenteEspecial = "";
            try
            {
                List<ConfiguracionReporte> confCompania = configuraciones.FindAll(x => x.RucCompania == objRideGuiaRemision._infoTributaria.Ruc);

                #region Configuracion de los separadores de miles de la Guia de remision
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
                #region Catalogo Ambiente Guia Remision
                List<CatalogoReporte> ambientesGuia = catalogoSistema.FindAll(x => x.CodigoReferencia == "AMBIENTE");
                bool validaAmbiente = false;
                if (ambientesGuia != null)
                    validaAmbiente = true;
                #endregion
                #region Catalogo Emisiones Guia Remision
                List<CatalogoReporte> emisionesGui = catalogoSistema.FindAll(x => x.CodigoReferencia == "EMISION");
                bool validaEmisionFactura = false;
                if (emisionesGui != null)
                    validaEmisionFactura = true;
                #endregion

                //
                string pathLogoEmpresa = CatalogoViaDoc.rutaLogoCompania;
                string rutaImagen = pathLogoEmpresa + objRideGuiaRemision._infoTributaria.Ruc.Trim() + ".png";
                string imagenNombre = objRideGuiaRemision._infoTributaria.Ruc.Trim();
                string nuevaRuta = "";
                if (!Directory.Exists(rutaImagen))
                {
                    rutaImagen = pathLogoEmpresa + imagenNombre + ".png";
                }

                #region Genera Codigo Barra de la Factura
                byte[] imgBar;
                Code.BarcodeGenerator bgCode128 = new Code.BarcodeGenerator();
                Code.Convertir cCode = new Code.Convertir();
                Graphics g = Graphics.FromImage(new Bitmap(1, 1));
                StringFormat fi = new StringFormat(StringFormatFlags.NoClip);
                fi.Alignment = StringAlignment.Center;
                imgBar = RetornarXml.ImageToByte2(Code128Rendering.MakeBarcodeImage(objRideGuiaRemision._infoTributaria.ClaveAcceso, 3, true));

                string pathCodBarra = CatalogoViaDoc.rutaCodigoBarra + objRideGuiaRemision._infoTributaria.ClaveAcceso.Trim();
                string pathRider = CatalogoViaDoc.rutaRide + objRideGuiaRemision._infoTributaria.Ruc.Trim() + "\\" + objRideGuiaRemision._infoTributaria.CodigoDocumento.Trim();
                if (!Directory.Exists(pathRider))
                {
                    Directory.CreateDirectory(pathRider);
                }
                File.WriteAllBytes(pathCodBarra + ".jpg", imgBar);
                #endregion


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

                EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.RazonSocial, true, 11, 35, 682);
                PdfPTable tbTributario = new PdfPTable(2);
                tbTributario.WidthPercentage = 50;
                float[] widthstbTributario = new float[] { 50f, 218f }; //Declaramos un array con los tamaños de nuestras columnas deben de coincidir con el tamaño de columna
                tbTributario.SetTotalWidth(widthstbTributario);
                PdfPCell celdaDireccion = EscribirCeldaReporte("Direccion Matriz:", 1, 1, 9, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", true);
                tbTributario.AddCell(celdaDireccion);
                PdfPCell celda_Dieccion = EscribirCeldaReporte(objRideGuiaRemision._infoTributaria.DirMatriz, 1, 1, 8, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", false);
                tbTributario.AddCell(celda_Dieccion);
                PdfPCell celdaSucursal = EscribirCeldaReporte("Direccion Sucursal:", 1, 1, 9, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", true);
                tbTributario.AddCell(celdaSucursal);
                PdfPCell celda_Sucursal = EscribirCeldaReporte(objRideGuiaRemision._infoGuiaRemision.DirEstablecimiento, 1, 1, 8, Element.ALIGN_LEFT, 0, BaseColor.BLACK, "White", false);
                tbTributario.AddCell(celda_Sucursal);
                tbTributario.WriteSelectedRows(0, -1, 35, 699 - 20, cb);

                if (!validaNumeroContribuyenteEspecial)
                {
                    EscribirTextoReporte(cb, "Contribuyente Especial Nro:", true, 9, 35, 625);
                    EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.ContribuyenteEspecial, false, 9, 180, 612);
                }

                EscribirTextoReporte(cb, "OBLIGADO A LLEVAR CONTABILIDAD:", true, 9, 35, 610);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.ObligadoContabilidad, false, 9, 210, 610);

                EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.regimenMicroempresas, false, 9, 35, 598);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.AgenteRetencion == "" ? "" : CatalogoViaDoc.LeyendaAgente.Trim(), false, 9, 35, 586);

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
                EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.Ruc, false, 9, 357, 793);
                EscribirTextoReporte(cb, "GUIA  DE  REMISIÓN", true, 12, 320, 776);
                EscribirTextoReporte(cb, "No.", true, 9, 320, 762);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.Establecimiento + "-" + objRideGuiaRemision._infoTributaria.PuntoEmision + "-" + objRideGuiaRemision._infoTributaria.Secuencial, false, 9, 354, 762);
                EscribirTextoReporte(cb, "NUMERO DE AUTORIZACION", true, 9, 320, 748);
                EscribirTextoReporte(cb, objRideGuiaRemision.NumeroAutorizacion, false, 8, 320, 735);
                EscribirTextoReporte(cb, "FECHA Y HORA", true, 9, 320, 720);
                EscribirTextoReporte(cb, "DE", true, 9, 320, 710);
                EscribirTextoReporte(cb, "AUTORIZACION", true, 9, 320, 700);
                EscribirTextoReporte(cb, objRideGuiaRemision.FechaHoraAutorizacion, false, 9, 398, 710);
                EscribirTextoReporte(cb, "AMBIENTE:", true, 9, 320, 685);

                if (validaAmbiente)
                {  //aquii se cae-------->
                    CatalogoReporte tipoAmbiente = ambientesGuia.Find(x => x.Codigo == objRideGuiaRemision._infoTributaria.Ambiente);
                    EscribirTextoReporte(cb, tipoAmbiente.Valor, false, 8, 373, 685);
                }
                else
                    EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.Ambiente, false, 8, 373, 685);
                EscribirTextoReporte(cb, "EMISION:", true, 9, 320, 670);
                if (validaEmisionFactura)
                {
                    CatalogoReporte tipoemision = emisionesGui.Find(x => x.Codigo == objRideGuiaRemision._infoTributaria.TipoEmision);
                    EscribirTextoReporte(cb, tipoemision.Valor, false, 8, 373, 670);
                }
                else
                    EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.TipoEmision, true, 9, 373, 670);
                EscribirTextoReporte(cb, "CLAVE DE ACCESO", true, 9, 320, 655);
                string rutaCodigoBarra = pathCodBarra + ".jpg";
                iTextSharp.text.Image imgCodigoBarra = iTextSharp.text.Image.GetInstance(@rutaCodigoBarra);
                imgCodigoBarra.SetAbsolutePosition(317, 612); //Posicion en el eje carteciano de X y Y; VALOR DE Y MAS ALTO LA IMAGEN SUBE; X MAS LATO LA IMAGE VA A LA DERECHA WJ
                imgCodigoBarra.ScaleAbsolute(240, 40);//Ancho y altura de la imagen
                documentoPdf.Add(imgCodigoBarra); // Agrega la imagen al documento
                EscribirTextoReporte(cb, objRideGuiaRemision._infoTributaria.ClaveAcceso, false, 8, 320, 600);
                #endregion

                #region Zona de la informacion del transportista
                float x_infoTransportista = 30f; // de derecha a izquierda
                float y_infoTransportista = 500f; // de abajo a arriba
                float w_infoTransportista = 532f; // ancho
                float h_infoTransportista = 70f; // altura de abaja hacia arriba
                cb.Rectangle(x_infoTransportista, y_infoTransportista, w_infoTransportista, h_infoTransportista);
                cb.Stroke();

                EscribirTextoReporte(cb, "Identificación (Transportista):", true, 9, 35, 555);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.RucTransportista, false, 9, 170, 555);
                EscribirTextoReporte(cb, "Razón Social / Nombres y Apellidos:", true, 9, 35, 543);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.RazonSocialTransportista, false, 9, 210, 543);
                EscribirTextoReporte(cb, "Placa:", true, 9, 35, 531);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.Placa, false, 9, 75, 531);
                EscribirTextoReporte(cb, "Punto de Partida:", true, 9, 35, 519);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.DirPartida, false, 9, 125, 519);
                EscribirTextoReporte(cb, "Fecha Inicio Transporte:", true, 9, 35, 507);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.FechaInicioTransporte, false, 9, 145, 507);
                EscribirTextoReporte(cb, "Fecha fin Transporte:", true, 9, 315, 507);
                EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.FechaFinTransporte, false, 9, 430, 507);
                #endregion

                #region Zona detalle del detinatario
                float x_infoDestinatario_lineaSuperior = 30f;
                float w_infoDestinatario_lineaSuperior = 560f;
                float y_infoDestinatario_lineaSuperior = 495f;
                float y_infoDestinatario_lineaInferior = 330f;
                List<CatalogoReporte> documentos = catalogoSistema.FindAll(x => x.CodigoReferencia == "DOCELECT");
                int cntDetalleDestinatario = 0;
                bool cantidadDetalles = false;
                foreach (Destinatario destinatario in objRideGuiaRemision._destinatarios)
                {


                    EscribirTextoReporte(cb, "Comprobante de venta:", true, 9, 35, 480);
                    CatalogoReporte documento = documentos.Find(x => x.Codigo == destinatario.CodDocSustento);
                    if (documento != null)
                        EscribirTextoReporte(cb, documento.Valor, false, 9, 150, 480);
                    else
                        EscribirTextoReporte(cb, destinatario.CodDocSustento, false, 9, 150, 480);

                    EscribirTextoReporte(cb, destinatario.NumDocSustento, false, 9, 310, 480);
                    EscribirTextoReporte(cb, "Fecha Emisión:", true, 9, 415, 480);
                    EscribirTextoReporte(cb, destinatario.FechaEmisionDocSustento, false, 9, 490, 480);
                    EscribirTextoReporte(cb, "Número de Autorización:", true, 9, 35, 468);
                    EscribirTextoReporte(cb, destinatario.NumAutDocSustento, false, 9, 170, 468);
                    EscribirTextoReporte(cb, "Motivo Traslado:", true, 9, 35, 448);
                    EscribirTextoReporte(cb, destinatario.MotivoTraslado, false, 9, 120, 448);
                    EscribirTextoReporte(cb, "Destino (Punto de llegada): ", true, 9, 35, 436);
                    EscribirTextoReporte(cb, destinatario.DirDestinatario, false, 9, 155, 436);
                    EscribirTextoReporte(cb, "Identificación (Destinatario):", true, 9, 35, 424);
                    EscribirTextoReporte(cb, destinatario.IdentificacionDestinatario, false, 9, 165, 424);
                    EscribirTextoReporte(cb, "Razón Social/Nombres Apellidos:", true, 9, 35, 412);
                    EscribirTextoReporte(cb, destinatario.RazonSocialDestinatario, false, 9, 195, 412);
                    EscribirTextoReporte(cb, "Documento Aduanero:", true, 9, 35, 400);
                    EscribirTextoReporte(cb, "", false, 9, 150, 400);
                    EscribirTextoReporte(cb, "Código Establecimiento Destino:", true, 9, 35, 388);
                    EscribirTextoReporte(cb, "", true, 9, 190, 388);
                    EscribirTextoReporte(cb, "Ruta:", true, 9, 35, 376);
                    EscribirTextoReporte(cb, destinatario.Ruta, false, 9, 85, 376);
                    EscribirTextoReporte(cb, "Identificación (Transportista):", true, 9, 35, 364);
                    EscribirTextoReporte(cb, objRideGuiaRemision._infoGuiaRemision.RucTransportista, false, 9, 185, 364);

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

                    cntDetalleDestinatario = destinatario._detallesDestinatario.Count;
                    foreach (DetalleDestinatario detalle in destinatario._detallesDestinatario)
                    {
                        PdfPCell celda_Cantidad = EscribirCeldaReporte(detalle.Cantidad, 1, 1, 8, Element.ALIGN_CENTER, 5, BaseColor.BLACK, "White", false);
                        PdfPCell celda_Descripcion = EscribirCeldaReporte(detalle.Descripcion, 1, 1, 8, Element.ALIGN_JUSTIFIED, 5, BaseColor.BLACK, "White", false);
                        PdfPCell celda_CodigoPrincipal = EscribirCeldaReporte(detalle.CodigoInterno, 1, 1, 8, Element.ALIGN_LEFT, 5, BaseColor.BLACK, "White", false);
                        
                        tblDetalleDestinatario.AddCell(celda_Cantidad);
                        tblDetalleDestinatario.AddCell(celda_Descripcion);
                        tblDetalleDestinatario.AddCell(celda_CodigoPrincipal);
                    }
                    int cantDetalle = objRideGuiaRemision._destinatarios.Count;
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
                foreach (InformacionAdicional informacionAdicional in objRideGuiaRemision._infoAdicional)
                {
                    PdfPCell celda_Nombre = EscribirCeldaReporte(informacionAdicional.Nombre, 1, 1, 8, Element.ALIGN_LEFT, 5, BaseColor.BLACK, "White", false);
                    PdfPCell celda_Valor = EscribirCeldaReporte(informacionAdicional.Valor, 1, 1, 8, Element.ALIGN_JUSTIFIED, 5, BaseColor.BLACK, "White", false);
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

                Stream stream = new MemoryStream(memoryStream.ToArray());
                array2 = ToArray(stream);
            }
            catch (Exception ex)
            {
                mensajeError = "Error al generar el byte[] para el ride Guia de Remision. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;
                array2 = null;
            }

            return array2;
        }

        /// <summary>
        /// Autor: William Jacome Choez (Viamatica S.A)
        /// Descripcion: Escribe una palabra en el reporte especificando su eje y fuente del texto
        /// </summary>
        /// <param name="cb">Canbas del reporte</param>
        /// <param name="texto">palabra a escribir</param>
        /// <param name="titulo">valida si la palabra va hacer resaltada</param>
        /// <param name="tamanyoLetra">tamaño de la letra</param>
        /// <param name="ejeX">eje x de la hoja</param>
        /// <param name="ejeY">eje y de la hoja</param>
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

        /// <summary>
        /// Autor: William Jacome Choez (Viamatica S.A)
        /// Descripcion: Da formato a una celda de una tabla
        /// </summary>
        /// <param name="TextoCelda">Texto</param>
        /// <param name="collspan">numero de celda a ocupar</param>
        /// <param name="rowspan">numero de columnas a ocupar</param>
        /// <param name="tamanyoLetra">tamaño de la letra</param>
        /// <param name="alignment">tipo aliniamiento</param>
        /// <param name="borde">tamaño del borde</param>
        /// <param name="colorTexto">color del texto</param>
        /// <param name="colorCelda">color del filo de la celda</param>
        /// <param name="cabecera">valida si el texto se quiere resaltar como titulo</param>
        /// <returns></returns>
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

        /// <summary>
        /// Autor: William Jacome Choez(Viamatica S.A)
        /// Descripcion: Convierte el Stream del PdfWriter en byte[] para descargarlo en la web
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Autor: William Jacome Choez(Viamatica S.A)
        /// Descripcion: Desarmar el xml Autorizado de la Guia de Remision para generar un objeto para procesar el byte[] del documento.
        /// </summary>
        /// <param name="mensajeError"></param>
        /// <param name="xmlDocumentoAutorizado"></param>
        /// <param name="fechaHoraAutorizacion"></param>
        /// <param name="numeroAutorizacion"></param>
        /// <returns></returns>
        public RideGuiaRemision ProcesarXmlAutorizadoGuiaRemision(ref string mensajeError, string xmlDocumentoAutorizado, string fechaHoraAutorizacion, string numeroAutorizacion)
        {
            RideGuiaRemision objRideGuiaRemision = new RideGuiaRemision();
            StringWriter stw = new System.IO.StringWriter();
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
                    objRideGuiaRemision.BanderaGeneracionObjeto = true;
                    objRideGuiaRemision.NumeroAutorizacion = numeroAutorizacion;
                    objRideGuiaRemision.FechaHoraAutorizacion = fechaHoraAutorizacion;


                    string NumeroAutorizacion = Informacion.SelectSingleNode("numeroAutorizacion").InnerText;
                    string FechaAutorizacion = Informacion.SelectSingleNode("fechaAutorizacion").InnerText;
                    string xmlComprobante = Informacion.SelectSingleNode("comprobante").InnerText;
                    document.LoadXml(xmlComprobante);
                    document.WriteTo(xtr);

                    XmlNodeList camposXML_infoTributaria = document.SelectNodes("guiaRemision/infoTributaria");
                    XmlNodeList camposXML_infoGuia = document.SelectNodes("guiaRemision/infoGuiaRemision");
                    XmlNodeList camposXML_destinatarios = document.SelectNodes("guiaRemision/destinatarios");

                    if (camposXML_infoTributaria.Count > 0 && camposXML_infoGuia.Count > 0 && camposXML_destinatarios.Count > 0)
                    {
                        XmlNodeList camposXML_infoAdicional = document.SelectNodes("guiaRemision/infoAdicional");
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
                            objRideGuiaRemision._infoAdicional = listaInfoAdicional;
                            #endregion
                        }

                        #region Informacion tributaria de la factura
                        //exception.GuardaReporte("Informacion tributaria de la factura");
                        XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                        objRideGuiaRemision._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                        objRideGuiaRemision._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                        objRideGuiaRemision._infoTributaria.NombreComercial = "";
                        try { objRideGuiaRemision._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText; }
                        catch (Exception ex) { objRideGuiaRemision._infoTributaria.NombreComercial = ""; }
                        objRideGuiaRemision._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                        objRideGuiaRemision._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                        objRideGuiaRemision._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                        objRideGuiaRemision._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                        objRideGuiaRemision._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                        objRideGuiaRemision._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                        objRideGuiaRemision._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                        try
                        {
                            objRideGuiaRemision._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("contribuyenteRimpe").InnerText;
                        }
                        catch (Exception)
                        {
                            objRideGuiaRemision._infoTributaria.regimenMicroempresas = CatalogoViaDoc.LeyendaRegimen;
                        }

                        try
                        {
                            objRideGuiaRemision._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                        }
                        catch (Exception)
                        {
                            objRideGuiaRemision._infoTributaria.AgenteRetencion = CatalogoViaDoc.LeyendaAgente;
                        }

                        objRideGuiaRemision._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                        #endregion

                        #region Informacion de la guia de remision
                        XmlNode tagInfoFactura = camposXML_infoGuia.Item(0);
                        objRideGuiaRemision._infoGuiaRemision.DirEstablecimiento = tagInfoFactura.SelectSingleNode("dirEstablecimiento").InnerText;
                        objRideGuiaRemision._infoGuiaRemision.DirPartida = tagInfoFactura.SelectSingleNode("dirPartida").InnerText;
                        objRideGuiaRemision._infoGuiaRemision.RazonSocialTransportista = tagInfoFactura.SelectSingleNode("razonSocialTransportista").InnerText;
                        objRideGuiaRemision._infoGuiaRemision.TipoIdentificacionTransportista = tagInfoFactura.SelectSingleNode("tipoIdentificacionTransportista").InnerText;
                        objRideGuiaRemision._infoGuiaRemision.RucTransportista = tagInfoFactura.SelectSingleNode("rucTransportista").InnerText;
                        objRideGuiaRemision._infoGuiaRemision.ObligadoContabilidad = tagInfoFactura.SelectSingleNode("obligadoContabilidad").InnerText;
                        try { objRideGuiaRemision._infoGuiaRemision.ContribuyenteEspecial = tagInfoFactura.SelectSingleNode("contribuyenteEspecial").InnerText; }
                        catch (Exception ex)
                        {
                            objRideGuiaRemision._infoGuiaRemision.ContribuyenteEspecial = "";
                            //exception.GuardaReporte("Informacion de la factura excepcion--" + ex.Message);
                        }
                        objRideGuiaRemision._infoGuiaRemision.FechaInicioTransporte = tagInfoFactura.SelectSingleNode("fechaIniTransporte").InnerText;
                        objRideGuiaRemision._infoGuiaRemision.FechaFinTransporte = tagInfoFactura.SelectSingleNode("fechaFinTransporte").InnerText;
                        objRideGuiaRemision._infoGuiaRemision.Placa = tagInfoFactura.SelectSingleNode("placa").InnerText;
                        #endregion Informacion de la guia de remision

                        #region Informacion de los destinatario
                        List<Destinatario> listaDestinatario = new List<Destinatario>();
                        foreach (XmlNode tagDestinatario in camposXML_destinatarios)
                        {
                            Destinatario destinatario = new Destinatario();
                            destinatario.IdentificacionDestinatario = tagDestinatario.SelectSingleNode("destinatario/identificacionDestinatario").InnerText;
                            destinatario.RazonSocialDestinatario = tagDestinatario.SelectSingleNode("destinatario/razonSocialDestinatario").InnerText;
                            destinatario.DirDestinatario = tagDestinatario.SelectSingleNode("destinatario/dirDestinatario").InnerText;
                            destinatario.MotivoTraslado = tagDestinatario.SelectSingleNode("destinatario/motivoTraslado").InnerText;

                            try { destinatario.Ruta = tagDestinatario.SelectSingleNode("destinatario/ruta").InnerText; } catch (Exception ex) { destinatario.Ruta = ""; }

                            try
                            { destinatario.CodDocSustento = tagDestinatario.SelectSingleNode("destinatario/codDocSustento").InnerText; }
                            catch (Exception ex) { destinatario.CodDocSustento = ""; }
                            try
                            { destinatario.NumDocSustento = tagDestinatario.SelectSingleNode("destinatario/numDocSustento").InnerText; }
                            catch (Exception ex) { destinatario.NumDocSustento = ""; }
                            try
                            { destinatario.FechaEmisionDocSustento = tagDestinatario.SelectSingleNode("destinatario/fechaEmisionDocSustento").InnerText; }
                            catch (Exception ex) { destinatario.FechaEmisionDocSustento = ""; }
                            string xml_detalleDestinatario = "<detalles>" + tagDestinatario.SelectSingleNode("destinatario/detalles").InnerXml + "</detalles>";
                            #region Informacion del detalle del destinatario
                            document.LoadXml(xml_detalleDestinatario);
                            document.WriteTo(xtr);
                            XmlNodeList camposXML_detalles = document.SelectNodes("detalles/detalle");
                            List<DetalleDestinatario> detalleDestinatario = new List<DetalleDestinatario>();
                            if (camposXML_detalles.Count > 0)
                            {
                                foreach (XmlNode detalle in camposXML_detalles)
                                {
                                    DetalleDestinatario detDestinatario = new DetalleDestinatario();
                                    detDestinatario.CodigoInterno = detalle.SelectSingleNode("codigoInterno").InnerText;
                                    detDestinatario.Descripcion = detalle.SelectSingleNode("descripcion").InnerText;
                                    detDestinatario.Cantidad = detalle.SelectSingleNode("cantidad").InnerText;

                                    //detDestinatario.CodigoAdicional = detalle.SelectSingleNode("").InnerText;
                                    detalleDestinatario.Add(detDestinatario);
                                }
                                destinatario._detallesDestinatario = detalleDestinatario;
                            }
                            #endregion
                            listaDestinatario.Add(destinatario);
                        }
                        objRideGuiaRemision._destinatarios = listaDestinatario;
                        #endregion
                    }
                }
                else
                    objRideGuiaRemision = ProcesarXmlFirmadoGuiaRemision(ref mensajeError, xmlDocumentoAutorizado);
            }
            catch (Exception ex)
            {
                objRideGuiaRemision.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml autorizado de la Guia Remision. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;

            }
            return objRideGuiaRemision;
        }

        /// <summary>
        /// Autor: William Jacome Choez(Viamatica S.A)
        /// Descripcion: Desarmar el xml firmado de la Guia de Remision para generar un objeto para procesar el byte[] del documento.
        /// </summary>
        /// <param name="mensajeError"></param>
        /// <param name="xmlDocumentoFirmado"></param>
        /// <returns></returns>
        public RideGuiaRemision ProcesarXmlFirmadoGuiaRemision(ref string mensajeError, string xmlDocumentoFirmado)
        {
            RideGuiaRemision objRideGuiaRemision = new RideGuiaRemision();
            StringWriter stw = new System.IO.StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stw);
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlDocumentoFirmado);
                document.WriteTo(xtr);
                string XMLString = document.InnerXml;
                XmlNodeList camposXML_infoTributaria = document.SelectNodes("guiaRemision/infoTributaria");
                XmlNodeList camposXML_infoGuiaRemision = document.SelectNodes("guiaRemision/infoGuiaRemision");
                XmlNodeList camposXML_destinatarios = document.SelectNodes("guiaRemision/destinatarios");
                XmlNodeList camposXML_infoAdicional = document.SelectNodes("guiaRemision/infoAdicional");

                if (camposXML_infoTributaria.Count > 0 && camposXML_infoGuiaRemision.Count > 0 && camposXML_destinatarios.Count > 0)
                {
                    objRideGuiaRemision.BanderaGeneracionObjeto = true;
                    if (camposXML_infoAdicional.Count > 0)
                    {
                        #region Informacion Adicional de la guia de remision
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
                        objRideGuiaRemision._infoAdicional = listaInfoAdicional;
                        #endregion
                    }

                    #region Informacion tributaria de la guia de remision
                    XmlNode informacionXML = camposXML_infoTributaria.Item(0);
                    objRideGuiaRemision._infoTributaria.Ambiente = informacionXML.SelectSingleNode("ambiente").InnerText;
                    objRideGuiaRemision._infoTributaria.RazonSocial = informacionXML.SelectSingleNode("razonSocial").InnerText;
                    objRideGuiaRemision._infoTributaria.NombreComercial = informacionXML.SelectSingleNode("nombreComercial").InnerText;
                    objRideGuiaRemision._infoTributaria.ClaveAcceso = informacionXML.SelectSingleNode("claveAcceso").InnerText;
                    objRideGuiaRemision._infoTributaria.Establecimiento = informacionXML.SelectSingleNode("estab").InnerText;
                    objRideGuiaRemision._infoTributaria.PuntoEmision = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                    objRideGuiaRemision._infoTributaria.Secuencial = informacionXML.SelectSingleNode("secuencial").InnerText;
                    objRideGuiaRemision._infoTributaria.Ruc = informacionXML.SelectSingleNode("ruc").InnerText;
                    objRideGuiaRemision._infoTributaria.TipoEmision = informacionXML.SelectSingleNode("tipoEmision").InnerText;
                    objRideGuiaRemision._infoTributaria.DirMatriz = informacionXML.SelectSingleNode("dirMatriz").InnerText;

                    try
                    {
                        objRideGuiaRemision._infoTributaria.regimenMicroempresas = informacionXML.SelectSingleNode("regimenMicroempresas").InnerText;
                    }
                    catch (Exception)
                    {
                        objRideGuiaRemision._infoTributaria.regimenMicroempresas = "";
                    }

                    try
                    {
                        objRideGuiaRemision._infoTributaria.AgenteRetencion = informacionXML.SelectSingleNode("agenteRetencion").InnerText;
                    }
                    catch (Exception)
                    {
                        objRideGuiaRemision._infoTributaria.AgenteRetencion = "";
                    }

                    objRideGuiaRemision._infoTributaria.CodigoDocumento = informacionXML.SelectSingleNode("codDoc").InnerText;
                    #endregion
                    #region Informacion de la guia de remision
                    XmlNode tagInfoGuia = camposXML_infoGuiaRemision.Item(0);
                    objRideGuiaRemision._infoGuiaRemision.DirEstablecimiento = tagInfoGuia.SelectSingleNode("dirEstablecimiento").InnerText;
                    objRideGuiaRemision._infoGuiaRemision.DirPartida = tagInfoGuia.SelectSingleNode("dirPartida").InnerText;
                    objRideGuiaRemision._infoGuiaRemision.RazonSocialTransportista = tagInfoGuia.SelectSingleNode("razonSocialTransportista").InnerText;
                    objRideGuiaRemision._infoGuiaRemision.TipoIdentificacionTransportista = tagInfoGuia.SelectSingleNode("tipoIdentificacionTransportista").InnerText;
                    objRideGuiaRemision._infoGuiaRemision.RucTransportista = tagInfoGuia.SelectSingleNode("rucTransportista").InnerText;
                    objRideGuiaRemision._infoGuiaRemision.ObligadoContabilidad = tagInfoGuia.SelectSingleNode("obligadoContabilidad").InnerText;

                    try
                    { objRideGuiaRemision._infoGuiaRemision.ContribuyenteEspecial = tagInfoGuia.SelectSingleNode("contribuyenteEspecial").InnerText; }
                    catch (Exception ex) { objRideGuiaRemision._infoGuiaRemision.ContribuyenteEspecial = ""; }

                    objRideGuiaRemision._infoGuiaRemision.FechaInicioTransporte = tagInfoGuia.SelectSingleNode("fechaIniTransporte").InnerText;
                    objRideGuiaRemision._infoGuiaRemision.FechaFinTransporte = tagInfoGuia.SelectSingleNode("fechaFinTransporte").InnerText;
                    objRideGuiaRemision._infoGuiaRemision.Placa = tagInfoGuia.SelectSingleNode("placa").InnerText;
                    #endregion
                    #region Informacion de los destinatario
                    List<Destinatario> listaDestinatario = new List<Destinatario>();
                    foreach (XmlNode tagDestinatario in camposXML_destinatarios)
                    {
                        Destinatario destinatario = new Destinatario();
                        destinatario.IdentificacionDestinatario = tagDestinatario.SelectSingleNode("destinatario/identificacionDestinatario").InnerText;
                        destinatario.RazonSocialDestinatario = tagDestinatario.SelectSingleNode("destinatario/razonSocialDestinatario").InnerText;
                        destinatario.DirDestinatario = tagDestinatario.SelectSingleNode("destinatario/dirDestinatario").InnerText;
                        destinatario.MotivoTraslado = tagDestinatario.SelectSingleNode("destinatario/motivoTraslado").InnerText;
                        try
                        { destinatario.Ruta = tagDestinatario.SelectSingleNode("destinatario/ruta").InnerText; }
                        catch (Exception ex) { destinatario.Ruta = ""; }

                        try
                        { destinatario.CodDocSustento = tagDestinatario.SelectSingleNode("destinatario/codDocSustento").InnerText; }
                        catch (Exception ex) { destinatario.CodDocSustento = ""; }
                        try
                        { destinatario.NumDocSustento = tagDestinatario.SelectSingleNode("destinatario/numDocSustento").InnerText; }
                        catch (Exception ex) { destinatario.NumDocSustento = ""; }
                        try
                        { destinatario.FechaEmisionDocSustento = tagDestinatario.SelectSingleNode("destinatario/fechaEmisionDocSustento").InnerText; }
                        catch (Exception ex) { destinatario.FechaEmisionDocSustento = ""; }

                        string xml_detalleDestinatario = "<detalles>" + tagDestinatario.SelectSingleNode("destinatario/detalles").InnerXml + "</detalles>";

                        #region Informacion del detalle del destinatario
                        document.LoadXml(xml_detalleDestinatario);
                        document.WriteTo(xtr);
                        XmlNodeList camposXML_detalles = document.SelectNodes("detalles/detalle");
                        List<DetalleDestinatario> detalleDestinatario = new List<DetalleDestinatario>();
                        if (camposXML_detalles.Count > 0)
                        {
                            foreach (XmlNode detalle in camposXML_detalles)
                            {
                                DetalleDestinatario detDestinatario = new DetalleDestinatario();
                                detDestinatario.CodigoInterno = detalle.SelectSingleNode("codigoInterno").InnerText;
                                detDestinatario.Descripcion = detalle.SelectSingleNode("descripcion").InnerText;
                                detDestinatario.Cantidad = detalle.SelectSingleNode("cantidad").InnerText;
                                detalleDestinatario.Add(detDestinatario);
                            }
                            destinatario._detallesDestinatario = detalleDestinatario;
                        }
                        #endregion
                        listaDestinatario.Add(destinatario);
                    }
                    objRideGuiaRemision._destinatarios = listaDestinatario;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                objRideGuiaRemision.BanderaGeneracionObjeto = false;
                mensajeError = "Error al procesar el xml firmado de la Guia Remision. Mensaje Error: " + ex.Message + ". En: " + ex.TargetSite + ". Origen error: " + ex.Source;

            }
            return objRideGuiaRemision;
        }
    }
}
