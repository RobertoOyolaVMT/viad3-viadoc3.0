using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;

namespace ViaDocEnvioCorreo.Negocios.procesos
{
    public class ProcesoEnvioPortal
    {
        public void EnviarComprobantesAutorizadosEnvioPortal(List<XmlGenerados> xmlComprobante, string proceso)
        {
            MetodoProcesoCorreo metodos = new MetodoProcesoCorreo();
            foreach (XmlGenerados item in xmlComprobante)
            {
                item.CiContingenciaDet = proceso == "FIRMA" ? 2 : 3;
                try
                {
                    if (!item.IdentificacionComprador.Equals(ConfigurationManager.AppSettings.Get("NOTIFICACION.CONSUMIDOR_FINAL")) 
                        && !item.RazonSocialComprador.Equals(ConfigurationManager.AppSettings.Get("NOTIFICACION.RAZON_SOCIAL_CONS_FINAL")))
                    {
                        String respuestaServicio = String.Empty;
                        using (WsPortalFacturacionElectronica.wsdocumento objWsPortal = new WsPortalFacturacionElectronica.wsdocumento())
                        {
                            try
                            {
                                objWsPortal.Credentials = System.Net.CredentialCache.DefaultCredentials;
                                objWsPortal.Url = CatalogoViaDoc.UrlPortal;
                                objWsPortal.Timeout = Convert.ToInt32(CatalogoViaDoc.TiempoRequest);


                                respuestaServicio = objWsPortal.insertar
                               (
                                   item.rucCompania,
                                   item.CiTipoDocumento,
                                   item.numeroDocumento,
                                   item.IdentificacionComprador,
                                   item.CiTipoEmision,
                                   item.ClaveAcceso,
                                   item.XmlComprobante,
                                   item.fechaEmision,
                                   item.txFechaHoraAutorizacion,
                                   item.CiContingenciaDet.ToString()
                               );
                                objWsPortal.Dispose();
                            }
                            catch (Exception ex)
                            {
                                respuestaServicio = "ErrorEnvio: " + ex.Message;
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Servicio Portal Extranet" + ex.ToString());
                            }
                        }

                        if (String.Compare(item.XmlEstado, CatalogoViaDoc.DocEstadoEnviado.Trim()) == 0
                            && String.Compare(respuestaServicio.Trim(), "OK") == 0 && proceso == "AUTORIZACION")
                        {
                            item.XmlEstado = CatalogoViaDoc.DocEstadoEnviadoClientePortal.Trim();
                            item.ciEstadoEnvioPortal = CatalogoViaDoc.DocEnviadoPortal.Trim();
                        }
                        else
                        {
                            //si devuelve OK se actualiza el estado a enviado
                            if (String.Compare(respuestaServicio.Trim(), "OK") == 0)
                            {
                                item.XmlEstado = CatalogoViaDoc.DocComprobanteEnviadoPortal.Trim();
                                item.ciEstadoEnvioPortal = CatalogoViaDoc.DocEnviadoPortal.Trim();
                            }
                            else
                            {
                                if (String.Compare(item.XmlEstado, CatalogoViaDoc.DocEstadoEnviado.Trim()) == 0)
                                {
                                    item.MensajeError = "RESPUESTA DEL PORTAL: " + respuestaServicio;
                                    item.XmlEstado = CatalogoViaDoc.DocComprobanteEnviadoPortal.Trim();
                                    item.txCodError = "105";
                                }
                                else
                                {
                                    item.MensajeError = "RESPUESTA DEL PORTAL: " + respuestaServicio;
                                    item.XmlEstado = CatalogoViaDoc.DocComprobanteNoEnviadoPortalEmail.Trim();
                                    item.txCodError = "106";
                                }
                            }
                        }
                    }
                    else
                    {
                        item.XmlEstado = CatalogoViaDoc.DocEstadoEnviadoClientePortal.Trim();
                        item.ciEstadoEnvioPortal = CatalogoViaDoc.DocEnviadoPortal.Trim();
                    }
                }
                catch (Exception ex)
                {
                    if ((ex.Message.Contains("tiempo") == false))
                    {
                        if ((ex.Message.Contains("timed") == false))
                        {
                            item.MensajeError = ex.Message;
                            if (String.Compare(item.XmlEstado, CatalogoViaDoc.DocEstadoEnviado.Trim()) == 0)
                            {
                                item.XmlEstado = CatalogoViaDoc.DocComprobanteEnviadoPortal.Trim();
                                item.txCodError = "105";
                            }
                            else
                            {
                                item.XmlEstado = CatalogoViaDoc.DocComprobanteNoEnviadoPortalEmail.Trim();
                                item.txCodError = "106";
                            }
                        }
                    }
                }
                metodos.ActualizarXmlComprobantes(item);
            }
        }
    }
}
