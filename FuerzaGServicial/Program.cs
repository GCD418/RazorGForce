using FuerzaGServicial.Facades.Services;
using FuerzaGServicial.Services.Facades.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// 1️⃣ Agregar Razor Pages
// ----------------------------
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

// ----------------------------
// 2️⃣ Leer URLs de los microservicios desde appsettings.json
// ----------------------------
var serviceApiUrl = builder.Configuration["ApiSettings:ServiceMicroserviceUrl"];
var userAccountApiUrl = builder.Configuration["ApiSettings:UserAccountMicroserviceUrl"];

// ----------------------------
// 3️⃣ Registrar HttpClients para los microservicios
// ----------------------------

// Registrar el JwtHttpMessageHandler como Transient (para HttpClient)
builder.Services.AddTransient<FuerzaGServicial.Services.Handlers.JwtHttpMessageHandler>();

// ServiceApiClient con JWT handler
builder.Services.AddHttpClient<FuerzaGServicial.Services.Clients.ServiceApiClient>(client =>
{
    client.BaseAddress = new Uri(serviceApiUrl);
})
.AddHttpMessageHandler<FuerzaGServicial.Services.Handlers.JwtHttpMessageHandler>();

// UserAccountApiClient con JWT handler
builder.Services.AddHttpClient<FuerzaGServicial.Services.Clients.UserAccountApiClient>(client =>
{
    client.BaseAddress = new Uri(userAccountApiUrl);
})
.AddHttpMessageHandler<FuerzaGServicial.Services.Handlers.JwtHttpMessageHandler>();

// ----------------------------
// 4️⃣ Registrar fachadas y servicios
// ----------------------------
builder.Services.AddScoped<IServiceFacade,
                           ServiceFacade>();

builder.Services.AddScoped<FuerzaGServicial.Facades.Auth.AuthFacade>();
builder.Services.AddScoped<FuerzaGServicial.Facades.UserAccounts.UserAccountFacade>();
builder.Services.AddScoped<FuerzaGServicial.Services.Session.JwtSessionManager>();

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
