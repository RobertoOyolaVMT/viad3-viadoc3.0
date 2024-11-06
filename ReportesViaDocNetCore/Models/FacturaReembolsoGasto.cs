using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class FacturaReembolsoGasto
{
    public int CiReembolso { get; set; }

    public int CiCompania { get; set; }

    public string? CiTipoDocumento { get; set; }

    public string? TxEstablecimiento { get; set; }

    public string? TxPuntoEmision { get; set; }

    public string? TxSecuencial { get; set; }

    public string? TxNumDocumento { get; set; }

    public string? TxFechaEmision { get; set; }

    public string? TxIdProveedor { get; set; }

    public string? TxClaveAcceso { get; set; }

    public decimal? SubTotNoIva { get; set; }

    public decimal? SubTotIvaCero { get; set; }

    public decimal? SubTotIva { get; set; }

    public decimal? SubTotExcentoIva { get; set; }

    public decimal? ImpIva { get; set; }

    public decimal? ImpIce { get; set; }

    public decimal? ImpIrbpnr { get; set; }

    public decimal? ValTotal { get; set; }

    public string? Detalle { get; set; }

    public decimal? ValorBase { get; set; }

    public string? Codigo { get; set; }

    public string? CodigoPorcentaje { get; set; }

    public string? TxTipoIdProveedor { get; set; }

    public string? CodPaisPagoProveedor { get; set; }

    public string? TipoProveedor { get; set; }

    public string? Tarifa { get; set; }
}
