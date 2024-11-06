using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class FacturaInfoAdicional
{
    public int CiFacturaInfoAdicional { get; set; }

    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string? TxNombre { get; set; }

    public string? TxValor { get; set; }

    public virtual Factura1 Factura1 { get; set; } = null!;
}
