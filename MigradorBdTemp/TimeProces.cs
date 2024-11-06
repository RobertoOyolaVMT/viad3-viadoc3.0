using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViaDoc.AccesoDatos;
using ViaDocMigrador.LogicaNegocios.PorocesoDocumentos;

namespace MigradorBdTemp
{
    public class TimeProces
    {
        DocumentoAD _metodosDocumentos = new DocumentoAD();
        string rutaXml = ConfigurationManager.AppSettings.Get("pathPrincipal").Trim();

        public void StarServ()
        {
            if (!System.IO.File.Exists(@rutaXml + "\\" + "HoraServMigracionBdTemp.txt"))
            {
                TextWriter tw = new StreamWriter(@rutaXml + "\\HoraServMigracionBdTemp.txt", true);
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

                        //Timer_Elapsed_Liquiacion();

                        //Timer_Elapsed_CompRetencion();

                        //Timer_Elapsed_NotaCredito_Debito_GuiaRemmision();
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
            try
            {
                MigraFactura MFactura = new MigraFactura();
                MFactura.Factura();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
            }
            

        }
    }
}
