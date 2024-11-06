using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using ViaDoc.AccesoDatos;
using ViaDoc.AccesoDatos.compania;
using ViaDoc.AccesoDatos.winServCorreos;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;

namespace ViaDocEnvioCorreo.Negocios
{
    public class MetodoProcesoCorreo
    {
        CompaniaAD _metodosConsulta = new CompaniaAD();
        string cons_final = ConfigurationManager.AppSettings.Get("NOTIFICACION.CONSUMIDOR_FINAL").Trim();
        string razon_consu_final = ConfigurationManager.AppSettings.Get("NOTIFICACION.RAZON_SOCIAL_CONS_FINAL").Trim();
        bool bandera_consumidor_final = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("NOTIFICACION.VALIDA_CONSUMIDOR_FINAL").Trim());
        bool bandera;
        bool consumidorFinal;

        

        public void EnviarComprobantesAutorizadosEnvioPortal(string proceso, string tipoDocumento, int cantidad)
        {
            List<XmlGenerados> ListaComprobante = new List<XmlGenerados>();
            ProcesoEnvioPortalAD _metodosPortal = new ProcesoEnvioPortalAD();
            DataSet dsConsultaCompania = new DataSet();
            DataTable dtConsultaDocumentosPortal = new DataTable();
            DateTime fecha = DateTime.Now;
            string descripcionRetorno = string.Empty;
            int codigoRetorno = 0;

            CompaniaAD companias = new CompaniaAD();
            dsConsultaCompania = companias.ConsultaCompanias_y_Certificados(1, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                if (dsConsultaCompania.Tables.Count > 0)
                {
                    DataTable dtConsultaCompania = dsConsultaCompania.Tables[0];

                    foreach (DataRow listaCompania in dtConsultaCompania.Rows)
                    {
                        dtConsultaDocumentosPortal = _metodosPortal.ConsultaDocumentosEnviarPortal(proceso,
                                                                                                   int.Parse(listaCompania["ciCompania"].ToString()),
                                                                                                   tipoDocumento,
                                                                                                   cantidad, "",
                                                                                                   ref codigoRetorno,
                                                                                                   ref descripcionRetorno);

                        if (codigoRetorno.Equals(0))
                        {
                            int contInicial = 0;

                            contInicial = dtConsultaDocumentosPortal.Select("estado='" + ConfigurationManager.AppSettings.Get("NOTIFICACION.ENVIADO").Trim() + "'").ToList().Count;
                            contInicial += dtConsultaDocumentosPortal.Select("estado='" + ConfigurationManager.AppSettings.Get("NOTIFICACION.NO_ENVIADO").Trim() + "'").ToList().Count;


                            foreach (DataRow listaDocumentos in dtConsultaDocumentosPortal.Rows)
                            {
                                XmlGenerados xmlGenerado = new XmlGenerados();
                                xmlGenerado.Identity = Convert.ToInt32(listaDocumentos["codIdentity"].ToString());
                                xmlGenerado.CiCompania = Convert.ToInt32(listaDocumentos["codCompania"].ToString());
                                xmlGenerado.CiTipoEmision = listaDocumentos["codTipoEmision"].ToString();
                                xmlGenerado.CiContingenciaDet = Convert.ToInt32(listaDocumentos["contigencia"].ToString());
                                xmlGenerado.CiTipoDocumento = listaDocumentos["codDocumento"].ToString();
                                xmlGenerado.ClaveAcceso = listaDocumentos["claveAcceso"].ToString();
                                xmlGenerado.XmlComprobante = listaDocumentos["xmlDocumentoAutorizado"].ToString();
                                xmlGenerado.TxNumeroAutorizacion = listaDocumentos["claveAcceso"].ToString().Trim();
                                xmlGenerado.txFechaHoraAutorizacion = listaDocumentos["fechaHoraAutorizacion"].ToString().Trim();
                                xmlGenerado.XmlEstado = listaDocumentos["estado"].ToString().Trim();
                               
                                ActualizarXmlComprobantes(xmlGenerado);
                                ListaComprobante.Add(xmlGenerado);
                                dtConsultaDocumentosPortal.Dispose();
                            }
                        }
                    }
                }
            }
        }


        public List<Smtp> ListarConfiguracionesSmtp(DataTable tblSmtp)
        {
            List<Smtp> configuracionesSmtp = new List<Smtp>();
            try
            {
                if (tblSmtp.Rows.Count > 0)
                {
                    #region Crea una lista de Objetos EntySmtp
                    var smtp = from info in tblSmtp.AsEnumerable()
                               let compania = info.Field<object>("ciCompania")
                               let host = info.Field<object>("HostServidor")
                               let puerto = info.Field<object>("puerto")
                               let ssl = info.Field<object>("EnableSsl")
                               let emailCredencial = info.Field<object>("emailCredencial")
                               let passCredencial = info.Field<object>("passCredencial")
                               let remitente = info.Field<object>("MailAddressfrom")
                               let destinatario = info.Field<object>("para")
                               let copia = info.Field<object>("cc")
                               let asunto = info.Field<object>("Asunto")
                               let urlPortal = info.Field<object>("urlPortal")
                               let razonSocial = info.Field<object>("txRazonSocial")
                               let rucCompania = info.Field<object>("txRuc")
                               let activarNotificacion = info.Field<object>("activarNotificacion")
                               select new Smtp 
                               {
                                   CiCompania = compania.ToString(),
                                   HostServidor = host.ToString().Trim(),
                                   Puerto = puerto.ToString(),
                                   EnableSsl = ssl.ToString().Trim(),
                                   EmailCredencial = emailCredencial.ToString().Trim(),
                                   PassCredencial = passCredencial.ToString().Trim(),
                                   MailAddressfrom = remitente.ToString().Trim(),
                                   Para = destinatario.ToString().Trim(),
                                   Cc = copia.ToString().Trim(),
                                   Asunto = asunto.ToString().Trim(),
                                   UrlPortal = urlPortal.ToString().Trim(),
                                   RazonSocial = razonSocial.ToString().Trim(),
                                   RucCompania = rucCompania.ToString().Trim(),
                                   ActivarNotificacion =  activarNotificacion.ToString()
                               };
                    #endregion
                    configuracionesSmtp = smtp.ToList();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
            return configuracionesSmtp;
        }


        public void ActualizarXmlComprobantes(XmlGenerados xmlComprobante)
        {
            int i = 0;
            try
            {
                if (xmlComprobante.ClaveAcceso.Length != 0)
                {
                    DocumentoAD actualiza = new DocumentoAD();
                    System.Data.DataSet ds = new System.Data.DataSet();
                    string resultXml = Serializacion.serializar(xmlComprobante);
                    byte[] byteArray = Encoding.UTF8.GetBytes(resultXml);
                    System.IO.Stream str = new System.IO.MemoryStream(byteArray);
                    ds.ReadXml(str);
                    i = actualiza.ActualizarComprobantes(ds);
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("catch del paso 4 ActualizaEstado: "+ ex.Message);
            }
            if (i == 0)
            {
                
            }
        }
    }
}
