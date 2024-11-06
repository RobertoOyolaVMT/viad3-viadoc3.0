using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MigradorBdTemp
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;

        public Service1()
        {
            InitializeComponent();
        }

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
            }
        }

        protected override void OnStop()
        {
        }

        public void Timer_Elapsed_Ciclo(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }
    }
}
