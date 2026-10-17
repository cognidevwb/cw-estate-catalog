using CwEstate.Catalog.Data;
using CwEstate.Catalog.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<CatalogDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Catalog")));
builder.Services.AddScoped<PricingService>();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost");
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.MapControllers();
app.Run();
