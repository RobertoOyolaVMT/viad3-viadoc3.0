using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Companium
{
    public int CiCompania { get; set; }

    public string? UiCompania { get; set; }

    public string TxRuc { get; set; } = null!;

    public string TxRazonSocial { get; set; } = null!;

    public string? TxNombreComercial { get; set; }

    public string TxDireccionMatriz { get; set; } = null!;

    public string? TxContribuyenteEspecial { get; set; }

    public string? TxObligadoContabilidad { get; set; }

    public string? TxAgenteRetencion { get; set; }

    public string? TxRegimenMicroempresas { get; set; }

    public string? TxContribuyenteRimpe { get; set; }

    public int CiTipoAmbiente { get; set; }

    public byte[]? CiLogo { get; set; }

    public string CiEstado { get; set; } = null!;

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    public virtual Estado CiEstadoNavigation { get; set; } = null!;

    public virtual TipoAmbiente CiTipoAmbienteNavigation { get; set; } = null!;

    public virtual ICollection<CompRetencion> CompRetencions { get; set; } = new List<CompRetencion>();

    public virtual ICollection<ErrorEnvioEmail> ErrorEnvioEmails { get; set; } = new List<ErrorEnvioEmail>();

    public virtual ICollection<Factura1> Factura1s { get; set; } = new List<Factura1>();

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    public virtual ICollection<LiquidacionReembolso> LiquidacionReembolsos { get; set; } = new List<LiquidacionReembolso>();

    public virtual ICollection<Liquidacion> Liquidacions { get; set; } = new List<Liquidacion>();

    public virtual ICollection<NotaCredito> NotaCreditos { get; set; } = new List<NotaCredito>();

    public virtual ICollection<NotaDebito> NotaDebitos { get; set; } = new List<NotaDebito>();

    public virtual Smtp? Smtp { get; set; }

    public virtual ICollection<Sucursal> Sucursals { get; set; } = new List<Sucursal>();
}
