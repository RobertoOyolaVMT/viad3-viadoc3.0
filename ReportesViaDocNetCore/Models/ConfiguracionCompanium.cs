using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class ConfiguracionCompanium
{
    public int IdConfiguracionCompania { get; set; }

    public int IdConfiguracion { get; set; }

    public int CiCompania { get; set; }

    public string? Param1 { get; set; }

    public string? Param2 { get; set; }

    public string? Param3 { get; set; }

    public string? Param4 { get; set; }

    public string? Param5 { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Configuracion IdConfiguracionNavigation { get; set; } = null!;
}
