using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Estado
{
    public string CiEstado { get; set; } = null!;

    public string? TxDescrpicion { get; set; }

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    public virtual ICollection<Companium> Compania { get; set; } = new List<Companium>();

    public virtual ICollection<Esquema> Esquemas { get; set; } = new List<Esquema>();

    public virtual ICollection<Sucursal> Sucursals { get; set; } = new List<Sucursal>();

    public virtual ICollection<TipoDocumento> TipoDocumentos { get; set; } = new List<TipoDocumento>();

    public virtual ICollection<TipoEmision> TipoEmisions { get; set; } = new List<TipoEmision>();

    public virtual ICollection<TipoGeneracion> TipoGeneracions { get; set; } = new List<TipoGeneracion>();

    public virtual ICollection<TipoIdentificacion> TipoIdentificacions { get; set; } = new List<TipoIdentificacion>();
}
