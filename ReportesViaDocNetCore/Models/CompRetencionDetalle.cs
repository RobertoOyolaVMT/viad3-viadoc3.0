using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class CompRetencionDetalle
{
    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public int CiImpuesto { get; set; }

    public string TxCodRetencion { get; set; } = null!;

    public decimal QnBaseImponible { get; set; }

    public decimal QnPorcentajeRetener { get; set; }

    public decimal QnValorRetenido { get; set; }

    public string TxCodDocSustento { get; set; } = null!;

    public string? TxNumDocSustento { get; set; }

    public string? TxFechaEmisionDocSustento { get; set; }
}
