using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using ViaDoc.AccesoDatos;
using ViaDoc.Configuraciones;
using ViaDocFirma.LogicaNegocios;

namespace FirmaViadoc.WinServ
{
    public partial class Service1 : ServiceBase
    {
        

        public Service1()
        {
            InitializeComponent();
        }

        Timer timer;

        protected override void OnStart(string[] args)
        {
            try
            {
                int ProcesoNormal = int.Parse(ConfigurationManager.AppSettings["ProcesoNormal"]);
                timer = new Timer(ProcesoNormal);
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Timer_Elapsed_Ciclo);
                timer.Start();
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: " + ex.Message);

                try
                {
                    OnStop();
                }
                catch (Exception e)
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Excpetion: " + e.Message);
                }
            }
        }

        private void Timer_Elapsed_Ciclo(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeProces TM = new TimeProces();
            try
            {
                TM.StarServ();
            }
            catch (Exception ex)
            {

                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Factura Cathc: " + ex.ToString());
            }
            finally
            {
                timer.Start();
            }
        }
        protected override void OnStop()
        {
        }
    }
}
