using System;
using System.Data;
using ViaDoc.AccesoDatos.winServAutorizacion;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;

namespace ViaDocAutorizacion.LogicaNegocios
{
    public class MetodosDocumentos
    {
        ProcesoAutorizacionRecepcion procesoAutorizacion = new ProcesoAutorizacionRecepcion();

        public void GenerarAutorizacionRecepcionFactura(string claveAcceso, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Compania companias = new Compania();
            UrlSriAC urlSriAC = new UrlSriAC();
            try
            {
                DataSet dsCompania = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    if (dsCompania.Tables.Count != 0)
                    {
                        foreach (DataRow resultadoCompania in dsCompania.Tables[0].Rows)
                        {
                            try
                            {
                                Compania compania = ObtenerDatosCompania(resultadoCompania);
                                DataTable datosUrlSri = urlSriAC.ObtenerURLSRI("C", "", "", "", compania.CiTipoAmbiente.ToString(),
                                                                               ref codigoRetorno, ref mensajeRetorno);

                                if (codigoRetorno.Equals(0))
                                {
                                    procesoAutorizacion.ProcesoRecepcionAutorizacion(compania, CatalogoViaDoc.DocumentoFactura, "",
                                                                                     datosUrlSri.Rows[0][1].ToString().Trim(),
                                                                                     datosUrlSri.Rows[0][0].ToString().Trim(),
                                                                                     claveAcceso, ref codigoRetorno, ref mensajeRetorno);
                                }
                            }
                            catch (Exception ex)
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception1:" + ex.Message + "--- " + ex.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception2:" + ex.Message
                               + "--- " + ex.StackTrace);
            }
        }

        public void GenerarAutorizacionRecepcionLiqudiacion(string claveAcceso, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Compania companias = new Compania();
            UrlSriAC urlSriAC = new UrlSriAC();

            try
            {
                DataSet dsCompania = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    if (dsCompania.Tables.Count != 0)
                    {
                        dsCompania.Relations.Add("COMPAÑIAS", dsCompania.Tables[0].Columns["ciCompania"], dsCompania.Tables[1].Columns["ciCompania"]);
                        foreach (DataRow resultadoCompania in dsCompania.Tables[0].Rows)
                        {
                            try
                            {
                                Compania compania = ObtenerDatosCompania(resultadoCompania);
                                DataTable datosUrlSri = urlSriAC.ObtenerURLSRI("C", "", "", "", compania.CiTipoAmbiente.ToString(),
                                                                               ref codigoRetorno, ref mensajeRetorno);

                                if (codigoRetorno.Equals(0))
                                {
                                    procesoAutorizacion.ProcesoRecepcionAutorizacion(compania, CatalogoViaDoc.DocumentoLiquidacion, "",
                                                                                     datosUrlSri.Rows[0][1].ToString().Trim(),
                                                                                     datosUrlSri.Rows[0][0].ToString().Trim(),
                                                                                     claveAcceso, ref codigoRetorno, ref mensajeRetorno);
                                }
                            }
                            catch (Exception ex)
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception1:" + ex.Message
                                    + "--- " + ex.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception2:" + ex.Message
                               + "--- " + ex.StackTrace);
            }
        }

        public void GenerarAutorizacionRecepcionCompRetencion(string claveAcceso, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Compania companias = new Compania();
            UrlSriAC urlSriAC = new UrlSriAC();
            try
            {
                DataSet dsCompania = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {

                    if (dsCompania.Tables.Count != 0)
                    {
                        dsCompania.Relations.Add("COMPAÑIAS", dsCompania.Tables[0].Columns["ciCompania"], dsCompania.Tables[1].Columns["ciCompania"]);
                        foreach (DataRow resultadoCompania in dsCompania.Tables[0].Rows)
                        {
                            try
                            {
                                Compania compania = ObtenerDatosCompania(resultadoCompania);
                                DataTable datosUrlSri = urlSriAC.ObtenerURLSRI("C", "", "", "", compania.CiTipoAmbiente.ToString(),
                                                                               ref codigoRetorno, ref mensajeRetorno);

                                if (codigoRetorno.Equals(0))
                                {
                                    procesoAutorizacion.ProcesoRecepcionAutorizacion(compania, CatalogoViaDoc.DocumentoCompRetencion, "",
                                                                                     datosUrlSri.Rows[0][1].ToString().Trim(),
                                                                                     datosUrlSri.Rows[0][0].ToString().Trim(),
                                                                                     claveAcceso, ref codigoRetorno, ref mensajeRetorno);
                                }
                            }
                            catch (Exception ex)
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception1:" + ex.Message
                                    + "--- " + ex.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception2:" + ex.Message
                               + "--- " + ex.StackTrace);
            }
        }


        public void GenerarAutorizacionRecepcionNotaCredito(string claveAcceso, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Compania companias = new Compania();
            UrlSriAC urlSriAC = new UrlSriAC();

            try
            {
                DataSet dsCompania = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    if (dsCompania.Tables.Count != 0)
                    {
                        dsCompania.Relations.Add("COMPAÑIAS", dsCompania.Tables[0].Columns["ciCompania"], dsCompania.Tables[1].Columns["ciCompania"]);
                        foreach (DataRow resultadoCompania in dsCompania.Tables[0].Rows)
                        {
                            try
                            {
                                Compania compania = ObtenerDatosCompania(resultadoCompania);
                                DataTable datosUrlSri = urlSriAC.ObtenerURLSRI("C", "", "", "", compania.CiTipoAmbiente.ToString(),
                                                                               ref codigoRetorno, ref mensajeRetorno);

                                if (codigoRetorno.Equals(0))
                                {
                                    procesoAutorizacion.ProcesoRecepcionAutorizacion(compania, CatalogoViaDoc.DocumentoNotaCredito, "",
                                                                                     datosUrlSri.Rows[0][1].ToString().Trim(),
                                                                                     datosUrlSri.Rows[0][0].ToString().Trim(),
                                                                                     claveAcceso, ref codigoRetorno, ref mensajeRetorno);
                                }
                            }
                            catch (Exception ex)
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception1:" + ex.Message
                                    + "--- " + ex.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception2:" + ex.Message
                               + "--- " + ex.StackTrace);
            }
        }


        public void GenerarAutorizacionRecepcionNotaDebito(string claveAcceso, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Compania companias = new Compania();
            UrlSriAC urlSriAC = new UrlSriAC();

            try
            {
                DataSet dsCompania = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    if (dsCompania.Tables.Count != 0)
                    {
                        dsCompania.Relations.Add("COMPAÑIAS", dsCompania.Tables[0].Columns["ciCompania"], dsCompania.Tables[1].Columns["ciCompania"]);
                        foreach (DataRow resultadoCompania in dsCompania.Tables[0].Rows)
                        {
                            try
                            {
                                Compania compania = ObtenerDatosCompania(resultadoCompania);
                                DataTable datosUrlSri = urlSriAC.ObtenerURLSRI("C", "", "", "", compania.CiTipoAmbiente.ToString(),
                                                                               ref codigoRetorno, ref mensajeRetorno);

                                if (codigoRetorno.Equals(0))
                                {
                                    procesoAutorizacion.ProcesoRecepcionAutorizacion(compania, CatalogoViaDoc.DocumentoNotaDebito, "",
                                                                                     datosUrlSri.Rows[0][1].ToString().Trim(),
                                                                                     datosUrlSri.Rows[0][0].ToString().Trim(),
                                                                                     claveAcceso, ref codigoRetorno, ref mensajeRetorno);
                                }
                            }
                            catch (Exception ex)
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception1:" + ex.Message
                                    + "--- " + ex.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception2:" + ex.Message
                               + "--- " + ex.StackTrace);
            }
        }


        public void GenerarAutorizacionRecepcionGuiaRemision(string claveAcceso, ref int codigoRetorno, ref string mensajeRetorno)
        {
            Compania companias = new Compania();
            UrlSriAC urlSriAC = new UrlSriAC();

            try
            {
                DataSet dsCompania = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                if (codigoRetorno.Equals(0))
                {
                    if (dsCompania.Tables.Count != 0)
                    {
                        dsCompania.Relations.Add("COMPAÑIAS", dsCompania.Tables[0].Columns["ciCompania"], dsCompania.Tables[1].Columns["ciCompania"]);
                        foreach (DataRow resultadoCompania in dsCompania.Tables[0].Rows)
                        {
                            try
                            {
                                Compania compania = ObtenerDatosCompania(resultadoCompania);
                                DataTable datosUrlSri = urlSriAC.ObtenerURLSRI("C", "", "", "", compania.CiTipoAmbiente.ToString(),
                                                                               ref codigoRetorno, ref mensajeRetorno);

                                if (codigoRetorno.Equals(0))
                                {
                                    procesoAutorizacion.ProcesoRecepcionAutorizacion(compania, CatalogoViaDoc.DocumentoGuiaRemision, "",
                                                                                     datosUrlSri.Rows[0][1].ToString().Trim(),
                                                                                     datosUrlSri.Rows[0][0].ToString().Trim(),
                                                                                     claveAcceso, ref codigoRetorno, ref mensajeRetorno);
                                }
                            }
                            catch (Exception ex)
                            {
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception1:" + ex.Message
                                    + "--- " + ex.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception2:" + ex.Message
                               + "--- " + ex.StackTrace);
            }
        }


        public void GenerarRecepcionesAutorizacionesWeb(string claveAcceso, int idCompania, string tipoDocumento,
                                                        ref int codigoRetorno, ref string mensajeRetorno)
        {
            Compania compania = new Compania();
            UrlSriAC urlSriAC = new UrlSriAC();
            Compania companias = new Compania();
            try
            {

                DataSet dsCompania = companias.ConsultaCompanias_y_Certificados(ref codigoRetorno, ref mensajeRetorno);
                string CiTipoAmbiente = string.Empty;
                if (codigoRetorno.Equals(0))
                {
                    DataTable data = dsCompania.Tables[0];
                    foreach (DataRow lista in data.Rows)
                    {
                        string companiaDt = lista["ciCompania"].ToString();
                        if (int.Parse(lista["ciCompania"].ToString()).Equals(idCompania))
                        {
                            CiTipoAmbiente = lista["ciTipoAmbiente"].ToString();
                        }
                    }
                    compania.CiCompania = idCompania;
                    compania.CiTipoAmbiente = int.Parse(CiTipoAmbiente);
                }

                if (codigoRetorno.Equals(0))
                {
                    DataTable datosUrlSri = urlSriAC.ObtenerURLSRI("C", "", "", "", CiTipoAmbiente,
                                                                               ref codigoRetorno, ref mensajeRetorno);

                    if (codigoRetorno.Equals(0))
                    {
                        procesoAutorizacion.ProcesoRecepcionAutorizacion(compania, tipoDocumento, "",
                                                                          datosUrlSri.Rows[0][1].ToString().Trim(),
                                                                          datosUrlSri.Rows[0][0].ToString().Trim(),
                                                                          claveAcceso, ref codigoRetorno, ref mensajeRetorno);
                    }
                }
            }
            catch
            {
                codigoRetorno = 9999;
                mensajeRetorno = "Error Inesperado...";
            }
        }

        private Compania ObtenerDatosCompania(DataRow datosCompania)
        {
            Compania compania = new Compania();
            compania.CiCompania = Convert.ToInt32(datosCompania["ciCompania"].ToString());
            compania.TxRuc = datosCompania["txRuc"].ToString();
            compania.TxNombreComercial = datosCompania["txNombreComercial"].ToString();
            compania.TxRazonSocial = datosCompania["txRazonSocial"].ToString();
            compania.CiTipoAmbiente = Convert.ToInt32(datosCompania["ciTipoAmbiente"].ToString());

            return compania;
        }

    }
}
