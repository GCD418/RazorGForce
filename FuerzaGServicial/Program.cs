var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// 1️⃣ Agregar Razor Pages
// ----------------------------
builder.Services.AddRazorPages();

// ----------------------------
// 2️⃣ Leer URL del microservicio desde appsettings.json
// ----------------------------
var serviceApiUrl = builder.Configuration["ApiSettings:ServiceMicroserviceUrl"];

// ----------------------------
// 3️⃣ Registrar HttpClient para el microservicio
// ----------------------------
builder.Services.AddHttpClient<FuerzaGServicial.Services.Clients.ServiceApiClient>(client =>
{
    client.BaseAddress = new Uri(serviceApiUrl);
});

// ----------------------------
// 4️⃣ Registrar la fachada
// ----------------------------
builder.Services.AddScoped<FuerzaGServicial.Services.Facades.Services.IServiceFacade,
                           FuerzaGServicial.Services.Facades.Services.ServiceFacade>();

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
