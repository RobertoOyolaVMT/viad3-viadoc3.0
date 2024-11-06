
using ReportesViaDoc;
using System;
using System.Data;
using ViaDoc.AccesoDatos.compania;


namespace ViaDocEnvioCorreo.Negocios
{
    public class ProcesoGenerarRideWeb
    {
        CompaniaAD _metodosConsulta = new CompaniaAD();

        public Byte[] GenerarRideDocumentos(int idCompania, string xmlComprobante, string fechaHoraAutorizacion, string numeroAutorizacion,
            string tipoDocumento, ref int codigoRetorno, ref string descripcionRetorno)
        {

            Byte[] pdfRide = null;

            try
            {
                DataSet dsCatalogo = null;
                DataSet dsConfiguracionCompania = _metodosConsulta.ConsularCatalogoSistema(5, idCompania, "", ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                    dsCatalogo = _metodosConsulta.ConsularCatalogoSistema(1, 0, "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                    pdfRide = GenerarRideDocumentoElectronico.GenerarRiderComprobantesAutorizados(ref descripcionRetorno, xmlComprobante, fechaHoraAutorizacion,
                                           numeroAutorizacion, tipoDocumento, "", dsConfiguracionCompania, dsCatalogo);

            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
                pdfRide = null;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

            return pdfRide;
        }

        public Byte[] GenerarRideDocumentosHistorico(string nombreHistorico, int idCompania, string xmlComprobante, string fechaHoraAutorizacion, string numeroAutorizacion,
            string tipoDocumento, ref int codigoRetorno, ref string descripcionRetorno)
        {
            Byte[] pdfRide = null;

            try
            {
                DataSet dsCatalogo = null;
                DataSet dsConfiguracionCompania = _metodosConsulta.ConsularCatalogoSistemaHistorico(nombreHistorico, 5, idCompania, "", ref codigoRetorno, ref descripcionRetorno);

                if (codigoRetorno.Equals(0))
                    dsCatalogo = _metodosConsulta.ConsularCatalogoSistemaHistorico(nombreHistorico, 1, 0, "", ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                    pdfRide = GenerarRideDocumentoElectronico.GenerarRiderComprobantesAutorizados(ref descripcionRetorno, xmlComprobante, fechaHoraAutorizacion,
                                           numeroAutorizacion, tipoDocumento, "", dsConfiguracionCompania, dsCatalogo);
            }

            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
                pdfRide = null;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
            return pdfRide;
        }

    }
}
