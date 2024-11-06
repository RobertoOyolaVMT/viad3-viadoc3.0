
using ReportesViaDoc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ViaDoc.AccesoDatos.compania;
using ViaDoc.AccesoDatos.winServCorreos;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.LogicaNegocios.portalweb;
using ViaDocEnvioCorreo.LogicaNegocios;
using ViaDocEnvioCorreo.Negocios.plantilla;
using System.Collections.Generic;
using System.Linq;

namespace ViaDocEnvioCorreo.Negocios.procesos
{
    public class ProcesoEnvioMail
    {
        EnviarMail Mail = new EnviarMail();
        CompaniaAD _metodosConsulta = new CompaniaAD();
        MetodoProcesoCorreo metodos = new MetodoProcesoCorreo();

        public void ProcesoEnvioRideXMLCliente(string txEmail, string TipoDocumento, List<XmlGenerados> xmlComprobante, DataTable TblSmtp
                                            , int CiContingenciaDet, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataTable ConfigCompañia = null;
            DataSet dsCatalogo = _metodosConsulta.ConsularCatalogoSistema(1, 0, "", ref codigoRetorno, ref descripcionRetorno);

            string plantillaCorreo = string.Empty;
            PDocumentos generarPlantilla = new PDocumentos();
            try
            {
                if (codigoRetorno.Equals(0))
                {
                    foreach (XmlGenerados item in xmlComprobante)
                    {
                        try
                        {
                            DataView Dv = new DataView(TblSmtp);
                            Dv.RowFilter = " ciCompania ='" + item.CiCompania.ToString() + "'";
                            ConfigCompañia = Dv.ToTable();

                            if (!item.IdentificacionComprador.Equals(ConfigurationManager.AppSettings.Get("NOTIFICACION.CONSUMIDOR_FINAL"))
                                && !item.RazonSocialComprador.Equals(ConfigurationManager.AppSettings.Get("NOTIFICACION.RAZON_SOCIAL_CONS_FINAL")))
                            {
                                if (item.Email.Trim().Length > 5 && item.Email.Contains("@"))
                                {
                                    try
                                    {
                                        item.Identity = item.Identity;
                                        item.CiContingenciaDet = CiContingenciaDet;
                                        item.ciNumeroIntento = item.ciNumeroIntento + 1;
                                        EnviarMail Mail = new EnviarMail();

                                        if (ConfigCompañia != null)
                                        {
                                            String strNameImagenIncrustada = ConfigCompañia.Rows[0]["ciCompania"].ToString().Trim();
                                            string nuevoAsunto = string.Empty;

                                            plantillaCorreo = generarPlantilla.GenerarCorreoDocumentos(ConfigCompañia.Rows[0]["txRuc"].ToString().Trim(), ConfigCompañia.Rows[0]["urlPortal"].ToString().Trim(), ConfigCompañia.Rows[0]["txRazonSocial"].ToString().Trim(),
                                                                                                       ConfigCompañia.Rows[0]["MailAddressfrom"].ToString().Trim(), item.RazonSocialComprador);

                                            string estable = item.ClaveAcceso.Substring(24, 3);
                                            string puntoemi = item.ClaveAcceso.Substring(27, 3);
                                            string secuen = item.ClaveAcceso.Substring(30, 9);
                                            string numeroDocumento = estable + "-" + puntoemi + "-" + secuen;

                                            Mail.servidorSTMP(ConfigCompañia.Rows[0]["HostServidor"].ToString().Trim(), Convert.ToInt32(ConfigCompañia.Rows[0]["puerto"].ToString().Trim()), Convert.ToBoolean(ConfigCompañia.Rows[0]["EnableSsl"].ToString().Trim()), ConfigCompañia.Rows[0]["emailCredencial"].ToString().Trim(), ConfigCompañia.Rows[0]["passCredencial"].ToString().Trim());

                                            string imagePath = CatalogoViaDoc.rutaLogoCompania + ConfigCompañia.Rows[0]["txRuc"].ToString().Trim() + ".png";
                                            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(plantillaCorreo, null, System.Net.Mime.MediaTypeNames.Text.Html);
                                            System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(imagePath);
                                            logo.ContentId = strNameImagenIncrustada;
                                            htmlView.LinkedResources.Add(logo);
                                            Mail.AlternateViews(htmlView);

                                            nuevoAsunto = string.Format(ConfigCompañia.Rows[0]["Asunto"].ToString().Trim(), TipoDocumento, numeroDocumento);

                                            if (txEmail.Trim().Equals(""))
                                            {
                                                Mail.llenarEmail(ConfigCompañia.Rows[0]["MailAddressfrom"].ToString().Trim(), item.Email.Trim(), "", "", nuevoAsunto, plantillaCorreo);
                                            }
                                            else
                                            {
                                                string emailEnvios = txEmail.Trim();
                                                Mail.llenarEmail(ConfigCompañia.Rows[0]["MailAddressfrom"].ToString().Trim(), emailEnvios.Trim().Replace(",,", ",")/*Quito la , en caso de que lleguen 2*/, "", "", nuevoAsunto, plantillaCorreo);
                                            }

                                            DataSet dsConfiguracionCompania = _metodosConsulta.ConsularCatalogoSistema(5, int.Parse(ConfigCompañia.Rows[0]["ciCompania"].ToString().Trim()), "", ref codigoRetorno, ref descripcionRetorno);

                                            string mensaje = "";
                                            byte[] riders = GenerarRideDocumentoElectronico.GenerarRiderComprobantesAutorizados(ref mensaje, item.XmlComprobante, item.txFechaHoraAutorizacion,
                                                item.TxNumeroAutorizacion, item.CiTipoDocumento, item.XmlEstado, dsConfiguracionCompania, dsCatalogo);


                                            if (riders != null)
                                            {

                                                System.IO.MemoryStream ms = new System.IO.MemoryStream(riders);   //<-- viene null
                                                Mail.adjuntar_Documento(ms, item.ClaveAcceso.Trim() + ".pdf");

                                                Byte[] xml = System.Text.Encoding.ASCII.GetBytes(item.XmlComprobante);
                                                System.IO.MemoryStream msXml = new System.IO.MemoryStream(xml);
                                                Mail.adjuntar_Documento(msXml, item.ClaveAcceso.Trim() + ".xml");

                                                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate,
                                                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                                {
                                                    return true;
                                                };

                                                if (Mail.enviarEmail())
                                                {
                                                    item.XmlEstado = CatalogoViaDoc.DocEstadoEnviado;
                                                    item.MensajeError = "no hubo error";
                                                    item.CiContingenciaDet = 3;
                                                }
                                                else
                                                {
                                                    item.MensajeError = "Se ha presentado un error al enviar el comprobante al correo: " + item.Email + " por favor revise las configuraciones de servidor de correos o verifique que el mail del cliente final sea valido";
                                                    item.XmlEstado = CatalogoViaDoc.DocEstadoErrorEnEnvio;
                                                    item.txCodError = "103";
                                                }
                                            }
                                            else
                                            {
                                                item.MensajeError = "Se ha presentado un error al enviar el comprobante al correo: " + item.Email + " por favor revise las configuraciones de servidor de correos o verifique que el mail del cliente final sea valido";
                                                item.XmlEstado = CatalogoViaDoc.DocEstadoErrorEnEnvio;
                                                item.txCodError = "103";
                                            }
                                        }
                                        else
                                        {
                                            item.MensajeError = "La empresa: " + item.CiCompania + " no tiene configuracion SMTP ";
                                            item.XmlEstado = CatalogoViaDoc.DocEstadoErrorEnEnvio;
                                            item.txCodError = "103";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        item.MensajeError = "Hubo un error al envio del correo electronico.. " + ex.Message;
                                        item.XmlEstado = CatalogoViaDoc.DocEstadoNoEnviado;
                                        item.txCodError = "999";
                                        ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion__1: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    item.MensajeError = "Cliente no dispone de un correo electronico valido";
                                    item.XmlEstado = CatalogoViaDoc.DocEstadoNoEnviado;
                                    item.txCodError = "104";
                                    item.CiContingenciaDet = 3;
                                    item.ciNumeroIntento = 3;
                                }
                            }
                            else
                            {
                                item.XmlEstado = CatalogoViaDoc.DocEstadoEnviado;
                                item.MensajeError = "no hubo error";
                                item.CiContingenciaDet = 3;
                            }

                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El tipo de error del mensaje es: " + item.MensajeError);
                            metodos.ActualizarXmlComprobantes(item);
                        }
                        catch (Exception ex)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion___2: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion__3: " + ex.Message);
            }
        }

        public void ProcesoEnvioRideXMLClienteHistorico(string nombreHistorico, string txEmail, string TipoDocumento, List<XmlGenerados> xmlComprobante, DataTable TblSmtp, int CiContingenciaDet, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataTable ConfigCompañia = null;
            DataSet dsCatalogo = _metodosConsulta.ConsularCatalogoSistemaHistorico(nombreHistorico, 1, 0, "", ref codigoRetorno, ref descripcionRetorno);

            string plantillaCorreo = string.Empty;
            PDocumentos generarPlantilla = new PDocumentos();
            try
            {
                if (codigoRetorno.Equals(0))
                {
                    foreach (XmlGenerados item in xmlComprobante)
                    {
                        try
                        {
                            if (item.Email.Trim().Length > 5 && item.Email.Contains("@"))
                            {
                                try
                                {
                                    item.Identity = item.Identity;
                                    item.CiContingenciaDet = CiContingenciaDet;
                                    item.ciNumeroIntento = item.ciNumeroIntento + 1;
                                    EnviarMail Mail = new EnviarMail();

                                    DataView Dv = new DataView(TblSmtp);
                                    Dv.RowFilter = " ciCompania ='" + item.CiCompania.ToString() + "'";
                                    ConfigCompañia = Dv.ToTable();

                                    if (ConfigCompañia != null)
                                    {
                                        String strNameImagenIncrustada = ConfigCompañia.Rows[0]["ciCompania"].ToString().Trim();
                                        string nuevoAsunto = string.Empty;

                                        plantillaCorreo = generarPlantilla.GenerarCorreoDocumentos(ConfigCompañia.Rows[0]["txRuc"].ToString().Trim(), ConfigCompañia.Rows[0]["urlPortal"].ToString().Trim(), ConfigCompañia.Rows[0]["txRazonSocial"].ToString().Trim(),
                                                                                                   ConfigCompañia.Rows[0]["MailAddressfrom"].ToString().Trim(), item.RazonSocialComprador);

                                        string estable = item.ClaveAcceso.Substring(24, 3);
                                        string puntoemi = item.ClaveAcceso.Substring(27, 3);
                                        string secuen = item.ClaveAcceso.Substring(30, 9);
                                        string numeroDocumento = estable + "-" + puntoemi + "-" + secuen;

                                        Mail.servidorSTMP(ConfigCompañia.Rows[0]["HostServidor"].ToString().Trim(), Convert.ToInt32(ConfigCompañia.Rows[0]["puerto"].ToString().Trim()), Convert.ToBoolean(ConfigCompañia.Rows[0]["EnableSsl"].ToString().Trim()), ConfigCompañia.Rows[0]["emailCredencial"].ToString().Trim(), ConfigCompañia.Rows[0]["passCredencial"].ToString().Trim());

                                        string imagePath = CatalogoViaDoc.rutaLogoCompania + ConfigCompañia.Rows[0]["txRuc"].ToString().Trim() + ".png";
                                        System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(plantillaCorreo, null, System.Net.Mime.MediaTypeNames.Text.Html);
                                        System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(imagePath);
                                        logo.ContentId = strNameImagenIncrustada;
                                        htmlView.LinkedResources.Add(logo);
                                        Mail.AlternateViews(htmlView);

                                        nuevoAsunto = string.Format(ConfigCompañia.Rows[0]["Asunto"].ToString().Trim(), TipoDocumento, numeroDocumento);

                                        if (txEmail.Trim().Equals(""))
                                        {
                                            Mail.llenarEmail(ConfigCompañia.Rows[0]["MailAddressfrom"].ToString().Trim(), item.Email.Trim(), "", "", nuevoAsunto, plantillaCorreo);
                                        }
                                        else
                                        {
                                            string emailEnvios = txEmail.Trim();
                                            Mail.llenarEmail(ConfigCompañia.Rows[0]["MailAddressfrom"].ToString().Trim(), emailEnvios.Trim().Replace(",,", ",")/*Quito la , en caso de que lleguen 2*/, "", "", nuevoAsunto, plantillaCorreo);
                                        }

                                        DataSet dsConfiguracionCompania = _metodosConsulta.ConsularCatalogoSistema(5, int.Parse(ConfigCompañia.Rows[0]["ciCompania"].ToString().Trim()), "", ref codigoRetorno, ref descripcionRetorno);

                                        string mensaje = "";
                                        byte[] riders = GenerarRideDocumentoElectronico.GenerarRiderComprobantesAutorizados(ref mensaje, item.XmlComprobante, item.txFechaHoraAutorizacion,
                                            item.TxNumeroAutorizacion, item.CiTipoDocumento, item.XmlEstado, dsConfiguracionCompania, dsCatalogo);


                                        if (riders != null)
                                        {

                                            System.IO.MemoryStream ms = new System.IO.MemoryStream(riders);   //<-- viene null
                                            Mail.adjuntar_Documento(ms, item.ClaveAcceso.Trim() + ".pdf");

                                            Byte[] xml = System.Text.Encoding.ASCII.GetBytes(item.XmlComprobante);
                                            System.IO.MemoryStream msXml = new System.IO.MemoryStream(xml);
                                            Mail.adjuntar_Documento(msXml, item.ClaveAcceso.Trim() + ".xml");

                                            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate,
                                            X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                            {
                                                return true;
                                            };

                                            if (Mail.enviarEmail())
                                            {
                                                item.XmlEstado = CatalogoViaDoc.DocEstadoEnviado;
                                                item.MensajeError = "no hubo error";
                                                ;
                                            }
                                            else
                                            {
                                                item.MensajeError = "Se ha presentado un error al enviar el comprobante al correo: " + item.Email + " por favor revise las configuraciones de servidor de correos o verifique que el mail del cliente final sea valido";
                                                item.XmlEstado = CatalogoViaDoc.DocEstadoErrorEnEnvio;
                                                item.txCodError = "103";
                                            }
                                        }
                                        else
                                        {
                                            item.MensajeError = "Se ha presentado un error al enviar el comprobante al correo: " + item.Email + " por favor revise las configuraciones de servidor de correos o verifique que el mail del cliente final sea valido";
                                            item.XmlEstado = CatalogoViaDoc.DocEstadoErrorEnEnvio;
                                            item.txCodError = "103";
                                        }
                                    }
                                    else
                                    {
                                        item.MensajeError = "La empresa: " + item.CiCompania + " no tiene configuracion SMTP ";
                                        item.XmlEstado = CatalogoViaDoc.DocEstadoErrorEnEnvio;
                                        item.txCodError = "103";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    item.MensajeError = "Hubo un error al envio del correo electronico.. " + ex.Message;
                                    item.XmlEstado = CatalogoViaDoc.DocEstadoNoEnviado;
                                    item.txCodError = "999";
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion__1: " + ex.Message);
                                }
                            }
                            else
                            {
                                item.MensajeError = "Cliente no dispone de un correo electronico valido";
                                item.XmlEstado = CatalogoViaDoc.DocEstadoNoEnviado;
                                item.txCodError = "104";
                                item.ciNumeroIntento = item.ciNumeroIntento + 1;
                            }

                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("El tipo de error del mensaje es: " + item.MensajeError);
                            //metodos.ActualizarXmlComprobantes(item);
                        }
                        catch (Exception ex)
                        {
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion___2: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion__3: " + ex.Message);
            }
        }


        public void EnviarMailNotificacionEstadistica(List<Smtp> listaSmtp)
        {
            try
            {
                Compania companias = new Compania();
                EstadisticasLista listEstadisticas = new EstadisticasLista();
                ProcesoDocumento objDocumentos = new ProcesoDocumento();
                ProcesoNotificacion proceso = new ProcesoNotificacion();
                PDocumentos generarPlantilla = new PDocumentos();

                string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").Trim();
                int codigoRetorno = 0;
                int diaAtras = ConfigurationManager.AppSettings.Get("DiasAtras").Trim() == "" ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings.Get("DiasAtras").Trim());
                string descripcionRetorno = string.Empty;
                string PlantillaNotificacion = string.Empty;
                dynamic objetoEstadicticaComp = null;
                string backgraundTable = string.Empty;
                string razonSocial = string.Empty;
                string fechaActual = string.Empty;

                String[] ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraNotificacion.txt");
                Int32 intHoraSystema = Convert.ToInt32(DateTime.Now.ToString("H:mm").Trim().Replace(":", ""));

                DataSet dataComp = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref descripcionRetorno);
                DataTable tableComp = dataComp.Tables[0];

                DataView dtV = tableComp.DefaultView;
                dtV.Sort = "txRazonSocial ASC";
                tableComp = dtV.ToTable();



                foreach (var lista in listaSmtp)
                {
                    if (bool.Parse(lista.ActivarNotificacion))
                    {
                        proceso.ConsultarNotificacion(1, "E", "", false, int.Parse(lista.CiCompania),
                                                                             ref codigoRetorno, ref descripcionRetorno);
                        if (codigoRetorno.Equals(0))
                        {
                            foreach (String strHoraEntre in ArrayStrHorasEjecucion)
                            {
                                String[] arrStrHoraEntre = strHoraEntre.Trim().Split('-');
                                Int32 intHoraInicio = Convert.ToInt32(arrStrHoraEntre[0].Trim().Replace(":", ""));
                                Int32 intHoraFin = Convert.ToInt32(arrStrHoraEntre[1].Trim().Replace(":", ""));

                                if (intHoraSystema >= intHoraInicio && intHoraSystema <= intHoraFin)
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Ingreso a EnviarMailNotificacionEstadistica");

                                    fechaActual = diaAtras == 0 ? DateTime.Now.ToString("dd/MM/yyyy") : DateTime.Now.AddDays(-diaAtras).ToString("dd/MM/yyyy");

                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("fechaActual" + fechaActual);

                                    listEstadisticas.objListEstadisticas = objDocumentos.ConsultarOpcionEstadisticasNotificacion(fechaActual, fechaActual, ref codigoRetorno, ref descripcionRetorno);


                                    if (codigoRetorno.Equals(0))
                                    {
                                        if (listEstadisticas.objListEstadisticas.Count > 0)
                                        {
                                            String htmlEstadisticas = string.Empty;
                                            EnviarMail objMail = new EnviarMail();

                                            #region Crea HTML Con el Informe Diario
                                            htmlEstadisticas += "<table align='center' style='width:100%;border-color:#0461ab'>";
                                            #region Crea la cabecera de la tabla HTML
                                            htmlEstadisticas += "<thead><tr>";
                                            htmlEstadisticas += "<th style='width:20%;background-color:#0461ab;color:#ffffff'>Documento</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Sin Procesa</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Firmados</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Errores de Firmas</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Errores de Recepción</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>En Proceso</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Errores de Autorización</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Autorizados</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>No Enviado al Cliente</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>No Enviado al Portal</th>";
                                            htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Total</th>";
                                            htmlEstadisticas += "</tr></thead>";
                                            htmlEstadisticas += "<tbody>";
                                            int count = 0;
                                            foreach (var estadistica in listEstadisticas.objListEstadisticas)
                                            {
                                                if (count % 2 == 0)
                                                    backgraundTable = "background-color:#f1f6fb";
                                                else
                                                    backgraundTable = "background-color:#B1F5AE";

                                                htmlEstadisticas += "<tr>";
                                                htmlEstadisticas += "<td align='center' style='width:20%;" + backgraundTable + "'>" + estadistica.documento + "</td>";
                                                if (estadistica.sinProcesar == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.sinProcesar + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.sinProcesar + "</td>";
                                                }
                                                if (estadistica.firmados == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.firmados + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.firmados + "</td>";
                                                }
                                                if (estadistica.errorFirma == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.errorFirma + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.errorFirma + "</td>";
                                                }
                                                if (estadistica.errorRecepcion == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.errorRecepcion + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.errorRecepcion + "</td>";
                                                }
                                                if (estadistica.enProceso == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.enProceso + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.enProceso + "</td>";
                                                }
                                                if (estadistica.errorAutorizacion == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.errorAutorizacion + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.errorAutorizacion + "</td>";
                                                }
                                                if (estadistica.autorizado == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.autorizado + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.autorizado + "</td>";
                                                }
                                                if (estadistica.noEnviadoCliente == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.noEnviadoCliente + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.noEnviadoCliente + "</td>";
                                                }
                                                if (estadistica.noEnviadoPortal == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.noEnviadoPortal + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.noEnviadoPortal + "</td>";
                                                }
                                                if (estadistica.total == "0")
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.total + "</td>";
                                                }
                                                else
                                                {
                                                    htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.total + "</td>";
                                                }
                                                htmlEstadisticas += "</tr>";

                                                count++;
                                            }

                                            htmlEstadisticas += "</tbody>";
                                            #endregion
                                            htmlEstadisticas += "</table><br>";
                                            #endregion Crea HTML Con el Informe Diario

                                            if (tableComp.Rows.Count > 1)
                                            {
                                                foreach (DataRow row in tableComp.Rows)
                                                {
                                                    objetoEstadicticaComp = objDocumentos.ConsultarOpcionEstadisticas(row["ciCompania"].ToString().Trim(), fechaActual, fechaActual, ref codigoRetorno, ref descripcionRetorno);

                                                    htmlEstadisticas += "<h2 style='color:#006289'>" + row["txRazonSocial"].ToString().Trim() + "</h2>";
                                                    htmlEstadisticas += "</br>";

                                                    #region Crea HTML Con el Informe Diario
                                                    htmlEstadisticas += "<table align='center' style='width:100%;border-color:#0461ab'>";
                                                    #region Crea la cabecera de la tabla HTML
                                                    htmlEstadisticas += "<thead><tr>";
                                                    htmlEstadisticas += "<th style='width:20%;background-color:#0461ab;color:#ffffff'>Documento</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Sin Procesa</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Firmados</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Errores de Firmas</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Errores de Recepción</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>En Proceso</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Errores de Autorización</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Autorizados</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>No Enviado al Cliente</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>No Enviado al Portal</th>";
                                                    htmlEstadisticas += "<th style='width:8%;background-color:#0461ab;color:#ffffff'>Total</th>";
                                                    htmlEstadisticas += "</tr></thead>";
                                                    htmlEstadisticas += "<tbody>";
                                                    count = 0;
                                                    foreach (var estadistica in objetoEstadicticaComp)
                                                    {
                                                        if (count % 2 == 0)
                                                            backgraundTable = "background-color:#f1f6fb";
                                                        else
                                                            backgraundTable = "background-color:#B1F5AE";

                                                        htmlEstadisticas += "<tr>";
                                                        htmlEstadisticas += "<td align='center' style='width:20%;" + backgraundTable + "'>" + estadistica.documento + "</td>";
                                                        if (estadistica.sinProcesar == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.sinProcesar + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.sinProcesar + "</td>";
                                                        }
                                                        if (estadistica.firmados == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.firmados + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.firmados + "</td>";
                                                        }
                                                        if (estadistica.errorFirma == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.errorFirma + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.errorFirma + "</td>";
                                                        }
                                                        if (estadistica.errorRecepcion == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.errorRecepcion + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.errorRecepcion + "</td>";
                                                        }
                                                        if (estadistica.enProceso == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.enProceso + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.enProceso + "</td>";
                                                        }
                                                        if (estadistica.errorAutorizacion == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.errorAutorizacion + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.errorAutorizacion + "</td>";
                                                        }
                                                        if (estadistica.autorizado == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.autorizado + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.autorizado + "</td>";
                                                        }
                                                        if (estadistica.noEnviadoCliente == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.noEnviadoCliente + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.noEnviadoCliente + "</td>";
                                                        }
                                                        if (estadistica.noEnviadoPortal == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.noEnviadoPortal + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.noEnviadoPortal + "</td>";
                                                        }
                                                        if (estadistica.total == "0")
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + ";color:#C8C8CB'>" + estadistica.total + "</td>";
                                                        }
                                                        else
                                                        {
                                                            htmlEstadisticas += "<td align='center' style='width:8%;" + backgraundTable + "'>" + estadistica.total + "</td>";
                                                        }
                                                        htmlEstadisticas += "</tr>";

                                                        count++;
                                                    }

                                                    htmlEstadisticas += "</tbody>";
                                                    #endregion
                                                    htmlEstadisticas += "</table><br>";
                                                    #endregion Crea HTML Con el Informe Diario
                                                }
                                            }

                                            htmlEstadisticas += "<br><br><p>Saludos Cordiales. </p><br><br>";

                                            razonSocial = tableComp.Rows.Count > 1 ? CatalogoViaDoc.RazonSocial : lista.RazonSocial;

                                            PlantillaNotificacion = generarPlantilla.GenerarCorreoEstadisticaDiaria(lista.RucCompania, razonSocial, fechaActual, htmlEstadisticas);

                                            //String nuevoAsunto = "ViaDoc - Reporte del dia " + fechaActual + " de Documentos Procesados de la Empresa: " + razonSocial;
                                            String nuevoAsunto = "Reporte de la Empresa: " + razonSocial + "del día " + fechaActual;

                                            objMail.servidorSTMP(lista.HostServidor, int.Parse(lista.Puerto), bool.Parse(lista.EnableSsl), lista.EmailCredencial, lista.PassCredencial);

                                            string imagePath = CatalogoViaDoc.rutaLogoCompania + lista.RucCompania + ".png";
                                            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(PlantillaNotificacion, null, System.Net.Mime.MediaTypeNames.Text.Html);
                                            System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(imagePath);
                                            logo.ContentId = lista.RucCompania;
                                            htmlView.LinkedResources.Add(logo);
                                            Mail.AlternateViews(htmlView);
                                            objMail.llenarEmail(lista.MailAddressfrom, lista.Para, "", lista.Cc, nuevoAsunto, PlantillaNotificacion);

                                            ServicePointManager.ServerCertificateValidationCallback = delegate (object obj, X509Certificate certificate,
                                                          X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                            {
                                                return true;
                                            };

                                            if (objMail.enviarEmail())
                                            {
                                                proceso.ConsultarNotificacion(2, "E", "Notificacion Estadistica", false, int.Parse(lista.CiCompania),
                                                                              ref codigoRetorno, ref descripcionRetorno);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("EnviarMailNotificacionEstadistica Error: " + ex.Message);
            }
        }

        public void EnviarMainNotificacionCertificado(DataTable DtSmtp)
        {
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            PDocumentos generarPlantilla = new PDocumentos();
            ProcesoNotificacion proceso = new ProcesoNotificacion();
            Mappeo mp = new Mappeo();
            var fecha = DateTime.Now.ToString("dd/MM/yyyy");
            DataTable dtCertificado = proceso.ConsultarNotificacion(1, "", "", true, 0, ref codigoRetorno, ref descripcionRetorno).Tables[0]; ;

            try
            {
                foreach (DataRow item in dtCertificado.Rows)
                {
                    List<MdSmtp> compSmtp = mp.MpSmtp(DtSmtp);

                    dynamic lista = compSmtp.Where(x => x.ciCompania == Convert.ToInt32(item["ciCompania"].ToString().Trim())).Distinct().ToList();

                    if (lista.Count > 0)
                    {
                        var FechaHasta = Convert.ToDateTime(item["fcHasta"].ToString()).ToString("dd/MM/yyyy");
                        var FechaXD = Convert.ToDateTime(FechaHasta).AddDays(-20).ToString("dd/MM/yyyy");

                        if (fecha.Equals(FechaXD) && (Convert.ToInt32(item["ciNumeroCerti"].ToString().Trim()).Equals(0) || item["ciNumeroCerti"].ToString().Trim().Equals("")))
                        {

                            if (codigoRetorno.Equals(0))
                            {
                                EnviarMail objMail = new EnviarMail();
                                int NuermoCerti = Convert.ToInt32(item["ciNumeroCerti"].ToString().Trim()) + 1;

                                String verificarEstado = "<p>Estimado Usuario le notificamos que el certificado para la empresa "
                                   + item["txRazonSocial"].ToString().Trim() + " va a caducar en 20 DIAS, una vez caducado no se procesarán los documentos, por favor ingrese un nuevo certificado.</p>";

                                String detalleCertificadoDesde = "<p>Fecha Ingreso del Certificado: " + item["fcDesde"].ToString() + "</p>";
                                String detalleCertificadoHasta = "<p>Fecha Caducida del Certificado: " + item["fcHasta"].ToString() + "</p>";

                                String PlantillaNotificacion = generarPlantilla.GenerarCorreoCertificadoDigital(item["txRuc"].ToString().Trim(), item["txRazonSocial"].ToString().Trim(), detalleCertificadoDesde,
                                    detalleCertificadoHasta, verificarEstado);
                                String nuevoAsunto = "ViaDoc - Notificación de Certificados Digital de la Empresa: " + item["txRazonSocial"].ToString().Trim();

                                objMail.servidorSTMP(lista[0].HostServidor.ToString().Trim(), int.Parse(lista[0].puerto.ToString().Trim()), bool.Parse(lista[0].EnableSsl.ToString().Trim()), lista[0].emailCredencial.ToString().Trim(), lista[0].passCredencial.ToString().Trim());

                                string imagePath = CatalogoViaDoc.rutaLogoCompania + item["txRuc"].ToString().Trim() + ".png";
                                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(PlantillaNotificacion, null, System.Net.Mime.MediaTypeNames.Text.Html);
                                System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(imagePath);
                                logo.ContentId = item["txRuc"].ToString().Trim(); ;
                                htmlView.LinkedResources.Add(logo);
                                Mail.AlternateViews(htmlView);

                                objMail.llenarEmail(lista[0].MailAddressfrom.ToString().Trim(), lista[0].para.ToString().Trim(), "", lista[0].cc.ToString().Trim(), nuevoAsunto, PlantillaNotificacion);

                                ServicePointManager.ServerCertificateValidationCallback = delegate (object obj, X509Certificate certificate,
                                              X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                {
                                    return true;
                                };


                                if (objMail.enviarEmail())
                                {
                                    proceso.ConsultarNotificacion(2, "", Convert.ToString(NuermoCerti), true, int.Parse(item["CiCompania"].ToString().Trim()), ref codigoRetorno, ref descripcionRetorno);
                                }
                            }
                        }
                        if (fecha.Equals(FechaHasta) && Convert.ToInt32(item["ciNumeroCerti"].ToString().Trim()).Equals(1))
                        {

                            if (codigoRetorno.Equals(0))
                            {
                                EnviarMail objMail = new EnviarMail();
                                int NuermoCerti = Convert.ToInt32(item["ciNumeroCerti"].ToString().Trim()) + 1;

                                String verificarEstado = "<p>Estimado Usuario le notificamos que el certificado para la empresa "
                                   + item["txRazonSocial"].ToString().Trim() + " ha caducado, una vez caducado no se procesarán los documentos, por favor ingrese un nuevo certificado.</p>";

                                String detalleCertificadoDesde = "<p>Fecha Ingreso del Certificado: " + item["fcDesde"].ToString() + "</p>";
                                String detalleCertificadoHasta = "<p>Fecha Caducida del Certificado: " + item["fcHasta"].ToString() + "</p>";

                                String PlantillaNotificacion = generarPlantilla.GenerarCorreoCertificadoDigital(item["txRuc"].ToString().Trim(), item["txRazonSocial"].ToString().Trim(), detalleCertificadoDesde,
                                    detalleCertificadoHasta, verificarEstado);
                                String nuevoAsunto = "ViaDoc - Notificación de Certificados Digital de la Empresa: " + item["txRazonSocial"].ToString().Trim();

                                objMail.servidorSTMP(lista[0].HostServidor.ToString().Trim(), int.Parse(lista[0].puerto.ToString().Trim()), bool.Parse(lista[0].EnableSsl.ToString().Trim()), lista[0].emailCredencial.ToString().Trim(), lista[0].passCredencial.ToString().Trim());

                                string imagePath = CatalogoViaDoc.rutaLogoCompania + item["txRuc"].ToString().Trim() + ".png";
                                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(PlantillaNotificacion, null, System.Net.Mime.MediaTypeNames.Text.Html);
                                System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(imagePath);
                                logo.ContentId = item["txRuc"].ToString().Trim(); ;
                                htmlView.LinkedResources.Add(logo);
                                Mail.AlternateViews(htmlView);

                                objMail.llenarEmail(lista[0].MailAddressfrom.ToString().Trim(), lista[0].para.ToString().Trim(), "", lista[0].cc.ToString().Trim(), nuevoAsunto, PlantillaNotificacion);

                                ServicePointManager.ServerCertificateValidationCallback = delegate (object obj, X509Certificate certificate,
                                              X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                {
                                    return true;
                                };


                                if (objMail.enviarEmail())
                                {
                                    proceso.ConsultarNotificacion(3, "", Convert.ToString(NuermoCerti), true, int.Parse(item["CiCompania"].ToString().Trim()), ref codigoRetorno, ref descripcionRetorno);
                                }
                            }
                        }
                        if (Convert.ToDateTime(fecha) > Convert.ToDateTime(FechaHasta))
                        {
                            proceso.ConsultarNotificacion(3, "", "2", true, int.Parse(item["CiCompania"].ToString().Trim()), ref codigoRetorno, ref descripcionRetorno);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("NotificacionCertificado: " + ex.Message);
            }
        }

        public void NotificacinDocError(DataTable DtSmtp, DataTable dtDocConsult)
        {
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            PDocumentos generarPlantilla = new PDocumentos();
            ProcesoCorreoAD proceso = new ProcesoCorreoAD();
            DateTime fecha = DateTime.Today;
            try
            {
                foreach (DataRow doc in dtDocConsult.Rows)
                {
                    foreach (DataRow lista in DtSmtp.Rows)
                    {
                        if (Convert.ToInt32(doc["codCompania"].ToString()).Equals(Convert.ToInt32(lista["ciCompania"])))
                        {
                            if (lista["activarNotificacion"].Equals(true))
                            {
                                EnviarMail objMail = new EnviarMail();
                                int NuermoCerti = Convert.ToInt32(doc["NumeroIntento"].ToString().Trim()) + 1;

                                String verificarEstado = "<h3 style='text-align:center;'>NOTIFICACIÓN DE NOVEDAD:</h3>";
                                verificarEstado += "<p>Tipo de Documento:  " + doc["documento"].ToString().Trim() + "<p>";
                                verificarEstado += "<p>Número de Documento:  " + doc["docnum"].ToString().Trim() + "<p>";
                                verificarEstado += "<p>Fecha de Emision:  " + doc["fechaEmision"].ToString().Trim() + "<p>";
                                verificarEstado += "<p>Estado del Documento:  " + doc["estado"].ToString().Trim() + "<p>";
                                verificarEstado += "<p>Mensaje de Error: <p>";
                                verificarEstado += "<h4>" + doc["MensajeError"].ToString().Trim() + "<h4>";

                                String PlantillaNotificacion = generarPlantilla.GenerarCorreoNotificionError(lista["txRuc"].ToString().Trim(), lista["txRazonSocial"].ToString().Trim(), verificarEstado);

                                String nuevoAsunto = "ViaDoc - Notificación de Documento Digital de la Empresa: " + lista["txRazonSocial"].ToString().Trim();

                                objMail.servidorSTMP(lista["HostServidor"].ToString().Trim(), int.Parse(lista["puerto"].ToString().Trim()), bool.Parse(lista["EnableSsl"].ToString().Trim()), lista["emailCredencial"].ToString().Trim(), lista["passCredencial"].ToString().Trim());

                                string imagePath = CatalogoViaDoc.rutaLogoCompania + lista["txRuc"].ToString().Trim() + ".png";
                                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(PlantillaNotificacion, null, System.Net.Mime.MediaTypeNames.Text.Html);
                                System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(imagePath);
                                logo.ContentId = lista["txRuc"].ToString().Trim(); ;
                                htmlView.LinkedResources.Add(logo);
                                Mail.AlternateViews(htmlView);

                                objMail.llenarEmail(lista["MailAddressfrom"].ToString().Trim(), lista["para"].ToString().Trim(), "", lista["cc"].ToString().Trim(), nuevoAsunto, PlantillaNotificacion);

                                ServicePointManager.ServerCertificateValidationCallback = delegate (object obj, X509Certificate certificate,
                                              X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                {
                                    return true;
                                };

                                if (objMail.enviarEmail())
                                {
                                    proceso.ConsultaCorreosEnviar("UPDATEERROR", 0, doc["codDocumento"].ToString().Trim(), 0, doc["claveAcceso"].ToString().Trim(), ref codigoRetorno, ref descripcionRetorno);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("NotificacionCertificado: " + ex.Message);
            }
        }

        public void EnvioMailProbadorWeb(Smtp smtp, ref int codigoRetorno, ref string descripcionRetorno)
        {
            EnviarMail objMail = new EnviarMail();
            String PlantillaNotificacion = String.Empty;

            try
            {
                PlantillaNotificacion += "<p>Pruebas de SMTP exitosa de la empresa: " + smtp.RazonSocial + "</p><br><br><br><br>";
                PlantillaNotificacion += "<p>Saludos Cordiales</p><br>";
                String nuevoAsunto = "ViaDoc - Pruebas de envio de Correo de la Empresa: " + smtp.RazonSocial;
                objMail.servidorSTMP(smtp.HostServidor, int.Parse(smtp.Puerto), bool.Parse(smtp.EnableSsl), smtp.EmailCredencial, smtp.PassCredencial);
                objMail.llenarEmail(smtp.MailAddressfrom, smtp.Para, "", "", nuevoAsunto, PlantillaNotificacion);

                ServicePointManager.ServerCertificateValidationCallback = delegate (object obj, X509Certificate certificate,
                              X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                if (objMail.enviarEmail(ref descripcionRetorno))
                {
                    codigoRetorno = 0;
                    descripcionRetorno = "Correo Enviado exitosamente...";
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "Correo no enviado: " + descripcionRetorno;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
        }

        public void Reenvio_a_Portal(List<XmlGenerados> dtDocConsult, string proceso)
        {
            EnviarMail objMail = new EnviarMail();
            ProcesoEnvioPortal _procesoEnvioPortal = new ProcesoEnvioPortal();
            try
            {

                _procesoEnvioPortal.EnviarComprobantesAutorizadosEnvioPortal(dtDocConsult, proceso);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Timer_Reenvio_a_Portal: " + ex.Message);
            }
        }
    }
}
