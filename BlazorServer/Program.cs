using BlazorServer.Servicios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;



var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration;
// Add services to the container.
var config = builder.Configuration;

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
