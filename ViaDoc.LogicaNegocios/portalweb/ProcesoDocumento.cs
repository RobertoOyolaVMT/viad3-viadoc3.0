﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ViaDoc.AccesoDatos.portalWeb;
using ViaDoc.EntidadNegocios.portalWeb;



namespace ViaDoc.LogicaNegocios.portalweb
{
    public class ProcesoDocumento
    {
        DocumentoAD metodosConsulta = new DocumentoAD();
        public List<Documento> ConsultarDocumentosTodos(Documento documento, ref int codigoRetorno, ref string mensajeRetorno)
        {
            string urlBase = ConfigurationManager.AppSettings.Get("Url_Base").ToString().Trim();

            List<Documento> listaDocumentos = new List<Documento>();
            try
            {
                DataSet dsResultado = metodosConsulta.ConsultosDocumentos("1", documento.razonSocial, documento.claveAcceso,
                                                                          documento.numeroAutorizacion,
                                                                          documento.NumeroDocumento, documento.identificacionComprador,
                                                                          documento.fechaDesde, documento.fechaHasta,
                                                                          documento.tipoDocumento, documento.razonSocialComprador,
                                                                          documento.filtroFechaDH, ref codigoRetorno,
                                                                          ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    DataTable dtResultado = dsResultado.Tables[0];
                    foreach (DataRow resultado in dtResultado.Rows)
                    {
                        Documento _documento = new Documento()
                        {
                            documentoAutorizado = resultado["documentoAutorizado"].ToString(),
                            razonSocial = resultado["razonSocial"].ToString(),
                            fechaHoraAutorizacion = resultado["fechaHoraAutorizacion"].ToString(),
                            claveAcceso = resultado["claveAcceso"].ToString(),
                            numeroAutorizacion = resultado["numeroAutorizacion"].ToString(),
                            NumeroDocumento = resultado["NumeroDocumento"].ToString(),
                            identificacionComprador = resultado["identificacionComprador"].ToString(),
                            razonSocialComprador = resultado["razonSocialComprador"].ToString(),
                            descripcion = resultado["descripcion"].ToString(),
                            descripcionEstado = resultado["descripcionEstado"].ToString(),
                            descripcionEmision = resultado["descripcionEmision"].ToString(),
                            tipoDocumento = resultado["descripcion"].ToString(),
                            idEstado = resultado["idEstado"].ToString(),
                            idCompania = resultado["idCompania"].ToString(),
                            email = resultado["email"].ToString(),
                            fechaEmision = resultado["FechaEmision"].ToString(),
                            valor = resultado["valor"].ToString().Replace(",", "."),
                            subtotal = resultado["Subtotal"].ToString().Replace(",", "."),
                            totaliva = resultado["TotalIva"].ToString().Replace(",", "."),
                            xml = "<form action='" + urlBase + "/Documentos/DescargarXML'enctype='multipart/form-data' method='post'><input type='hidden' name='txtClaveAcceso' value='" + resultado["claveAcceso"].ToString() + "' /><input type='hidden' name='txtTipoDocumento' value='" + resultado["descripcion"].ToString() + "' /><button type='submit' class='btn btn-danger btn-sm'><span class='glyphicon glyphicon-download-alt'></span> Descargar XML</button></form>",
                            pdf = "<form action='" + urlBase + "/Documentos/DescargarPDF'enctype='multipart/form-data' method='post'><input type='hidden' name='ciIdCompania' value='" + resultado["idCompania"].ToString() + "' /><input type='hidden' name='txFechaHoraAutorizacion' value='" + resultado["fechaHoraAutorizacion"].ToString() + "' /><input type='hidden' name='txNumeroAutorizacion' value='" + resultado["numeroAutorizacion"].ToString() + "' /><input type='hidden' name='txTipoDocumento' value='" + resultado["descripcion"].ToString() + "' /><input type='hidden' name='txNumeroDocumento' value='" + resultado["NumeroDocumento"].ToString() + "' /><button type='submit' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-download-alt'></span> Descargar PDF</button></form>",
                            enviarMail = "<input type='button' value='Enviar Email' class='btn btn-info' onclick='EnviaEmail(\"" + resultado["idEstado"].ToString() + "\", \"" + resultado["idCompania"].ToString() + "\", \"" + resultado["descripcion"].ToString() + "\", \"" + resultado["claveAcceso"].ToString() + "\", \"" + resultado["email"].ToString() + "\", \"" + resultado["razonSocial"].ToString() + "\")' />",
                            enviarPortal = "<input type='button' value='Enviar Portal' class='btn btn-primary' onclick='EnviaPortal(\"" + resultado["idEstado"].ToString() + "\", \"" + resultado["idCompania"].ToString() + "\", \"" + resultado["descripcion"].ToString() + "\", \"" + resultado["claveAcceso"].ToString() + "\")' />",
                            corrigdeta = "<input type='button' value='Corregir Detalle' class='btn btn-danger btn-sm' onclick='CorrigeDetalleModal(\"" + resultado["claveAcceso"].ToString() + "\", \"" + resultado["descripcion"].ToString() + "\")' />",
                        };
                        listaDocumentos.Add(_documento);
                    }

                }

            }
            catch (Exception message)
            {
                System.IO.StreamWriter sb = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\MigradorViaDoc" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
                sb.Write("[Metodo]: error: " + message + "\r\n");
                sb.Close();
            }
            return listaDocumentos;
        }

        public void ConsultaEliminarDocumentos(string tipoDocumento, string idCompania, string establecimiento, string puntoEmision, string secuencial, ref int codigoRetorno, ref string mensajeRetorno)
        {
            metodosConsulta.ConsultaEliminarDocumentos(tipoDocumento, idCompania, establecimiento, puntoEmision, secuencial, ref codigoRetorno, ref mensajeRetorno);
        }

        public List<Documento> ConsultarDocumentosTodosHistoricos(string nombreHistorico, Documento documento, ref int codigoRetorno, ref string mensajeRetorno)
        {
            string urlBase = ConfigurationManager.AppSettings.Get("Url_Base").ToString().Trim();
            List<Documento> listaDocumentos = new List<Documento>();
            try
            {
                DataSet dsResultado = metodosConsulta.ConsultosDocumentosHistoricos(nombreHistorico, "1", documento.razonSocial, documento.claveAcceso,
                                                                          documento.numeroAutorizacion,
                                                                          documento.NumeroDocumento, documento.identificacionComprador,
                                                                          documento.fechaDesde, documento.fechaHasta,
                                                                          documento.tipoDocumento, documento.razonSocialComprador,
                                                                          documento.filtroFechaDH, ref codigoRetorno,
                                                                          ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    DataTable dtResultado = dsResultado.Tables[0];
                    foreach (DataRow resultado in dtResultado.Rows)
                    {
                        Documento _documento = new Documento()
                        {
                            documentoAutorizado = resultado["documentoAutorizado"].ToString(),
                            razonSocial = resultado["razonSocial"].ToString(),
                            fechaHoraAutorizacion = resultado["fechaHoraAutorizacion"].ToString(),
                            claveAcceso = resultado["claveAcceso"].ToString(),
                            numeroAutorizacion = resultado["numeroAutorizacion"].ToString(),
                            NumeroDocumento = resultado["NumeroDocumento"].ToString(),
                            identificacionComprador = resultado["identificacionComprador"].ToString(),
                            razonSocialComprador = resultado["razonSocialComprador"].ToString(),
                            descripcion = resultado["descripcion"].ToString(),
                            descripcionEstado = resultado["descripcionEstado"].ToString(),
                            descripcionEmision = resultado["descripcionEmision"].ToString(),
                            tipoDocumento = resultado["descripcion"].ToString(),
                            idEstado = resultado["idEstado"].ToString(),
                            idCompania = resultado["idCompania"].ToString(),
                            email = resultado["email"].ToString(),
                            xml = "<form action='" + urlBase + "/DocumentosHistoricos/DescargarXML'enctype='multipart/form-data' method='post'><input type='hidden' name='empresaHistorico' value='" + nombreHistorico + "' /><input type='hidden' name='txtClaveAcceso' value='" + resultado["claveAcceso"].ToString() + "' /><input type='hidden' name='txtTipoDocumento' value='" + resultado["descripcion"].ToString() + "' /><button type='submit' class='btn btn-danger btn-sm'><span class='glyphicon glyphicon-download-alt'></span> Descargar XML</button></form>",
                            pdf = "<form action='" + urlBase + "/DocumentosHistoricos/DescargarPDF'enctype='multipart/form-data' method='post'><input type='hidden' name='empresaHistorico' value='" + nombreHistorico + "' /><input type='hidden' name='ciIdCompania' value='" + resultado["idCompania"].ToString() + "' /><input type='hidden' name='txFechaHoraAutorizacion' value='" + resultado["fechaHoraAutorizacion"].ToString() + "' /><input type='hidden' name='txNumeroAutorizacion' value='" + resultado["numeroAutorizacion"].ToString() + "' /><input type='hidden' name='txTipoDocumento' value='" + resultado["descripcion"].ToString() + "' /><input type='hidden' name='txNumeroDocumento' value='" + resultado["NumeroDocumento"].ToString() + "' /><button type='submit' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-download-alt'></span> Descargar PDF</button></form>",
                            enviarMail = "<input type='button' value='Enviar Email' class='btn btn-info' onclick='EnviaEmail(\"" + resultado["idEstado"].ToString() + "\", \"" + resultado["idCompania"].ToString() + "\", \"" + resultado["descripcion"].ToString() + "\", \"" + resultado["claveAcceso"].ToString() + "\", \"" + resultado["email"].ToString() + "\", \"" + resultado["razonSocial"].ToString() + "\")' />",
                            enviarPortal = "<input type='button' value='Enviar Portal' class='btn btn-primary' onclick='EnviaPortal(\"" + resultado["idEstado"].ToString() + "\", \"" + resultado["idCompania"].ToString() + "\", \"" + resultado["descripcion"].ToString() + "\", \"" + resultado["claveAcceso"].ToString() + "\")' />",
                        };
                        listaDocumentos.Add(_documento);
                    }
                }
            }
            catch (Exception message)
            {
                System.IO.StreamWriter sb = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\MigradorViaDoc" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
                sb.Write("[Metodo]: error: " + message + "\r\n");
                sb.Close();
            }
            return listaDocumentos;
        }

        public List<Autorizar> ConsultarDocumentosAutorizar(Documento documento, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<Autorizar> listaDocumentos = new List<Autorizar>();
            try
            {
                DataSet dsResultado = metodosConsulta.ConsultosDocumentos("2", documento.razonSocial, documento.claveAcceso,
                                                                          documento.numeroAutorizacion,
                                                                          documento.NumeroDocumento, documento.identificacionComprador,
                                                                          documento.fechaDesde, documento.fechaHasta,
                                                                          documento.tipoDocumento, documento.razonSocialComprador,
                                                                          documento.filtroFechaDH, ref codigoRetorno,
                                                                          ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    DataTable dtResultado = dsResultado.Tables[0];
                    foreach (DataRow lista in dtResultado.Rows)
                    {
                        Autorizar doc = new Autorizar
                        {
                            NumeroDocumento = lista["NumeroDocumento"].ToString(),
                            claveAcceso = lista["ClaveAcceso"].ToString(),
                            fechaAutorizacion = lista["fechaHoraAutorizacion"].ToString(),
                            descripcion = lista["descripcionEstado"].ToString(),
                            identificacionComprador = lista["identificacionComprador"].ToString(),
                            tipoDocumento = lista["tipoDocumento"].ToString(),
                            idCompania = lista["idCompania"].ToString(),
                        };
                        listaDocumentos.Add(doc);
                    }
                }
                else
                {
                    //listaDocumentos = null;
                }
            }
            catch (Exception ex)
            {
                //listaDocumentos = null;
            }
            return listaDocumentos;
        }

        public string ConsultarXMLDescargar(string claveAcceso, string tipoDocumento, string xml, ref int codigoRetorno, ref string mensajeRetorno)
        {
            string xmlRetorno = string.Empty;
            DataSet dsResultado = metodosConsulta.ConsultarXMLDescargar(claveAcceso, tipoDocumento, xml, ref codigoRetorno,
                                                                      ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables[1].Rows.Count > 0)
                {
                    xmlRetorno = dsResultado.Tables[1].Rows[0]["xmlDocumentoAutorizado"].ToString();
                }
                else
                {
                    xmlRetorno = "";
                }
            }
            return xmlRetorno;
        }

        public string ConsultarXMLDescargarWs(string claveAcceso, string tipoDocumento, ref string ciCompania, ref string txFechaAutorizacion, ref string numAutorizacion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            string xmlRetorno = string.Empty;
            DataSet dsResultado = metodosConsulta.ConsultarXMLDescargar(claveAcceso, tipoDocumento, "", ref codigoRetorno,
                                                                      ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables[1].Rows.Count > 0)
                {
                    xmlRetorno = dsResultado.Tables[1].Rows[0]["xmlDocumentoAutorizado"].ToString();
                    ciCompania = dsResultado.Tables[1].Rows[0]["ciCompania"].ToString();
                    txFechaAutorizacion = dsResultado.Tables[1].Rows[0]["txFechaHoraAutorizacion"].ToString();
                    numAutorizacion = dsResultado.Tables[1].Rows[0]["txNumeroAutorizacion"].ToString();
                }
                else
                {
                    xmlRetorno = "";
                }
            }
            return xmlRetorno;
        }

        public string ConsultarXMLDescargarHistoricos(string empresaHistorico, string claveAcceso, string tipoDocumento, ref int codigoRetorno, ref string mensajeRetorno)
        {
            string xmlRetorno = string.Empty;
            DataSet dsResultado = metodosConsulta.ConsultarXMLDescargarHistoricos(empresaHistorico, claveAcceso, tipoDocumento, ref codigoRetorno, ref mensajeRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsResultado.Tables[1].Rows.Count > 0)
                {
                    xmlRetorno = dsResultado.Tables[1].Rows[0]["xmlDocumentoAutorizado"].ToString();
                }
                else
                {
                    xmlRetorno = "";
                }
            }
            return xmlRetorno;
        }

        public List<Estadisticas> ConsultarOpcionEstadisticas(string idCompania, string fechaInicio, string txtFechaFin, ref int codigoRetorno, ref string mensajeRetorno)
        {

            List<Estadisticas> objList = new List<Estadisticas>();
            try
            {
                DataSet dsResultado = metodosConsulta.ConsultaEstadisticas(idCompania, fechaInicio, txtFechaFin, ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in dsResultado.Tables[0].Rows)
                        {
                            Estadisticas e = new Estadisticas();
                            e.documento = item["documento"].ToString();
                            e.sinProcesar = item["SinProcesar"].ToString();
                            e.firmados = item["Firmados"].ToString();
                            e.errorFirma = item["ErroresFirma"].ToString();
                            e.errorRecepcion = item["ErroresRecepcion"].ToString();
                            e.enProceso = item["EnProceso"].ToString();
                            e.errorAutorizacion = item["ErroresAutorizacion"].ToString();
                            e.autorizado = item["Autorizados"].ToString();
                            e.noEnviadoCliente = item["NoEnviadoCliente"].ToString();
                            e.noEnviadoPortal = item["NoEnviadoPortal"].ToString();
                            e.total = item["Total"].ToString();

                            objList.Add(e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }
            return objList;
        }

        public List<Estadisticas> ConsultarOpcionEstadisticasNotificacion(string fechaInicio, string txtFechaFin, ref int codigoRetorno, ref string mensajeRetorno)
        {

            List<Estadisticas> objList = new List<Estadisticas>();
            try
            {
                DataSet dsResultado = metodosConsulta.ConsultaEstadisticasNotificacion(fechaInicio, txtFechaFin, ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in dsResultado.Tables[0].Rows)
                        {
                            Estadisticas e = new Estadisticas();
                            e.documento = item["documento"].ToString();
                            e.sinProcesar = item["SinProcesar"].ToString();
                            e.firmados = item["Firmados"].ToString();
                            e.errorFirma = item["ErroresFirma"].ToString();
                            e.errorRecepcion = item["ErroresRecepcion"].ToString();
                            e.enProceso = item["EnProceso"].ToString();
                            e.errorAutorizacion = item["ErroresAutorizacion"].ToString();
                            e.autorizado = item["Autorizados"].ToString();
                            e.noEnviadoCliente = item["NoEnviadoCliente"].ToString();
                            e.noEnviadoPortal = item["NoEnviadoPortal"].ToString();
                            e.total = item["Total"].ToString();

                            objList.Add(e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }
            return objList;
        }

        public List<EstadisticasDetalle> ConsultarOpcionEstadisticasDetalles(string compania, string fecha, string fechaHasta, string tipoDocumento, string ciEstado, ref int codigoRetorno, ref string mensajeRetorno)
        {
            List<EstadisticasDetalle> objList = new List<EstadisticasDetalle>();
            try
            {
                DataSet dsResultado = metodosConsulta.ConsultaEstadisticasDetalles(compania, fecha, fechaHasta, tipoDocumento, ciEstado, ref codigoRetorno, ref mensajeRetorno);

                if (codigoRetorno.Equals(0))
                {
                    if (dsResultado.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in dsResultado.Tables[0].Rows)
                        {
                            EstadisticasDetalle e = new EstadisticasDetalle();
                            e.NumDocumento = item["NumDocumento"].ToString();
                            e.TxMensajeError = item["txMensajeError"].ToString();
                            e.txClaveAcceso = item["txClaveAcceso"].ToString();

                            objList.Add(e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }
            return objList;
        }
    }
}
