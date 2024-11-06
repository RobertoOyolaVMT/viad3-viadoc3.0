using Microsoft.EntityFrameworkCore;
using ReportesViaDocNetCore.Interfaces;
using ReportesViaDocNetCore.Models;
using ReportesViaDocNetCore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FacturacionElectronicaQaContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("ViaDocConexion")));

builder.Services.AddScoped<IGeneraRideFactura, GeneraRideFacturaServices>();
builder.Services.AddScoped<ICatalogos, CatalogoServices>();
builder.Services.AddScoped<IGeneraRideCompRetencion, GeneraRideCompRetencionServices>();
builder.Services.AddScoped<IGeneraRideNotaCredito, GeneraRideNotaCreditoServices>();
builder.Services.AddScoped<IGeneraRideNotaDebito, GeneraRideNotaDebitoServices>();
builder.Services.AddScoped<IGeneraRideLiquidacion, GeneraRideLiquidacionServices>();
builder.Services.AddScoped<IGeneraRideGuiaRemision, GeneraRideGuiaRemisionServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
