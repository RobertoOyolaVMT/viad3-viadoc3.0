using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Prueba
{
    public string? TxClaveAcceso { get; set; }

    public string? CiEstado { get; set; }

    public int? CiCompania { get; set; }

    public string? CiTipoDocumento { get; set; }

    public string? FechaAutorizacion { get; set; }

    public string? XmlDocumento { get; set; }

    public string? TxNumeroAutorizacion { get; set; }

    public int? CiNumeroIntento { get; set; }

    public int? CiIndetificador { get; set; }
}
