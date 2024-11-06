using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using ViaDoc.AccesoDatos;
using ViaDoc.Configuraciones;
using ViaDoc.EntidadNegocios;
using ViaDocAutorizacion.LogicaNegocios.procesos;
using wsUrlSRI = eSync.ServicioSRI.WSHelper;
using eSync;
using eSync.ServicioSRI;

namespace ViaDocAutorizacion.LogicaNegocios
{
    public class ProcesoAutorizacionRecepcion
    {
        ProcesoDocumentos _procesoDocumentos = new ProcesoDocumentos();

        public void ProcesoRecepcionAutorizacion(Compania compania, string CodDoc, string TipoDocumento, string recepcionDB, string autorizacionDB,
                                                 string claveAcceso, ref int codigoRetorno, ref string descripcionRetorno)
        {
            int codError = 0;
            string MjError = "";
            int contInicial;
            int milliseconds = int.Parse(CatalogoViaDoc.TiempoProceso);
            List<XmlGenerados> xmlComprobantes = new List<XmlGenerados>();
            try
            {
                List<DocumentoError> listaDocumentoError = new List<DocumentoError>();

                xmlComprobantes = _procesoDocumentos.ConsultaComprobantesPorEstado(compania.CiCompania, CodDoc, CatalogoViaDoc.DocEstadoFirmado,
                    claveAcceso, ref codigoRetorno, ref descripcionRetorno);

                if (xmlComprobantes.Count > 0)
                {
                    MjError += "<br/><b>ERRORES EN EL PROCESO DE RECEPCION:</b><br/><table style=\"width:70%\">";
                    contInicial = 0;
                    contInicial = xmlComprobantes.Count(i => i.XmlEstado.Trim() == CatalogoViaDoc.DocEstadoFirmado);
                    contInicial += xmlComprobantes.Count(i => i.XmlEstado.Trim() == CatalogoViaDoc.DocEstadoRecibido);
                    foreach (XmlGenerados itemComprobantes in xmlComprobantes)
                    {
                        string mensajeError = "";
                        bool bandDocError = false;

                        RecepcionResponse recepcion = new RecepcionResponse();
                        try
                        {
                            itemComprobantes.CiContingenciaDet = 3;
                            itemComprobantes.ciNumeroIntento++;
                            if (String.Compare(itemComprobantes.XmlEstado.Trim(), CatalogoViaDoc.DocEstadoFirmado) == 0)
                            {
                                Sync.DocumentoXml = itemComprobantes.XmlComprobante;
                                Sync.ReadWriteTimeout = -1;
                                Sync.Timeout = -1;
                                recepcion = RecepcionComprobantesXmlSRIActualizado(itemComprobantes.XmlComprobante, recepcionDB.Trim());

                                if (recepcion.TieneExcepcion == true)
                                {
                                    if ((recepcion.Excepcion.Message.Contains("tiempo") == false))
                                    {
                                        if ((recepcion.Excepcion.Message.Contains("timed") == false))
                                        {
                                            bandDocError = true;
                                            itemComprobantes.MensajeError = recepcion.Excepcion.Message;
                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoFirmado;
                                            codError = 1;
                                        }
                                        else
                                        {
                                            bandDocError = true;
                                            itemComprobantes.MensajeError = recepcion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo, si el motivo fue por timeOut del SRI";
                                            codError = 1;
                                        }
                                    }
                                    else
                                    {
                                        bandDocError = true;
                                        itemComprobantes.MensajeError = recepcion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo, si el motivo fue por timeOut del SRI";
                                        codError = 1;
                                    }
                                }
                                else
                                {
                                    if (recepcion.Recibido == true)
                                    {
                                        itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                        try
                                        {
                                            XmlGenerados docAutorizado = ProcesoAutorizacionDocumentoActualizado(itemComprobantes, ref bandDocError, ref mensajeError, ref MjError, milliseconds, autorizacionDB, 1);
                                            if (itemComprobantes.XmlEstado.Trim() == CatalogoViaDoc.DocEstadoRecibido)
                                            {
                                                Sync.DocumentoXml = itemComprobantes.XmlComprobante; // ' ASIGNO EL DOCUMENTO XML
                                                Sync.ReadWriteTimeout = -1;
                                                Sync.Timeout = -1;
                                                AutorizacionResponse autorizacion = AutorizacionComprobantesXmlSRIActualizado(itemComprobantes.ClaveAcceso, autorizacionDB);

                                                if (autorizacion.TieneExcepcion == true)
                                                {
                                                    if ((autorizacion.Excepcion.Message.Contains("tiempo") == false))
                                                    {
                                                        if ((autorizacion.Excepcion.Message.Contains("timed") == false))
                                                        {
                                                            bandDocError = true;
                                                            itemComprobantes.MensajeError = autorizacion.Excepcion.Message;
                                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoEAutorizado;
                                                            codError = 1;
                                                        }
                                                        else
                                                        {
                                                            bandDocError = true;
                                                            itemComprobantes.MensajeError = autorizacion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo si el motivo fue por timeOut con el SRI";
                                                            codError = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bandDocError = true;
                                                        itemComprobantes.MensajeError = autorizacion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo si el motivo fue por timeOut con el SRI";
                                                        codError = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (autorizacion.Autorizaciones != null)
                                                    {
                                                        if (autorizacion.Autorizado != null)
                                                        {
                                                            if (autorizacion.Autorizado.Estado.Trim() == "AUTORIZADO".Trim())
                                                            {
                                                                itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoAutorizado;
                                                                itemComprobantes.XmlComprobante = autorizacion.Autorizado.Comprobante;
                                                                itemComprobantes.TxNumeroAutorizacion = autorizacion.Autorizado.NumeroAutorizacion.Trim();
                                                                itemComprobantes.txFechaHoraAutorizacion = autorizacion.Autorizado.FechaAutorizacion.Trim();
                                                                codError = 0;
                                                            }
                                                            else if (autorizacion.Autorizado.Estado.Trim() == "EN PROCESAMIENTO".Trim())
                                                            {
                                                                itemComprobantes.CiContingenciaDet = 3;
                                                                itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                                                itemComprobantes.MensajeError = "Documento pendiente de autorizar por el SRI, se reenviará en el siguiente ciclo";
                                                                codError = 1;
                                                            }
                                                            else
                                                            {
                                                                if (autorizacion.Autorizado.Estado.Trim() == "NO AUTORIZADO".Trim())
                                                                {
                                                                    bandDocError = true;
                                                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoNoAutorizado;
                                                                    #region MENSAJE_NO_AUTORIZADO
                                                                    if (autorizacion.Autorizaciones != null)
                                                                    {
                                                                        string mensaje = "\n";
                                                                        foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                                        {
                                                                            mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                                            foreach (Mensaje item2 in item1.Mensajes)
                                                                            {
                                                                                itemComprobantes.txCodError = item2.Identificador;
                                                                                mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                                                mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                                                mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                                                mensaje += "TIPO: " + item2.Tipo + "\n";
                                                                            }
                                                                        }
                                                                        itemComprobantes.MensajeError = mensaje;
                                                                        codError = 1;
                                                                    }
                                                                    #endregion MENSAJE_NO_AUTORIZADO
                                                                }
                                                                else
                                                                {
                                                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRAutorizado;
                                                                    #region MENSAJE_NO_AUTORIZADO
                                                                    if (autorizacion.Autorizaciones != null)
                                                                    {
                                                                        bandDocError = true;
                                                                        string mensaje = "\n";
                                                                        foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                                        {
                                                                            mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                                            foreach (Mensaje item2 in item1.Mensajes)
                                                                            {
                                                                                itemComprobantes.txCodError = item2.Identificador;
                                                                                mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                                                mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                                                mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                                                mensaje += "TIPO: " + item2.Tipo + "\n";
                                                                            }
                                                                        }
                                                                        itemComprobantes.MensajeError = mensaje;
                                                                        codError = 1;
                                                                    }
                                                                    #endregion MENSAJE_NO_AUTORIZADO
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            bandDocError = true;
                                                            itemComprobantes.XmlComprobante = autorizacion.Autorizado.Comprobante;
                                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRAutorizado;
                                                            #region MENSAJE_EXCEPCION_AU

                                                            if (autorizacion.Autorizaciones != null)
                                                            {
                                                                string mensaje = "\n";
                                                                foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                                {
                                                                    mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                                    foreach (Mensaje item2 in item1.Mensajes)
                                                                    {
                                                                        itemComprobantes.txCodError = item2.Identificador;
                                                                        mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                                        mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                                        mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                                        mensaje += "TIPO: " + item2.Tipo + "\n";
                                                                    }
                                                                }
                                                                itemComprobantes.MensajeError = mensaje;

                                                                codError = 1;
                                                                mensajeError = itemComprobantes.MensajeError;
                                                            }
                                                            #endregion MENSAJE_EXCEPCION_AU
                                                        }
                                                    }
                                                    else
                                                    {
                                                        itemComprobantes.MensajeError = "En la Autorizacion: Debido a que no hay respuesta del comprobante de parte del SRI, se volvera a enviar al proceso de Recepcion, si el problema persiste nuevamente en la autorizacion cambie su estado y revise los datos del comprobante";
                                                        itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoFirmado;
                                                    }
                                                    itemComprobantes.CiContingenciaDet = 3;
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception ex...." + ex.Message);

                                            bandDocError = false;
                                            if (ex.Message.Trim().Contains("objeto") == false)
                                            {
                                                if (ex.Message.Trim().Contains("object") == false)
                                                {
                                                    itemComprobantes.MensajeError = ex.Message;
                                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoEAutorizado;
                                                    codError = 1;
                                                }
                                                else
                                                {
                                                    itemComprobantes.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                                    codError = 1;
                                                }
                                            }
                                            else
                                            {
                                                itemComprobantes.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                                                itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                                codError = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (recepcion.Estado == "DEVUELTA")
                                        {
                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoERecibido;

                                            string mensaje = "DEVUELTA: \n";
                                            foreach (var item1 in recepcion.Comprobantes)
                                            {
                                                foreach (Mensaje item2 in item1.Mensajes)
                                                {
                                                    itemComprobantes.txCodError = item2.Identificador;
                                                    mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                    mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                    mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                    mensaje += "TIPO: " + item2.Tipo + "\n";
                                                }
                                            }
                                            codError = 1;
                                            itemComprobantes.MensajeError = mensaje;
                                        }
                                        else
                                        {
                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoERecibido;
                                            string mensaje = recepcion.Estado + " \n";
                                            if (recepcion.Comprobantes.Count > 0)
                                            {
                                                foreach (var item1 in recepcion.Comprobantes)
                                                {
                                                    foreach (Mensaje item2 in item1.Mensajes)
                                                    {
                                                        itemComprobantes.txCodError = item2.Identificador;
                                                        mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                        mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                        mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                        mensaje += "TIPO: " + item2.Tipo + "\n";
                                                    }
                                                }
                                                codError = 1;
                                            }
                                            else
                                            {
                                                mensaje += "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado";
                                                codError = 1;
                                            }
                                            itemComprobantes.MensajeError = mensaje;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (itemComprobantes.XmlEstado.Trim().Equals(CatalogoViaDoc.DocEstadoRecibido.Trim()))
                                    {
                                        Sync.DocumentoXml = itemComprobantes.XmlComprobante; // ' ASIGNO EL DOCUMENTO XML
                                        Sync.ReadWriteTimeout = -1;
                                        Sync.Timeout = -1;
                                        AutorizacionResponse autorizacion = AutorizacionComprobantesXmlSRIActualizado(itemComprobantes.ClaveAcceso, autorizacionDB);

                                        if (autorizacion.TieneExcepcion == true)
                                        {
                                            if ((autorizacion.Excepcion.Message.Contains("tiempo") == false))
                                            {
                                                if ((autorizacion.Excepcion.Message.Contains("timed") == false))
                                                {
                                                    bandDocError = true;
                                                    itemComprobantes.MensajeError = autorizacion.Excepcion.Message;
                                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoEAutorizado;
                                                    codError = 1;
                                                }
                                                else
                                                {
                                                    bandDocError = true;
                                                    itemComprobantes.MensajeError = autorizacion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo si el motivo fue por timeOut con el SRI";
                                                    codError = 1;
                                                }
                                            }
                                            else
                                            {
                                                bandDocError = true;
                                                itemComprobantes.MensajeError = autorizacion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo si el motivo fue por timeOut con el SRI";
                                                codError = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (autorizacion.Autorizaciones != null)
                                            {
                                                if (autorizacion.Autorizado != null)
                                                {
                                                    if (autorizacion.Autorizado.Estado.Trim() == "AUTORIZADO".Trim())
                                                    {
                                                        itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoAutorizado;
                                                        itemComprobantes.XmlComprobante = autorizacion.Autorizado.Comprobante;
                                                        itemComprobantes.TxNumeroAutorizacion = autorizacion.Autorizado.NumeroAutorizacion.Trim();
                                                        itemComprobantes.txFechaHoraAutorizacion = autorizacion.Autorizado.FechaAutorizacion.Trim();
                                                        codError = 0;
                                                    }
                                                    else if (autorizacion.Autorizado.Estado.Trim() == "EN PROCESAMIENTO".Trim())
                                                    {
                                                        itemComprobantes.CiContingenciaDet = 3;
                                                        itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                                        itemComprobantes.MensajeError = "Documento pendiente de autorizar por el SRI, se reenviará en el siguiente ciclo";
                                                        codError = 1;
                                                    }
                                                    else
                                                    {
                                                        if (autorizacion.Autorizado.Estado.Trim() == "NO AUTORIZADO".Trim())
                                                        {
                                                            bandDocError = true;
                                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoNoAutorizado;
                                                            #region MENSAJE_NO_AUTORIZADO
                                                            if (autorizacion.Autorizaciones != null)
                                                            {
                                                                string mensaje = "\n";
                                                                foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                                {
                                                                    mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                                    foreach (Mensaje item2 in item1.Mensajes)
                                                                    {
                                                                        itemComprobantes.txCodError = item2.Identificador;
                                                                        mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                                        mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                                        mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                                        mensaje += "TIPO: " + item2.Tipo + "\n";
                                                                    }
                                                                }
                                                                itemComprobantes.MensajeError = mensaje;
                                                                codError = 1;
                                                            }
                                                            #endregion MENSAJE_NO_AUTORIZADO
                                                        }
                                                        else
                                                        {
                                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRAutorizado;
                                                            #region MENSAJE_NO_AUTORIZADO
                                                            if (autorizacion.Autorizaciones != null)
                                                            {
                                                                bandDocError = true;
                                                                string mensaje = "\n";
                                                                foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                                {
                                                                    mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                                    foreach (Mensaje item2 in item1.Mensajes)
                                                                    {
                                                                        itemComprobantes.txCodError = item2.Identificador;
                                                                        mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                                        mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                                        mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                                        mensaje += "TIPO: " + item2.Tipo + "\n";
                                                                    }
                                                                }
                                                                itemComprobantes.MensajeError = mensaje;
                                                                codError = 1;
                                                            }
                                                            #endregion MENSAJE_NO_AUTORIZADO
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    bandDocError = true;
                                                    //itemComprobantes.XmlComprobante = autorizacion.Autorizado.Comprobante;
                                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRAutorizado;
                                                    #region MENSAJE_EXCEPCION_AU

                                                    if (autorizacion.Autorizaciones != null)
                                                    {
                                                        string mensaje = "\n";
                                                        foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                        {
                                                            mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                            foreach (Mensaje item2 in item1.Mensajes)
                                                            {
                                                                itemComprobantes.txCodError = item2.Identificador;
                                                                mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                                mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                                mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                                mensaje += "TIPO: " + item2.Tipo + "\n";
                                                            }
                                                        }
                                                        itemComprobantes.MensajeError = mensaje;

                                                        codError = 1;
                                                        mensajeError = itemComprobantes.MensajeError;
                                                    }
                                                    #endregion MENSAJE_EXCEPCION_AU
                                                }

                                            }
                                            else
                                            {
                                                itemComprobantes.MensajeError = "En la Autorizacion: Debido a que no hay respuesta del comprobante de parte del SRI, se volvera a enviar al proceso de Recepcion, si el problema persiste nuevamente en la autorizacion cambie su estado y revise los datos del comprobante";
                                                itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoFirmado;
                                            }
                                            itemComprobantes.CiContingenciaDet = 3;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Exception ex...." + ex.Message);
                                    bandDocError = false;
                                    if (ex.Message.Trim().Contains("objeto") == false)
                                    {
                                        if (ex.Message.Trim().Contains("object") == false)
                                        {
                                            itemComprobantes.MensajeError = ex.Message;
                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoEAutorizado;
                                            codError = 1;
                                        }
                                        else
                                        {
                                            itemComprobantes.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                                            itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                            codError = 1;
                                        }
                                    }
                                    else
                                    {
                                        itemComprobantes.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                                        itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                        codError = 1;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Trim().Contains("objeto") == false)
                            {
                                if (ex.Message.Trim().Contains("objeto") == false)
                                {
                                    itemComprobantes.MensajeError = ex.Message;
                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoERecibido;
                                    codError = 1;
                                }
                                else
                                {
                                    itemComprobantes.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                                    itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoFirmado;
                                    codError = 1;
                                }
                            }
                            else
                            {
                                itemComprobantes.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                                itemComprobantes.XmlEstado = CatalogoViaDoc.DocEstadoFirmado;
                                codError = 1;
                            }
                            ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message);
                        }

                        ActualizarXmlComprobantes(itemComprobantes); 
                        if (!claveAcceso.Equals(""))
                        {
                            codigoRetorno = codError;
                            descripcionRetorno = itemComprobantes.MensajeError;
                        }
                    }
                    xmlComprobantes.Clear();
                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("ProcesoRecepcionAutorizacion:" + ex.Message + "----" + ex.StackTrace);
                descripcionRetorno = "ProcesoRecepcionAutorizacion:" + ex.Message + "----" + ex.StackTrace;
                codigoRetorno = 9999;
            }
        }


        protected XmlGenerados ProcesoAutorizacionDocumentoActualizado(XmlGenerados itemDocumento, ref bool bandDocError, ref string mensajeError, ref string MjError, int milliseconds, string urlWsSRI_autorizacion, int cantidadIntentoAutorizacion)
        {
            try
            {
                if (itemDocumento.XmlEstado.Trim() == CatalogoViaDoc.DocEstadoRecibido)
                {
                    #region ENVIO_AUTORIZACION
                    Sync.DocumentoXml = itemDocumento.XmlComprobante; //ASIGNO EL DOCUMENTO XML
                    Sync.ReadWriteTimeout = -1;
                    Sync.Timeout = -1;
                    AutorizacionResponse autorizacion = AutorizacionComprobantesXmlSRIActualizado(itemDocumento.ClaveAcceso, urlWsSRI_autorizacion);

                    Thread.Sleep(milliseconds);
                    #endregion ENVIO_AUTORIZACION

                    #region VALÍDO SI EL DOCUMENTO HA SIDO AUTORIZADO
                    if (autorizacion.TieneExcepcion == true)
                    {
                        #region Error de timeOut del SRI
                        if ((autorizacion.Excepcion.Message.Contains("tiempo") == false))
                        {
                            if ((autorizacion.Excepcion.Message.Contains("timed") == false))
                            {
                                bandDocError = true;
                                itemDocumento.MensajeError = autorizacion.Excepcion.Message;
                                itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                            }
                            else
                            {
                                bandDocError = true;
                                itemDocumento.MensajeError = autorizacion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo si el motivo fue por timeOut con el SRI";
                            }
                        }
                        else
                        {
                            bandDocError = true;
                            itemDocumento.MensajeError = autorizacion.Excepcion.Message + "\nNota: Este comprobante se reenviara en el siguiente ciclo si el motivo fue por timeOut con el SRI";
                        }
                        #endregion
                    }
                    else
                    {
                        #region Autorizacion exitosa del documento 
                        if (autorizacion.Autorizaciones != null)
                        {
                            if (autorizacion.Autorizado != null)
                            {
                                #region No hubo un timeOut del SRI pero verifica si se autorizo el documento
                                if (autorizacion.Autorizado.Estado.Trim() == "AUTORIZADO".Trim())
                                {
                                    itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoAutorizado;
                                    // itemDocumento.XmlComprobante = ReportesFacturacion.RetornaXml.RetornaComprobanteAutorizado(autorizacion.RespuestaSoap);
                                    itemDocumento.XmlComprobante = autorizacion.Autorizado.Comprobante;
                                    itemDocumento.TxNumeroAutorizacion = autorizacion.Autorizado.NumeroAutorizacion.Trim();
                                    itemDocumento.txFechaHoraAutorizacion = autorizacion.Autorizado.FechaAutorizacion.Trim();
                                }
                                else
                                {
                                    #region Verifica por que no se autorizo el documento en el SRI
                                    if (autorizacion.Autorizado.Estado.Trim() == "EN PROCESAMIENTO".Trim())
                                    {
                                        #region Documento en proceso, el xml se encuentra autorizado en sri pero el xml no se pudo actualizar
                                        itemDocumento.CiContingenciaDet = 3;
                                        itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                        itemDocumento.MensajeError = "Documento pendiente de autorizar por el SRI, se reenviará en el siguiente ciclo";
                                        #endregion
                                    }
                                    if (autorizacion.Autorizado.Estado.Trim() == "EN PROCESO".Trim())
                                    {
                                        #region Documento en proceso, el xml se encuentra autorizado en sri pero el xml no se pudo actualizar
                                        itemDocumento.CiContingenciaDet = 3;
                                        itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                        itemDocumento.MensajeError = "Documento pendiente de autorizar por el SRI, se reenviará en el siguiente ciclo";
                                        #endregion
                                    }
                                    else
                                    {
                                        if (autorizacion.Autorizado.Estado.Trim() == "NO AUTORIZADO".Trim())
                                        {
                                            #region Documento no Autorizado por el SRI (NAUTORIZADO)
                                            if (cantidadIntentoAutorizacion > 0)
                                            {
                                                for (int i = 0; i < cantidadIntentoAutorizacion; i++)
                                                {
                                                    #region ENVIO_AUTORIZACION
                                                    Sync.DocumentoXml = itemDocumento.XmlComprobante;
                                                    Sync.ReadWriteTimeout = -1;
                                                    Sync.Timeout = -1;
                                                    Thread.Sleep(milliseconds);
                                                    AutorizacionResponse reAutorizacion = AutorizacionComprobantesXmlSRIActualizado(itemDocumento.ClaveAcceso, urlWsSRI_autorizacion);

                                                    #endregion ENVIO_AUTORIZACION

                                                    if (reAutorizacion.Autorizado.Estado.Trim() == "NO AUTORIZADO".Trim())
                                                    {
                                                        bandDocError = true;
                                                        itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoNoAutorizado;
                                                        #region MENSAJE_NO_AUTORIZADO
                                                        if (reAutorizacion.Autorizaciones != null)
                                                        {
                                                            string mensaje = "\n";
                                                            foreach (Autorizacion item1 in reAutorizacion.Autorizaciones)
                                                            {
                                                                mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                                foreach (Mensaje item2 in item1.Mensajes)
                                                                {
                                                                    itemDocumento.txCodError = item2.Identificador;
                                                                    mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                                    mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                                    mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                                    mensaje += "TIPO: " + item2.Tipo + "\n";
                                                                }
                                                            }

                                                            itemDocumento.MensajeError = mensaje;
                                                        }
                                                        #endregion MENSAJE_NO_AUTORIZADO
                                                    }
                                                    else
                                                    {
                                                        if (reAutorizacion.Autorizado.Estado.Trim() == "EN PROCESO".Trim())
                                                        {
                                                            #region Documento en proceso, el xml se encuentra autorizado en sri pero el xml no se pudo actualizar
                                                            itemDocumento.CiContingenciaDet = 3;
                                                            itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                                            itemDocumento.MensajeError = "Documento pendiente de autorizar por el SRI, se reenviará en el siguiente ciclo";
                                                            #endregion
                                                        }
                                                        if (reAutorizacion.Autorizado.Estado.Trim() == "EN PROCESAMIENTO".Trim())
                                                        {
                                                            #region Documento en proceso, el xml se encuentra autorizado en sri pero el xml no se pudo actualizar
                                                            itemDocumento.CiContingenciaDet = 3;
                                                            itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                                            itemDocumento.MensajeError = "Documento pendiente de autorizar por el SRI, se reenviará en el siguiente ciclo";
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            if (reAutorizacion.Autorizado.Estado.Trim() == "AUTORIZADO".Trim())
                                                            {
                                                                itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoAutorizado;
                                                                itemDocumento.XmlComprobante = reAutorizacion.Autorizado.Comprobante;
                                                                itemDocumento.TxNumeroAutorizacion = reAutorizacion.Autorizado.NumeroAutorizacion.Trim();
                                                                itemDocumento.txFechaHoraAutorizacion = reAutorizacion.Autorizado.FechaAutorizacion.Trim();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bandDocError = true;
                                                itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoNoAutorizado;
                                                #region MENSAJE_NO_AUTORIZADO
                                                if (autorizacion.Autorizaciones != null)
                                                {
                                                    string mensaje = "\n";
                                                    foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                    {
                                                        mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                        foreach (Mensaje item2 in item1.Mensajes)
                                                        {
                                                            itemDocumento.txCodError = item2.Identificador;
                                                            mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                            mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                            mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                            mensaje += "TIPO: " + item2.Tipo + "\n";
                                                        }
                                                    }

                                                    itemDocumento.MensajeError = mensaje;
                                                }
                                                #endregion MENSAJE_NO_AUTORIZADO
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Documento rechazado por el SRI - RAUTORIZADO
                                            itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRAutorizado;
                                            #region MENSAJE_NO_AUTORIZADO
                                            if (autorizacion.Autorizaciones != null)
                                            {
                                                bandDocError = true;
                                                string mensaje = "\n";
                                                foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                                {
                                                    mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                                    foreach (Mensaje item2 in item1.Mensajes)
                                                    {
                                                        itemDocumento.txCodError = item2.Identificador;
                                                        mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                                        mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                                        mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                                        mensaje += "TIPO: " + item2.Tipo + "\n";
                                                    }
                                                }

                                                itemDocumento.MensajeError = mensaje;
                                            }
                                            #endregion MENSAJE_NO_AUTORIZADO
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                #endregion No hubo un timeOut del SRI pero verifica si se autorizo el documento
                            }
                            else
                            {
                                bandDocError = true;
                                if (autorizacion.Autorizado != null)
                                {
                                    itemDocumento.XmlComprobante = autorizacion.Autorizado.Comprobante;
                                    itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoNoAutorizado;
                                }
                                else
                                {
                                    itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                                }

                                #region MENSAJE_EXCEPCION_AU
                                if (autorizacion.Autorizaciones != null)
                                {
                                    string mensaje = "\n";
                                    foreach (Autorizacion item1 in autorizacion.Autorizaciones)
                                    {
                                        mensaje += "\nESTADO DEL DOCUMENTO: " + item1.Estado + "\n";
                                        foreach (Mensaje item2 in item1.Mensajes)
                                        {
                                            itemDocumento.txCodError = item2.Identificador;
                                            mensaje += "IDENTIFICADOR: " + item2.Identificador + "\n";
                                            mensaje += "MENSAJE DE RESPUESTA: " + item2.MensajeRespuesta + "\n";
                                            mensaje += "INFORMACION ADICIONAL: " + item2.InformacionAdicional + "\n";
                                            mensaje += "TIPO: " + item2.Tipo + "\n";
                                        }
                                    }
                                    itemDocumento.MensajeError = mensaje;
                                }
                                #endregion MENSAJE_EXCEPCION_AU
                            }
                        }
                        else
                        {
                            itemDocumento.MensajeError = "En la Autorizacion: Debido a que no hay respuesta del del comprobante de parte del SRI, se volvera a enviar al proceso de Recepcion, si el problema persiste nuevamente en la autorizacion cambie su estado y revise los datos del comprobante";
                            itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoFirmado;
                        }
                        itemDocumento.CiContingenciaDet = 3;
                        #endregion
                    }
                    #endregion RESPUESTA_AUTORIZACION
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Autorizacion: " + ex.ToString());
                bandDocError = false;
                if (ex.Message.Trim().Contains("objeto") == false)
                {
                    //object
                    if (ex.Message.Trim().Contains("object") == false)
                    {
                        itemDocumento.MensajeError = ex.Message;
                        itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoEAutorizado;
                    }
                    else
                    {
                        itemDocumento.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                        itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                    }
                }
                else
                {
                    itemDocumento.MensajeError = "El servicio del SRI ha demorado en responder, en breve se procede a enviar el documento para ser procesado: " + ex.Message;
                    itemDocumento.XmlEstado = CatalogoViaDoc.DocEstadoRecibido;
                }
            }
            XmlGenerados documentoAutorizado = itemDocumento;
            return documentoAutorizado;
        }

        protected AutorizacionResponse AutorizacionComprobantesXmlSRIActualizado(string claveAcceso, string UrlAutorizacion)
        {
            AutorizacionResponse Autorizacion = new AutorizacionResponse();
            ConfigHelper conf = new ConfigHelper();
            RespuestaSRI respuesta = new RespuestaSRI();
            Sync.UrlAutorizacion = UrlAutorizacion.Trim();
            //XmlDocument xmlDoc;
            List<Autorizacion> Autorizaciones = new List<Autorizacion>();
            if (!string.IsNullOrEmpty(claveAcceso))
            {
                conf.URL_Autorizacion = UrlAutorizacion;
                conf.ClaveAcceso = claveAcceso;
                string res = wsUrlSRI.ConfigInicial(conf);
                XmlDocument xmlAut;

                try
                {
                    respuesta = wsUrlSRI.AutorizacionComprobante(out xmlAut);
                    Autorizacion.ProcesarRespuestaObjeto(respuesta);
                }
                catch (Exception ex)
                {
                    Autorizacion.TieneExcepcion = true;
                    Autorizacion.Excepcion = ex;
                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("AutorizacionComprobantesXmlSRIActualizado" + ex.Message + "---" + ex.StackTrace);
                }
            }
            return Autorizacion;
        }

        protected RecepcionResponse RecepcionComprobantesXmlSRIActualizado(string sXmlDocumento, string UrlRecepcion)
        {
            RecepcionResponse Recepcion = new RecepcionResponse();
            ConfigHelper conf = new ConfigHelper();
            RespuestaSRI respuesta = new RespuestaSRI();
            //XmlDocument xmlDoc;

            if (!string.IsNullOrEmpty(sXmlDocumento))
            {
                conf.URL_Envio = UrlRecepcion;
                conf.ContenidoXML = sXmlDocumento;

                conf.RutaXML = "";
                string res = wsUrlSRI.ConfigInicial(conf);
                //XmlDocument xmlAut;
                try
                {
                    respuesta = wsUrlSRI.EnvioComprobante();
                    Recepcion.ProcesarRespuestaObjeto(respuesta);
                }
                catch (Exception ex)
                {
                    Recepcion.TieneExcepcion = true;
                    Recepcion.Excepcion = ex;

                    ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin(ex.Message + "---" + ex.StackTrace);
                }
            }
            return Recepcion;
        }

        public void ActualizarXmlComprobantes(XmlGenerados xmlComprobante)
        {
            int codigoRetorno = 0;
            string descripcionRetorno = string.Empty;
            try
            {
                DocumentoAD Actualiza = new DocumentoAD();

                Actualiza.ActualizarComprobantesProcesados(xmlComprobante.Identity, xmlComprobante.CiCompania, xmlComprobante.CiTipoDocumento,
                                                           xmlComprobante.ClaveAcceso, xmlComprobante.txFechaHoraAutorizacion, xmlComprobante.XmlComprobante,
                                                           xmlComprobante.XmlEstado, xmlComprobante.CiContingenciaDet.ToString(), xmlComprobante.ciNumeroIntento,
                                                           xmlComprobante.MensajeError, ref codigoRetorno, ref descripcionRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Error Autorizacion: " + ex.ToString());
            }
        }

        public void ResprocesoDocEncolado(ref int codigoRetorno, ref string mensajeRetorno)
        {
            ProcesoDocumentos _proceso = new ProcesoDocumentos();
            try
            {
                string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                 dynamic res = _proceso.ConsultaDocError("9999","","", fecha, fecha,"","1", ref codigoRetorno, ref mensajeRetorno);
            }
            catch (Exception ex)
            {
                ViaDoc.Utilitarios.logs.LogsFactura.LogsInicioFin("Se presentan proceso encolados: "+ex.Message);
            }
        }
    }
}
