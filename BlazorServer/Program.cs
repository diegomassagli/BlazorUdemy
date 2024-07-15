using BlazorServer.Servicios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using CurrieTechnologies.Razor.SweetAlert2;


var logger = LogManager.Setup()
              .LoadConfigurationFromXml("nlog.config")
              .GetCurrentClassLogger();


try
{
    var builder = WebApplication.CreateBuilder(args);

    IConfiguration Configuration;
    // Add services to the container.
    var config = builder.Configuration;

    //NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //SweetAlert2
    builder.Services.AddSweetAlert2();

    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddHttpClient<IServicioAlumnos, ServicioAlumnos>(cliente =>  //donde cliente representa una instancia de httpClient que se esta configurando. registra un HttpClient configurado para la interfaz IServicioAlumnos y su implementación ServicioAlumnos
    {
        cliente.BaseAddress = new Uri(config["urlAPI"]);
    });

    builder.Services.AddHttpClient<IServicioCurso, ServicioCurso>(cliente =>
    {
        cliente.BaseAddress = new Uri(config["urlAPI"]);
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

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


