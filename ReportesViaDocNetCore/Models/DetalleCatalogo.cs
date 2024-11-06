using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class DetalleCatalogo
{
    public int CiDetalle { get; set; }

    public int CiCatalogo { get; set; }

    public string Param1 { get; set; } = null!;

    public string? Param2 { get; set; }

    public string? Param3 { get; set; }

    public string? Param4 { get; set; }

    public string? Param5 { get; set; }

    public string? Ciestado { get; set; }

    public virtual CabeceraCatalogo CiCatalogoNavigation { get; set; } = null!;
}
