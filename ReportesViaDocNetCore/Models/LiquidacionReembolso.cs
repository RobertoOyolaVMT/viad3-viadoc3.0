using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class LiquidacionReembolso
{
    public int CiLiquidacionreembolso { get; set; }

    public int? CiCompania { get; set; }

    public string? TxTipoIdentificacionProveedorReembolso { get; set; }

    public string? TxIdentificacionProveedorReembolso { get; set; }

    public string? TxCodPaisPagoProveedorReembolso { get; set; }

    public string? TxTipoProveedorReembolso { get; set; }

    public string? CodDocReembolso { get; set; }

    public string? EstabDocReembolso { get; set; }

    public string? PtoEmiDocReembolso { get; set; }

    public string? SecuencialDocReembolso { get; set; }

    public string? TxFechaEmisionDocReembolso { get; set; }

    public string? NumeroautorizacionDocReemb { get; set; }

    public string? Codigo { get; set; }

    public string? CodigoPorcentaje { get; set; }

    public string? Tarifa { get; set; }

    public decimal? BaseImponibleReembolso { get; set; }

    public decimal? ImpuestoReembolso { get; set; }

    public virtual Companium? CiCompaniaNavigation { get; set; }
}
