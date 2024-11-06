using Negocios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ViaDoc.EntidadNegocios.compRetencion;
using ViaDoc.EntidadNegocios.factura;
using ViaDoc.EntidadNegocios.guiaRemision;
using ViaDoc.EntidadNegocios.Liquidacion;
using ViaDoc.EntidadNegocios.notaCredito;
using ViaDoc.EntidadNegocios.notaDebito;
using ViaDoc.EntidadNegocios.portalWeb;
using ViaDoc.ServicioWcf.modelo;

namespace ViaDoc.ServicioWcf
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IService1
    {

        MetodosWcf metodosWcf = new MetodosWcf();
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public FacturaResponse Factura(List<Factura> _request)
        {
            return metodosWcf.ProcesarFacturas(_request);
        }

        public CompRetencionResponse CompRetencion(List<CompRetencion> _request)
        {
            return metodosWcf.ProcesarCompRetencion(_request);
        }


        public NotaCreditoResponse NotaCredito(List<NotaCredito> _request)
        {
            return metodosWcf.ProcesarNotaCredito(_request);
        }

        public NotaDebitoResponse NotaDebito(List<NotaDebito> _request)
        {
            return metodosWcf.ProcesarNotaDebito(_request);
        }


        public GuiaRemisionResponse GuiaRemision(List<GuiaRemision> _request)
        {
            return metodosWcf.ProcesaGuiaRemision(_request);
        }

        public LiquidacionResponse Liquidacion(List<Liquidacion> _request)
        {
            return metodosWcf.ProcesaLiquisacion(_request);
        }

        public FacturaResponse Prueba(Valor valor)
        {
            //Utilitarios.logs.LogsFactura.LogsInicioFin("************PRUEBASSS************");
            FacturaResponse response = new FacturaResponse();
            try
            {
                response.codigoRetorno = 0;
                response.mensajeRetorno = "Nombre: " + valor.nombre + " VALOR: " + valor.valor;
            }
            catch (Exception ex)
            {
                response.codigoRetorno = 9999;
                response.mensajeRetorno = ex.Message;
            }
            return response;
        }

        public RideComprobanteElectronico ConsultaJsonRideComprobanteElectronico(string tipoDocumento, string claveAcceso)
        {
            RideComprobanteElectronico objRide = new RideComprobanteElectronico();
            objRide.ConsultaRideJSON(claveAcceso, tipoDocumento);
            return objRide;
        }

        public ViaDoc.EntidadNegocios.Retorno EliminarDocumento(string tipoDocumento, string idCompania, string establecimiento, string puntoEmision, string secuencial)
        {
            RideComprobanteElectronico objRide = new RideComprobanteElectronico();
            return objRide.ElimarDocumentosElectronicos(tipoDocumento, idCompania, establecimiento, puntoEmision, secuencial);


        }

        public List<Autorizar> ConsultaDocAutorizado(Documento documento, ref int codigoRetorno, ref string mensajeRetorno)
        {
            RideComprobanteElectronico objRide = new RideComprobanteElectronico();
            return objRide.ConsultaDocAutorizado(documento, ref codigoRetorno, ref mensajeRetorno);

        }
    }

    [DataContract]
    public class Valor
    {
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string valor { get; set; }
    }
}

public class ConsultaRide
{
    public string tipoDocumento { get; set; }
    public string claveAcceso { get; set; }
}
