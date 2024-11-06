using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class NotaCreditoDetalle
{
    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxCodigoInterno { get; set; } = null!;

    public string? TxCodigoAdicional { get; set; }

    public string TxDescripcion { get; set; } = null!;

    public int QnCantidad { get; set; }

    public decimal QnPrecioUnitario { get; set; }

    public decimal QnDescuento { get; set; }

    public decimal QnPrecioTotalSinImpuesto { get; set; }

    public virtual NotaCredito NotaCredito { get; set; } = null!;

    public virtual ICollection<NotaCreditoDetalleAdicional> NotaCreditoDetalleAdicionals { get; set; } = new List<NotaCreditoDetalleAdicional>();

    public virtual ICollection<NotaCreditoDetalleImpuesto> NotaCreditoDetalleImpuestos { get; set; } = new List<NotaCreditoDetalleImpuesto>();
}
