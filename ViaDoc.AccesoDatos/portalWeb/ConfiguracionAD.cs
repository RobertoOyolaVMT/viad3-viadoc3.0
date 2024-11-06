using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace ViaDoc.AccesoDatos.portalWeb
{
    public class ConfiguracionAD
    {
        public DataSet MantenimientoDocumentos(int opcion, string idTipoDocumento, int idCompania, int idRegistro,
                                         int cantidadFirma, int cantidadAutorizacion, int cantidadCorreo,
                                         int reprocesoFirma, int reprocesoCorreo, int reprocesoAutorizacion,
                                         ref int codigoRetorno, ref string mensajeRetorno)
        {
            ConexionViaDoc conexion = new ConexionViaDoc();
            DataSet dsResultado = null;
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebMantenimientoDocumentos");
                conexion.agregarParametroSP("@Opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", idTipoDocumento, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@CiCompania", idCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@idRegistro", idRegistro, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@cantidadFirma", cantidadFirma, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@cantidadAutorizacion", cantidadAutorizacion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@cantidadCorreo", cantidadCorreo, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ReprocesoFirma", reprocesoFirma, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ReprocesoCorreo", reprocesoCorreo, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ReprocesoAutorizacion", reprocesoAutorizacion, DbType.Int32, ParameterDirection.Input);
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



        public String[] MantenimientoDocumentosHorasNotificacion(int opcion, string horasNotificacion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            string rutaXml = ConfigurationManager.AppSettings.Get("NOTIFICACION.CONFIGURACION").Trim();
            String[] ArrayStrHorasEjecucion = new string[0];
            try
            {
                if (!System.IO.File.Exists(@rutaXml + "HoraNotificacion.txt"))
                {
                    TextWriter tw = new StreamWriter(@rutaXml + "HoraNotificacion.txt", true);
                    //tw.WriteLine("07:00-23:00");
                    tw.Close();
                }

                if (opcion.Equals(1))
                {
                    ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraNotificacion.txt");
                    codigoRetorno = 0;
                    mensajeRetorno = "";
                }

                if (opcion.Equals(2))
                {
                    //TextWriter tw = new StreamWriter(@rutaXml + "\\HoraServFirma.txt", true);
                    //tw.WriteLine(horasNotificacion);
                    //tw.Close();

                    File.WriteAllText(@rutaXml + "HoraNotificacion.txt", horasNotificacion);
                    codigoRetorno = 0;
                    mensajeRetorno = "Se actualizaron los registro de Notificaciones.";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL";
            }
            return ArrayStrHorasEjecucion;

        }



        public String[] MantenimientoDocumentosHorasReproceso(int opcion, int tipoProceso, string horasNotificacion, ref int codigoRetorno, ref string mensajeRetorno)
        {
            string rutaXml = ConfigurationManager.AppSettings.Get("NOTIFICACION.CONFIGURACION").Trim();
            String[] ArrayStrHorasEjecucion = new string[0];
            try
            {
                if (!System.IO.File.Exists(@rutaXml + "HoraReprocesoDocumento.txt"))
                {
                    TextWriter tw = new StreamWriter(@rutaXml + "HoraReprocesoDocumento.txt", true);
                    tw.Close();
                }
                //if (!System.IO.File.Exists(@rutaXml + "HoraReprocesoAutorizacion.txt"))
                //{
                //    TextWriter tw = new StreamWriter(@rutaXml + "HoraReprocesoAutorizacion.txt", true);
                //    tw.Close();
                //}
                //if (!System.IO.File.Exists(@rutaXml + "HoraReprocesoCorreo.txt"))
                //{
                //    TextWriter tw = new StreamWriter(@rutaXml + "HoraReprocesoCorreo.txt", true);
                //    tw.Close();
                //}

                if (opcion.Equals(1))
                {
                    //Proceso de Firma
                    //if (tipoProceso.Equals(1))
                    ///{
                    ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraReprocesoDocumento.txt");
                    codigoRetorno = 0;
                    mensajeRetorno = "";
                    //}//Proceso de Autorizacion
                    //else if (tipoProceso.Equals(2))
                    //{
                    //    ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraReprocesoAutorizacion.txt");
                    //    codigoRetorno = 0;
                    //    mensajeRetorno = "";
                    //}
                    //else //Proceso de Envio de Notificacion
                    //{
                    //    ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraReprocesoCorreo.txt");
                    //    codigoRetorno = 0;
                    //    mensajeRetorno = "";
                    //}
                }

                if (opcion.Equals(2))
                {
                    //Proceso de Firma
                    //if (tipoProceso.Equals(1))
                    //{
                    File.WriteAllText(@rutaXml + "HoraReprocesoDocumento.txt", horasNotificacion);
                    codigoRetorno = 0;
                    mensajeRetorno = "Se actualizaron los registro de Notificaciones.";
                    // } Proceso de Autorizacion
                    //else if (tipoProceso.Equals(2))
                    //{
                    //    File.WriteAllText(@rutaXml + "HoraReprocesoAutorizacion.txt", horasNotificacion);
                    //    codigoRetorno = 0;
                    //    mensajeRetorno = "Se actualizaron los registro de Notificaciones.";
                    //}
                    //else //Proceso de Envio de Notificacion
                    //{
                    //    File.WriteAllText(@rutaXml + "HoraReprocesoCorreo.txt", horasNotificacion);
                    //    codigoRetorno = 0;
                    //    mensajeRetorno = "Se actualizaron los registro de Notificaciones.";
                    //}
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                mensajeRetorno = "DataSet de consulta NULL";
            }
            return ArrayStrHorasEjecucion;

        }
    }
}
