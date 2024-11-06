using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class TipoGeneracion
{
    public int CiTipoGeneracion { get; set; }

    public string TxDescripcion { get; set; } = null!;

    public string CiEstado { get; set; } = null!;

    public virtual Estado CiEstadoNavigation { get; set; } = null!;

    public virtual ICollection<Esquema> Esquemas { get; set; } = new List<Esquema>();
}
