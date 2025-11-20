using FuerzaGServicial.Facades;
using FuerzaGServicial.Facades.Services;
using FuerzaGServicial.Services;
using FuerzaGServicial.Services.Facades.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// 1️⃣ Agregar Razor Pages
// ----------------------------
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

// ----------------------------
// 1.5️⃣ Configurar autenticación basada en cookies (para Razor Pages)
// ----------------------------
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

// ----------------------------
// 2️⃣ Leer URLs de los microservicios desde appsettings.json
// ----------------------------
var serviceApiUrl = builder.Configuration["ApiSettings:ServiceMicroserviceUrl"];
var userAccountApiUrl = builder.Configuration["ApiSettings:UserAccountMicroserviceUrl"];
var technicianApiUrl = builder.Configuration["ApiSettings:TechnicianMicroserviceUrl"];
var ownerApiUrl = builder.Configuration["ApiSettings:OwnerMicroserviceUrl"];


// ----------------------------
// 3️⃣ Registrar HttpClients para los microservicios
// ----------------------------

// Registrar el JwtHttpMessageHandler como Transient (para HttpClient)
builder.Services.AddTransient<JwtHttpMessageHandler>();

// ServiceApiClient con JWT handler
builder.Services.AddHttpClient<ServiceApiClient>(client =>
{
    client.BaseAddress = new Uri(serviceApiUrl);
})
.AddHttpMessageHandler<JwtHttpMessageHandler>();

// UserAccountApiClient con JWT handler
builder.Services.AddHttpClient<UserAccountApiClient>(client =>
{
    client.BaseAddress = new Uri(userAccountApiUrl);
})
.AddHttpMessageHandler<JwtHttpMessageHandler>();

// ----------------------------
// 4️⃣ Registrar fachadas y servicio
// ----------------------------
builder.Services.AddScoped<IServiceFacade,
                           ServiceFacade>();

builder.Services.AddScoped<AuthFacade>();
builder.Services.AddScoped<UserAccountFacade>();
builder.Services.AddScoped<JwtSessionManager>();

builder.Services.AddDataProtection();

// ----------------------------
// 5️⃣ Construir app
// ----------------------------
var app = builder.Build();

// ----------------------------
// 6️⃣ Configurar pipeline
// ----------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

// Para autenticación/authorization
app.UseAuthentication();
app.UseAuthorization();

// Mapeo de assets y Razor Pages
app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

// ----------------------------
// 7️⃣ Ejecutar app
// ----------------------------
app.Run();
