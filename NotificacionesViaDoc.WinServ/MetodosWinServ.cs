using System;
using System.Data;
using System.Linq;
using ViaDoc.AccesoDatos;
using ViaDocEnvioCorreo.LogicaNegocios;

namespace NotificacionesViaDoc.WinServ
{
    public class MetodosWinServ
    {
        DocumentoAD _metodosDocumentos = new DocumentoAD();
        ProcesoEnvioCorreo _metodosCorreo = new ProcesoEnvioCorreo();
        int codigoRetorno = 0;
        string descripcionRetorno = string.Empty;

        public void EnvioDocumentosCorreoElectronico()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("5", "", "", 0, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                {
                    _metodosCorreo.EnvioDocumentosCorreoElectronico("", listaDocumentos["procesoCorreo"].ToString(),
                                                                    int.Parse(listaDocumentos["ciCompania"].ToString()),
                                                                    listaDocumentos["tipoDocumento"].ToString(),
                                                                    int.Parse(listaDocumentos["cantidadCorreo"].ToString()),
                                                                    "", ref descripcionRetorno);
                }
            }
        }

        public void EnvioDocumentosPortalWeb()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("5", "", "", 0, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                {
                    _metodosCorreo.EnviarDocumentosPortalWeb(listaDocumentos["procesoCorreo"].ToString(),
                                                             int.Parse(listaDocumentos["ciCompania"].ToString()),
                                                             listaDocumentos["tipoDocumento"].ToString(),
                                                             int.Parse(listaDocumentos["cantidadCorreo"].ToString()),
                                                             "", ref descripcionRetorno);
                }
            }
        }

        public void EnvioNotificacionCertificadoCaducado()
        {
            _metodosCorreo.EnviarNotifacionCertificadoCaducado();
        }

        public void EnvioEstadisticaDocumentos()
        {
            _metodosCorreo.EnviarNotificacionesEstadistica();
        }

        public void NotificacinDocError()
        {
            _metodosCorreo.NotificacinDocError();
        }

        public void NotificacinDocAtrasados()
        {
            _metodosCorreo.NotificacinDocAtrasados();
        }

        public void EnvioMailError()
        {
            _metodosCorreo.EnvioMailError();
        }

        public void Reenvio_a_Portal()
        {
            _metodosCorreo.Reenvio_a_Portal();
        }
    }
}
