using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using ViaDoc.AccesoDatos.portalWeb;
using ViaDoc.EntidadNegocios;
using ViaDoc.EntidadNegocios.portalWeb;

namespace ViaDocAutorizacion.LogicaNegocios.procesos
{
    public class ProcesoDocumentos
    {
        public List<XmlGenerados> ConsultaComprobantesPorEstado(int ciCompania, string ciTipoDocumento, string ciEstado,
                                                                string claveAcceso, ref int codigoRetorno, ref string descripcionRetorno)
        {

            List<XmlGenerados> ListaComprobante = new List<XmlGenerados>();
            ViaDoc.AccesoDatos.DocumentoAD _documentosConsulta = new ViaDoc.AccesoDatos.DocumentoAD();
            DataTable dtComprobantesConsuta = new DataTable();
           
            DataSet dsParametrizacion = _documentosConsulta.ObtenerTipoDocumentos("3", ciTipoDocumento, "", ciCompania, ref codigoRetorno, ref descripcionRetorno);
            int numRegistros = int.Parse(dsParametrizacion.Tables[0].Rows[0]["cantidadAutorizacion"].ToString());
            try
            {
                dtComprobantesConsuta = _documentosConsulta.ConsultaComprobantePorEstado(ciCompania, ciEstado, numRegistros, ciTipoDocumento,
                                                                                         claveAcceso, ref codigoRetorno, ref descripcionRetorno);
                
                if (codigoRetorno.Equals(0))
                {
                    string estado = "";
                    foreach (DataRow itemComprobantes in dtComprobantesConsuta.Rows)
                    {
                        if (itemComprobantes["ciEstadoRecepcionAutorizacion"].ToString().Trim().Equals("ERE") || itemComprobantes["ciEstadoRecepcionAutorizacion"].ToString().Trim().Equals("RAU"))
                        {
                            estado = "RE";
                        }
                        else
                        {
                            estado = itemComprobantes["ciEstadoRecepcionAutorizacion"].ToString().Trim();
                        }

                        XmlGenerados xmlGenerado = new XmlGenerados();
                        xmlGenerado.Identity = Convert.ToInt32(itemComprobantes["Identitycomprobante"].ToString().Trim());
                        xmlGenerado.CiCompania = Convert.ToInt32(itemComprobantes["ciCompania"].ToString().Trim());
                        xmlGenerado.CiTipoEmision = itemComprobantes["ciTipoEmision"].ToString().Trim();
                        xmlGenerado.CiContingenciaDet = Convert.ToInt32(itemComprobantes["ciContingenciaDet"].ToString().Trim());
                        xmlGenerado.CiTipoDocumento = itemComprobantes["ciTipoDocumento"].ToString().Trim();
                        xmlGenerado.TxNumeroAutorizacion = itemComprobantes["txNumeroAutorizacion"].ToString().Trim();
                        xmlGenerado.txFechaHoraAutorizacion = itemComprobantes["txFechaHoraAutorizacion"].ToString().Trim();
                        xmlGenerado.ClaveAcceso = itemComprobantes["txClaveAcceso"].ToString().Trim();
                        xmlGenerado.XmlComprobante = itemComprobantes["xmlDocumentoAutorizado"].ToString().Trim(); //xmlDoc.OuterXml;
                        xmlGenerado.XmlEstado = estado;
                        xmlGenerado.ciNumeroIntento = Convert.ToInt32(itemComprobantes["ciNumeroIntento"].ToString().Trim());
                        ListaComprobante.Add(xmlGenerado); ///FALTABA
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message + "----" + ex.StackTrace);
            }
            return ListaComprobante;
        }

        public string ObtenerFechaEmisionDocumentoXMLFirmado(string tipoDocumento, string xmlFirmado)
        {
            string fechaEmision = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlFirmado);
                XmlNodeList CamposXML;
                switch (tipoDocumento)
                {
                    case "01":
                        CamposXML = xml.SelectNodes("factura/infoFactura");
                        break;
                    case "07":
                        CamposXML = xml.SelectNodes("comprobanteRetencion/infoCompRetencion");
                        break;
                    case "05":
                        CamposXML = xml.SelectNodes("NotaDebito/infoNotaDebito");
                        break;
                    case "04":
                        CamposXML = xml.SelectNodes("notaCredito/infoNotaCredito");
                        break;
                    default:
                        CamposXML = xml.SelectNodes("guiaRemision/infoGuiaRemision");
                        break;
                }
                XmlNode informacionXML = CamposXML.Item(0);
                fechaEmision = informacionXML.SelectSingleNode("fechaEmision").InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message + "---" + ex.StackTrace);
            }
            return fechaEmision;
        }


        public string ObtenerNumeroDocumentoXMLFirmado(string tipoDocumento, string xmlFirmado)
        {
            string numeroDocumento = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlFirmado);
                XmlNodeList CamposXML;
                switch (tipoDocumento)
                {
                    case "01":
                        CamposXML = xml.SelectNodes("factura/infoTributaria");
                        break;
                    case "07":
                        CamposXML = xml.SelectNodes("comprobanteRetencion/infoTributaria");
                        break;
                    case "05":
                        CamposXML = xml.SelectNodes("NotaDebito/infoTributaria");
                        break;
                    case "04":
                        CamposXML = xml.SelectNodes("notaCredito/infoTributaria");
                        break;
                    default:
                        CamposXML = xml.SelectNodes("guiaRemision/infoTributaria");
                        break;
                }
                XmlNode informacionXML = CamposXML.Item(0);
                string estable = informacionXML.SelectSingleNode("estab").InnerText;
                string puntoemi = informacionXML.SelectSingleNode("ptoEmi").InnerText;
                string secuen = informacionXML.SelectSingleNode("secuencial").InnerText;
                numeroDocumento = estable + "-" + puntoemi + "-" + secuen;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message + "---" + ex.StackTrace);
            }
            return numeroDocumento;
        }

        public List<ResprocesoMD> ConsultaDocError(string compania, string Tipodocu, string NumDocu, string Fecha, string FechaHAsta, string CLaveAcceso, string Opcion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ReprocesoAD Rep_Doc = new ReprocesoAD();
            List<ResprocesoMD> objDocError = new List<ResprocesoMD>();
            try
            {
                DataSet dsRespuesta = Rep_Doc.ConsutaReproceso(compania, Tipodocu, NumDocu, Fecha, FechaHAsta, CLaveAcceso, Opcion, ref codigoRetorno, ref mensajeRetorno);
                if (dsRespuesta.Tables[0].Rows.Count > 0)
                {
                    bool existe = dsRespuesta.Tables[0].Columns.Contains("RazonSocial") ? true : false;
                    if (existe)
                    {
                        foreach (DataRow row in dsRespuesta.Tables[0].Rows)
                        {
                            ResprocesoMD RM = new ResprocesoMD()
                            {
                                RazonSocial = row["RazonSocial"].ToString().Trim(),
                                TipoDocumento = row["TipoDocumento"].ToString().Trim(),
                                NumeroDocumento = row["NumeroDocumento"].ToString().Trim(),
                                ClaveAcceso = row["ClaveAcceso"].ToString().Trim(),
                                FechaEmision = row["FechaEmision"].ToString().Trim(),
                                FechaHoraAutorizacion = row["FechaHoraAutorizacion"].ToString().Trim(),
                                Estado = row["Estado"].ToString().Trim(),
                                CiEstado = row["CiEstado"].ToString().Trim(),
                                CodError = row["CodError"].ToString().Trim(),
                                MenError = row["MenError"].ToString().Trim(),
                                NumeroCiclos = Convert.ToInt32(row["NumeroCiclos"].ToString().Trim())
                            };
                            objDocError.Add(RM);
                        }
                    }
                    else
                    {
                        mensajeRetorno = "Estado Modificado";
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return objDocError;
        }
    }
}
