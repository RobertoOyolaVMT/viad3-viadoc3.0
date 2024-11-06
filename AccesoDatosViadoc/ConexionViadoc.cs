using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatosViadoc
{
    public class ConexionViadoc
    {
        private DbConnection conexion = null;
        private DbCommand comando = null;
        private DbDataAdapter adapter = null;
        private DbTransaction transaccion = null;
        private static DbProviderFactory factory = null;
        private string cadenaConexion = string.Empty;
        private static string connectionString;
        private SqlConnectionStringBuilder _sqlConnStrBuilder;

        public void tipoBase(string baseDatos)
        {
            if(baseDatos == "Viadoc")
            {
                cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionViaDoc"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConexionViaDoc"].ProviderName);
            }

            if(baseDatos == "DocElectronicos")
            {

            }


            try
            {
                if(this.conexion == null)
                {
                    this.conexion = factory.CreateConnection();
                    this.conexion.ConnectionString = cadenaConexion;
                }
                this.conexion.Open();
            }catch(Exception ex)
            {
                throw new Exception("Error al conectarse a la Base Datos");
                
            }
            finally{
                this.conexion.Close();
            }
        }

        public void crearComandoSql(string sentenciaSql)
        {
            this.comando = factory.CreateCommand();
            this.comando.Connection = this.conexion;
            this.comando.CommandType = CommandType.StoredProcedure;
            this.comando.CommandText = sentenciaSql;

            if (this.transaccion != null)
                this.comando.Transaction = this.transaccion;
        }

        public void agregarParametroSP(string nombre, object valor, DbType tipo, ParameterDirection direccion)
        {
            DbParameter parametro = comando.CreateParameter();
            parametro.Direction = direccion;
            parametro.ParameterName = nombre;
            if (direccion != ParameterDirection.Output)
                parametro.Value = valor;

            parametro.DbType = tipo;
            this.comando.Parameters.Add(parametro);
        }

        public DataSet EjecutarConsultaDatSet()
        {
            DataSet dsRespuesta = new DataSet();
            adapter = factory.CreateDataAdapter(); //
            adapter.SelectCommand = this.comando;
            adapter.Fill(dsRespuesta);
            return dsRespuesta;
        }

        public void desconectar()
        {
            if (this.conexion.State.Equals(ConnectionState.Open))
                this.conexion.Close();

            this.conexion.Dispose();
        }
 
        

    }
}
