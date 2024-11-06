using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class FacturaDetalleAdicional
{
    public int CiFacturaDetalleAdicional { get; set; }

    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxCodigoPrincipal { get; set; } = null!;

    public string? TxNombre { get; set; }

    public string? TxValor { get; set; }

    public virtual FacturaDetalle1 FacturaDetalle1 { get; set; } = null!;
}
