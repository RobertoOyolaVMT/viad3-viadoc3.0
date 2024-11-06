using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.portalWeb
{
    public class GenerarPDFAD
    {
        public DataSet ConsultaGenerePDF(int Opcion, string codEmpresa, string codDocumento,string fechaDesde, string fechaHasta, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebGenerarPDF");
                conexion.agregarParametroSP("@Opcion", Opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", codEmpresa, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@TipoDocumento", codDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@FechaEmision", fechaDesde, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaHasta", fechaHasta, DbType.String, ParameterDirection.Input);

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
