using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class TipoDocumento
{
    public string CiTipoDocumento { get; set; } = null!;

    public string TxDescripcion { get; set; } = null!;

    public string CiEstado { get; set; } = null!;

    public virtual Estado CiEstadoNavigation { get; set; } = null!;

    public virtual ICollection<CompRetencion> CompRetencions { get; set; } = new List<CompRetencion>();

    public virtual ICollection<ErrorEnvioEmail> ErrorEnvioEmails { get; set; } = new List<ErrorEnvioEmail>();

    public virtual ICollection<Esquema> Esquemas { get; set; } = new List<Esquema>();

    public virtual ICollection<Factura1> Factura1s { get; set; } = new List<Factura1>();

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();

    public virtual ICollection<NotaCredito> NotaCreditoCiTipoDocumentoModificadoNavigations { get; set; } = new List<NotaCredito>();

    public virtual ICollection<NotaCredito> NotaCreditoCiTipoDocumentoNavigations { get; set; } = new List<NotaCredito>();

    public virtual ICollection<NotaDebito> NotaDebitoCiTipoDocumentoModificadoNavigations { get; set; } = new List<NotaDebito>();

    public virtual ICollection<NotaDebito> NotaDebitoCiTipoDocumentoNavigations { get; set; } = new List<NotaDebito>();
}
