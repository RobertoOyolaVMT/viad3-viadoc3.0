using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class InformacionGuiaRemision
    {

        public string DirEstablecimiento { get; set; }
        public string DirPartida { get; set; }
        public string RazonSocialTransportista { get; set; }
        public string TipoIdentificacionTransportista { get; set; }
        public string RucTransportista { get; set; }
        public string ObligadoContabilidad { get; set; }
        public string ContribuyenteEspecial { get; set; }
        public string FechaInicioTransporte { get; set; }
        public string FechaFinTransporte { get; set; }
        public string Placa { get; set; }

        public InformacionGuiaRemision()
        {
            DirEstablecimiento = "";
            DirPartida = "";
            RazonSocialTransportista = "";
            TipoIdentificacionTransportista = "";
            RucTransportista = "";
            ObligadoContabilidad = "";
            ContribuyenteEspecial = "";
            FechaInicioTransporte = "";
            FechaFinTransporte = "";
            Placa = "";
        }
        

    }
}
