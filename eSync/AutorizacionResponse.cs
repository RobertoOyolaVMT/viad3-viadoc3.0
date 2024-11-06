using eSync.ServicioSRI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace eSync
{
    public class AutorizacionResponse
    {
        public String ClaveAccesoConsultada { get; private set; }
        public String NumeroComprobantes { get; private set; }
        public List<Autorizacion> Autorizaciones { get; private set; }
        public String RespuestaSoap { get; private set; }
        public Boolean TieneExcepcion { get; set; }
        public Exception Excepcion { get; set; }

        public Autorizacion Autorizado
        {
            get
            {
                try
                {
                    if (Autorizaciones != null && Autorizaciones.Count > 0)
                    {
                        //IEnumerable<Autorizacion> result = Autorizaciones.Where(x => x.Estado == "AUTORIZADO");
                        IEnumerable<Autorizacion> result = Autorizaciones;
                        foreach (Autorizacion aut in result) { return aut; }
                        return null;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public AutorizacionResponse()
        {
            ClaveAccesoConsultada = "";
            NumeroComprobantes = "";
            Autorizaciones = null;
            RespuestaSoap = "";
            TieneExcepcion = false;
            Excepcion = null;

        }

        public void ProcesarRespuesta(String vRespuestaSoap)
        {
            XmlDocument Respuesta = new XmlDocument();
            XmlNode nodo = null;
            XmlNode xmlAutorizacion = null;
            XmlNode xmlMensaje = null;
            XmlNodeList xmlAutorizaciones = null;
            XmlNodeList xmlMensajes = null;
            Autorizacion oAutorizacion = null;
            Mensaje oMensaje = null;

            RespuestaSoap = vRespuestaSoap;
            Respuesta.LoadXml(RespuestaSoap);

            nodo = Respuesta.SelectSingleNode("//claveAccesoConsultada/node()");
            if (nodo != null) ClaveAccesoConsultada = nodo.Value;

            nodo = Respuesta.SelectSingleNode("//numeroComprobantes/node()");
            if (nodo != null) NumeroComprobantes = nodo.Value;

            xmlAutorizaciones = Respuesta.SelectNodes("//autorizacion");

            if (xmlAutorizaciones.Count > 0)
            {
                Autorizaciones = new List<Autorizacion>();

                foreach (XmlNode _xmlAutorizacion in xmlAutorizaciones)
                {
                    xmlAutorizacion = _xmlAutorizacion.Clone();
                    oAutorizacion = new Autorizacion();

                    nodo = xmlAutorizacion.SelectSingleNode("//estado/node()");
                    if (nodo != null) oAutorizacion.Estado = nodo.Value;

                    nodo = xmlAutorizacion.SelectSingleNode("//numeroAutorizacion/node()");
                    if (nodo != null) oAutorizacion.NumeroAutorizacion = nodo.Value;

                    nodo = xmlAutorizacion.SelectSingleNode("//fechaAutorizacion/node()");
                    if (nodo != null) oAutorizacion.FechaAutorizacion = nodo.Value;

                    nodo = xmlAutorizacion.SelectSingleNode("//ambiente/node()");
                    if (nodo != null) oAutorizacion.Ambiente = nodo.Value;

                    nodo = xmlAutorizacion.SelectSingleNode("//comprobante/node()");
                    if (nodo != null) oAutorizacion.Comprobante = nodo.Value;

                    xmlMensajes = xmlAutorizacion.SelectNodes("//mensajes/mensaje");

                    if (xmlMensajes.Count > 0)
                    {
                        oAutorizacion.Mensajes = new List<Mensaje>();

                        foreach (XmlNode xmlMensajeTmp in xmlMensajes)
                        {
                            xmlMensaje = xmlMensajeTmp.Clone();
                            oMensaje = new Mensaje();

                            nodo = xmlMensaje.SelectSingleNode("//identificador/node()");
                            if (nodo != null) oMensaje.Identificador = nodo.Value;

                            nodo = xmlMensaje.SelectSingleNode("//mensaje/node()");
                            if (nodo != null) oMensaje.MensajeRespuesta = nodo.Value;

                            nodo = xmlMensaje.SelectSingleNode("//informacionAdicional/node()");
                            if (nodo != null) oMensaje.InformacionAdicional = nodo.Value;

                            nodo = xmlMensaje.SelectSingleNode("//tipo/node()");
                            if (nodo != null) oMensaje.Tipo = nodo.Value;

                            oAutorizacion.Mensajes.Add(oMensaje);
                        }
                    }
                    Autorizaciones.Add(oAutorizacion);
                }
            }
        }

        public void ProcesarRespuestaObjeto(RespuestaSRI respuesta)
        {
            Autorizacion oAutorizacion = new Autorizacion();
            Autorizaciones = new List<Autorizacion>();
            if (respuesta.Estado != null)
            {
                oAutorizacion.Estado = respuesta.Estado;
                oAutorizacion.Mensajes = new List<Mensaje>();

                if (respuesta.Estado == "ERROR SRI")
                {
                    TieneExcepcion = true;
                    Excepcion = new ArgumentException(respuesta.ErrorMensaje);
                }
                oAutorizacion.NumeroAutorizacion = respuesta.NumeroAutorizacion;
                oAutorizacion.FechaAutorizacion = respuesta.FechaAutorizacion;
                oAutorizacion.Ambiente = respuesta.Ambiente;
                oAutorizacion.Comprobante = respuesta.SxmlRespuesta;
                Mensaje oMensaje = new Mensaje();
                oAutorizacion.Mensajes = new List<Mensaje>();
                if (respuesta.ErrorMensaje!=null)
                {
                    oMensaje.Identificador = respuesta.ErrorIdentificador;
                    oMensaje.MensajeRespuesta = respuesta.ErrorMensaje;
                    oMensaje.InformacionAdicional = respuesta.ErrorInfoAdicional;
                    oMensaje.Tipo = respuesta.ErrorTipo;
                }
                oAutorizacion.Mensajes.Add(oMensaje);
                Autorizaciones.Add(oAutorizacion);
            }
        }       
    }
}
