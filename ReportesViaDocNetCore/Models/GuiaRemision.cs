using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class GuiaRemision
{
    public int CiGuiaRemision { get; set; }

    public int CiCompania { get; set; }

    public int CiTipoEmision { get; set; }

    public string TxClaveAcceso { get; set; } = null!;

    public string CiTipoDocumento { get; set; } = null!;

    public string TxEstablecimiento { get; set; } = null!;

    public string TxPuntoEmision { get; set; } = null!;

    public string TxSecuencial { get; set; } = null!;

    public string TxDireccionPartida { get; set; } = null!;

    public string TxRazonSocialTransportista { get; set; } = null!;

    public string CiTipoIdentificacionTransportista { get; set; } = null!;

    public string TxRucTransportista { get; set; } = null!;

    public string? TxRise { get; set; }

    public string TxFechaIniTransporte { get; set; } = null!;

    public string TxFechaFinTransporte { get; set; } = null!;

    public string TxPlaca { get; set; } = null!;

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

    public virtual TipoIdentificacion CiTipoIdentificacionTransportistaNavigation { get; set; } = null!;

    public virtual ICollection<GuiaRemisionDestinatario> GuiaRemisionDestinatarios { get; set; } = new List<GuiaRemisionDestinatario>();

    public virtual ICollection<GuiaRemisionInfoAdicional> GuiaRemisionInfoAdicionals { get; set; } = new List<GuiaRemisionInfoAdicional>();
}
