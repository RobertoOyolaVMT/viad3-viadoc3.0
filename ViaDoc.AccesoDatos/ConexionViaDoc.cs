using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ViaDoc.AccesoDatos
{
    public class ConexionViaDoc
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
            if (baseDatos == "Viadoc") {
                cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionViaDoc"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConexionViaDoc"].ProviderName);
            }
            else if (baseDatos == "DocElectronicos") 
            {
                cadenaConexion = ConfigurationManager.ConnectionStrings["DocElectronicos"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["DocElectronicos"].ProviderName);
            }
            else if (baseDatos == "Vernaza") {
                cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionViaDocVernaza"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConexionViaDocVernaza"].ProviderName);
            }
            else if (baseDatos == "Metrovia") {
                cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionViaDocMetrovia"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConexionViaDocMetrovia"].ProviderName);
            }
            else if (baseDatos == "Alborada") {
                cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionViaDocAlborada"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConexionViaDocAlborada"].ProviderName);
            }
            else if (baseDatos == "Garzota") {
                cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionViaDocGarzota"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConexionViaDocGarzota"].ProviderName);
            }
            else if (baseDatos == "Sur") {
                cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionViaDocSur"].ConnectionString;
                factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConexionViaDocSur"].ProviderName);
            }

            try
            {
                this.conexion = factory.CreateConnection();
                this.conexion.ConnectionString = cadenaConexion;
                this.conexion.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectarse a la Base Datos");
            }
            finally
            {
                this.conexion.Close();
            }
        }

        public void crearComandoSql(string sentenciaSql)
        {
            this.comando = factory.CreateCommand();
            this.comando.Connection = this.conexion;
            this.comando.CommandTimeout = 300;
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
            adapter = factory.CreateDataAdapter();
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

        public int EjecutarExecuteNonQuery()
        {
            int dsRespuesta = 0;
            SqlCommand s = new SqlCommand();
            adapter = factory.CreateDataAdapter();
            adapter.SelectCommand = this.comando;
            return dsRespuesta = s.ExecuteNonQuery();
        }


    }
}