using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class CodigoErroresSri
{
    public int CiTipoError { get; set; }

    public string TxCodigoError { get; set; } = null!;

    public string? TxDesccripcion { get; set; }

    public string? TxPosibleSolucion { get; set; }

    public virtual TipoErrorSri CiTipoErrorNavigation { get; set; } = null!;
}
