﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViaDoc.EntidadNegocios.Liquidacion
{
    [DataContract]
    public class LiquidacionDetalleAdicional
    {
        [DataMember]
        public string codigoPrincipal { get; set; }
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string valor { get; set; }
    }
}