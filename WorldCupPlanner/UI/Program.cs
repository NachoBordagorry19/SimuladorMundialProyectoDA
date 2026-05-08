using UI.Components;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Interfaces;
using UI.Estado;
using Dominio.Enums;
using Servicios.Interfaces;
using Servicios.Modelo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<BaseDeDatosEnMemoria>();

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IEquipoRepositorio, EquipoRepositorio>();
builder.Services.AddScoped<IEstadioRepositorio, EstadioRepositorio>();
builder.Services.AddScoped<IPartidoRepositorio, PartidoRepositorio>();

builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<EquipoServicios>();
builder.Services.AddScoped<IServicioEstadio, EstadioServicios>();
builder.Services.AddScoped<PartidoServicios>();

builder.Services.AddScoped<UsuarioSesion>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    IServicioUsuario servicioUsuario = scope.ServiceProvider.GetRequiredService<IServicioUsuario>();

    if (!servicioUsuario.ObtenerUsuarios().Any())
    {
        servicioUsuario.AgregarUsuario(new UsuarioDTO()
        {
            Nombre = "admin",
            Apellido = "Prueba",
            Email = "admin@admin.com",
            FechaNacimiento = new DateTime(2000, 1, 1),
            Contraseña = "Admin123!",
            Roles = new List<Rol> { Rol.Administrador, Rol.Editor }
        });
    }
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
