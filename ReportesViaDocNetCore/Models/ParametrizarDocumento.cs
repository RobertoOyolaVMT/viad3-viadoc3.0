using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class ParametrizarDocumento
{
    public int IdRegistro { get; set; }

    public string? CiTipoDocumento { get; set; }

    public int? CantidadFirma { get; set; }

    public int? CantidadAutorizacion { get; set; }

    public int? CantidadCorreo { get; set; }

    public bool? CorreoAutorizacion { get; set; }

    public int? ReprocesoFirma { get; set; }

    public int? ReprocesoAutorizacion { get; set; }

    public int? ReprocesoCorreo { get; set; }

    public int? CiCompania { get; set; }

    public string? CiEstado { get; set; }
}
