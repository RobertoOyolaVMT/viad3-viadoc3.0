using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.portalWeb
{
    public class DocumentoAD
    {

        public DataSet ConsultosDocumentos(string opcion, string txRazonSocial, string txClaveAcceso, string txNumeroAutorizacion,
                                           string txSecuencial, string txIdentificacionComprador,
                                           string fechaDesde, string fechaHasta, string tipoDocumento,
                                           string txtNombreComprador, string filtroFechaDH,
                                           ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebConsultarDocumentos");
                conexion.agregarParametroSP("@Opcion", opcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocial", txRazonSocial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumeroAutorizacion", txNumeroAutorizacion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionComprador", txIdentificacionComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaDesde", fechaDesde, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaHasta", fechaHasta, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipoDocumento", tipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@filtroFechaDH", filtroFechaDH, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txtNombreComprador", txtNombreComprador, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }



            return dsResultado;
        }
        public DataSet ConsultosDocumentosHistoricos(string nombreHistorico, string opcion, string txRazonSocial, string txClaveAcceso, string txNumeroAutorizacion,
                                           string txSecuencial, string txIdentificacionComprador,
                                           string fechaDesde, string fechaHasta, string tipoDocumento,
                                           string txtNombreComprador, string filtroFechaDH,
                                           ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase(nombreHistorico);
                conexion.crearComandoSql("ViaDoc_WebConsultarDocumentos");
                conexion.agregarParametroSP("@Opcion", opcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocial", txRazonSocial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNumeroAutorizacion", txNumeroAutorizacion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSecuencial", txSecuencial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txIdentificacionComprador", txIdentificacionComprador, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaDesde", fechaDesde, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaHasta", fechaHasta, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipoDocumento", tipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@filtroFechaDH", filtroFechaDH, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txtNombreComprador", txtNombreComprador, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }



            return dsResultado;
        }

        public DataSet ConsultarXMLDescargar(string txClaveAcceso, string txTipoDocumento, string xml,
                                           ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            var opcion = string.Empty;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_RetornaXML");

                opcion = string.IsNullOrEmpty(xml) ? "XML2" : "XMLUPD";

                conexion.agregarParametroSP("@opcion", opcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@documento", txTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@xml", xml, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[1].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return dsResultado;
        }

        public DataSet ConsultarXMLDescargarHistoricos(string empresaHistorico, string txClaveAcceso, string txTipoDocumento, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase(empresaHistorico);
                conexion.crearComandoSql("ViaDoc_RetornaXML");
                conexion.agregarParametroSP("@opcion", "XML2", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClaveAcceso", txClaveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@documento", txTipoDocumento, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[1].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }

            return dsResultado;
        }

        public void ConsultaEliminarDocumentos(string tipoDocumento, string idCompania, string establecimiento, string puntoEmision, string secuencial, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_EliminaDocumentos");
                conexion.agregarParametroSP("@tipoDocumento", tipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@idCompania", idCompania, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@establecimiento", establecimiento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@puntoEmision", puntoEmision, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@secuencial", secuencial, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            mensajeRetorno = dsResultado.Tables[0].Rows[0][0].ToString();
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0][1].ToString());
                        }
                        else
                        {
                            codigoRetorno = 999;
                            mensajeRetorno = "Error no controlado";
                        }
                    }
                    else
                    {
                        codigoRetorno = 999;
                        mensajeRetorno = "Error no controlado";
                    }
                }
                else
                {
                    codigoRetorno = 999;
                    mensajeRetorno = "Error no controlado";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error en la consulta - Elimina Documentos: " + ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "Error no controlado: " + ex.ToString();
            }
        }

        public DataSet ConsultaEstadisticas(string idCompania, string txtFechaInicio, string txtFechaFin,
                                           ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ConsultaEstadisticas");

                conexion.agregarParametroSP("@compania", idCompania, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fecha", txtFechaInicio, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaFin", txtFechaFin, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }



            return dsResultado;
        }

        public DataSet ConsultaEstadisticasDetalles(string compania, string fecha, string fechaHasta, string tipoDocumento, string ciEstado,
                                           ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ConsultaEstadisticasDetalles");

                conexion.agregarParametroSP("@opcion", "C", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@compania", compania, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fecha", fecha, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaHasta", fechaHasta, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipoDocumento", tipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }



            return dsResultado;
        }

        public DataSet ConsultaEstadisticasNotificacion(string txtFechaInicio, string txtFechaFin,
                                           ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ConsultaEstadisticas_Notificacion");

                conexion.agregarParametroSP("@fecha", txtFechaInicio, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaFin", txtFechaFin, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else
                        {
                            codigoRetorno = 1;
                        }
                    }
                    else
                    {
                        codigoRetorno = 1;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.ToString());
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL" + ex.ToString();
            }



            return dsResultado;
        }

    }
}
