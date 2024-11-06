using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class TipoEmision
{
    public int CiTipoEmision { get; set; }

    public string TxDescripcion { get; set; } = null!;

    public string CiEstado { get; set; } = null!;

    public virtual Estado CiEstadoNavigation { get; set; } = null!;

    public virtual ICollection<CompRetencion> CompRetencions { get; set; } = new List<CompRetencion>();

    public virtual ICollection<Factura1> Factura1s { get; set; } = new List<Factura1>();

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();

    public virtual ICollection<NotaCredito> NotaCreditos { get; set; } = new List<NotaCredito>();

    public virtual ICollection<NotaDebito> NotaDebitos { get; set; } = new List<NotaDebito>();
}
