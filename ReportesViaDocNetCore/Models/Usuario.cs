using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Usuario
{
    public int TxCodigoUsuario { get; set; }

    public string TxUsuario { get; set; } = null!;

    public string? TxPassword { get; set; }

    public string? TxCodigoPerfiles { get; set; }

    public string? CiEstado { get; set; }

    public string? TxNombre { get; set; }
}
