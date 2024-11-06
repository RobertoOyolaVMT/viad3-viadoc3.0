using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class OpcionesPerfile
{
    public int CiCodigoPerfiles { get; set; }

    public int? CiCodigoOpcion { get; set; }

    public string? TxNombreOpcion { get; set; }

    public string? TxControlador { get; set; }

    public string? TxAccion { get; set; }

    public string? TxIconoOpcion { get; set; }

    public string? CiEstado { get; set; }
}
