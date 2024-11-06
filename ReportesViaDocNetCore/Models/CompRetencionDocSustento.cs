using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class CompRetencionDocSustento
{
    public int CiCompRetencionDocSus { get; set; }

    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxFechaEmision { get; set; } = null!;

    public string TxCodSustento { get; set; } = null!;

    public string TxCodDocSustento { get; set; } = null!;

    public string TxFechaRegistroContable { get; set; } = null!;

    public string TxCodImpuestoDocSustento { get; set; } = null!;

    public string TxCodigoPorcentaje { get; set; } = null!;

    public string TxTotalSinImpuesto { get; set; } = null!;

    public string TxImporteTotal { get; set; } = null!;

    public string TxBaseImponible { get; set; } = null!;

    public string TxTarifa { get; set; } = null!;

    public string TxValorImpuesto { get; set; } = null!;
}
