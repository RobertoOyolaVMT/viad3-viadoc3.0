using NotificacionesViaDoc.WinServ;
using System;
using System.Windows.Forms;
using ViaDoc.Logs;
using ViaDoc.Logs.Entidades;
using ViaDoc.Utilitarios;
using ViaDocAutorizacion.LogicaNegocios;
using ViaDocEnvioCorreo.LogicaNegocios;

namespace ViaDoc.Probador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ProcesoEnvioCorreo _procesoEnvio = new ProcesoEnvioCorreo();
            MetodosWinServ enviocoreo = new MetodosWinServ();
            
            enviocoreo.EnvioDocumentosCorreoElectronico();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //int codigoRetorno = 0;
            //string descripcionRetorno = string.Empty;
            //ProcesoAutorizacionRecepcion _procesoAutorizacion = new ProcesoAutorizacionRecepcion();
            //_procesoAutorizacion.GenerarAutorizacionRecepcion(ref codigoRetorno, ref descripcionRetorno);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProcesoEnvioCorreo _procesoEnvio = new ProcesoEnvioCorreo();
            //_procesoEnvio.EnviarDocumentosPortalWeb();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ConexionBDMongo objLog = new ConexionBDMongo();

            ModelLogs objL = new ModelLogs();
            objL.Clase = "Clase";
            objL.CodigoError = "Codigo Error";
            objL.IdCompania = "idCompania";
            objL.Metodo = "Metodo";
            objL.MensajeError = "Mensaje De Error";
            objL.Fecha = DateTime.Today.AddDays(1).ToShortDateString();

            objL.Hora = "10:10";
            objL.Solucion = "Solucion";

            int codigo = 0;
            string descripcion = "";

            objLog.GuardarLogs("Logs",objL);
            //objLog.ConsultaLogs("Logs", "idCompania", "Solucion", DateTime.Today.AddDays(1).ToShortDateString(), ref codigo, ref descripcion);
        }
    }
}
