using Proyecto.Application.Profiles;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Repository.Implementations;
using Proyecto.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Proyecto.Web;
using Proyecto.Application.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure D.I.
builder.Services.AddTransient<IRepositoryPais, RepositoryPais>();
builder.Services.AddTransient<IServicesPais, ServicePais>();

builder.Services.AddTransient<IRepositoryTarjeta, RepositoryTarjeta>();
builder.Services.AddTransient<IServicesTarjeta, ServiceTarjeta>();

// config Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<PaisProfile>();
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<TarjetaProfile>();
});

// Config Connection to SQLServer DataBase
builder.Services.AddDbContext<ProyectoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
