namespace FuerzaGServicial.Services;

public class JwtHttpMessageHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TokenCookieName = "JwtToken";

    public JwtHttpMessageHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Obtener el token directamente de las cookies
        var token = _httpContextAccessor.HttpContext?.Request.Cookies[TokenCookieName];

        if (!string.IsNullOrEmpty(token))
        {
            // Agregar el token al header Authorization
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
