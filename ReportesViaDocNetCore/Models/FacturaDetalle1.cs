using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class FacturaDetalle1
{
    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxCodigoPrincipal { get; set; } = null!;

    public string? TxCodigoAuxiliar { get; set; }

    public string TxDescripcion { get; set; } = null!;

    public int QnCantidad { get; set; }

    public decimal? QnPrecioUnitario { get; set; }

    public decimal QnDescuento { get; set; }

    public decimal QnPrecioTotalSinImpuesto { get; set; }

    public virtual Factura1 Factura1 { get; set; } = null!;

    public virtual ICollection<FacturaDetalleAdicional> FacturaDetalleAdicionals { get; set; } = new List<FacturaDetalleAdicional>();

    public virtual ICollection<FacturaDetalleImpuesto1> FacturaDetalleImpuesto1s { get; set; } = new List<FacturaDetalleImpuesto1>();
}
