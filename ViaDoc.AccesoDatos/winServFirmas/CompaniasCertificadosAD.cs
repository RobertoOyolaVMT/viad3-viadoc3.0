using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.Utilitarios;

namespace ViaDoc.AccesoDatos.winServFirmas
{
    public class CompaniasCertificadosAD
    {
        ConexionViaDoc conexion = new ConexionViaDoc();

 

        public DataSet MantenimientoCertificado(int opcion, int ciCompania, int ciCertificado, string uiSemilla, string txClave, string txKey,
            string obCertificado, DateTime fcDesde, DateTime fcHasta, string ciEstado, ref int codigoRetorno, ref string mensajeRetorno)
        {

            DataSet dsResultado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_MantenimientoCertificado");
                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCertificado", ciCertificado, DbType.Int32, ParameterDirection.Input);
                if(!uiSemilla.Equals(""))
                    conexion.agregarParametroSP("@uiSemilla", uiSemilla.ToString(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClave", txClave, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txKey", txKey, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@obCertificado", obCertificado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fcDesde", fcDesde, DbType.DateTime, ParameterDirection.Input);
                conexion.agregarParametroSP("@fcHasta", fcHasta, DbType.DateTime, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (!opcion.Equals(4))
                        {
                            codigoRetorno = int.Parse(dsResultado.Tables[0].Rows[0]["codigoRetorno"].ToString());
                            mensajeRetorno = dsResultado.Tables[0].Rows[0]["mensajeRetorno"].ToString();
                        }
                        else
                        {
                            codigoRetorno = 0;
                        }
                    }
                }
                else
                {
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return dsResultado;
        }


        public DataTable ConsultaComprobantePorEstado(int ciCompania, string TipoDocumento, string ciEstado)
        {
            DataTable esquemas = new DataTable();
            DataSet dsResultado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("dbo.ViaDoc_ConsultarComprobantesPorEstado");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", TipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        esquemas = dsResultado.Tables[0];
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                conexion.desconectar();
            }
            return esquemas;
        }

        public DataTable ConsultaComprobanteAutorizadosGeneracionRider(int ciCompania, String TipoDocumento, String ciEstado)
        {
            DataTable DocumentosAutorizados = new DataTable();
            DataSet dsResultado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("dbo.ViaDoc_ConsultaComprobanteAutorizados");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", TipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        DocumentosAutorizados = dsResultado.Tables[0];
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
            }
            return DocumentosAutorizados;
        }
    }
}