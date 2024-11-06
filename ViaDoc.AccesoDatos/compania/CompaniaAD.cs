using System;
using System.Data;
using System.Linq;

namespace ViaDoc.AccesoDatos.compania
{
    public class CompaniaAD
    {
        ConexionViaDoc conexion = new ConexionViaDoc();

        public DataSet ConsultaCompanias_y_Certificados(int opcion, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet companiaCertificado = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServConsultaFactElectronica");
                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                companiaCertificado = conexion.EjecutarConsultaDatSet();

                if (companiaCertificado != null)
                    codigoRetorno = 0;
                else
                {
                    codigoRetorno = 1;
                    descripcionRetorno = "DataSet de consulta NULL";
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception:" + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return companiaCertificado;
        }

        public DataSet ConsultarEsquemas(string ciTipoDocumento, string txtVersionEsquema, ref int codigoRetorno, ref string mensajeRetorno)
        {
            DataSet esquemasDocumentos = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WinServConsultaFactElectronica");
                conexion.agregarParametroSP("@opcion", 2, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@versioEsquema", txtVersionEsquema, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoDocumento", ciTipoDocumento, DbType.String, ParameterDirection.Input);
                esquemasDocumentos = conexion.EjecutarConsultaDatSet();

                if (esquemasDocumentos != null)
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
                mensajeRetorno = ex.Message;
            }
            return esquemasDocumentos;
        }


        public DataTable GuardaConfigSMTP(string opcion, string ciCompania, string txRazonSocial, string hostServidor, string puerto,
                                          string enableSSL, string emailCredencial, string passCredenciales, string mailAddressFrom,
                                          string to, string cc, string asunto, string activarNotificacion, ref int codigoRetorno,
                                          ref string descripcionRetorno)
        {
            DataTable responseConfiguracionSMTP = new DataTable();
            DataSet dsConfiguracionSMTP = new DataSet();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_ConfiguracionSMTP");
                conexion.agregarParametroSP("@opcion", opcion.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocial", txRazonSocial.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@hostServidor", hostServidor.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@puerto", puerto.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@enableSSL", enableSSL.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@emailCredencial", emailCredencial.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@passCredenciales", passCredenciales.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@mailAddressFrom", mailAddressFrom.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@to", to.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@cc", cc.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@asunto", asunto.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@activarNotificacion", activarNotificacion, DbType.String, ParameterDirection.Input);
                dsConfiguracionSMTP = conexion.EjecutarConsultaDatSet();

                if (dsConfiguracionSMTP != null)
                {
                    if (dsConfiguracionSMTP.Tables.Count > 0)
                    {
                        if (dsConfiguracionSMTP.Tables[0].Rows.Count > 0)
                        {
                            responseConfiguracionSMTP = dsConfiguracionSMTP.Tables[0];
                            codigoRetorno = 0;
                        }
                        else
                            codigoRetorno = 1;
                    }
                    else
                        codigoRetorno = 1;
                }
                else
                    codigoRetorno = 1;
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return responseConfiguracionSMTP;
        }

        public DataTable GuardaConfigSMTPHistorico(string nombreHistorico, string opcion, string ciCompania, string txRazonSocial, string hostServidor, string puerto,
                                          string enableSSL, string emailCredencial, string passCredenciales, string mailAddressFrom,
                                          string to, string cc, string asunto, string activarNotificacion, ref int codigoRetorno,
                                          ref string descripcionRetorno)
        {
            DataTable responseConfiguracionSMTP = new DataTable();
            DataSet dsConfiguracionSMTP = new DataSet();
            try
            {
                conexion.tipoBase(nombreHistorico);
                conexion.crearComandoSql("ViaDoc_ConfiguracionSMTP");
                conexion.agregarParametroSP("@opcion", opcion.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocial", txRazonSocial.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@hostServidor", hostServidor.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@puerto", puerto.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@enableSSL", enableSSL.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@emailCredencial", emailCredencial.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@passCredenciales", passCredenciales.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@mailAddressFrom", mailAddressFrom.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@to", to.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@cc", cc.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@asunto", asunto.Trim(), DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@activarNotificacion", activarNotificacion, DbType.String, ParameterDirection.Input);
                dsConfiguracionSMTP = conexion.EjecutarConsultaDatSet();

                if (dsConfiguracionSMTP != null)
                {
                    if (dsConfiguracionSMTP.Tables.Count > 0)
                    {
                        if (dsConfiguracionSMTP.Tables[0].Rows.Count > 0)
                        {
                            responseConfiguracionSMTP = dsConfiguracionSMTP.Tables[0];
                            codigoRetorno = 0;
                        }
                        else
                            codigoRetorno = 1;
                    }
                    else
                        codigoRetorno = 1;
                }
                else
                    codigoRetorno = 1;
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return responseConfiguracionSMTP;
        }

        public DataSet ConsularCatalogoSistema(int opcion, int ciCompania, string rucCompania, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsCatalogos = new DataSet();
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_GeneralConsultarCatalogo");
                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@rucCompania", rucCompania, DbType.String, ParameterDirection.Input);
                dsCatalogos = conexion.EjecutarConsultaDatSet();



                if (dsCatalogos != null)
                {
                    codigoRetorno = 0;
                }
                else
                {
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }

            return dsCatalogos;
        }

        public DataSet ConsularCatalogoSistemaHistorico(string nombreHistorico, int opcion, int ciCompania, string rucCompania, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsCatalogos = new DataSet();
            ConexionViaDoc conexion = new ConexionViaDoc();
            try
            {
                conexion.tipoBase(nombreHistorico);
                conexion.crearComandoSql("ViaDoc_GeneralConsultarCatalogo");
                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@rucCompania", rucCompania, DbType.String, ParameterDirection.Input);
                dsCatalogos = conexion.EjecutarConsultaDatSet();

                if (dsCatalogos != null)
                {
                    codigoRetorno = 0;
                }
                else
                {
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = "Exception: " + ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }

            return dsCatalogos;
        }


        public DataSet MantenimientoCompania(int opcion, int ciCompania, string txRuc, string txRazonSocial, string txNombreComercial, string uiCompania, string txDireccionMatriz
                                             , string txContribuyenteEspecial, string txObligadoContabilidad, string txAgenteRetencion,string txRegimenMicroempresas,string txContribuyenteRimpe,
                                               string ciTipoAmbiente, byte[] logoCompania, string ciEstado, ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = new DataSet();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebMantenimientoCompania");
                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRuc", txRuc, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocial", txRazonSocial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombreComercial", txNombreComercial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@uiCompania", uiCompania, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txDireccionMatriz", txDireccionMatriz, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txContribuyenteEspecial", txContribuyenteEspecial, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txObligadoContabilidad", txObligadoContabilidad, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txAgenteRetencion", txAgenteRetencion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRegimenMicroempresas", txRegimenMicroempresas, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txContribuyenteRimpe", txContribuyenteRimpe, DbType.String, ParameterDirection.Input);
                if (ciTipoAmbiente.Equals("")) ciTipoAmbiente = "0";
                conexion.agregarParametroSP("@ciTipoAmbiente", Convert.ToInt32(ciTipoAmbiente.Trim()), DbType.Int32, ParameterDirection.Input);
                if (logoCompania != null)
                    conexion.agregarParametroSP("@logoCompania", BitConverter.ToString(logoCompania), DbType.AnsiString, ParameterDirection.Input);
                else
                    conexion.agregarParametroSP("@logoCompania", "", DbType.AnsiString, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0 || dsResultado.Tables[0].Rows.Count > 0)
                    {
                        codigoRetorno = 0;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return dsResultado;
        }

        public DataSet MantenimientoSucursal(int opcion, int ciCompania, string ciSucursal, string txDireccion, string ciEstado,
                                            ref int codigoRetorno, ref string descripcionRetorno)
        {
            DataSet dsResultado = new DataSet();

            try
            {
                conexion.tipoBase("Viadoc");
                conexion.crearComandoSql("ViaDoc_WebMantenimientoCompania");
                conexion.agregarParametroSP("@opcion", opcion, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciCompania", ciCompania, DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRuc", "", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txRazonSocial", "", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txNombreComercial", "", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@uiCompania", "", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txDireccionMatriz", txDireccion, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txContribuyenteEspecial", "", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txObligadoContabilidad", "", DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciTipoAmbiente", Convert.ToInt32("0"), DbType.Int32, ParameterDirection.Input);
                conexion.agregarParametroSP("@logoCompania", "", DbType.AnsiString, ParameterDirection.Input);
                conexion.agregarParametroSP("@ciEstado", ciEstado, DbType.String, ParameterDirection.Input);
                conexion.agregarParametroSP("@txSucursal", ciSucursal, DbType.String, ParameterDirection.Input);
                dsResultado = conexion.EjecutarConsultaDatSet();

                if (dsResultado != null)
                {
                    if (dsResultado.Tables.Count > 0 || dsResultado.Tables[0].Rows.Count > 0)
                    {
                        codigoRetorno = 0;
                    }
                }
                else
                {
                    codigoRetorno = 1;
                }
            }
            catch (Exception ex)
            {
                codigoRetorno = 9999;
                descripcionRetorno = ex.Message;
            }
            finally
            {
                conexion.desconectar();
            }
            return dsResultado;
        }
    }
}
