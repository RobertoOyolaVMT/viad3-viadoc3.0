using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Configuracion
{
    public int IdConfiguracion { get; set; }

    public string CodReferencia { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? Estado { get; set; }

    public virtual ICollection<ConfiguracionCompanium> ConfiguracionCompania { get; set; } = new List<ConfiguracionCompanium>();
}
