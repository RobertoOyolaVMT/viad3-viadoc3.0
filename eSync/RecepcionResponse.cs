using eSync.ServicioSRI;
using System;
using System.Collections.Generic;
using System.Xml;

namespace eSync
{
    public class RecepcionResponse
    {
        public String Estado { get; set; }
        public List<Comprobante> Comprobantes { get; private set; }
        public String RespuestaSoap { get; private set; }
        public Boolean TieneExcepcion { get; set; }
        public Exception Excepcion
        {
            get; set;
        }

        public Boolean Recibido
        {
            get
            {
                return Estado.Equals("RECIBIDA");
            }
            set{

            }
        }

        public RecepcionResponse()
        {
            Estado = "";
            Comprobantes = null;
            RespuestaSoap = "";
            TieneExcepcion = false;
            Excepcion = null;
        }

        public void ProcesarRespuesta(String vRespuestaSoap)
        {
            XmlDocument Respuesta = new XmlDocument();
            XmlNode nodo = null;
            XmlNode xmlComprobante = null;
            XmlNode xmlMensaje = null;
            XmlNodeList xmlComprobantes = null;
            XmlNodeList xmlMensajes = null;
            Comprobante oComprobante = null;
            Mensaje oMensaje = null;

            RespuestaSoap = vRespuestaSoap;

            Respuesta.LoadXml(RespuestaSoap);
            nodo = Respuesta.SelectSingleNode("//estado/node()");
            if (nodo != null) Estado = nodo.Value;

            xmlComprobantes = Respuesta.SelectNodes("//comprobante");

            if (xmlComprobantes.Count > 0)
            {
                Comprobantes = new List<Comprobante>();

                foreach (XmlNode _xmlComprobante in xmlComprobantes)
                {
                    xmlComprobante = _xmlComprobante.Clone();
                    oComprobante = new Comprobante();

                    nodo = xmlComprobante.SelectSingleNode("//claveAcceso/node()");
                    if (nodo != null) oComprobante.ClaveAcceso = nodo.Value;

                    xmlMensajes = xmlComprobante.SelectNodes("//mensajes/mensaje");

                    if (xmlMensajes.Count > 0)
                    {
                        oComprobante.Mensajes = new List<Mensaje>();

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

                            oComprobante.Mensajes.Add(oMensaje);
                        }
                    }
                    Comprobantes.Add(oComprobante);
                }
            }
        }
        public void ProcesarRespuestaObjeto(RespuestaSRI respuesta)
        {
            Comprobante oComprobante = new Comprobante();
            Comprobantes = new List<Comprobante>();
            if (respuesta.Estado != null)
            {

                Estado = respuesta.Estado;

                if (respuesta.Estado == "ERROR SRI")
                {

                    TieneExcepcion = true;
                    Excepcion = new ArgumentException(respuesta.ErrorMensaje);
                }

                Mensaje oMensaje = new Mensaje();
                oComprobante.Mensajes = new List<Mensaje>();
                if (respuesta.ErrorMensaje != null)
                {


                    oMensaje.Identificador = respuesta.ErrorIdentificador;
                    oMensaje.MensajeRespuesta = respuesta.ErrorMensaje;
                    oMensaje.InformacionAdicional = respuesta.ErrorInfoAdicional;
                    oMensaje.Tipo = respuesta.ErrorTipo;
                }
                oComprobante.Mensajes.Add(oMensaje);
                Comprobantes.Add(oComprobante);
            }

        }
    }
}
