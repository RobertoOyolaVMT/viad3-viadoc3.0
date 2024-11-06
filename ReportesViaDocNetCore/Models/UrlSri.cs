using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class UrlSri
{
    public int CiUrlSri { get; set; }

    public string? TxUrlRecepcion { get; set; }

    public string? TxUrlAutorizacion { get; set; }

    public string? CiTipoAmbiente { get; set; }

    public string? CiEstado { get; set; }
}
