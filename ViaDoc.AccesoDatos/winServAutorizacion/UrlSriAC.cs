using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.winServAutorizacion
{
    public class UrlSriAC
    {
        ConexionViaDoc conexion = new ConexionViaDoc();

        public DataTable ObtenerURLSRI(string opcion, string urlRecepcion, string urlAutorizacion, string ciCompania, string tipoAmbiente
                                      , ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataTable esquemas = new DataTable();
            DataSet dsResultado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_MantenimientoUrlSRI");
                conexion.agregarParametroSP("@opcion", opcion.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@urlRecepcion", urlRecepcion.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@urlAutorizacion", urlAutorizacion.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciCompania.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@tipoAmbiente", tipoAmbiente.Trim(), DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();
                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0)
                    {
                        esquemas = dsResultado.Tables[0];
                    }else
                    {
                        codigoRetorno = 1;
                        descripcionRetorno = "No se obtuvo ningun Registro";
                    }
                }
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "No se obtuvo ningun Registro";
                }
            }
            catch (Exception ex)
            {
                 codigoRetorno = 9999;
                 descripcionRetorno = "Exception" + ex.Message;
            }
             finally
            {
                conexion.desconectar();
            }
            return esquemas;
        }
    }
}
