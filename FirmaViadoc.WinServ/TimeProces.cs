using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using ViaDoc.AccesoDatos;
using ViaDoc.Configuraciones;

namespace FirmaViadoc.WinServ
{
    public class TimeProces
    {
        DocumentoAD _metodosDocumentos = new DocumentoAD();
        string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").Trim();

        public void StarServ()
        {
            if (!System.IO.File.Exists(@rutaXml + "\\" + "HoraServFirma.txt"))
            {
                TextWriter tw = new StreamWriter(@rutaXml + "\\HoraServFirma.txt", true);
                tw.WriteLine("07:00-23:00");
                tw.Close();
            }

            try
            {
                String[] ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraServFirma.txt");
                Int32 intHoraSystema = Convert.ToInt32(DateTime.Now.ToString("H:mm").Trim().Replace(":", ""));

                foreach (String strHoraEntre in ArrayStrHorasEjecucion)
                {

                    String[] arrStrHoraEntre = strHoraEntre.Trim().Split('-');
                    Int32 intHoraInicio = Convert.ToInt32(arrStrHoraEntre[0].Trim().Replace(":", ""));
                    Int32 intHoraFin = Convert.ToInt32(arrStrHoraEntre[1].Trim().Replace(":", ""));
                    int ProcesoNormal = int.Parse(ConfigurationManager.AppSettings["ProcesoNormal"]);

                    if (intHoraSystema >= intHoraInicio && intHoraSystema <= intHoraFin)
                    {
                        Timer_Elapsed_Factura();

                        Timer_Elapsed_Liquiacion();

                        Timer_Elapsed_CompRetencion();

                        Timer_Elapsed_NotaCredito_Debito_GuiaRemmision();
                    }
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);

            }
        }

        private void Timer_Elapsed_Factura()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;

            try
            {

                DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("1", CatalogoViaDoc.DocumentoFactura, "", 0, ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                    DataTable dtContadorDocumentos = dsTipoDocumento.Tables[1];
                    foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                    {
                        if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoFactura)
                        {
                            int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContFactura"].ToString());
                            if (contador > 0)
                            {
                                metodos.GenerarFirmaElectronicaFactura();
                            }
                        }
                    }
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Factura: " + descripcionRetorno);
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Factura Cathc: " + ex.ToString());
            }
        }

        private void Timer_Elapsed_Liquiacion()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            try
            {
                DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("1", CatalogoViaDoc.DocumentoLiquidacion, "", 0, ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                    DataTable dtContadorDocumentos = dsTipoDocumento.Tables[1];

                    foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                    {
                        if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoLiquidacion)
                        {
                            int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContLiquidacion"].ToString());
                            if (contador > 0)
                            {
                                metodos.GenerarFirmaElectronicaLiquidacion();
                            }
                        }
                    }
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Liquidacion: " + descripcionRetorno);
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Liquidacion: " + ex.Message);
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Liquidacion: " + descripcionRetorno);
            }
        }

        private void Timer_Elapsed_CompRetencion()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;

            try
            {
                DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("1", CatalogoViaDoc.DocumentoFactura, "", 0, ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                    DataTable dtContadorDocumentos = dsTipoDocumento.Tables[1];

                    foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                    {
                        if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoCompRetencion)
                        {
                            int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContCompRetencion"].ToString());
                            if (contador > 0)
                            {
                                metodos.GenerarFirmaElectronicaCompRetencion();
                            }
                        }
                    }
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("CompRetecnion: " + descripcionRetorno);
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("CompRetencion Cathc: " + ex.ToString());
            }
        }

        private void Timer_Elapsed_NotaCredito_Debito_GuiaRemmision()
        {
            MetodosWinServ metodos = new MetodosWinServ();
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;

            try
            {
                DataSet dsTipoDocumento = _metodosDocumentos.ObtenerTipoDocumentos("1", CatalogoViaDoc.DocumentoFactura, "", 0, ref codigoRetorno, ref descripcionRetorno);
                if (codigoRetorno.Equals(0))
                {
                    DataTable dtTipoDocumento = dsTipoDocumento.Tables[0];
                    DataTable dtContadorDocumentos = dsTipoDocumento.Tables[1];

                    foreach (DataRow listaDocumentos in dtTipoDocumento.Rows)
                    {
                        if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoNotaCredito)
                        {
                            int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContNotaCredito"].ToString());
                            if (contador > 0)
                            {
                                metodos.GenerarFirmaElectronicaNotaCredito();
                            }
                        }
                        if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoNotaDebito)
                        {
                            int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContNotaDebito"].ToString());
                            if (contador > 0)
                            {
                                metodos.GenerarFirmaElectronicaNotaDebito();
                            }
                        }
                        if (listaDocumentos["tipoDocumento"].ToString() == CatalogoViaDoc.DocumentoGuiaRemision)
                        {
                            int contador = int.Parse(dtContadorDocumentos.Rows[0]["ContGuiaRemision"].ToString());
                            if (contador > 0)
                            {
                                metodos.GenerarFirmaElectronicaGuiaRemision();
                            }
                        }
                    }
                }
                else
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("NC - ND - GUIA" + descripcionRetorno);
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("NC - ND - GUIA Cathc: " + ex.ToString());
            }
        }
    }
}