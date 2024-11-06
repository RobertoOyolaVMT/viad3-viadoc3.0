using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.usuario
{
    public class UsuarioAD
    {

        public DataSet MantenimientoUsuarios(string opcion, string txCodigoPerfiles, string txNombreOpcion, string ciEstadoPerfiles, string txPermisos, string txCodigoUsuario,
                                             string txUsuario, string txPassword, string ciEstadoUsuario, string ciEstadoUsuarioPerfiles, string XMLUsuarioPerfiles,
                                             string txNombre, int pPageSize, int pPageNumber, string pSortColumn, string pSortOrder,
                                             ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebMantenimientoUsuariosPerfiles");
                conexion.agregarParametroSP("@opcion", opcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoPerfiles", txCodigoPerfiles, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombreOpcion", txNombreOpcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstadoPerfiles", ciEstadoPerfiles, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPermisos", txPermisos, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txCodigoUsuario", txCodigoUsuario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txUsuario", txUsuario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txPassword", txPassword, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstadoUsuario", ciEstadoUsuario, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstadoUsuarioPerfiles", ciEstadoUsuarioPerfiles, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@XMLUsuarioPerfiles", XMLUsuarioPerfiles, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombre", txNombre, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@PageSize", pPageSize, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@CurrentPage", pPageNumber, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@SortColumn", pSortColumn, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@SortOrder", pSortOrder, DbType.String, ParameterDirection.Input);

                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    codigoRetorno = 0;
                }
                else
                {
                    codigoRetorno = 1;
                    mensajeRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL";
            }
            return dsResultado;
        }
    }
}
