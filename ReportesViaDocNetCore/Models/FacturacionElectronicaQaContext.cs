using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReportesViaDocNetCore.Models;

public partial class FacturacionElectronicaQaContext : DbContext
{
    public FacturacionElectronicaQaContext()
    {
    }

    public FacturacionElectronicaQaContext(DbContextOptions<FacturacionElectronicaQaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CabeceraCatalogo> CabeceraCatalogos { get; set; }

    public virtual DbSet<Certificado> Certificados { get; set; }

    public virtual DbSet<CodigoErroresSri> CodigoErroresSris { get; set; }

    public virtual DbSet<CompRetencion> CompRetencions { get; set; }

    public virtual DbSet<CompRetencionDetalle> CompRetencionDetalles { get; set; }

    public virtual DbSet<CompRetencionDocSustento> CompRetencionDocSustentos { get; set; }

    public virtual DbSet<CompRetencionFormaPago> CompRetencionFormaPagos { get; set; }

    public virtual DbSet<CompRetencionInfoAdicional> CompRetencionInfoAdicionals { get; set; }

    public virtual DbSet<Companium> Compania { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<ConfiguracionCompanium> ConfiguracionCompania { get; set; }

    public virtual DbSet<DetalleCatalogo> DetalleCatalogos { get; set; }

    public virtual DbSet<ErrorEnvioEmail> ErrorEnvioEmails { get; set; }

    public virtual DbSet<Esquema> Esquemas { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstadoComprobante> EstadoComprobantes { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Factura1> Facturas1 { get; set; }

    public virtual DbSet<FacturaDetalle> FacturaDetalles { get; set; }

    public virtual DbSet<FacturaDetalle1> FacturaDetalles1 { get; set; }

    public virtual DbSet<FacturaDetalleAdicional> FacturaDetalleAdicionals { get; set; }

    public virtual DbSet<FacturaDetalleFormaPago> FacturaDetalleFormaPagos { get; set; }

    public virtual DbSet<FacturaDetalleFormaPago1> FacturaDetalleFormaPagos1 { get; set; }

    public virtual DbSet<FacturaDetalleImpuesto> FacturaDetalleImpuestos { get; set; }

    public virtual DbSet<FacturaDetalleImpuesto1> FacturaDetalleImpuestos1 { get; set; }

    public virtual DbSet<FacturaInfoAdicional> FacturaInfoAdicionals { get; set; }

    public virtual DbSet<FacturaReembolsoGasto> FacturaReembolsoGastos { get; set; }

    public virtual DbSet<FacturaTotalImpuesto> FacturaTotalImpuestos { get; set; }

    public virtual DbSet<GuiaRemision> GuiaRemisions { get; set; }

    public virtual DbSet<GuiaRemisionDestinatario> GuiaRemisionDestinatarios { get; set; }

    public virtual DbSet<GuiaRemisionDestinatarioDetalle> GuiaRemisionDestinatarioDetalles { get; set; }

    public virtual DbSet<GuiaRemisionDestinatarioDetalleAdicional> GuiaRemisionDestinatarioDetalleAdicionals { get; set; }

    public virtual DbSet<GuiaRemisionInfoAdicional> GuiaRemisionInfoAdicionals { get; set; }

    public virtual DbSet<Liquidacion> Liquidacions { get; set; }

    public virtual DbSet<LiquidacionDetalle> LiquidacionDetalles { get; set; }

    public virtual DbSet<LiquidacionDetalleAdicional> LiquidacionDetalleAdicionals { get; set; }

    public virtual DbSet<LiquidacionDetalleFormaPago> LiquidacionDetalleFormaPagos { get; set; }

    public virtual DbSet<LiquidacionDetalleImpuesto> LiquidacionDetalleImpuestos { get; set; }

    public virtual DbSet<LiquidacionInfoAdicional> LiquidacionInfoAdicionals { get; set; }

    public virtual DbSet<LiquidacionReembolso> LiquidacionReembolsos { get; set; }

    public virtual DbSet<LiquidacionTotalImpuesto> LiquidacionTotalImpuestos { get; set; }

    public virtual DbSet<NotaCredito> NotaCreditos { get; set; }

    public virtual DbSet<NotaCreditoDetalle> NotaCreditoDetalles { get; set; }

    public virtual DbSet<NotaCreditoDetalleAdicional> NotaCreditoDetalleAdicionals { get; set; }

    public virtual DbSet<NotaCreditoDetalleImpuesto> NotaCreditoDetalleImpuestos { get; set; }

    public virtual DbSet<NotaCreditoInfoAdicional> NotaCreditoInfoAdicionals { get; set; }

    public virtual DbSet<NotaCreditoTotalImpuesto> NotaCreditoTotalImpuestos { get; set; }

    public virtual DbSet<NotaDebito> NotaDebitos { get; set; }

    public virtual DbSet<NotaDebitoImpuesto> NotaDebitoImpuestos { get; set; }

    public virtual DbSet<NotaDebitoInfoAdicional> NotaDebitoInfoAdicionals { get; set; }

    public virtual DbSet<NotaDebitoMotivo> NotaDebitoMotivos { get; set; }

    public virtual DbSet<OpcionesPerfile> OpcionesPerfiles { get; set; }

    public virtual DbSet<ParametrizarDocumento> ParametrizarDocumentos { get; set; }

    public virtual DbSet<Perfile> Perfiles { get; set; }

    public virtual DbSet<PerfilesRespaldo> PerfilesRespaldos { get; set; }

    public virtual DbSet<PerfilesUsuario> PerfilesUsuarios { get; set; }

    public virtual DbSet<PerfilesUsuariosRespaldo> PerfilesUsuariosRespaldos { get; set; }

    public virtual DbSet<Prueba> Pruebas { get; set; }

    public virtual DbSet<PruebaIcono> PruebaIconos { get; set; }

    public virtual DbSet<ReembolsoGasto> ReembolsoGastos { get; set; }

    public virtual DbSet<Smtp> Smtps { get; set; }

    public virtual DbSet<SriMensajeError> SriMensajeErrors { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<TiempoServicio> TiempoServicios { get; set; }

    public virtual DbSet<TipoAmbiente> TipoAmbientes { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoEmision> TipoEmisions { get; set; }

    public virtual DbSet<TipoErrorSri> TipoErrorSris { get; set; }

    public virtual DbSet<TipoGeneracion> TipoGeneracions { get; set; }

    public virtual DbSet<TipoIdentificacion> TipoIdentificacions { get; set; }

    public virtual DbSet<UrlSri> UrlSris { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("ViaDocConexion"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<CabeceraCatalogo>(entity =>
        {
            entity.HasKey(e => e.CiCatalogo).HasName("pk_CabeceraCatalogo");

            entity.ToTable("CabeceraCatalogo");

            entity.Property(e => e.CiCatalogo).HasColumnName("ciCatalogo");
            entity.Property(e => e.Ciestado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('A')")
                .HasColumnName("ciestado");
            entity.Property(e => e.CodReferencia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("codReferencia");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Certificado>(entity =>
        {
            entity.HasKey(e => new { e.CiCertificado, e.CiCompania });

            entity.ToTable("Certificado");

            entity.Property(e => e.CiCertificado)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciCertificado");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiNumeroCerti).HasColumnName("ciNumeroCerti");
            entity.Property(e => e.FcDesde)
                .HasColumnType("datetime")
                .HasColumnName("fcDesde");
            entity.Property(e => e.FcHasta)
                .HasColumnType("datetime")
                .HasColumnName("fcHasta");
            entity.Property(e => e.ObCertificado)
                .IsUnicode(false)
                .HasColumnName("obCertificado");
            entity.Property(e => e.TxClave)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("txClave");
            entity.Property(e => e.TxKey)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("txKey");
            entity.Property(e => e.UiSemilla)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("uiSemilla");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.Certificados)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Certificado_Compania");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.Certificados)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Certificado_Estado");
        });

        modelBuilder.Entity<CodigoErroresSri>(entity =>
        {
            entity.HasKey(e => new { e.CiTipoError, e.TxCodigoError });

            entity.ToTable("CodigoErroresSRI");

            entity.Property(e => e.CiTipoError).HasColumnName("ciTipoError");
            entity.Property(e => e.TxCodigoError)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txCodigoError");
            entity.Property(e => e.TxDesccripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("txDesccripcion");
            entity.Property(e => e.TxPosibleSolucion)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("txPosibleSolucion");

            entity.HasOne(d => d.CiTipoErrorNavigation).WithMany(p => p.CodigoErroresSris)
                .HasForeignKey(d => d.CiTipoError)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodigoErroresSRI_TipoErrorSRI");
        });

        modelBuilder.Entity<CompRetencion>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial }).HasName("PK_ComprobanteRetencion");

            entity.ToTable("CompRetencion");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.CiCompRetencion)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciCompRetencion");
            entity.Property(e => e.CiContingenciaDet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ciContingenciaDet");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiEstadoRecepcionAutorizacion)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstadoRecepcionAutorizacion");
            entity.Property(e => e.CiNumeroIntento).HasColumnName("ciNumeroIntento");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiTipoIdentificacionSujetoRetenido)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacionSujetoRetenido");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxCodError)
                .HasColumnType("text")
                .HasColumnName("txCodError");
            entity.Property(e => e.TxEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txEmail");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxFechaHoraAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txFechaHoraAutorizacion");
            entity.Property(e => e.TxIdentificacionSujetoRetenido)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionSujetoRetenido");
            entity.Property(e => e.TxMensajeError)
                .HasColumnType("text")
                .HasColumnName("txMensajeError");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.TxPeriodoFiscal)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("txPeriodoFiscal");
            entity.Property(e => e.TxRazonSocialSujetoRetenido)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialSujetoRetenido");
            entity.Property(e => e.XmlDocumentoAutorizado)
                .IsUnicode(false)
                .HasColumnName("xmlDocumentoAutorizado");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.CompRetencions)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComprobanteRetencion_Compania");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.CompRetencions)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComprobanteRetencion_TipoDocumento");

            entity.HasOne(d => d.CiTipoEmisionNavigation).WithMany(p => p.CompRetencions)
                .HasForeignKey(d => d.CiTipoEmision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComprobanteRetencion_TipoEmision");

            entity.HasOne(d => d.CiTipoIdentificacionSujetoRetenidoNavigation).WithMany(p => p.CompRetencions)
                .HasForeignKey(d => d.CiTipoIdentificacionSujetoRetenido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComprobanteRetencion_TipoIdentificacion");
        });

        modelBuilder.Entity<CompRetencionDetalle>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.CiImpuesto, e.TxCodRetencion });

            entity.ToTable("CompRetencionDetalle");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.CiImpuesto).HasColumnName("ciImpuesto");
            entity.Property(e => e.TxCodRetencion)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("txCodRetencion");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnPorcentajeRetener)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("qnPorcentajeRetener");
            entity.Property(e => e.QnValorRetenido)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("qnValorRetenido");
            entity.Property(e => e.TxCodDocSustento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodDocSustento");
            entity.Property(e => e.TxFechaEmisionDocSustento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmisionDocSustento");
            entity.Property(e => e.TxNumDocSustento)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("txNumDocSustento");
        });

        modelBuilder.Entity<CompRetencionDocSustento>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CompRetencionDocSustento");

            entity.Property(e => e.CiCompRetencionDocSus)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciCompRetencionDocSus");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxBaseImponible)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txBaseImponible");
            entity.Property(e => e.TxCodDocSustento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txCodDocSustento");
            entity.Property(e => e.TxCodImpuestoDocSustento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("txCodImpuestoDocSustento");
            entity.Property(e => e.TxCodSustento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txCodSustento");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxFechaRegistroContable)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaRegistroContable");
            entity.Property(e => e.TxImporteTotal)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txImporteTotal");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txTarifa");
            entity.Property(e => e.TxTotalSinImpuesto)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txTotalSinImpuesto");
            entity.Property(e => e.TxValorImpuesto)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txValorImpuesto");
        });

        modelBuilder.Entity<CompRetencionFormaPago>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CompRetencionFormaPago");

            entity.Property(e => e.CiCompRetencionFormaPago)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciCompRetencionFormaPago");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.QnTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotal");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxFormaPago)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("txFormaPago");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
        });

        modelBuilder.Entity<CompRetencionInfoAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiCompRetencionInfoAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("CompRetencionInfoAdicional");

            entity.Property(e => e.CiCompRetencionInfoAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciCompRetencionInfoAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.CompRetencion).WithMany(p => p.CompRetencionInfoAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompRetencionInfoAdicional_CompRetencion");
        });

        modelBuilder.Entity<Companium>(entity =>
        {
            entity.HasKey(e => e.CiCompania).HasName("PK__Compania__9A6FAD1807020F21");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiLogo)
                .HasColumnType("image")
                .HasColumnName("ciLogo");
            entity.Property(e => e.CiTipoAmbiente).HasColumnName("ciTipoAmbiente");
            entity.Property(e => e.TxContribuyenteEspecial)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("txContribuyenteEspecial");
            entity.Property(e => e.TxAgenteRetencion)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("txAgenteRetencion");
            entity.Property(e => e.TxRegimenMicroempresas)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("txRegimenMicroempresas");
            entity.Property(e => e.TxContribuyenteRimpe)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("txContribuyenteRimpe");
            entity.Property(e => e.TxDireccionMatriz)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDireccionMatriz");
            entity.Property(e => e.TxNombreComercial)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txNombreComercial");
            entity.Property(e => e.TxObligadoContabilidad)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txObligadoContabilidad");
            entity.Property(e => e.TxRazonSocial)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocial");
            entity.Property(e => e.TxRuc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txRuc");
            entity.Property(e => e.UiCompania)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("uiCompania");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.Compania)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compania_Estado");

            entity.HasOne(d => d.CiTipoAmbienteNavigation).WithMany(p => p.Compania)
                .HasForeignKey(d => d.CiTipoAmbiente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compania_TipoAmbiente");
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.HasKey(e => e.IdConfiguracion).HasName("pk_idConfiguracion");

            entity.ToTable("Configuracion");

            entity.Property(e => e.IdConfiguracion).HasColumnName("idConfiguracion");
            entity.Property(e => e.CodReferencia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("codReferencia");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('A')")
                .IsFixedLength()
                .HasColumnName("estado");
        });

        modelBuilder.Entity<ConfiguracionCompanium>(entity =>
        {
            entity.HasKey(e => e.IdConfiguracionCompania).HasName("pk_idConfiguracionCompania");

            entity.Property(e => e.IdConfiguracionCompania).HasColumnName("idConfiguracionCompania");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('A')")
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.IdConfiguracion).HasColumnName("idConfiguracion");
            entity.Property(e => e.Param1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("param1");
            entity.Property(e => e.Param2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("param2");
            entity.Property(e => e.Param3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("param3");
            entity.Property(e => e.Param4)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("param4");
            entity.Property(e => e.Param5)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("param5");

            entity.HasOne(d => d.IdConfiguracionNavigation).WithMany(p => p.ConfiguracionCompania)
                .HasForeignKey(d => d.IdConfiguracion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idConfiguracion");
        });

        modelBuilder.Entity<DetalleCatalogo>(entity =>
        {
            entity.HasKey(e => e.CiDetalle).HasName("pk_DetalleCatalogo");

            entity.ToTable("DetalleCatalogo");

            entity.Property(e => e.CiDetalle).HasColumnName("ciDetalle");
            entity.Property(e => e.CiCatalogo).HasColumnName("ciCatalogo");
            entity.Property(e => e.Ciestado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('A')")
                .HasColumnName("ciestado");
            entity.Property(e => e.Param1)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("param1");
            entity.Property(e => e.Param2)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("param2");
            entity.Property(e => e.Param3)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("param3");
            entity.Property(e => e.Param4)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("param4");
            entity.Property(e => e.Param5)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("param5");

            entity.HasOne(d => d.CiCatalogoNavigation).WithMany(p => p.DetalleCatalogos)
                .HasForeignKey(d => d.CiCatalogo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DetalleCatalogo");
        });

        modelBuilder.Entity<ErrorEnvioEmail>(entity =>
        {
            entity.HasKey(e => e.CiSecuencial).HasName("pk_ciSecuencial");

            entity.ToTable("ErrorEnvioEmail");

            entity.Property(e => e.CiSecuencial).HasColumnName("ciSecuencial");
            entity.Property(e => e.Anyo).HasColumnName("anyo");
            entity.Property(e => e.CiCodigoError).HasColumnName("ciCodigoError");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.Dia).HasColumnName("dia");
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.Minuto).HasColumnName("minuto");
            entity.Property(e => e.Segundo).HasColumnName("segundo");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxFechaError)
                .HasColumnType("date")
                .HasColumnName("txFechaError");
            entity.Property(e => e.TxMensajeError)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("txMensajeError");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.ErrorEnvioEmails)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_email_compania");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.ErrorEnvioEmails)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_email_documento");
        });

        modelBuilder.Entity<Esquema>(entity =>
        {
            entity.HasKey(e => new { e.CiTipoDocumento, e.CiTipoGeneracion });

            entity.ToTable("Esquema");

            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoGeneracion).HasColumnName("ciTipoGeneracion");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxVersion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txVersion");
            entity.Property(e => e.XmlEsquema)
                .HasColumnType("xml")
                .HasColumnName("xmlEsquema");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.Esquemas)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Esquema_Estado");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.Esquemas)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Esquema_TipoDocumento");

            entity.HasOne(d => d.CiTipoGeneracionNavigation).WithMany(p => p.Esquemas)
                .HasForeignKey(d => d.CiTipoGeneracion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Esquema_TipoGeneracion");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.CiEstado).HasName("PK__Estado__EB36E1E17F60ED59");

            entity.ToTable("Estado");

            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDescrpicion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("txDescrpicion");
        });

        modelBuilder.Entity<EstadoComprobante>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");
            entity.Property(e => e.TxNameEstadoConfig)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txNameEstadoConfig");
            entity.Property(e => e.TxtNameEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txtNameEstado");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("_Factura");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiContingenciaDet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ciContingenciaDet");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiEstadoRecepcionAutorizacion)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstadoRecepcionAutorizacion");
            entity.Property(e => e.CiFactura)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciFactura");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiTipoIdentificacionComprador)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacionComprador");
            entity.Property(e => e.QnImporteTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnImporteTotal");
            entity.Property(e => e.QnPropina)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnPropina");
            entity.Property(e => e.QnTotalDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalDescuento");
            entity.Property(e => e.QnTotalSinImpuestos)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalSinImpuestos");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxCodError)
                .HasColumnType("text")
                .HasColumnName("txCodError");
            entity.Property(e => e.TxEmail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("txEmail");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxFechaHoraAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txFechaHoraAutorizacion");
            entity.Property(e => e.TxGuiaRemision)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("txGuiaRemision");
            entity.Property(e => e.TxIdentificacionComprador)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionComprador");
            entity.Property(e => e.TxMensajeError)
                .HasColumnType("text")
                .HasColumnName("txMensajeError");
            entity.Property(e => e.TxMoneda)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("txMoneda");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxRazonSocialComprador)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialComprador");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.XmlDocumentoAutorizado)
                .IsUnicode(false)
                .HasColumnName("xmlDocumentoAutorizado");
        });

        modelBuilder.Entity<Factura1>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("Factura");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.CiContingenciaDet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ciContingenciaDet");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiEstadoRecepcionAutorizacion)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstadoRecepcionAutorizacion");
            entity.Property(e => e.CiFactura)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciFactura");
            entity.Property(e => e.CiNumeroIntento).HasColumnName("ciNumeroIntento");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiTipoIdentificacionComprador)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacionComprador");
            entity.Property(e => e.QnImporteTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnImporteTotal");
            entity.Property(e => e.QnPropina)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnPropina");
            entity.Property(e => e.QnTotalDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalDescuento");
            entity.Property(e => e.QnTotalSinImpuestos)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalSinImpuestos");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxCodError)
                .HasColumnType("text")
                .HasColumnName("txCodError");
            entity.Property(e => e.TxEmail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("txEmail");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxFechaHoraAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txFechaHoraAutorizacion");
            entity.Property(e => e.TxGuiaRemision)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("txGuiaRemision");
            entity.Property(e => e.TxIdentificacionComprador)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionComprador");
            entity.Property(e => e.TxMensajeError)
                .HasColumnType("text")
                .HasColumnName("txMensajeError");
            entity.Property(e => e.TxMoneda)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("txMoneda");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.TxRazonSocialComprador)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialComprador");
            entity.Property(e => e.XmlDocumentoAutorizado)
                .IsUnicode(false)
                .HasColumnName("xmlDocumentoAutorizado");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.Factura1s)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Compania");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.Factura1s)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_TipoDocumento");

            entity.HasOne(d => d.CiTipoEmisionNavigation).WithMany(p => p.Factura1s)
                .HasForeignKey(d => d.CiTipoEmision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_TipoEmision");

            entity.HasOne(d => d.CiTipoIdentificacionCompradorNavigation).WithMany(p => p.Factura1s)
                .HasForeignKey(d => d.CiTipoIdentificacionComprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_TipoIdentificacion");
        });

        modelBuilder.Entity<FacturaDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("_FacturaDetalle");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.QnCantidad).HasColumnName("qnCantidad");
            entity.Property(e => e.QnDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnDescuento");
            entity.Property(e => e.QnPrecioTotalSinImpuesto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnPrecioTotalSinImpuesto");
            entity.Property(e => e.QnPrecioUnitario)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("qnPrecioUnitario");
            entity.Property(e => e.TxCodigoAuxiliar)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoAuxiliar");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
        });

        modelBuilder.Entity<FacturaDetalle1>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoPrincipal });

            entity.ToTable("FacturaDetalle");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.QnCantidad).HasColumnName("qnCantidad");
            entity.Property(e => e.QnDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnDescuento");
            entity.Property(e => e.QnPrecioTotalSinImpuesto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnPrecioTotalSinImpuesto");
            entity.Property(e => e.QnPrecioUnitario)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("qnPrecioUnitario");
            entity.Property(e => e.TxCodigoAuxiliar)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoAuxiliar");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");

            entity.HasOne(d => d.Factura1).WithMany(p => p.FacturaDetalle1s)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaDetalle_Factura");
        });

        modelBuilder.Entity<FacturaDetalleAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiFacturaDetalleAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoPrincipal });

            entity.ToTable("FacturaDetalleAdicional");

            entity.Property(e => e.CiFacturaDetalleAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciFacturaDetalleAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.FacturaDetalle1).WithMany(p => p.FacturaDetalleAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxCodigoPrincipal })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaDetalleAdicional_FacturaDetalle");
        });

        modelBuilder.Entity<FacturaDetalleFormaPago>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("_FacturaDetalleFormaPago");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiFacturaDetalleFormaPago)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciFacturaDetalleFormaPago");
            entity.Property(e => e.QnTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotal");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxFormaPago)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("txFormaPago");
            entity.Property(e => e.TxPlazo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("txPlazo");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxUnidadTiempo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txUnidadTiempo");
        });

        modelBuilder.Entity<FacturaDetalleFormaPago1>(entity =>
        {
            entity.HasKey(e => new { e.CiFacturaDetalleFormaPago, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial }).HasName("PK_FacturaDetalleFormaPago_1");

            entity.ToTable("FacturaDetalleFormaPago");

            entity.Property(e => e.CiFacturaDetalleFormaPago)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciFacturaDetalleFormaPago");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.QnTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotal");
            entity.Property(e => e.TxFormaPago)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("txFormaPago");
            entity.Property(e => e.TxPlazo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("txPlazo");
            entity.Property(e => e.TxUnidadTiempo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txUnidadTiempo");
        });

        modelBuilder.Entity<FacturaDetalleImpuesto>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("_FacturaDetalleImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txTarifa");
        });

        modelBuilder.Entity<FacturaDetalleImpuesto1>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoPrincipal, e.TxCodigo, e.TxCodigoPorcentaje });

            entity.ToTable("FacturaDetalleImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txTarifa");

            entity.HasOne(d => d.FacturaDetalle1).WithMany(p => p.FacturaDetalleImpuesto1s)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxCodigoPrincipal })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaDetalleImpuesto_FacturaDetalle");
        });

        modelBuilder.Entity<FacturaInfoAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiFacturaInfoAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("FacturaInfoAdicional");

            entity.Property(e => e.CiFacturaInfoAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciFacturaInfoAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.Factura1).WithMany(p => p.FacturaInfoAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaInfoAdicional_Factura");
        });

        modelBuilder.Entity<FacturaReembolsoGasto>(entity =>
        {
            entity.HasKey(e => e.CiReembolso).HasName("PK__FacturaR__E995088857732A79");

            entity.ToTable("FacturaReembolsoGasto");

            entity.Property(e => e.CiReembolso).HasColumnName("ciReembolso");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CodPaisPagoProveedor)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codPaisPagoProveedor");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codigo");
            entity.Property(e => e.CodigoPorcentaje)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codigoPorcentaje");
            entity.Property(e => e.Detalle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("detalle");
            entity.Property(e => e.ImpIce)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impIce");
            entity.Property(e => e.ImpIrbpnr)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impIRBPNR");
            entity.Property(e => e.ImpIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impIva");
            entity.Property(e => e.SubTotExcentoIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotExcentoIva");
            entity.Property(e => e.SubTotIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotIva");
            entity.Property(e => e.SubTotIvaCero)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotIvaCero");
            entity.Property(e => e.SubTotNoIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotNoIva");
            entity.Property(e => e.Tarifa)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tarifa");
            entity.Property(e => e.TipoProveedor)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipoProveedor");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxIdProveedor)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txIdProveedor");
            entity.Property(e => e.TxNumDocumento)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txNumDocumento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxTipoIdProveedor)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txTipoIdProveedor");
            entity.Property(e => e.ValTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valTotal");
            entity.Property(e => e.ValorBase)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valorBase");
        });

        modelBuilder.Entity<FacturaTotalImpuesto>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigo, e.TxCodigoPorcentaje });

            entity.ToTable("FacturaTotalImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnDescuentoAdicional)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnDescuentoAdicional");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txTarifa");

            entity.HasOne(d => d.Factura1).WithMany(p => p.FacturaTotalImpuestos)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaTotalImpuesto_Factura");
        });

        modelBuilder.Entity<GuiaRemision>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("GuiaRemision");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.CiContingenciaDet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ciContingenciaDet");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiEstadoRecepcionAutorizacion)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstadoRecepcionAutorizacion");
            entity.Property(e => e.CiGuiaRemision)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciGuiaRemision");
            entity.Property(e => e.CiNumeroIntento).HasColumnName("ciNumeroIntento");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiTipoIdentificacionTransportista)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacionTransportista");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxCodError)
                .HasColumnType("text")
                .HasColumnName("txCodError");
            entity.Property(e => e.TxDireccionPartida)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDireccionPartida");
            entity.Property(e => e.TxEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txEmail");
            entity.Property(e => e.TxFechaFinTransporte)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaFinTransporte");
            entity.Property(e => e.TxFechaHoraAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txFechaHoraAutorizacion");
            entity.Property(e => e.TxFechaIniTransporte)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaIniTransporte");
            entity.Property(e => e.TxMensajeError)
                .HasColumnType("text")
                .HasColumnName("txMensajeError");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.TxPlaca)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("txPlaca");
            entity.Property(e => e.TxRazonSocialTransportista)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialTransportista");
            entity.Property(e => e.TxRise)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("txRise");
            entity.Property(e => e.TxRucTransportista)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txRucTransportista");
            entity.Property(e => e.XmlDocumentoAutorizado)
                .IsUnicode(false)
                .HasColumnName("xmlDocumentoAutorizado");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemision_Compania");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemision_TipoDocumento");

            entity.HasOne(d => d.CiTipoEmisionNavigation).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.CiTipoEmision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemision_TipoEmision");

            entity.HasOne(d => d.CiTipoIdentificacionTransportistaNavigation).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.CiTipoIdentificacionTransportista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemision_TipoIdentificacion");
        });

        modelBuilder.Entity<GuiaRemisionDestinatario>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxIdentificacionDestinatario });

            entity.ToTable("GuiaRemisionDestinatario");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxIdentificacionDestinatario)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionDestinatario");
            entity.Property(e => e.CiTipoDocumentoSustento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumentoSustento");
            entity.Property(e => e.TxCodigoEstablecimientoDestino)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigoEstablecimientoDestino");
            entity.Property(e => e.TxDireccionDestinatario)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDireccionDestinatario");
            entity.Property(e => e.TxDocumentoAduaneroUnico)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("txDocumentoAduaneroUnico");
            entity.Property(e => e.TxFechaEmisionDocumentoSustento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmisionDocumentoSustento");
            entity.Property(e => e.TxMotivoTraslado)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txMotivoTraslado");
            entity.Property(e => e.TxNumeroAutorizacionDocumentoSustento)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacionDocumentoSustento");
            entity.Property(e => e.TxNumeroDocumentoSustento)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("txNumeroDocumentoSustento");
            entity.Property(e => e.TxRazonSocialDestinatario)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialDestinatario");
            entity.Property(e => e.TxRuta)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRuta");

            entity.HasOne(d => d.GuiaRemision).WithMany(p => p.GuiaRemisionDestinatarios)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemisionDestinatario_GuiaRemision");
        });

        modelBuilder.Entity<GuiaRemisionDestinatarioDetalle>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxIdentificacionDestinatario, e.TxCodigoInterno });

            entity.ToTable("GuiaRemisionDestinatarioDetalle");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxIdentificacionDestinatario)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionDestinatario");
            entity.Property(e => e.TxCodigoInterno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoInterno");
            entity.Property(e => e.QnCantidad)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("qnCantidad");
            entity.Property(e => e.TxCodigoAdicional)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoAdicional");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");

            entity.HasOne(d => d.GuiaRemisionDestinatario).WithMany(p => p.GuiaRemisionDestinatarioDetalles)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxIdentificacionDestinatario })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemisionDestinatarioDetalle_GuiaRemisionDestinatario");
        });

        modelBuilder.Entity<GuiaRemisionDestinatarioDetalleAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiGuiaRemisionDestinatarioDetalleAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxIdentificacionDestinatario, e.TxCodigoInterno });

            entity.ToTable("GuiaRemisionDestinatarioDetalleAdicional");

            entity.Property(e => e.CiGuiaRemisionDestinatarioDetalleAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciGuiaRemisionDestinatarioDetalleAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxIdentificacionDestinatario)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionDestinatario");
            entity.Property(e => e.TxCodigoInterno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoInterno");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.GuiaRemisionDestinatarioDetalle).WithMany(p => p.GuiaRemisionDestinatarioDetalleAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxIdentificacionDestinatario, d.TxCodigoInterno })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemisionDestinatarioDetalleAdicional_GuiaRemisionDestinatarioDetalle");
        });

        modelBuilder.Entity<GuiaRemisionInfoAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiGuiaRemisionInfoAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("GuiaRemisionInfoAdicional");

            entity.Property(e => e.CiGuiaRemisionInfoAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciGuiaRemisionInfoAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.GuiaRemision).WithMany(p => p.GuiaRemisionInfoAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaRemisionInfoAdicional_GuiaRemision");
        });

        modelBuilder.Entity<Liquidacion>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("Liquidacion");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.CiContingenciaDet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ciContingenciaDet");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiEstadoRecepcionAutorizacion)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstadoRecepcionAutorizacion");
            entity.Property(e => e.CiLiquidacion)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciLiquidacion");
            entity.Property(e => e.CiNumeroIntento).HasColumnName("ciNumeroIntento");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiTipoIdentificacionProvedor)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacionProvedor");
            entity.Property(e => e.CodDocReembolso)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("codDocReembolso");
            entity.Property(e => e.QnImporteTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnImporteTotal");
            entity.Property(e => e.QnTotalDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalDescuento");
            entity.Property(e => e.QnTotalSinImpuestos)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalSinImpuestos");
            entity.Property(e => e.TotalBaseImponibleReembolso)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalBaseImponibleReembolso");
            entity.Property(e => e.TotalComprobantesReembolso)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalComprobantesReembolso");
            entity.Property(e => e.TotalImpuestoReembolso)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalImpuestoReembolso");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxCodError)
                .HasColumnType("text")
                .HasColumnName("txCodError");
            entity.Property(e => e.TxEmail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("txEmail");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxFechaHoraAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txFechaHoraAutorizacion");
            entity.Property(e => e.TxIdentificacionProvedor)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionProvedor");
            entity.Property(e => e.TxMensajeError)
                .HasColumnType("text")
                .HasColumnName("txMensajeError");
            entity.Property(e => e.TxMoneda)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("txMoneda");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.TxRazonSocialProvedor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialProvedor");
            entity.Property(e => e.XmlDocumentoAutorizado)
                .IsUnicode(false)
                .HasColumnName("xmlDocumentoAutorizado");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.Liquidacions)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Liquidacion_Compania");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.Liquidacions)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Liquidacion_TipoDocumento");

            entity.HasOne(d => d.CiTipoEmisionNavigation).WithMany(p => p.Liquidacions)
                .HasForeignKey(d => d.CiTipoEmision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Liquidacion_TipoEmision");
        });

        modelBuilder.Entity<LiquidacionDetalle>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoPrincipal });

            entity.ToTable("LiquidacionDetalle");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.QnCantidad).HasColumnName("qnCantidad");
            entity.Property(e => e.QnDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnDescuento");
            entity.Property(e => e.QnPrecioTotalSinImpuesto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnPrecioTotalSinImpuesto");
            entity.Property(e => e.QnPrecioUnitario)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("qnPrecioUnitario");
            entity.Property(e => e.TxCodigoAuxiliar)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoAuxiliar");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");
        });

        modelBuilder.Entity<LiquidacionDetalleAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiLiquidacionDetalleAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoPrincipal });

            entity.ToTable("LiquidacionDetalleAdicional");

            entity.Property(e => e.CiLiquidacionDetalleAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciLiquidacionDetalleAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.LiquidacionDetalle).WithMany(p => p.LiquidacionDetalleAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxCodigoPrincipal })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LiquidacionDetalleAdicional_LiquidacionDetalle");
        });

        modelBuilder.Entity<LiquidacionDetalleFormaPago>(entity =>
        {
            entity.HasKey(e => new { e.CiLiquidacionDetalleFormaPago, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial }).HasName("PK_LiquidacionDetalleFormaPago_1");

            entity.ToTable("LiquidacionDetalleFormaPago");

            entity.Property(e => e.CiLiquidacionDetalleFormaPago)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciLiquidacionDetalleFormaPago");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.QnTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotal");
            entity.Property(e => e.TxFormaPago)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("txFormaPago");
            entity.Property(e => e.TxPlazo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("txPlazo");
            entity.Property(e => e.TxUnidadTiempo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txUnidadTiempo");
        });

        modelBuilder.Entity<LiquidacionDetalleImpuesto>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoPrincipal, e.TxCodigo, e.TxCodigoPorcentaje });

            entity.ToTable("LiquidacionDetalleImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoPrincipal)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoPrincipal");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txTarifa");

            entity.HasOne(d => d.LiquidacionDetalle).WithMany(p => p.LiquidacionDetalleImpuestos)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxCodigoPrincipal })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LiquidacionDetalleImpuesto_LiquidacionDetalle");
        });

        modelBuilder.Entity<LiquidacionInfoAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiLiquidacionInfoAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("LiquidacionInfoAdicional");

            entity.Property(e => e.CiLiquidacionInfoAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciLiquidacionInfoAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");
        });

        modelBuilder.Entity<LiquidacionReembolso>(entity =>
        {
            entity.HasKey(e => e.CiLiquidacionreembolso).HasName("PK__Liquidac__2D164316B9CA8D8B");

            entity.ToTable("LiquidacionReembolso");

            entity.Property(e => e.CiLiquidacionreembolso).HasColumnName("ciLiquidacionreembolso");
            entity.Property(e => e.BaseImponibleReembolso)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("baseImponibleReembolso");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CodDocReembolso)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codigo");
            entity.Property(e => e.CodigoPorcentaje)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codigoPorcentaje");
            entity.Property(e => e.EstabDocReembolso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ImpuestoReembolso)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impuestoReembolso");
            entity.Property(e => e.NumeroautorizacionDocReemb)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("numeroautorizacionDocReemb");
            entity.Property(e => e.PtoEmiDocReembolso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SecuencialDocReembolso)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.Tarifa)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tarifa");
            entity.Property(e => e.TxCodPaisPagoProveedorReembolso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodPaisPagoProveedorReembolso");
            entity.Property(e => e.TxFechaEmisionDocReembolso)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmisionDocReembolso");
            entity.Property(e => e.TxIdentificacionProveedorReembolso)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionProveedorReembolso");
            entity.Property(e => e.TxTipoIdentificacionProveedorReembolso)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txTipoIdentificacionProveedorReembolso");
            entity.Property(e => e.TxTipoProveedorReembolso)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txTipoProveedorReembolso");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.LiquidacionReembolsos)
                .HasForeignKey(d => d.CiCompania)
                .HasConstraintName("FK__Liquidaci__ciCom__5CACADF9");
        });

        modelBuilder.Entity<LiquidacionTotalImpuesto>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigo, e.TxCodigoPorcentaje });

            entity.ToTable("LiquidacionTotalImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnDescuentoAdicional)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnDescuentoAdicional");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txTarifa");
        });

        modelBuilder.Entity<NotaCredito>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("NotaCredito");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.CiContingenciaDet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ciContingenciaDet");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiEstadoRecepcionAutorizacion)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstadoRecepcionAutorizacion");
            entity.Property(e => e.CiNotaCredito)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciNotaCredito");
            entity.Property(e => e.CiNumeroIntento).HasColumnName("ciNumeroIntento");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoDocumentoModificado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumentoModificado");
            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiTipoIdentificacionComprador)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacionComprador");
            entity.Property(e => e.QnTotalSinImpuestos)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalSinImpuestos");
            entity.Property(e => e.QnValorModificacion)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValorModificacion");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxCodError)
                .HasColumnType("text")
                .HasColumnName("txCodError");
            entity.Property(e => e.TxEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txEmail");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxFechaEmisionDocumentoModificado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmisionDocumentoModificado");
            entity.Property(e => e.TxFechaHoraAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txFechaHoraAutorizacion");
            entity.Property(e => e.TxIdentificacionComprador)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionComprador");
            entity.Property(e => e.TxMensajeError)
                .HasColumnType("text")
                .HasColumnName("txMensajeError");
            entity.Property(e => e.TxMoneda)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("txMoneda");
            entity.Property(e => e.TxMotivo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txMotivo");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.TxNumeroDocumentoModificado)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("txNumeroDocumentoModificado");
            entity.Property(e => e.TxRazonSocialComprador)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialComprador");
            entity.Property(e => e.TxRise)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("txRise");
            entity.Property(e => e.XmlDocumentoAutorizado)
                .IsUnicode(false)
                .HasColumnName("xmlDocumentoAutorizado");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.NotaCreditos)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCredito_Compania");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.NotaCreditoCiTipoDocumentoNavigations)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCredito_TipoDocumento");

            entity.HasOne(d => d.CiTipoDocumentoModificadoNavigation).WithMany(p => p.NotaCreditoCiTipoDocumentoModificadoNavigations)
                .HasForeignKey(d => d.CiTipoDocumentoModificado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCredito_TipoDocumentoModificado");

            entity.HasOne(d => d.CiTipoEmisionNavigation).WithMany(p => p.NotaCreditos)
                .HasForeignKey(d => d.CiTipoEmision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCredito_TipoEmision");

            entity.HasOne(d => d.CiTipoIdentificacionCompradorNavigation).WithMany(p => p.NotaCreditos)
                .HasForeignKey(d => d.CiTipoIdentificacionComprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCredito_TipoIdentificacion");
        });

        modelBuilder.Entity<NotaCreditoDetalle>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoInterno });

            entity.ToTable("NotaCreditoDetalle");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoInterno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoInterno");
            entity.Property(e => e.QnCantidad).HasColumnName("qnCantidad");
            entity.Property(e => e.QnDescuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnDescuento");
            entity.Property(e => e.QnPrecioTotalSinImpuesto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnPrecioTotalSinImpuesto");
            entity.Property(e => e.QnPrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnPrecioUnitario");
            entity.Property(e => e.TxCodigoAdicional)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoAdicional");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");

            entity.HasOne(d => d.NotaCredito).WithMany(p => p.NotaCreditoDetalles)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCreditoDetalle_NotaCredito");
        });

        modelBuilder.Entity<NotaCreditoDetalleAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiNotaCreditoDetalleAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoInterno });

            entity.ToTable("NotaCreditoDetalleAdicional");

            entity.Property(e => e.CiNotaCreditoDetalleAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciNotaCreditoDetalleAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoInterno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoInterno");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.NotaCreditoDetalle).WithMany(p => p.NotaCreditoDetalleAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxCodigoInterno })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCreditoDetalleAdicional_NotaCreditoDetalle");
        });

        modelBuilder.Entity<NotaCreditoDetalleImpuesto>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigoInterno, e.TxCodigo, e.TxCodigoPorcentaje });

            entity.ToTable("NotaCreditoDetalleImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigoInterno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoInterno");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txTarifa");

            entity.HasOne(d => d.NotaCreditoDetalle).WithMany(p => p.NotaCreditoDetalleImpuestos)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial, d.TxCodigoInterno })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCreditoDetalleImpuesto_NotaCreditoDetalle");
        });

        modelBuilder.Entity<NotaCreditoInfoAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiNotaCreditoInfoAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("NotaCreditoInfoAdicional");

            entity.Property(e => e.CiNotaCreditoInfoAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciNotaCreditoInfoAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.NotaCredito).WithMany(p => p.NotaCreditoInfoAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCreditoInfoAdicional_NotaCredito");
        });

        modelBuilder.Entity<NotaCreditoTotalImpuesto>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigo, e.TxCodigoPorcentaje });

            entity.ToTable("NotaCreditoTotalImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");

            entity.HasOne(d => d.NotaCredito).WithMany(p => p.NotaCreditoTotalImpuestos)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaCreditoTotalImpuesto_NotaCredito");
        });

        modelBuilder.Entity<NotaDebito>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("NotaDebito");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.CiContingenciaDet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ciContingenciaDet");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiEstadoRecepcionAutorizacion)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstadoRecepcionAutorizacion");
            entity.Property(e => e.CiNotaDebito)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciNotaDebito");
            entity.Property(e => e.CiNumeroIntento).HasColumnName("ciNumeroIntento");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiTipoDocumentoModificado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumentoModificado");
            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiTipoIdentificacionComprador)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacionComprador");
            entity.Property(e => e.QnTotalSinImpuestos)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnTotalSinImpuestos");
            entity.Property(e => e.QnValorTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValorTotal");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxCodError)
                .HasColumnType("text")
                .HasColumnName("txCodError");
            entity.Property(e => e.TxEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txEmail");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxFechaEmisionDocumentoModificado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaEmisionDocumentoModificado");
            entity.Property(e => e.TxFechaHoraAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txFechaHoraAutorizacion");
            entity.Property(e => e.TxIdentificacionComprador)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("txIdentificacionComprador");
            entity.Property(e => e.TxMensajeError)
                .HasColumnType("text")
                .HasColumnName("txMensajeError");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(49)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.TxNumeroDocumentoModificado)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("txNumeroDocumentoModificado");
            entity.Property(e => e.TxRazonSocialComprador)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazonSocialComprador");
            entity.Property(e => e.TxRise)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("txRise");
            entity.Property(e => e.XmlDocumentoAutorizado)
                .IsUnicode(false)
                .HasColumnName("xmlDocumentoAutorizado");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.NotaDebitos)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebito_Compania");

            entity.HasOne(d => d.CiTipoDocumentoNavigation).WithMany(p => p.NotaDebitoCiTipoDocumentoNavigations)
                .HasForeignKey(d => d.CiTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebito_TipoDocumento");

            entity.HasOne(d => d.CiTipoDocumentoModificadoNavigation).WithMany(p => p.NotaDebitoCiTipoDocumentoModificadoNavigations)
                .HasForeignKey(d => d.CiTipoDocumentoModificado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebito_TipoDocumentoModificado");

            entity.HasOne(d => d.CiTipoEmisionNavigation).WithMany(p => p.NotaDebitos)
                .HasForeignKey(d => d.CiTipoEmision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebito_TipoEmision");

            entity.HasOne(d => d.CiTipoIdentificacionCompradorNavigation).WithMany(p => p.NotaDebitos)
                .HasForeignKey(d => d.CiTipoIdentificacionComprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebito_TipoIdentificacion");
        });

        modelBuilder.Entity<NotaDebitoImpuesto>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial, e.TxCodigo, e.TxCodigoPorcentaje });

            entity.ToTable("NotaDebitoImpuesto");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxCodigo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txCodigo");
            entity.Property(e => e.TxCodigoPorcentaje)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txCodigoPorcentaje");
            entity.Property(e => e.QnBaseImponible)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnBaseImponible");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxTarifa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("txTarifa");

            entity.HasOne(d => d.NotaDebito).WithMany(p => p.NotaDebitoImpuestos)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebitoImpuesto_NotaDebito");
        });

        modelBuilder.Entity<NotaDebitoInfoAdicional>(entity =>
        {
            entity.HasKey(e => new { e.CiNotaDebitoInfoAdicional, e.CiCompania, e.TxEstablecimiento, e.TxPuntoEmision, e.TxSecuencial });

            entity.ToTable("NotaDebitoInfoAdicional");

            entity.Property(e => e.CiNotaDebitoInfoAdicional)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciNotaDebitoInfoAdicional");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxValor)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txValor");

            entity.HasOne(d => d.NotaDebito).WithMany(p => p.NotaDebitoInfoAdicionals)
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebitoInfoAdicional_NotaDebito");
        });

        modelBuilder.Entity<NotaDebitoMotivo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NotaDebitoMotivo");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.QnValor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("qnValor");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxRazon)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txRazon");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("txSecuencial");

            entity.HasOne(d => d.NotaDebito).WithMany()
                .HasForeignKey(d => new { d.CiCompania, d.TxEstablecimiento, d.TxPuntoEmision, d.TxSecuencial })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotaDebitoMotivo_NotaDebito");
        });

        modelBuilder.Entity<OpcionesPerfile>(entity =>
        {
            entity.HasKey(e => e.CiCodigoPerfiles);

            entity.Property(e => e.CiCodigoPerfiles)
                .ValueGeneratedNever()
                .HasColumnName("ciCodigoPerfiles");
            entity.Property(e => e.CiCodigoOpcion).HasColumnName("ciCodigoOpcion");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxAccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txAccion");
            entity.Property(e => e.TxControlador)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txControlador");
            entity.Property(e => e.TxIconoOpcion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("txIconoOpcion");
            entity.Property(e => e.TxNombreOpcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txNombreOpcion");
        });

        modelBuilder.Entity<ParametrizarDocumento>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PK_RegistroDocumentos");

            entity.Property(e => e.IdRegistro).HasColumnName("idRegistro");
            entity.Property(e => e.CantidadAutorizacion).HasColumnName("cantidadAutorizacion");
            entity.Property(e => e.CantidadCorreo).HasColumnName("cantidadCorreo");
            entity.Property(e => e.CantidadFirma).HasColumnName("cantidadFirma");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CorreoAutorizacion).HasColumnName("correoAutorizacion");
        });

        modelBuilder.Entity<Perfile>(entity =>
        {
            entity.HasKey(e => e.TxCodigoPerfiles).HasName("PK_Perfiles_1");

            entity.Property(e => e.TxCodigoPerfiles)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txCodigoPerfiles");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxNombreOpcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txNombreOpcion");
            entity.Property(e => e.TxOpcionIcono)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txOpcionIcono");
            entity.Property(e => e.TxRuta)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txRuta");
        });

        modelBuilder.Entity<PerfilesRespaldo>(entity =>
        {
            entity.HasKey(e => e.CiCodigoPerfiles);

            entity.ToTable("Perfiles_Respaldo");

            entity.Property(e => e.CiCodigoPerfiles)
                .ValueGeneratedNever()
                .HasColumnName("ciCodigoPerfiles");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxNombreOpcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txNombreOpcion");
            entity.Property(e => e.TxOpcionIcono)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("txOpcionIcono");
        });

        modelBuilder.Entity<PerfilesUsuario>(entity =>
        {
            entity.HasKey(e => new { e.CiCodigoPerfilesUsuarios, e.TxCodigoUsuarios, e.TxCodigoPerfiles });

            entity.Property(e => e.CiCodigoPerfilesUsuarios)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciCodigoPerfilesUsuarios");
            entity.Property(e => e.TxCodigoUsuarios)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoUsuarios");
            entity.Property(e => e.TxCodigoPerfiles)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txCodigoPerfiles");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxFechaCreación)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaCreación");
            entity.Property(e => e.TxHoraCreación)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txHoraCreación");
            entity.Property(e => e.TxRura)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("txRura");

            entity.HasOne(d => d.TxCodigoPerfilesNavigation).WithMany(p => p.PerfilesUsuarios)
                .HasForeignKey(d => d.TxCodigoPerfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilesUsuarios_Perfiles");
        });

        modelBuilder.Entity<PerfilesUsuariosRespaldo>(entity =>
        {
            entity.HasKey(e => new { e.CiCodigoPerfilesUsuarios, e.TxCodigoUsuarios, e.CiCodigoPerfiles });

            entity.ToTable("PerfilesUsuarios_Respaldo");

            entity.Property(e => e.CiCodigoPerfilesUsuarios)
                .ValueGeneratedOnAdd()
                .HasColumnName("ciCodigoPerfilesUsuarios");
            entity.Property(e => e.TxCodigoUsuarios)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("txCodigoUsuarios");
            entity.Property(e => e.CiCodigoPerfiles).HasColumnName("ciCodigoPerfiles");
            entity.Property(e => e.CiCodigoOpciones).HasColumnName("ciCodigoOpciones");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxFechaCreación)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txFechaCreación");
            entity.Property(e => e.TxHoraCreación)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("txHoraCreación");
        });

        modelBuilder.Entity<Prueba>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiIndetificador).HasColumnName("ciIndetificador");
            entity.Property(e => e.CiNumeroIntento).HasColumnName("ciNumeroIntento");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.FechaAutorizacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxNumeroAutorizacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txNumeroAutorizacion");
            entity.Property(e => e.XmlDocumento)
                .IsUnicode(false)
                .HasColumnName("xmlDocumento");
        });

        modelBuilder.Entity<PruebaIcono>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxCodigoPerfiles)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txCodigoPerfiles");
            entity.Property(e => e.TxIconoOpcion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("txIconoOpcion");
            entity.Property(e => e.TxNombreOpcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txNombreOpcion");
            entity.Property(e => e.TxRuta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txRuta");
        });

        modelBuilder.Entity<ReembolsoGasto>(entity =>
        {
            entity.HasKey(e => e.CiReembolso).HasName("PK__Reembols__E9950888D2892513");

            entity.Property(e => e.CiReembolso).HasColumnName("ciReembolso");
            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CodPaisPagoProveedor)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codPaisPagoProveedor");
            entity.Property(e => e.Codigo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codigo");
            entity.Property(e => e.CodigoPorcentaje)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codigoPorcentaje");
            entity.Property(e => e.Detalle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("detalle");
            entity.Property(e => e.ImpIce)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impIce");
            entity.Property(e => e.ImpIrbpnr)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impIRBPNR");
            entity.Property(e => e.ImpIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impIva");
            entity.Property(e => e.SubTotExcentoIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotExcentoIva");
            entity.Property(e => e.SubTotIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotIva");
            entity.Property(e => e.SubTotIvaCero)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotIvaCero");
            entity.Property(e => e.SubTotNoIva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotNoIva");
            entity.Property(e => e.Tarifa)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tarifa");
            entity.Property(e => e.TipoProveedor)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipoProveedor");
            entity.Property(e => e.TxClaveAcceso)
                .HasMaxLength(49)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txClaveAcceso");
            entity.Property(e => e.TxEstablecimiento)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txEstablecimiento");
            entity.Property(e => e.TxFechaEmision)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txFechaEmision");
            entity.Property(e => e.TxIdProveedor)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txIdProveedor");
            entity.Property(e => e.TxNumDocumento)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txNumDocumento");
            entity.Property(e => e.TxPuntoEmision)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txPuntoEmision");
            entity.Property(e => e.TxSecuencial)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txSecuencial");
            entity.Property(e => e.TxTipoIdProveedor)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("txTipoIdProveedor");
            entity.Property(e => e.ValTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valTotal");
            entity.Property(e => e.ValorBase)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valorBase");
        });

        modelBuilder.Entity<Smtp>(entity =>
        {
            entity.HasKey(e => e.CiCompania);

            entity.ToTable("SMTP");

            entity.Property(e => e.CiCompania)
                .ValueGeneratedNever()
                .HasColumnName("ciCompania");
            entity.Property(e => e.ActivarNotificacion).HasColumnName("activarNotificacion");
            entity.Property(e => e.Asunto)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Cc)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("cc");
            entity.Property(e => e.EmailCredencial)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("emailCredencial");
            entity.Property(e => e.EnableSsl)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.HostServidor)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MailAddressfrom)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Para)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("para");
            entity.Property(e => e.PassCredencial)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("passCredencial");
            entity.Property(e => e.Puerto).HasColumnName("puerto");
            entity.Property(e => e.UrlPortal)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("urlPortal");

            entity.HasOne(d => d.CiCompaniaNavigation).WithOne(p => p.Smtp)
                .HasForeignKey<Smtp>(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SMTP_Compania");
        });

        modelBuilder.Entity<SriMensajeError>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SriMensajeError");

            entity.Property(e => e.CiCodEstado)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ciCodEstado");
            entity.Property(e => e.CiCodSri).HasColumnName("ciCodSri");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDetalle)
                .IsUnicode(false)
                .HasColumnName("txDetalle");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => new { e.CiCompania, e.CiSucursal });

            entity.ToTable("Sucursal");

            entity.Property(e => e.CiCompania).HasColumnName("ciCompania");
            entity.Property(e => e.CiSucursal)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciSucursal");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDireccion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txDireccion");

            entity.HasOne(d => d.CiCompaniaNavigation).WithMany(p => p.Sucursals)
                .HasForeignKey(d => d.CiCompania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sucursal_Compania");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.Sucursals)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sucursal_Estado");
        });

        modelBuilder.Entity<TiempoServicio>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TiempoServicio");

            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.HoraFin)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("horaFin");
            entity.Property(e => e.HoraInicio)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("horaInicio");
            entity.Property(e => e.IdRegistro).HasColumnName("idRegistro");
            entity.Property(e => e.IdServicioWindows).HasColumnName("idServicioWindows");
        });

        modelBuilder.Entity<TipoAmbiente>(entity =>
        {
            entity.HasKey(e => e.CiTipoAmbiente).HasName("PK__TipoAmbi__6AE464A059147F5A");

            entity.ToTable("TipoAmbiente");

            entity.Property(e => e.CiTipoAmbiente)
                .ValueGeneratedNever()
                .HasColumnName("ciTipoAmbiente");
            entity.Property(e => e.TxDescrpicion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("txDescrpicion");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.CiTipoDocumento).HasName("PK__TipoDocu__8C36D5B31A14E395");

            entity.ToTable("TipoDocumento");

            entity.Property(e => e.CiTipoDocumento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoDocumento");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.TipoDocumentos)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoDocumento_Estado");
        });

        modelBuilder.Entity<TipoEmision>(entity =>
        {
            entity.HasKey(e => e.CiTipoEmision).HasName("PK__TipoEmis__B2C94393239E4DCF");

            entity.ToTable("TipoEmision");

            entity.Property(e => e.CiTipoEmision).HasColumnName("ciTipoEmision");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.TipoEmisions)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoEmision_Estado");
        });

        modelBuilder.Entity<TipoErrorSri>(entity =>
        {
            entity.HasKey(e => e.CiCodTipoError);

            entity.ToTable("TipoErrorSRI");

            entity.Property(e => e.CiCodTipoError)
                .ValueGeneratedNever()
                .HasColumnName("ciCodTipoError");
            entity.Property(e => e.TxDescripcionTipoError)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("txDescripcionTipoError");
        });

        modelBuilder.Entity<TipoGeneracion>(entity =>
        {
            entity.HasKey(e => e.CiTipoGeneracion).HasName("PK__TipoGene__9081EC951ED998B2");

            entity.ToTable("TipoGeneracion");

            entity.Property(e => e.CiTipoGeneracion).HasColumnName("ciTipoGeneracion");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.TipoGeneracions)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoGeneracion_Estado");
        });

        modelBuilder.Entity<TipoIdentificacion>(entity =>
        {
            entity.HasKey(e => e.CiTipoIdentificacion).HasName("PK__TipoIden__885FC218286302EC");

            entity.ToTable("TipoIdentificacion");

            entity.Property(e => e.CiTipoIdentificacion)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciTipoIdentificacion");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxDescripcion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("txDescripcion");

            entity.HasOne(d => d.CiEstadoNavigation).WithMany(p => p.TipoIdentificacions)
                .HasForeignKey(d => d.CiEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoIdentificacion_Estado");
        });

        modelBuilder.Entity<UrlSri>(entity =>
        {
            entity.HasKey(e => e.CiUrlSri);

            entity.ToTable("UrlSRI");

            entity.Property(e => e.CiUrlSri).HasColumnName("ciUrlSRI");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.CiTipoAmbiente)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciTipoAmbiente");
            entity.Property(e => e.TxUrlAutorizacion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txUrlAutorizacion");
            entity.Property(e => e.TxUrlRecepcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("txUrlRecepcion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => new { e.TxCodigoUsuario, e.TxUsuario }).HasName("PK_Usuarios_1");

            entity.Property(e => e.TxCodigoUsuario)
                .ValueGeneratedOnAdd()
                .HasColumnName("txCodigoUsuario");
            entity.Property(e => e.TxUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txUsuario");
            entity.Property(e => e.CiEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ciEstado");
            entity.Property(e => e.TxCodigoPerfiles)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txCodigoPerfiles");
            entity.Property(e => e.TxNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("txNombre");
            entity.Property(e => e.TxPassword)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("txPassword");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
