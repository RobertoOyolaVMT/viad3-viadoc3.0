using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class EstadoComprobante
{
    public string CiEstado { get; set; } = null!;

    public string? TxtNameEstado { get; set; }

    public string? TxNameEstadoConfig { get; set; }

    public string? TxDescripcion { get; set; }
}
