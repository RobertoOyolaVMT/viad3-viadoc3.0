using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.winServFirmas
{
    public class MantenimientoDocumentosAD
    {
        ConexionViaDoc conexion = new ConexionViaDoc();


        public bool MantenimientoDocumentoProcesado(int opcion, int ciCompania, string codDocumento, string claveAcceso, string fechaInicial, string fechaProceso, string fechaCorta, string horaCorta, string proceso, string observacion)
        {
            bool respuesta = false;
            int numAfectadas = 0;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_Servicio_MantenimientoDocumentoProcesado");
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);

                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@codDocumento", codDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@claveAcceso", claveAcceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaInicialProcesamiento", fechaInicial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fechaProcesando", fechaProceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@horaCortaProcesado", horaCorta, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ultimoProceso", proceso, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@observacionUltimoProceso", observacion, DbType.String, ParameterDirection.Input);
                numAfectadas = conexion.EjecutarExecuteNonQuery();

                if (numAfectadas > 0)
                    respuesta = true;
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                conexion.desconectar();
            }

            return respuesta;
        }
    }
}
