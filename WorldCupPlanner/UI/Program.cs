using UI.Components;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Interfaces;
using UI.Estado;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;
using Servicios.Modelo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<SqlContexto>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString(name: "DefaultConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure()));


builder.Services.AddScoped<SqlContexto>();

builder.Services.AddScoped<BaseDeDatosEnMemoria>();
builder.Services.AddScoped<UsuarioRepositorioSql>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorioSql>();


builder.Services.AddScoped<IEquipoRepositorio, EquipoRepositorio>();
builder.Services.AddScoped<IEstadioRepositorio, EstadioRepositorio>();
builder.Services.AddScoped<IPartidoRepositorio, PartidoRepositorio>();
builder.Services.AddScoped<IAuditoriaRepositorio, AuditoriaRepositorio>();

builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IServicioAuditoria, ServicioAuditoria>();
builder.Services.AddScoped<IServicioEquipo, EquipoServicios>();
builder.Services.AddScoped<IServicioEstadio, EstadioServicios>();
builder.Services.AddScoped<IServicioPartido, PartidoServicios>();
builder.Services.AddScoped<IImportadorEquiposCSV, ImportadorEquiposCSV>();
builder.Services.AddScoped<IServicioFixturePrimeraFase, FixturePrimeraFaseServicio>();
builder.Services.AddScoped<IServicioCrucesSegundaFase, CrucesSegundaFaseServicio>();

builder.Services.AddScoped<UsuarioSesion>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<FixtureEstado>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    IServicioUsuario servicioUsuario = scope.ServiceProvider.GetRequiredService<IServicioUsuario>();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
