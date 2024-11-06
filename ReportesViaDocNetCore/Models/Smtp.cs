using System;
using System.Collections.Generic;

namespace ReportesViaDocNetCore.Models;

public partial class Smtp
{
    public int CiCompania { get; set; }

    public string? HostServidor { get; set; }

    public int? Puerto { get; set; }

    public string? EnableSsl { get; set; }

    public string? EmailCredencial { get; set; }

    public string? PassCredencial { get; set; }

    public string? MailAddressfrom { get; set; }

    public string Para { get; set; } = null!;

    public string Cc { get; set; } = null!;

    public string? Asunto { get; set; }

    public string? UrlPortal { get; set; }

    public bool? ActivarNotificacion { get; set; }

    public virtual Companium CiCompaniaNavigation { get; set; } = null!;
}
