using FuerzaGServicial.Facades;
using FuerzaGServicial.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar Razor Pages
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// Configurar autenticación basada en cookies (para Razor Pages)
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Leer URLs de los microservicios desde appsettings.json
var serviceApiUrl = builder.Configuration["ApiSettings:ServiceMicroserviceUrl"];
var userAccountApiUrl = builder.Configuration["ApiSettings:UserAccountMicroserviceUrl"];
var technicianApiUrl = builder.Configuration["ApiSettings:TechnicianMicroserviceUrl"];
var ownerApiUrl = builder.Configuration["ApiSettings:OwnerMicroserviceUrl"];

// Registrar HttpClients para los microservicios
builder.Services.AddTransient<JwtHttpMessageHandler>();

// ServiceApiClient
builder.Services.AddHttpClient<ServiceApiClient>(client =>
{
    client.BaseAddress = new Uri(serviceApiUrl);
})
.AddHttpMessageHandler<JwtHttpMessageHandler>();

// UserAccountApiClient
builder.Services.AddHttpClient<UserAccountApiClient>(client =>
{
    client.BaseAddress = new Uri(userAccountApiUrl);
})
.AddHttpMessageHandler<JwtHttpMessageHandler>();

// TechnicianApiClient
builder.Services.AddHttpClient<TechnicianApiClient>(client =>
{
    client.BaseAddress = new Uri(technicianApiUrl);
})
.AddHttpMessageHandler<JwtHttpMessageHandler>();

// OwnerApiClient
builder.Services.AddHttpClient<OwnerApiClient>(client =>
{
    client.BaseAddress = new Uri(ownerApiUrl);
})
.AddHttpMessageHandler<JwtHttpMessageHandler>();

// Registrar fachadas
builder.Services.AddScoped<ServiceFacade>();
builder.Services.AddScoped<UserAccountFacade>();
builder.Services.AddScoped<TechnicianFacade>();
builder.Services.AddScoped<OwnerFacade>();
builder.Services.AddScoped<AuthFacade>();
builder.Services.AddScoped<JwtSessionManager>();

builder.Services.AddDataProtection();

// Construir app
var app = builder.Build();

// Configurar pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

// Ejecutar app
app.Run();
