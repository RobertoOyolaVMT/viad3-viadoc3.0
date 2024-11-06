using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class GuiaRemisionDestinatario
{
    public int CiCompania { get; set; }

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxIdentificacionDestinatario { get; set; } = null!;

    public string TxRazonSocialDestinatario { get; set; } = null!;

    public string TxDireccionDestinatario { get; set; } = null!;

    public string TxMotivoTraslado { get; set; } = null!;

    public string? TxDocumentoAduaneroUnico { get; set; }

    public string? TxCodigoEstablecimientoDestino { get; set; }

    public string? TxRuta { get; set; }

    public string? CiTipoDocumentoSustento { get; set; }

    public string? TxNumeroDocumentoSustento { get; set; }

    public string? TxNumeroAutorizacionDocumentoSustento { get; set; }

    public string? TxFechaEmisionDocumentoSustento { get; set; }

    public virtual GuiaRemision GuiaRemision { get; set; } = null!;

    public virtual ICollection<GuiaRemisionDestinatarioDetalle> GuiaRemisionDestinatarioDetalles { get; set; } = new List<GuiaRemisionDestinatarioDetalle>();
}
