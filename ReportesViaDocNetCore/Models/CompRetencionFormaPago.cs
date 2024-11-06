using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class CompRetencionFormaPago
{
    public int CiCompRetencionFormaPago { get; set; }

    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxFormaPago { get; set; } = null!;

    public decimal QnTotal { get; set; }
}
