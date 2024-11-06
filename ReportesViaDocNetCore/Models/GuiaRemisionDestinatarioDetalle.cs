using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class GuiaRemisionDestinatarioDetalle
{
    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxIdentificacionDestinatario { get; set; } = null!;

    public string TxCodigoInterno { get; set; } = null!;

    public string? TxCodigoAdicional { get; set; }

    public string? TxDescripcion { get; set; }

    public decimal QnCantidad { get; set; }

    public virtual GuiaRemisionDestinatario GuiaRemisionDestinatario { get; set; } = null!;

    public virtual ICollection<GuiaRemisionDestinatarioDetalleAdicional> GuiaRemisionDestinatarioDetalleAdicionals { get; set; } = new List<GuiaRemisionDestinatarioDetalleAdicional>();
}
