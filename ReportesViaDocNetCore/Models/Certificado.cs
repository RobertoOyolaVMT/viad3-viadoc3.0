using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Certificado
{
    public int CiCertificado { get; set; }

    public int CiCompania { get; set; }

    public Guid UiSemilla { get; set; }

    public string TxClave { get; set; } = null!;

    public string TxKey { get; set; } = null!;

    public string ObCertificado { get; set; } = null!;

    public DateTime FcDesde { get; set; }

    public DateTime FcHasta { get; set; }

    public string CiEstado { get; set; } = null!;

    public int? CiNumeroCerti { get; set; }

    public virtual Companium CiCompaniaNavigation { get; set; } = null!;

    public virtual Estado CiEstadoNavigation { get; set; } = null!;
}
