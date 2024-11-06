using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Perfile
{
    public string TxCodigoPerfiles { get; set; } = null!;

    public string? TxNombreOpcion { get; set; }

    public string? TxRuta { get; set; }

    public string? CiEstado { get; set; }

    public string? TxOpcionIcono { get; set; }

    public virtual ICollection<PerfilesUsuario> PerfilesUsuarios { get; set; } = new List<PerfilesUsuario>();
}
