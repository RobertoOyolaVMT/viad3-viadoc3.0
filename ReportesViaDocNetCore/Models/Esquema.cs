using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Esquema
{
    public string CiTipoDocumento { get; set; } = null!;

    public int CiTipoGeneracion { get; set; }

    public string TxVersion { get; set; } = null!;

    public string XmlEsquema { get; set; } = null!;

    public string CiEstado { get; set; } = null!;

    public virtual Estado CiEstadoNavigation { get; set; } = null!;

    public virtual TipoDocumento CiTipoDocumentoNavigation { get; set; } = null!;

    public virtual TipoGeneracion CiTipoGeneracionNavigation { get; set; } = null!;
}
