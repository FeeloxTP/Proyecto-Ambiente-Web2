using Proyecto.Application.Profiles;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Repository.Implementations;
using Proyecto.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Serilog.Events;
using Serilog;
using Proyecto.Web.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Config;

var builder = WebApplication.CreateBuilder(args);
// Mapping AppConfig Class to read  appsettings.json
builder.Services.Configure<AppConfig>(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure D.I.
builder.Services.AddTransient<IRepositoryPais, RepositoryPais>();
builder.Services.AddTransient<IServicesPais, ServicePais>();

builder.Services.AddTransient<IRepositoryTarjeta, RepositoryTarjeta>();
builder.Services.AddTransient<IServicesTarjeta, ServiceTarjeta>();

builder.Services.AddTransient<IRepositoryCliente, RepositoryCliente>();
builder.Services.AddTransient<IServiceCliente, ServiceCliente>();

builder.Services.AddTransient<IRepositoryNFT, RepositoryNFT>();
builder.Services.AddTransient<IServiceNFT, ServiceNFT>();

builder.Services.AddTransient<IRepositoryFactura, RepositoryFactura>();
builder.Services.AddTransient<IServiceFactura, ServiceFactura>();

builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();

builder.Services.AddTransient<IRepositoryPerfil, RepositoryPerfil>();
builder.Services.AddTransient<IServicePerfil, ServicePerfil>();

builder.Services.AddTransient<IRepositoryMovimientosCompras, RepositoryMovimientosCompras>();
builder.Services.AddTransient<IServiceMovimientosCompras, ServiceMovimientosCompras>();

builder.Services.AddTransient<IRepositoryFacturaDetalle, RepositoryFacturaDetalle>();

builder.Services.AddTransient<IServiceReportes, ServiceReportes>();

//// Security
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});

// config Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<PaisProfile>();
    config.AddProfile<TarjetaProfile>();
    config.AddProfile<ClienteProfile>();
    config.AddProfile<NFTProfile>();
    config.AddProfile<FacturaProfile>();
    config.AddProfile<UsuarioProfile>();
    config.AddProfile<PerfilProfile>();
    config.AddProfile<MovimientosComprasProfile>();
});

// Config Connection to SQLServer DataBase
builder.Services.AddDbContext<ProyectoContext>(options =>
{
    // it read appsettings.json file
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Logger. P.E. Verbose = it shows SQl Statement
var logger = new LoggerConfiguration()
                    // .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(LogEventLevel.Verbose)
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

builder.Host.UseSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

// Error access control 
app.UseStatusCodePagesWithReExecute("/error/{0}");

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activate Antiforgery DEBE COLOCARSE ACA!
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
