using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace AutorizacionViaDoc.WinServ
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        bool band = false;
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
            if (band == false)
            {
                band = true;
                TimerProces TM = new TimerProces();
                try
                {
                    TM.StarServ();
                }
                catch (Exception ex)
                {
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Factura Cathc: " + ex.ToString());
                }
                band = false;
            }

        }

        protected override void OnStop()
        {

        }
    }
}
