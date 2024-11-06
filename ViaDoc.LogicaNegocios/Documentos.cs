using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos;
using ViaDoc.EntidadNegocios;
using ViaDoc.Utilitarios;

namespace ViaDoc.LogicaNegocios
{
    public class Documentos
    {
        public DataSet verificaDocumentos(string citipoDocumento, int ciCompania, string txEstablecimeinto, string txPuntoEmision,
                                          string txSecuencial, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DocumentoAD objMetodos = new DocumentoAD();
            DataSet dsResultado = new DataSet();


            return dsResultado = objMetodos.verificaExisteDocumento(citipoDocumento, ciCompania, txEstablecimeinto, txPuntoEmision,
                                                                    txSecuencial, ref codigoRetorno, ref descripcionRetorno);
        }


        public void ActualizarEstadosComprobantes(XmlGenerados xmlComprobante, ref int codigoRetorno, ref string descripcionRetorno)
        {
            try
            {
                DocumentoAD Actualiza = new DocumentoAD();
                string mensajeError = "";
                if (xmlComprobante.MensajeError != null)
                {
                    mensajeError = xmlComprobante.MensajeError;
                }

                Actualiza.ActualizarComprobantesProcesados(xmlComprobante.Identity, xmlComprobante.CiCompania, xmlComprobante.CiTipoDocumento,
                                                           xmlComprobante.ClaveAcceso, xmlComprobante.txFechaHoraAutorizacion, xmlComprobante.XmlComprobante,
                                                           xmlComprobante.XmlEstado, xmlComprobante.CiContingenciaDet.ToString(), xmlComprobante.ciNumeroIntento,
                                                           mensajeError, ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Error Inesperado: " + ex.Message;
            }
        }
    }
}
