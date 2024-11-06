using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Liquidacion
{
    public int CiLiquidacion { get; set; }

    public int CiCompania { get; set; }

    public int CiTipoEmision { get; set; }

    public string TxClaveAcceso { get; set; } = null!;

    public string CiTipoDocumento { get; set; } = null!;

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxFechaEmision { get; set; } = null!;

    public string CiTipoIdentificacionProvedor { get; set; } = null!;

    public string TxRazonSocialProvedor { get; set; } = null!;

    public string TxIdentificacionProvedor { get; set; } = null!;

    public decimal QnTotalSinImpuestos { get; set; }

    public decimal QnTotalDescuento { get; set; }

    public string CodDocReembolso { get; set; } = null!;

    public decimal? TotalComprobantesReembolso { get; set; }

    public decimal? TotalBaseImponibleReembolso { get; set; }

    public decimal? TotalImpuestoReembolso { get; set; }

    public decimal? QnImporteTotal { get; set; }

    public string? TxMoneda { get; set; }

    public int? CiContingenciaDet { get; set; }

    public string? TxEmail { get; set; }

    public string? TxNumeroAutorizacion { get; set; }

    public string? TxFechaHoraAutorizacion { get; set; }

    public string? XmlDocumentoAutorizado { get; set; }

    public string? CiEstado { get; set; }

    public string? CiEstadoRecepcionAutorizacion { get; set; }

    public string? TxCodError { get; set; }

    public string? TxMensajeError { get; set; }

    public int? CiNumeroIntento { get; set; }

    public virtual Companium CiCompaniaNavigation { get; set; } = null!;

    public virtual TipoDocumento CiTipoDocumentoNavigation { get; set; } = null!;

    public virtual TipoEmision CiTipoEmisionNavigation { get; set; } = null!;
}
