using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.winServMigracion
{
    public class ProcesoMigracion
    {
        ConexionViaDoc conexion = new ConexionViaDoc();

        public DataSet ConsultaFactura()
        {
            DataSet dsRespuesta = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("PA_ConsultarFactura");
                dsRespuesta = conexion.EjecutarConsultaDatSet();

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
            return dsRespuesta;
        }
    }
}
