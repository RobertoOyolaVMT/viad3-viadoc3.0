using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Sucursal
{
    public int CiCompania { get; set; }

    public string CiSucursal { get; set; } = null!;

    public string? TxDireccion { get; set; }

    public string CiEstado { get; set; } = null!;

    public virtual Companium CiCompaniaNavigation { get; set; } = null!;

    public virtual Estado CiEstadoNavigation { get; set; } = null!;
}
