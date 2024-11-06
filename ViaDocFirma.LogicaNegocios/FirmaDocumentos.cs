using eSign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ViaDoc.AccesoDatos;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDoc.Logs;

namespace ViaDocFirma.LogicaNegocios
{
    public class FirmaDocumentos
    {
        ConexionBDMongo objLogs = new ConexionBDMongo();

        public void ProcesosFirmarDocumentos(ref List<XmlGenerados> xmlComprobantes, Sign eSign, Compania compañia, String EsquemaXSD)
        {
            String MjError = "";
            int contInicial = 0;
            int con = 0; 

            DateTime fecha = DateTime.Now;
            if (xmlComprobantes.Count != 0)
            {
                contInicial = xmlComprobantes.Count(i => i.XmlEstado == CatalogoViaDoc.DocEstadoGenerado);
                string txClaveAcceso = "";
                #region FIRMA_COMPROBANTES
                MjError = "<br/>ERRORES EN EL PROCESO DE FIRMAS:<br/><table style=\"width:70%\">";
                foreach (XmlGenerados item in xmlComprobantes)
                {
                    item.CiContingenciaDet = 1;
                    try
                    {
                        string MensajeError = "";
                        //Envia documentos a la validacion para ser firmado
                        item.XmlComprobante = GenerarFirmaXMLComprobantes(EsquemaXSD, item.XmlComprobante, eSign, compañia.CiCompania, ref MensajeError);
                        item.MensajeError = MensajeError;
                        if (item.XmlComprobante != "") //Si hay dato asigna el estado de firmado
                        {
                            item.XmlEstado = CatalogoViaDoc.DocEstadoFirmado;
                            item.txCodError = "101";
                        }
                        if (MensajeError != "")//Si hay Error asigna el estado de error firmado
                        {
                            item.ciNumeroIntento++;
                            item.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                            MjError += "<tr><td><p style=\"text-align:justify\">" + "DATOS DEL DOCUMENTO ClaveAcceso " + item.ClaveAcceso + " Maensaje: " + item.MensajeError + "</p></td></tr>";
                        }

                        if (item.XmlEstado.Equals("FI"))
                            item.ciNumeroIntento = 0;

                        #region Actualiza el documento en la Base de FacturacionElectronicaQA
                        if (con != 10)
                        {

                            ActualizarXmlComprobantes(item);//Guarda informacion actualizada en la BD
                            con++;
                        }
                        else
                        {

                            ActualizarXmlComprobantes(item);//Guarda informacion actualizada en la BD
                            Thread.Sleep(10000);
                            con = 0;
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        #region Excepcion al firmar el documento

                        item.MensajeError = ex.Message;
                        item.XmlEstado = CatalogoViaDoc.DocEstadoEFirmado;
                        item.txCodError = "101";

                        ActualizarXmlComprobantes(item);
                        #endregion
                    }
                }
                #endregion FIRMA_COMPROBANTES
            }
            //return false;
        }


        protected String GenerarFirmaXMLComprobantes(String EsquemaXSD, String txXML, Sign eSign, int Cicompania, ref string MensajeError)
        {
            String xmlGenerado = "";
            try
            {
                #region VALIDACIONES
                eSign.SeteaDocumentoXML(txXML);
                if (!eSign.XmlEsValido)
                {
                    throw new System.ArgumentException(eSign.Excepcion.Message);
                }
                eSign.SeteaEsquema(EsquemaXSD, Encoding.UTF8);
                if (!eSign.EsquemaEsValido)
                {
                    throw new System.ArgumentException(eSign.Excepcion.Message);
                }
                eSign.ValidaEsquemaXML();
                if (eSign.TieneErroresEnValidacion)
                {
                    throw new System.ArgumentException(eSign.ErroresEnValidacion);
                }
                if (eSign.TieneExcepcionEnValidacion)
                {
                    throw new System.ArgumentException(eSign.Excepcion.Message);
                }

                eSign.FirmarXML("#comprobante");
                if (eSign.TieneErroresEnFirma)
                {
                    throw new System.ArgumentException(eSign.ErroresEnFirma);
                }
                if (eSign.TieneExcepcionEnFirma)
                {
                    throw new System.ArgumentException(eSign.Excepcion.Message);
                }
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Factura: 41");
                #endregion VALIDACIONES
                xmlGenerado = eSign.XmlFirmado;
            }
            catch (Exception ex)
            {
                if (xmlGenerado == "")
                {
                    xmlGenerado = txXML;
                }
                MensajeError = ex.Message;

                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
            }
            return xmlGenerado;
        }


        protected int ActualizarEstadosComprobantes(XmlGenerados xmlComprobante)
        {
            int i = 0;
            try
            {

                if (xmlComprobante.ClaveAcceso.Length != 0)
                {
                    DocumentoAD Actualiza = new DocumentoAD();
                    System.Data.DataSet ds = new System.Data.DataSet();
                    string resultXml = Serializacion.serializar(xmlComprobante);
                    byte[] byteArray = Encoding.UTF8.GetBytes(resultXml);
                    System.IO.Stream str = new System.IO.MemoryStream(byteArray);
                    ds.ReadXml(str);
                    i = Actualiza.ActualizarComprobantes(ds);//actualiza el estado de FI A EENV
                }
            }
            catch (Exception ex)
            {
                int codigoRetorno = 0;
                string mensajeError = "";
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
               
            }
            return i;
        }

        public void ActualizarXmlComprobantes(XmlGenerados xmlComprobante)
        {
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            try
            {
                DocumentoAD Actualiza = new DocumentoAD();
                Actualiza.ActualizarComprobantesProcesados(xmlComprobante.Identity, xmlComprobante.CiCompania, xmlComprobante.CiTipoDocumento,
                                                           xmlComprobante.ClaveAcceso, xmlComprobante.txFechaHoraAutorizacion, xmlComprobante.XmlComprobante,
                                                           xmlComprobante.XmlEstado, xmlComprobante.CiContingenciaDet.ToString(), xmlComprobante.ciNumeroIntento,
                                                           xmlComprobante.MensajeError, ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
              
            }
        }
    }
}
