using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class TiempoServicio
{
    public int? IdRegistro { get; set; }

    public int? IdServicioWindows { get; set; }

    public string? HoraInicio { get; set; }

    public string? HoraFin { get; set; }

    public string? CiEstado { get; set; }
}
