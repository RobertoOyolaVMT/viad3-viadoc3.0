using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class TipoErrorSri
{
    public int CiCodTipoError { get; set; }

    public string? TxDescripcionTipoError { get; set; }

    public virtual ICollection<CodigoErroresSri> CodigoErroresSris { get; set; } = new List<CodigoErroresSri>();
}
