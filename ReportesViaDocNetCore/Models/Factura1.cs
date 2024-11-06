using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Factura1
{
    public int CiFactura { get; set; }

    public int CiCompania { get; set; }

    public int CiTipoEmision { get; set; }

    public string TxClaveAcceso { get; set; } = null!;

    public string CiTipoDocumento { get; set; } = null!;

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxFechaEmision { get; set; } = null!;

    public string CiTipoIdentificacionComprador { get; set; } = null!;

    public string? TxGuiaRemision { get; set; }

    public string TxRazonSocialComprador { get; set; } = null!;

    public string TxIdentificacionComprador { get; set; } = null!;

    public decimal QnTotalSinImpuestos { get; set; }

    public decimal QnTotalDescuento { get; set; }

    public decimal QnPropina { get; set; }

    public decimal QnImporteTotal { get; set; }

    public string TxMoneda { get; set; } = null!;

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

    public virtual TipoIdentificacion CiTipoIdentificacionCompradorNavigation { get; set; } = null!;

    public virtual ICollection<FacturaDetalle1> FacturaDetalle1s { get; set; } = new List<FacturaDetalle1>();

    public virtual ICollection<FacturaInfoAdicional> FacturaInfoAdicionals { get; set; } = new List<FacturaInfoAdicional>();

    public virtual ICollection<FacturaTotalImpuesto> FacturaTotalImpuestos { get; set; } = new List<FacturaTotalImpuesto>();
}
