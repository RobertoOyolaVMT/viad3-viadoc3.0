using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.guiaRemision
{
    [DataContract]
    public class GuiaRemision
    {
        [DataMember(Order = 1)]
        public int idGuiaRemision{ get; set; }
        [DataMember(Order = 2)]
        public int compania { get; set; }
        [DataMember(Order = 3)]
        public string establecimiento { get; set; }
        [DataMember(Order = 4)]
        public string puntoEmision { get; set; }
        [DataMember(Order = 5)]
        public string secuencial { get; set; }
        [DataMember(Order = 7)]
        public string claveAcceso { get; set; }
        [DataMember(Order = 8)]
        public int tipoEmision { get; set; }
        [DataMember(Order = 8)]
        public string direccionPartida { get; set; }
        [DataMember(Order = 9)]
        public string razonSocialTransportista { get; set; }
        [DataMember(Order = 10)]
        public string tipoIdentificacionTransportista { get; set; }
        [DataMember(Order = 11)]
        public string rucTransportista { get; set; }
        [DataMember(Order = 12)]
        public string rise { get; set; }
        [DataMember(Order = 13)]
        public string fechaIniTransporte { get; set; }
        [DataMember(Order = 14)]
        public string fechaFinTransporte { get; set; }
        [DataMember(Order = 15)]
        public string placa { get; set; }
        [DataMember(Order = 16)]
        public string email { get; set; }
        [DataMember(Order = 17)]
        public string ambiente { get; set; }
        [DataMember(Order = 18)]
        public string codigoNumerico { get; set; }
        [DataMember(Order = 19)]
        public string ruc { get; set; }
        [DataMember(Order = 20)]
        public int contingenciaDet { get; set; }
        [DataMember(Order = 21)]
        public List<GuiaRemisionInfoAdicional> infoAdicional { get; set; }
        [DataMember(Order = 22)]
        public List<GuiaRemisionDestinatario> destinatario { get; set; }

        public GuiaRemision()
        {
            infoAdicional = new List<GuiaRemisionInfoAdicional>();
            destinatario = new List<GuiaRemisionDestinatario>();
        }
    }
}
