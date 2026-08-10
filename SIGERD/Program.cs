using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGERD.Data;
using SIGERD.Data.Seed;
using SIGERD.Interfaces.IRespositories.Dashboard;
using SIGERD.Interfaces.IRespositories.Despachos;
using SIGERD.Interfaces.IRespositories.Envios;
using SIGERD.Interfaces.IRespositories.Inventario;
using SIGERD.Interfaces.IRespositories.Recepciones;
using SIGERD.Interfaces.IRespositories.Reportes;
using SIGERD.Interfaces.IRespositories.Seguridad;
using SIGERD.Interfaces.IRespositories.Ubicacion;
using SIGERD.Interfaces.IServices.Common;
using SIGERD.Interfaces.IServices.Dashboard;
using SIGERD.Interfaces.IServices.Despachos;
using SIGERD.Interfaces.IServices.Envios;
using SIGERD.Interfaces.IServices.Inventario;
using SIGERD.Interfaces.IServices.Recepciones;
using SIGERD.Interfaces.IServices.Reportes;
using SIGERD.Interfaces.IServices.Seguridad;
using SIGERD.Interfaces.IServices.Ubicacion;
using SIGERD.Models.Seguridad;
using SIGERD.Repositories.Dashboard;
using SIGERD.Repositories.Despachos;
using SIGERD.Repositories.Envios;
using SIGERD.Repositories.Inventario;
using SIGERD.Repositories.Recepciones;
using SIGERD.Repositories.Reportes;
using SIGERD.Repositories.Seguridad;
using SIGERD.Repositories.Ubicacion;
using SIGERD.Services.Common;
using SIGERD.Services.Dashboard;
using SIGERD.Services.Despachos;
using SIGERD.Services.Envios;
using SIGERD.Services.Inventario;
using SIGERD.Services.Recepciones;
using SIGERD.Services.Reportes;
using SIGERD.Services.Seguridad;
using SIGERD.Services.Ubicacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSIGERD"));
});

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ISelectListService, SelectListService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService,  DashboardService>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddScoped<IDelegacionRepository, DelegacionRepository>();
builder.Services.AddScoped<IDelegacionService, DelegacionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;

    options.Cookie.Name = "SIGERD.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<IEnvioRepository, EnvioRepository>();
builder.Services.AddScoped<IEnvioService, EnvioService>();
builder.Services.AddScoped<IDespachoRepository, DespachoRepository>();
builder.Services.AddScoped<IDespachoService, DespachoService>();
builder.Services.AddScoped<IRecepcionRepository, RecepcionRepository>();
builder.Services.AddScoped<IRecepcionService, RecepcionService>();
builder.Services.AddScoped<ICategoriaRepository,  CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IArticuloRepository, ArticuloRepository>();
builder.Services.AddScoped<IArticuloService, ArticuloService>();
builder.Services.AddScoped<IReporteEnviosRepository, ReporteEnviosRepository>();
builder.Services.AddScoped<IReporteEnviosService, ReporteEnviosService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

    await dataSeeder.SeedAsync();
}

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

app.UseAuthentication();

app.Use(async (context, next) =>
{
    bool usuarioAutenticado = context.User.Identity?.IsAuthenticated == true;

    bool debeCambiarClave =
        context.User.FindFirst("DebeCambiarClave")?.Value == "true";

    string rutaActual = context.Request.Path.Value ?? string.Empty;

    bool esRutaPermitida =
        rutaActual.StartsWith("/Auth/CambiarClaveInicial", StringComparison.OrdinalIgnoreCase) ||
        rutaActual.StartsWith("/Auth/Logout", StringComparison.OrdinalIgnoreCase) ||
        rutaActual.StartsWith("/Auth/Login", StringComparison.OrdinalIgnoreCase) ||
        rutaActual.StartsWith("/Auth/AccessDenied", StringComparison.OrdinalIgnoreCase);

    if (usuarioAutenticado && debeCambiarClave && !esRutaPermitida)
    {
        context.Response.Redirect("/Auth/CambiarClaveInicial");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();


