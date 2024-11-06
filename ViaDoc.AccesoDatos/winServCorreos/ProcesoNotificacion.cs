using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.winServCorreos
{
    public class ProcesoNotificacion
    {
        ConexionViaDoc conexion = new ConexionViaDoc();
        
        public DataSet ConsultarNotificacion(int opcion, string tipoNotificacion, string descripcion, bool envioNotificacion,
                                             int idCompania, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = new DataSet();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServNotificacion");
                conexion.agregarParametroSP("@Opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@TipoNotificacion", tipoNotificacion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@Descripcion", descripcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@EnvioNotificacion", envioNotificacion, DbType.Boolean, ParameterDirection.Input);
                conexion.agregarParametroSP("@CiCompania", idCompania, DbType.Int32, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();
                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Rows.Count > 0)
                        {
                            codigoRetorno = 0;
                        }
                        else codigoRetorno = 1;
                    }
                    else codigoRetorno = 1;
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "Nose se obtuve ninguna Registro";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin($"Metodo: ConsultarNotificacion, Excpetion: {ex.Message}, Linea: {ex.Source}");
            }
            finally
            {
                conexion.desconectar();
            }
            return dsResultado;
        }
    }
}
