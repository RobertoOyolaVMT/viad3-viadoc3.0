using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class CabeceraCatalogo
{
    public int CiCatalogo { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Ciestado { get; set; }

    public string? CodReferencia { get; set; }

    public virtual ICollection<DetalleCatalogo> DetalleCatalogos { get; set; } = new List<DetalleCatalogo>();
}
