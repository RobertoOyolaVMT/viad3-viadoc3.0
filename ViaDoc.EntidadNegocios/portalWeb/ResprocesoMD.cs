﻿using System.Collections.Generic;

namespace ViaDoc.EntidadNegocios.portalWeb
{
    public class ResprocesoMD
    {
        public string RazonSocial { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string ClaveAcceso { get; set; }
        public string FechaEmision { get; set; }
        public string FechaHoraAutorizacion { get; set; }
        public string Estado { get; set; }
        public string CiEstado { get; set; }
        public string CodError { get; set; }
        public string MenError { get; set; }
        public int NumeroCiclos { get; set; }
    }

    public class ListaReproceso
    {
        public List<ResprocesoMD> objResproceso { get; set; }
    }
}