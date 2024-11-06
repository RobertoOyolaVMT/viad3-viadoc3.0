using Newtonsoft.Json;
using ReportesViaDoc.EntidadesReporte;
using ReportesViaDoc.LogicaReporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ViaDoc.Configuraciones;

namespace ReportesViaDoc
{
    public static class GenerarRideDocumentoElectronico
    {
        public static byte[] GenerarRiderComprobantesAutorizados(ref string mensajeRespuesta, String XmlAutorizado, String txFechaHoraAutorizacion, String TxNumeroAutorizacion, String CodDoc, String CiEstado, DataSet dsConfiguraciones, DataSet dsCatalogoReporte)
        {
           

            bool banderaConfiguracion = true;
            bool banderaCatalogo = true;
            byte[] pdfByte = null;
            ExcepcionesReportes reporte = new ExcepcionesReportes();
            try
            {
                string mensajeError = "";

                List<ConfiguracionReporte> listaConfiguracion = new List<ConfiguracionReporte>();
                List<CatalogoReporte> catalogoReporte = new List<CatalogoReporte>();
                #region Crea lista de Configuracion Reporte
                if (dsConfiguraciones.Tables.Count > 0)
                {
                    if (dsConfiguraciones.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            var configuracion = from dato in dsConfiguraciones.Tables[0].AsEnumerable()
                                                let idConf = dato.Field<int>("idConfiguracion")
                                                let codrefe = dato.Field<string>("codReferencia")
                                                let desc = dato.Field<string>("descripcion")
                                                let idcompania = dato.Field<int>("ciCompania")
                                                let ruc = dato.Field<string>("ruc")
                                                let param1 = dato.Field<string>("param1")
                                                let param2 = dato.Field<string>("param2")
                                                let param3 = dato.Field<string>("param3")
                                                let param4 = dato.Field<string>("param4")
                                                let param5 = dato.Field<string>("param5")
                                                select new ConfiguracionReporte()
                                                {
                                                    IdConfiguracion = idConf,
                                                    CodigoReferencia = codrefe,
                                                    Descripcion = desc,
                                                    IdCompania = idcompania,
                                                    RucCompania = ruc,
                                                    Configuracion1 = param1,
                                                    Configuracion2 = param2,
                                                    Configuracion3 = param3,
                                                    Configuracion4 = param4,
                                                    Configuracion5 = param5,
                                                };
                            listaConfiguracion = configuracion.ToList();
                        }
                        catch (Exception ex)
                        {
                            //sb.Write("catch 1");
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("1 ex: " + ex.ToString());
                            banderaConfiguracion = false;
                        }
                    }
                    else
                        banderaConfiguracion = false;
                }
                else
                    banderaConfiguracion = false;
                #endregion

                #region Crea lista de catalogos del sistema
                if (dsCatalogoReporte.Tables.Count > 0)
                {
                    if (dsCatalogoReporte.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            var catalogo = from dato in dsCatalogoReporte.Tables[0].AsEnumerable()
                                           let codReferencia = dato.Field<string>("codReferencia")
                                           let descripcion = dato.Field<string>("descripcion")
                                           let param1 = dato.Field<string>("codigo")
                                           let param2 = dato.Field<string>("valor")
                                           select new CatalogoReporte()
                                           {
                                               CodigoReferencia = codReferencia,
                                               Descripcion = descripcion,
                                               Codigo = param1,
                                               Valor = param2
                                           };
                            catalogoReporte = catalogo.ToList();
                        }
                        catch (Exception ex)
                        {
                            banderaCatalogo = false;
                        }
                    }
                    else
                        banderaCatalogo = false;
                }
                else
                    banderaCatalogo = false;
                #endregion

                if (banderaConfiguracion)
                {
                    ExcepcionesReportes reportes = new ExcepcionesReportes();
                    if (banderaCatalogo)
                    {
                        if (CodDoc == CatalogoViaDoc.DocumentoFactura)
                        {
                            
                            #region GenerarRiderFactura
                            ProcesarRideFactura objPdfFactura = new ProcesarRideFactura();

                            RideFactura objRideFactura = objPdfFactura.ProcesarXmlAutorizadoFactura(ref mensajeError, XmlAutorizado, txFechaHoraAutorizacion, TxNumeroAutorizacion);

                            if (mensajeError.Equals(""))
                            {
                                if (objRideFactura.BanderaGeneracionObjeto)
                                {
                                    mensajeError = "";
                                    pdfByte = objPdfFactura.GenerarRideFactura(ref mensajeError, objRideFactura, listaConfiguracion, catalogoReporte);
                                    mensajeRespuesta = mensajeError;

                                }
                                else
                                    mensajeRespuesta = mensajeError;
                            }
                            #endregion
                        }
                        if (CodDoc == CatalogoViaDoc.DocumentoNotaCredito)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("ingresa la nota de credito");
                            #region GenerarRiderNotaCredito
                            ProcesarRideNotaCredito objPdfNotaCredito = new ProcesarRideNotaCredito();
                            RideNotaCredito objRideNotaCredito = objPdfNotaCredito.ProcesarXmlAutorizadoNotaCredito(ref mensajeError, XmlAutorizado, txFechaHoraAutorizacion, TxNumeroAutorizacion);
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("finaliza proceso xml");
                            if (mensajeError.Equals(""))
                            {
                                if (objRideNotaCredito.BanderaGeneracionObjeto)
                                {
                                    mensajeError = "";
                                    pdfByte = objPdfNotaCredito.GenerarRideNotaCredito(ref mensajeError, objRideNotaCredito, listaConfiguracion, catalogoReporte);
                                    mensajeRespuesta = mensajeError;
                                }
                                else
                                    mensajeRespuesta = mensajeError;
                            }
                            #endregion GenerarRiderNotaCredito
                        }
                        if (CodDoc == CatalogoViaDoc.DocumentoNotaDebito)
                        {
                            #region GenerarRiderNotaDebito
                            ProcesarRideNotaDebito objPdfNotaDebito = new ProcesarRideNotaDebito();
                            RideNotaDebito objRideNotaDebito = objPdfNotaDebito.ProcesarXmlAutorizadoNotaCredito(ref mensajeError, XmlAutorizado, txFechaHoraAutorizacion, TxNumeroAutorizacion);
                            if (mensajeError.Equals(""))
                            {
                                if (objRideNotaDebito.BanderaGeneracionObjeto)
                                {
                                    mensajeError = "";
                                    pdfByte = objPdfNotaDebito.GenerarRideNotaDebito(ref mensajeError, objRideNotaDebito, listaConfiguracion, catalogoReporte);
                                    mensajeRespuesta = mensajeError;
                                }
                                else
                                    mensajeRespuesta = mensajeError;
                            }
                            #endregion GenerarRiderNotaCredito
                        }//--------------------------------------------------------------------------------------------------------------------------------------------------------
                        if (CodDoc == CatalogoViaDoc.DocumentoGuiaRemision)
                        {
                            #region Generar Ride de Guia de Remision
                            ProcesarRideGuiaRemision objPdfGuiaRemision = new ProcesarRideGuiaRemision();
                            RideGuiaRemision objRideGuiaRemision = objPdfGuiaRemision.ProcesarXmlAutorizadoGuiaRemision(ref mensajeError, XmlAutorizado, txFechaHoraAutorizacion, TxNumeroAutorizacion);
                            if (mensajeError.Equals(""))
                            {
                                if (objRideGuiaRemision.BanderaGeneracionObjeto)
                                {
                                    mensajeError = "";
                                    pdfByte = objPdfGuiaRemision.GenerarRideGuiaRemision(ref mensajeError, objRideGuiaRemision, listaConfiguracion, catalogoReporte);
                                    mensajeRespuesta = mensajeError;
                                }
                                else 
                                    mensajeRespuesta = mensajeError;
                            }
                            #endregion
                        }
                        if (CodDoc == CatalogoViaDoc.DocumentoCompRetencion)
                        {
                            #region GenerarPdfComprobanteRetencion
                            ProcesarRideCompRetencion objPdfCompRetencion = new ProcesarRideCompRetencion();
                            RideCompRetencion objRideCompRetencion = objPdfCompRetencion.GeneraComprobanteRetencionPDF(ref mensajeError, XmlAutorizado, txFechaHoraAutorizacion, TxNumeroAutorizacion);
                            if (mensajeError.Equals(""))
                            {
                                if (objRideCompRetencion.BanderaGeneracionObjeto)
                                {
                                    mensajeError = "";
                                    pdfByte = objPdfCompRetencion.GenerarRideCompRetencion(ref mensajeError, objRideCompRetencion, listaConfiguracion, catalogoReporte);
                                    mensajeRespuesta = mensajeError;
                                }
                                else
                                    mensajeRespuesta = mensajeError;
                            }
                            #endregion
                        }
                        if (CodDoc == CatalogoViaDoc.DocumentoLiquidacion)
                        {
                            #region GenerarRiderLiquidacion
                            ProcesarRideLiquidacion objPdfLiquiacion = new ProcesarRideLiquidacion();
                            RideLiquidacion objRideLiquidacion = objPdfLiquiacion.ProcesarXmlAutorizadoLiquidacion(ref mensajeError, XmlAutorizado, txFechaHoraAutorizacion, TxNumeroAutorizacion);
                            if (mensajeError.Equals(""))
                            {
                                if (objRideLiquidacion.BanderaGeneracionObjeto)
                                {
                                    mensajeError = "";
                                    pdfByte = objPdfLiquiacion.GenerarRideLiquidaicon(ref mensajeError, objRideLiquidacion, listaConfiguracion, catalogoReporte);
                                    mensajeRespuesta = mensajeError;
                                }
                                else
                                    mensajeRespuesta = mensajeError;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        mensajeRespuesta = "Error al generar byte del RIDE. Mensaje error: Error al generar la lista de catalogos";
                    }
                }
                else
                {
                    mensajeRespuesta = "Error al generar byte del RIDE. Mensaje error: Error al generar la lista de configuracion del reporte";
                }
                #region ELIMINA_IMAGENES_CREADAS_POR_LOS_RIDERS
                string[] archivos = Directory.GetFiles(CatalogoViaDoc.rutaCodigoBarra);
                foreach (string ArchivosPng in archivos)
                {
                    if (ArchivosPng.Contains(".jpg") == true)
                    {
                        System.IO.File.Delete(ArchivosPng);
                    }
                }
                #endregion ELIMINA_IMAGENES_CREADAS_POR_LOS_RIDERS
            }
            catch (Exception ex)
            {
                string err = "Exception GenerarRiderComprobantesAutorizados: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(err);
            }
            return pdfByte;
        }
    }
}
