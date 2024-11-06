using System;
using System.ServiceProcess;
using System.Timers;
using ViaDoc.Configuraciones;

namespace NotificacionesViaDoc.WinServ
{
    public partial class Service1 : ServiceBase
    {

        public Service1()
        {
            InitializeComponent();
        }

        Timer timer;

        bool CicloCorreo = false;

        protected override void OnStart(string[] args)
        {
            try
            {
                int ProcesoNormal = int.Parse(CatalogoViaDoc.TiempoProceso);
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
            if (!CicloCorreo)
            {
                CicloCorreo = true;
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
                CicloCorreo = false;
            }
        }

        protected override void OnStop()
        {
        }
    }
}
