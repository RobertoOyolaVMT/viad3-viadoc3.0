using Negocios;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using ViaDoc.EntidadNegocios.compRetencion;
using ViaDoc.EntidadNegocios.factura;
using ViaDoc.EntidadNegocios.guiaRemision;
using ViaDoc.EntidadNegocios.Liquidacion;
using ViaDoc.EntidadNegocios.notaCredito;
using ViaDoc.EntidadNegocios.notaDebito;
using ViaDoc.ServicioWcf.modelo;

namespace ViaDoc.ServicioWcf
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        FacturaResponse Prueba(Valor valor);

        [OperationContract]
        FacturaResponse Factura(List<Factura> _request);

        [OperationContract]
        CompRetencionResponse CompRetencion(List<CompRetencion> _request);

        [OperationContract]
        NotaCreditoResponse NotaCredito(List<NotaCredito> _request);


        [OperationContract]
        NotaDebitoResponse NotaDebito(List<NotaDebito> _request);


        [OperationContract]
        GuiaRemisionResponse GuiaRemision(List<GuiaRemision> _request);

        [OperationContract]
        LiquidacionResponse Liquidacion(List<Liquidacion> _request);

        [OperationContract]
        RideComprobanteElectronico ConsultaJsonRideComprobanteElectronico(string tipoDocumento, string claveAcceso);

        [OperationContract]
        ViaDoc.EntidadNegocios.Retorno EliminarDocumento(string tipoDocumento, string idCompania, string establecimiento, string puntoEmision, string secuencial);
        // TODO: agregue aquí sus operaciones de servicio
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
