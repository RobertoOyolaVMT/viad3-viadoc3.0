using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.AccesoDatos.certificado
{
    public class CertificadoAD
    {
        public DataSet ManteniemtoCertificado(string opcion, string Data, byte[] obCertificado, string Ruc, ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            string[] Cert = Data.Split('|');
            string claveCertificado = string.Empty;
            string _PathName = string.Empty;
            string result = string.Empty;

            try
            {
                claveCertificado = Convert.ToString(Cert[2].ToString());
                result = BitConverter.ToString(obCertificado);

                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebMantenimientoCertificado");
                conexion.agregarParametroSP("@opcion", opcion, DbType.String, ParameterDirection.Input);

                /*=========== Los siguientes valores se toman desde el DataSet: ( Cert ) ============*/
                conexion.agregarParametroSP("@ciCompania", Convert.ToInt32(Cert[0].ToString().Trim()), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@uiSemilla", Convert.ToString(Cert[1].ToString().Trim()), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txClave", Convert.ToString(Cert[2].ToString().Trim()), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txKey", Convert.ToString(Cert[3].ToString().Trim()), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@obCertificado", result, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@fcDesde", Convert.ToDateTime(Cert[4].ToString().Trim()), DbType.DateTime, ParameterDirection.Input);
                conexion.agregarParametroSP("@fcHasta", Convert.ToDateTime(Cert[5].ToString().Trim()), DbType.DateTime, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", Cert[6].ToString().Trim(), DbType.String, ParameterDirection.Input);

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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("ManteniemtoCertificado: "+ex.Message+" - "+ mensajeRetorno.ToString());
            }

            return dsResultado;
        }

        public DataSet ConsultaSucursalCompania(string opcion, string Ruc, ref int codigoRetorno, ref string mensajeRetorno)
        {
            DataSet dsResultado = null;
            ConexionViaDoc conexion = new ConexionViaDoc();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebMantenimientoCertificado");
                conexion.agregarParametroSP("@opcion", opcion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRuc", Ruc, DbType.String, ParameterDirection.Input);

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
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("error" + ex.Message );
            }

            return dsResultado;
        }
    }
}
