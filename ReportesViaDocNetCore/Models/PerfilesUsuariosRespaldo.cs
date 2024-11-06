using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class PerfilesUsuariosRespaldo
{
    public int CiCodigoPerfilesUsuarios { get; set; }

    public string TxCodigoUsuarios { get; set; } = null!;

    public int CiCodigoPerfiles { get; set; }

    public int? CiCodigoOpciones { get; set; }

    public string? TxFechaCreación { get; set; }

    public string? TxHoraCreación { get; set; }

    public string? CiEstado { get; set; }
}
