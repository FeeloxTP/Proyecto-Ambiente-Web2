using Microsoft.EntityFrameworkCore;
using Proyecto.Application.Profiles;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Repository.Implementations;
using Proyecto.Infraestructure.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Configure D.I
builder.Services.AddTransient<IRepositoryNFT, RepositoryNFT>();
builder.Services.AddTransient<IServiceNFT, ServiceNFT>();

// Config Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<NFTProfile>();
});

// Config Connection to SQLServer DataBase
builder.Services.AddDbContext<ProyectoContext>(options =>
{
    // it read appsettings.json file
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

//Configure Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Configure Swagger 
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
