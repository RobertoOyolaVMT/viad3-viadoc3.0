using System;
using System.Configuration;
using System.Data;
using System.IO;
using ViaDoc.AccesoDatos.winServAutorizacion;

namespace ViaDocAutorizacion.LogicaNegocios
{
    public class ReprocesoServWin
    {
        ProcesarDocumentosAD _documentosConsulta = new ProcesarDocumentosAD();
        public void ReprocesoDoc(ref int codigoRetorno, ref string descripcionRetorno)
        {
            DateTime fecha = DateTime.Now;
            string opcion = string.Empty;
            string claveAcceso = string.Empty;
            string fechaDesde = fecha.AddMonths(-3).ToString("dd/MM/yyyy");
            string Fechahasta = fecha.ToString("dd/MM/yyyy");
            string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").Trim();
            string codDocElec = ConfigurationManager.AppSettings.Get("codDocElec").Trim();
            string[] codArreglo = codDocElec.Split('|');

            try
            {
                String[] ArrayStrHorasEjecucion = File.ReadAllLines(rutaXml + "HoraReprocesoDocumento.txt");
                Int32 intHoraSystema = Convert.ToInt32(DateTime.Now.ToString("H:mm").Trim().Replace(":", ""));
                foreach (String strHoraEntre in ArrayStrHorasEjecucion)
                {
                    String[] arrStrHoraEntre = strHoraEntre.Trim().Split('-');
                    Int32 intHoraInicio = Convert.ToInt32(arrStrHoraEntre[0].Trim().Replace(":", ""));
                    Int32 intHoraFin = Convert.ToInt32(arrStrHoraEntre[1].Trim().Replace(":", ""));
                    if (intHoraSystema >= intHoraInicio && intHoraSystema <= intHoraFin)
                    {
                        foreach (var cod in codArreglo)
                        {
                            DataSet consultaDoc = _documentosConsulta.DocReproceso(cod, fechaDesde, Fechahasta, null, "1", ref codigoRetorno, ref descripcionRetorno);

                            if (consultaDoc.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow row in consultaDoc.Tables[0].Rows)
                                {
                                    opcion = row["CiEstado"].ToString().Trim() == "EFI" ? "2" : row["CiEstado"].ToString().Trim() == "FI" ? "4" : "3";

                                    _documentosConsulta.DocReproceso(cod, null, null, row["ClaveAcceso"].ToString(), opcion, ref codigoRetorno, ref descripcionRetorno);

                                    claveAcceso = row["ClaveAcceso"].ToString().Trim() + " | " + row["CiEstado"].ToString().Trim() + " | " + cod + " | | " + claveAcceso;
                                }
                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Documentos Reprocesados: " + claveAcceso);
                            }
                            else
                            {
                                var docName = string.Empty;
                                switch (cod)
                                {
                                    case "01":
                                        docName = "Factura";
                                        break;
                                    case "03":
                                        docName = "Liquidacion";
                                        break;
                                    case "04":
                                        docName = "NotaCredito";
                                        break;
                                    case "05":
                                        docName = "NotaDebito";
                                        break;
                                    case "06":
                                        docName = "GuiaRemision";
                                        break;
                                    default:
                                        docName = "CompRetencion";
                                        break;
                                }

                                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("ReprocesoDoc-" + "-No hay Documentos si procesar: " + docName);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
        }
    }
}
