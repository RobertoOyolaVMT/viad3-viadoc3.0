using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class ErrorEnvioEmail
{
    public int CiSecuencial { get; set; }

    public int CiCompania { get; set; }

    public string CiTipoDocumento { get; set; } = null!;

    public string TxClaveAcceso { get; set; } = null!;

    public string? TxMensajeError { get; set; }

    public DateTime? TxFechaError { get; set; }

    public int? Anyo { get; set; }

    public int? Mes { get; set; }

    public int? Dia { get; set; }

    public int? Hora { get; set; }

    public int? Minuto { get; set; }

    public int? Segundo { get; set; }

    public int? CiCodigoError { get; set; }

    public string? CiEstado { get; set; }

    public virtual Companium CiCompaniaNavigation { get; set; } = null!;

    public virtual TipoDocumento CiTipoDocumentoNavigation { get; set; } = null!;
}
