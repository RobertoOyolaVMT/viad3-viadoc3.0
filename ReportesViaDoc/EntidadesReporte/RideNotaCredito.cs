﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesViaDoc.EntidadesReporte
{
    public class RideNotaCredito
    {
        public bool BanderaGeneracionObjeto { get; set; }
        public string FechaHoraAutorizacion { get; set; }
        public string NumeroAutorizacion { get; set; }
        public InformacionTributaria _infoTributaria { get; set; }
        public InformacionNotaCredito _infoNotaCredito { get; set; }
        public List<Detalle> _detalles { get; set; }
        public List<InformacionAdicional> _infoAdicional { get; set; }

        public RideNotaCredito()
        {
            BanderaGeneracionObjeto = false;
            FechaHoraAutorizacion = "";
            NumeroAutorizacion = "";
            _infoTributaria = new InformacionTributaria();
            _infoNotaCredito = new InformacionNotaCredito();
            _detalles = new List<Detalle>();
            _infoAdicional = new List<InformacionAdicional>();
        }

    }
}