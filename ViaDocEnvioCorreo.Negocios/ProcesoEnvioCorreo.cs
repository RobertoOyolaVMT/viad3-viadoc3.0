using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using ViaDoc.AccesoDatos.compania;
using ViaDoc.AccesoDatos.winServCorreos;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;
using ViaDocEnvioCorreo.Negocios;
using ViaDocEnvioCorreo.Negocios.procesos;

namespace ViaDocEnvioCorreo.LogicaNegocios
{
    public class ProcesoEnvioCorreo
    {
        MetodoProcesoCorreo _metodosProceso = new MetodoProcesoCorreo();
        ProcesoCorreoAD _metodosCorreo = new ProcesoCorreoAD();
        CompaniaAD _metodosConsulta = new CompaniaAD();
        ProcesoEnvioPortalAD _metodosPortal = new ProcesoEnvioPortalAD();
        ProcesoEnvioMail procesoEnvioCorreo = new ProcesoEnvioMail();
        int codigoRetorno = 0;
        string descripcionRetorno = string.Empty;

        public void EnvioDocumentosCorreoElectronico(string txEmail, string procesoCorreo, int ciCompania, string tipoDocumento, int cantidadCorreo, string claveAccesoDocumento,
            ref string mensajeRetorno)
        {
            try
            {
                List<XmlGenerados> xmlComprobantes;
                DataTable dtConsultaCorreos = _metodosCorreo.ConsultaCorreosEnviar(procesoCorreo, ciCompania, tipoDocumento, cantidadCorreo,
                                                                                claveAccesoDocumento, ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                {
                    xmlComprobantes = new List<XmlGenerados>();

                    #region Crea una lista de Objetos EntySmtp
                    var correoSMTP = from info in dtConsultaCorreos.AsEnumerable()
                                     let ciTipoEmision = info.Field<object>("codTipoEmision")
                                     let codDocumento = info.Field<object>("codDocumento")
                                     let codIdentity = info.Field<object>("codIdentity")
                                     let codCompania = info.Field<object>("codCompania")
                                     let claveAcceso = info.Field<object>("claveAcceso")
                                     let Email = info.Field<object>("Email")
                                     let xmlDocumentoAutorizado = info.Field<object>("xmlDocumentoAutorizado")
                                     let txFechaHoraAutorizacion = info.Field<object>("fechaHoraAutorizacion")
                                     let TxNumeroAutorizacion = info.Field<object>("numeroAutorizacion")
                                     let IdentificacionComprador = info.Field<object>("IdentificacionComprador")
                                     let RazonSocialComprador = info.Field<object>("RazonSocialComprador")
                                     let ciNumeroIntento = info.Field<object>("NumeroIntento")

                                     select new XmlGenerados
                                     {
                                         CiTipoEmision = ciTipoEmision.ToString(),
                                         Identity = int.Parse(codIdentity.ToString()),
                                         CiTipoDocumento = codDocumento.ToString(),
                                         CiCompania = int.Parse(codCompania.ToString()),
                                         ClaveAcceso = claveAcceso.ToString().Trim(),
                                         Email = Email.ToString(),
                                         XmlComprobante = xmlDocumentoAutorizado.ToString(),
                                         TxNumeroAutorizacion = TxNumeroAutorizacion.ToString(),
                                         txFechaHoraAutorizacion = txFechaHoraAutorizacion.ToString(),
                                         IdentificacionComprador = IdentificacionComprador.ToString(),
                                         RazonSocialComprador = RazonSocialComprador.ToString(),
                                         ciNumeroIntento = Convert.ToInt32(ciNumeroIntento.ToString().Trim())
                                     };
                    xmlComprobantes = correoSMTP.ToList();

                    #endregion Crea una lista de Objetos EntySmtp

                    int contigencia = procesoCorreo.Equals("FIRMA") ? 2 : 3;
                    if (xmlComprobantes.Count > 0)
                    {
                        DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTP("C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);

                        if (codigoRetorno.Equals(0))
                        {
                            if (responseSMTP.Rows.Count > 0)
                            {
                                procesoEnvioCorreo.ProcesoEnvioRideXMLCliente(txEmail, tipoDocumento, xmlComprobantes, responseSMTP, contigencia, ref codigoRetorno, ref descripcionRetorno);
                                mensajeRetorno = descripcionRetorno;
                            }
                        }
                    }
                    else
                    {
                        mensajeRetorno = "No hay datos disponibles";
                    }
                }
                else
                {
                    mensajeRetorno = descripcionRetorno;
                }
            }
            catch (Exception ex)
            {
                mensajeRetorno = descripcionRetorno;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin($"Metodo: EnvioDocumentosCorreoElectronico, Excpetion: {ex.Message}, MensajeError: {mensajeRetorno}, LimeaError: {ex.TargetSite}");
            }
        }

        public void EnvioDocumentosCorreoElectronicoHistorico(string nombreHistorico,
            string txEmail, string procesoCorreo, int ciCompania, string tipoDocumento, int cantidadCorreo, string claveAccesoDocumento,
            ref string mensajeRetorno)
        {
            try
            {
                List<XmlGenerados> xmlComprobantes;
                DataTable dtConsultaCorreos = _metodosCorreo.ConsultaCorreosEnviarHistorico(nombreHistorico, procesoCorreo, ciCompania, tipoDocumento, cantidadCorreo,
                                                                                claveAccesoDocumento, ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                {
                    xmlComprobantes = new List<XmlGenerados>();

                    #region Crea una lista de Objetos EntySmtp
                    var correoSMTP = from info in dtConsultaCorreos.AsEnumerable()
                                     let ciTipoEmision = info.Field<object>("codTipoEmision")
                                     let codDocumento = info.Field<object>("codDocumento")
                                     let codIdentity = info.Field<object>("codIdentity")
                                     let codCompania = info.Field<object>("codCompania")
                                     let claveAcceso = info.Field<object>("claveAcceso")
                                     let Email = info.Field<object>("Email")
                                     let xmlDocumentoAutorizado = info.Field<object>("xmlDocumentoAutorizado")
                                     let txFechaHoraAutorizacion = info.Field<object>("fechaHoraAutorizacion")
                                     let TxNumeroAutorizacion = info.Field<object>("numeroAutorizacion")
                                     let IdentificacionComprador = info.Field<object>("IdentificacionComprador")
                                     let RazonSocialComprador = info.Field<object>("RazonSocialComprador")
                                     let ciNumeroIntento = info.Field<object>("NumeroIntento")

                                     select new XmlGenerados
                                     {
                                         CiTipoEmision = ciTipoEmision.ToString(),
                                         Identity = int.Parse(codIdentity.ToString()),
                                         CiTipoDocumento = codDocumento.ToString(),
                                         CiCompania = int.Parse(codCompania.ToString()),
                                         ClaveAcceso = claveAcceso.ToString().Trim(),
                                         Email = Email.ToString(),
                                         XmlComprobante = xmlDocumentoAutorizado.ToString(),
                                         TxNumeroAutorizacion = TxNumeroAutorizacion.ToString(),
                                         txFechaHoraAutorizacion = txFechaHoraAutorizacion.ToString(),
                                         IdentificacionComprador = IdentificacionComprador.ToString(),
                                         RazonSocialComprador = RazonSocialComprador.ToString(),
                                         ciNumeroIntento = Convert.ToInt32(ciNumeroIntento.ToString().Trim())
                                     };
                    xmlComprobantes = correoSMTP.ToList();

                    #endregion Crea una lista de Objetos EntySmtp

                    int contigencia = procesoCorreo.Equals("FIRMA") ? 2 : 3;
                    if (xmlComprobantes.Count > 0)
                    {
                        DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTPHistorico(nombreHistorico, "C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);

                        if (codigoRetorno.Equals(0))
                        {
                            if (responseSMTP.Rows.Count > 0)
                            {
                                procesoEnvioCorreo.ProcesoEnvioRideXMLClienteHistorico(nombreHistorico, txEmail, tipoDocumento, xmlComprobantes, responseSMTP, contigencia, ref codigoRetorno, ref descripcionRetorno);
                                mensajeRetorno = descripcionRetorno;
                            }
                        }
                    }
                    else
                    {
                        mensajeRetorno = "No hay datos disponibles";
                    }
                }
                else
                {
                    mensajeRetorno = descripcionRetorno;
                }
            }
            catch (Exception ex)
            {
                mensajeRetorno = descripcionRetorno;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin($"Metodo: EnvioDocumentosCorreoElectronicoHistorico, Excpetion: {ex.Message}, MensajeError: {mensajeRetorno}, LimeaError: {ex.TargetSite}");
            }
        }

        public void EnviarDocumentosPortalWeb(string procesoCorreo, int ciCompania, string tipoDocumento, int cantidadCorreo, string claveAcceso, ref string mensajeRetorno)
        {
            List<XmlGenerados> listaXmlGenerados = new List<XmlGenerados>();
            ProcesoEnvioPortal _procesoEnvioPortal = new ProcesoEnvioPortal();
            string urlPortal = string.Empty;
            try
            {
                DataTable dtDocumentosPortal = _metodosPortal.ConsultaDocumentosEnviarPortal(procesoCorreo, ciCompania, tipoDocumento,
                                                                                             cantidadCorreo, claveAcceso,
                                                                                             ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                {
                    foreach (DataRow listaDocumentos in dtDocumentosPortal.Rows)
                    {
                        int contigenciaDet = 0;
                        string XmlEstado = string.Empty;

                        if (procesoCorreo == "Firma")
                            contigenciaDet = 2;
                        else
                            contigenciaDet = 3;

                        XmlGenerados xmlGenerado = new XmlGenerados
                        {
                            Identity = Convert.ToInt32(listaDocumentos["codIdentity"].ToString()),
                            CiCompania = Convert.ToInt32(listaDocumentos["codCompania"].ToString()),
                            CiTipoEmision = listaDocumentos["codTipoEmision"].ToString(),
                            CiContingenciaDet = contigenciaDet,
                            CiTipoDocumento = listaDocumentos["codDocumento"].ToString(),
                            ClaveAcceso = listaDocumentos["claveAcceso"].ToString(),
                            XmlComprobante = listaDocumentos["xmlDocumentoAutorizado"].ToString(),
                            TxNumeroAutorizacion = listaDocumentos["claveAcceso"].ToString().Trim(),
                            txFechaHoraAutorizacion = listaDocumentos["fechaHoraAutorizacion"].ToString().Trim(),
                            XmlEstado = listaDocumentos["estado"].ToString().Trim(),
                            rucCompania = listaDocumentos["rucCompania"].ToString().Trim(),
                            fechaEmision = listaDocumentos["fechaEmision"].ToString().Trim(),
                            numeroDocumento = listaDocumentos["num_documento"].ToString().Trim(),
                            IdentificacionComprador = listaDocumentos["identificacionComprador"].ToString().Trim(),
                        };
                        listaXmlGenerados.Add(xmlGenerado);
                    }

                    if (listaXmlGenerados.Count > 0)
                        _procesoEnvioPortal.EnviarComprobantesAutorizadosEnvioPortal(listaXmlGenerados, procesoCorreo);
                }
                else
                {
                    mensajeRetorno = descripcionRetorno;
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error envio email: " + ex.Message);
            }
        }

        public void EnviarDocumentosPortalWebHistorico(string nombreHistorico, string procesoCorreo, int ciCompania, string tipoDocumento, int cantidadCorreo, string claveAcceso, ref string mensajeRetorno)
        {
            List<XmlGenerados> listaXmlGenerados = new List<XmlGenerados>();
            ProcesoEnvioPortal _procesoEnvioPortal = new ProcesoEnvioPortal();
            string urlPortal = string.Empty;
            try
            {
                DataTable dtDocumentosPortal = _metodosPortal.ConsultaDocumentosEnviarPortalHistorico(nombreHistorico, procesoCorreo, ciCompania, tipoDocumento,
                                                                                             cantidadCorreo, claveAcceso,
                                                                                             ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                {
                    foreach (DataRow listaDocumentos in dtDocumentosPortal.Rows)
                    {
                        int contigenciaDet = 0;
                        string XmlEstado = string.Empty;

                        if (procesoCorreo == "Firma")
                            contigenciaDet = 2;
                        else
                            contigenciaDet = 3;

                        XmlGenerados xmlGenerado = new XmlGenerados
                        {
                            Identity = Convert.ToInt32(listaDocumentos["codIdentity"].ToString()),
                            CiCompania = Convert.ToInt32(listaDocumentos["codCompania"].ToString()),
                            CiTipoEmision = listaDocumentos["codTipoEmision"].ToString(),
                            CiContingenciaDet = contigenciaDet,
                            CiTipoDocumento = listaDocumentos["codDocumento"].ToString(),
                            ClaveAcceso = listaDocumentos["claveAcceso"].ToString(),
                            XmlComprobante = listaDocumentos["xmlDocumentoAutorizado"].ToString(),
                            TxNumeroAutorizacion = listaDocumentos["claveAcceso"].ToString().Trim(),
                            txFechaHoraAutorizacion = listaDocumentos["fechaHoraAutorizacion"].ToString().Trim(),
                            XmlEstado = listaDocumentos["estado"].ToString().Trim(),
                            rucCompania = listaDocumentos["rucCompania"].ToString().Trim(),
                            fechaEmision = listaDocumentos["fechaEmision"].ToString().Trim(),
                            numeroDocumento = listaDocumentos["num_documento"].ToString().Trim(),
                            IdentificacionComprador = listaDocumentos["identificacionComprador"].ToString().Trim(),
                        };
                        listaXmlGenerados.Add(xmlGenerado);
                    }

                    if (listaXmlGenerados.Count > 0)
                        _procesoEnvioPortal.EnviarComprobantesAutorizadosEnvioPortal(listaXmlGenerados, procesoCorreo);
                }
                else
                {
                    mensajeRetorno = descripcionRetorno;
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error envio email: " + ex.Message);
            }
        }


        public void EnviarNotificacionesEstadistica()
        {
            ProcesoEnvioMail procesoEnvioCorreo = new ProcesoEnvioMail();
            try
            {
                DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTP("C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    List<Smtp> listaSmtp = _metodosProceso.ListarConfiguracionesSmtp(responseSMTP);

                    if (listaSmtp.Count > 0)
                    {
                        procesoEnvioCorreo.EnviarMailNotificacionEstadistica(listaSmtp);
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: " + ex.Message);
            }
        }

        public void EnviarNotifacionCertificadoCaducado()
        {
            try
            {
                DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTP("C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    if (responseSMTP.Rows.Count > 0)
                    {
                        procesoEnvioCorreo.EnviarMainNotificacionCertificado(responseSMTP);

                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Autorizacion: " + ex.ToString());
            }
        }

        public void NotificacinDocError()
        {
            try
            {
                string codDocElec = ConfigurationManager.AppSettings.Get("codDocElec").Trim();
                string[] codDoc = codDocElec.Split('|');

                foreach (var cod in codDoc)
                {
                    DataTable dtDocConsult = _metodosCorreo.ConsultaCorreosEnviar("NOTIFICACION", 0, cod, 5,
                                                                                "", ref codigoRetorno, ref descripcionRetorno);

                    DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTP("C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);
                    if (codigoRetorno.Equals(0))
                    {
                        if (responseSMTP.Rows.Count > 0 && dtDocConsult.Rows.Count > 0)
                        {

                            procesoEnvioCorreo.NotificacinDocError(responseSMTP, dtDocConsult);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Autorizacion: " + ex.ToString());
            }
        }

        public void NotificacinDocAtrasados()
        {
            try
            {
                string codDocElec = ConfigurationManager.AppSettings.Get("codDocElec").Trim();

                DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTP("C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    procesoEnvioCorreo.NotificacinDocAtrasados(responseSMTP, codDocElec);
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Autorizacion: " + ex.ToString());
            }
        }

        public void EnvioMailError()
        {
            ProcesoEnvioMail procesoEnvioCorreo = new ProcesoEnvioMail();
            try
            {
                DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTP("C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    List<Smtp> listaSmtp = _metodosProceso.ListarConfiguracionesSmtp(responseSMTP);

                    if (listaSmtp.Count > 0)
                    {
                        procesoEnvioCorreo.EnvioMailError(listaSmtp);
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: " + ex.Message);
            }
        }

        public void Reenvio_a_Portal()
        {
            List<XmlGenerados> listaXmlGenerados = new List<XmlGenerados>();
            string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").Trim();
            var procesoCorreo = "REEPROCESOWEB";
            try
            {
                String[] ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraReprocesoDocumento.txt");
                Int32 intHoraSystema = Convert.ToInt32(DateTime.Now.ToString("H:mm").Trim().Replace(":", ""));
                foreach (String strHoraEntre in ArrayStrHorasEjecucion)
                {
                    String[] arrStrHoraEntre = strHoraEntre.Trim().Split('-');
                    Int32 intHoraInicio = Convert.ToInt32(arrStrHoraEntre[0].Trim().Replace(":", ""));
                    Int32 intHoraFin = Convert.ToInt32(arrStrHoraEntre[1].Trim().Replace(":", ""));
                    if (intHoraSystema >= intHoraInicio && intHoraSystema <= intHoraFin)
                    {
                        DataTable responseSMTP = _metodosConsulta.GuardaConfigSMTP("C2", "", "", "", "", "", "", "", "", "", "", "", "", ref codigoRetorno, ref descripcionRetorno);
                        string codDocElec = ConfigurationManager.AppSettings.Get("codDocElec").Trim();
                        string[] codDoc = codDocElec.Split('|');
                        foreach (DataRow smtp in responseSMTP.Rows)
                        {
                            foreach (var cod in codDoc)
                            {
                                DataTable dtDocConsult = _metodosPortal.ConsultaDocumentosEnviarPortal(procesoCorreo, Convert.ToUInt16(smtp["ciCompania"].ToString().Trim()), cod,
                                                                                                         20, null,
                                                                                                         ref codigoRetorno, ref descripcionRetorno);

                                if (dtDocConsult.Rows.Count > 0)
                                {
                                    foreach (DataRow listaDocumentos in dtDocConsult.Rows)
                                    {
                                        int contigenciaDet = 0;

                                        contigenciaDet = procesoCorreo == "Firma" ? 2 : 3;

                                        XmlGenerados xmlGenerado = new XmlGenerados
                                        {
                                            Identity = Convert.ToInt32(listaDocumentos["codIdentity"].ToString()),
                                            CiCompania = Convert.ToInt32(listaDocumentos["codCompania"].ToString()),
                                            CiTipoEmision = listaDocumentos["codTipoEmision"].ToString(),
                                            CiContingenciaDet = contigenciaDet,
                                            CiTipoDocumento = listaDocumentos["codDocumento"].ToString(),
                                            ClaveAcceso = listaDocumentos["claveAcceso"].ToString(),
                                            XmlComprobante = listaDocumentos["xmlDocumentoAutorizado"].ToString(),
                                            TxNumeroAutorizacion = listaDocumentos["claveAcceso"].ToString().Trim(),
                                            txFechaHoraAutorizacion = listaDocumentos["fechaHoraAutorizacion"].ToString().Trim(),
                                            XmlEstado = listaDocumentos["estado"].ToString().Trim(),
                                            rucCompania = listaDocumentos["rucCompania"].ToString().Trim(),
                                            fechaEmision = listaDocumentos["fechaEmision"].ToString().Trim(),
                                            numeroDocumento = listaDocumentos["num_documento"].ToString().Trim(),
                                            IdentificacionComprador = listaDocumentos["identificacionComprador"].ToString().Trim(),
                                        };
                                        listaXmlGenerados.Add(xmlGenerado);
                                    }

                                    if (codigoRetorno.Equals(0))
                                    {
                                        if (dtDocConsult.Rows.Count > 0 && dtDocConsult.Rows.Count > 0)
                                        {
                                            procesoEnvioCorreo.Reenvio_a_Portal(listaXmlGenerados, procesoCorreo);

                                        }
                                    }
                                }
                                else
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("No se encontro documentos pendiemtes para envio al portal.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Autorizacion: " + ex.ToString());
            }
        }
    }
}
