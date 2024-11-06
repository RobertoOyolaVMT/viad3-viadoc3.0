using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class PerfilesUsuario
{
    public int CiCodigoPerfilesUsuarios { get; set; }

    public string TxCodigoUsuarios { get; set; } = null!;

    public string TxCodigoPerfiles { get; set; } = null!;

    public string? TxRura { get; set; }

    public string? TxFechaCreación { get; set; }

    public string? TxHoraCreación { get; set; }

    public string? CiEstado { get; set; }

    public virtual Perfile TxCodigoPerfilesNavigation { get; set; } = null!;
}
