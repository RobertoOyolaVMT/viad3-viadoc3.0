using System;
using System.Data;
using System.Linq;
using ViaDoc.AccesoDatos;
using ViaDoc.Configuraciones;
using ViaDocAutorizacion.LogicaNegocios;

namespace AutorizacionViaDoc.WinServ
{
    public class MetodosWinServ
    {
        int codigoRetorno = 0;
        string descripcionRetorno = string.Empty;
        DocumentoAD _metodosDocumentos = new DocumentoAD();
        MetodosDocumentos _procesoDocumentos = new MetodosDocumentos();
        ReprocesoServWin _reprocesoServWin = new ReprocesoServWin();


        public void ProbadorServicio()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("4", CatalogoViaDoc.DocumentoFactura, "", 0, ref codigoRetorno, ref descripcionRetorno);

            if (codigoRetorno.Equals(0))
            {
                DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                DataTable dtContadorDocumentos = dsTipoDocumento.Tables[1];

                foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                {
                    if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoFactura)
                    {
                        int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContFactura"].ToString());
                        if (contador > 0)
                        {
                            GenerarAutorizacionRecepcionFactura();
                        }
                    }
                    if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoCompRetencion)
                    {
                        int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContCompRetencion"].ToString());
                        if (contador > 0)
                        {
                            GenerarAutorizacionRecepcionCompRecepcion();
                        }
                    }
                    if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoNotaCredito)
                    {
                        int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContNotaCredito"].ToString());
                        if (contador > 0)
                        {
                            GenerarAutorizacionRecepcionNotaCredito();
                        }
                    }
                    if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoNotaDebito)
                    {
                        int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContNotaDebito"].ToString());
                        if (contador > 0)
                        {
                            GenerarAutorizacionRecepcionNotaDebito();
                        }
                    }
                    if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoGuiaRemision)
                    {
                        int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContGuiaRemision"].ToString());
                        if (contador > 0)
                        {
                            GenerarAutorizacionRecepcionGuiaRemision();
                        }
                    }
                }
            }
        }


        public void GenerarAutorizacionRecepcionFactura()
        {
            try
            {
                _procesoDocumentos.GenerarAutorizacionRecepcionFactura("", ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
        }

        public void GenerarAutorizacionRecepcionLiquidacion()
        {
            try
            {
                _procesoDocumentos.GenerarAutorizacionRecepcionLiqudiacion("", ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

        }

        public void GenerarAutorizacionRecepcionCompRecepcion()
        {
            try
            {
                _procesoDocumentos.GenerarAutorizacionRecepcionCompRetencion("", ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

        }

        public void GenerarAutorizacionRecepcionNotaCredito()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoNotaCredito, "", 0, ref codigoRetorno, ref descripcionRetorno);
            try
            {
                _procesoDocumentos.GenerarAutorizacionRecepcionNotaCredito("", ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

        }

        public void GenerarAutorizacionRecepcionNotaDebito()
        {
            DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("2", CatalogoViaDoc.DocumentoNotaDebito, "", 0, ref codigoRetorno, ref descripcionRetorno);
            try
            {
                _procesoDocumentos.GenerarAutorizacionRecepcionNotaDebito("", ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

        }

        public void GenerarAutorizacionRecepcionGuiaRemision()
        {
            try
            {
                _procesoDocumentos.GenerarAutorizacionRecepcionGuiaRemision("", ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }

        }

        public void ReprocesoDoc()
        {
            try
            {
                _reprocesoServWin.ReprocesoDoc(ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
        }
    }
}
