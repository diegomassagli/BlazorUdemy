using APIAlumnos.Datos;
using APIAlumnos.Repositorio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;


var logger = LogManager.Setup()
              .LoadConfigurationFromXml("nlog.config")
              .GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    IConfiguration Configuracion;
    // Add services to the container.
    //string connectionString = builder.Configuration.GetConnectionString("SQL");
    //var cadenaConexionSqlConfiguracion = new AccesoDatos(connectionString);
    var config = builder.Configuration;
    var cadenaConexionSqlConfiguracion = new AccesoDatos(config.GetConnectionString("SQL"));
    builder.Services.AddSingleton(cadenaConexionSqlConfiguracion);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<IRepositorioAlumnos, RepositorioAlumnos>();
    builder.Services.AddScoped<IRepositorioCursos, RepositorioCursos>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
    {
        // NLog: catch any exception and log it.
        logger.Error(ex, "Stopped program because of exception");
        throw;
    }
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}

