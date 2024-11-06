using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class NotaDebitoMotivo
{
    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxRazon { get; set; } = null!;

    public decimal QnValor { get; set; }

    public virtual NotaDebito NotaDebito { get; set; } = null!;
}
