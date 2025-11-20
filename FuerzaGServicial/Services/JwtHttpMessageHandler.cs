using FuerzaGServicial.Services.Session;

namespace FuerzaGServicial.Services.Handlers;

public class JwtHttpMessageHandler : DelegatingHandler
{
    private readonly JwtSessionManager _sessionManager;

    public JwtHttpMessageHandler(JwtSessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _sessionManager.Token;

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
