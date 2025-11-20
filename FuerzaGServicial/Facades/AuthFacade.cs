using FuerzaGServicial.Models.Auth;
using FuerzaGServicial.Services.Clients;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FuerzaGServicial.Facades.Auth;

public class AuthFacade
{
    private readonly UserAccountApiClient _apiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TokenCookieName = "JwtToken";

    public AuthFacade(UserAccountApiClient apiClient, IHttpContextAccessor httpContextAccessor)
    {
        _apiClient = apiClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var loginRequest = new LoginRequest
        {
            UserName = username,
            Password = password
        };

        var response = await _apiClient.LoginAsync(loginRequest);

        if (response == null || string.IsNullOrEmpty(response.Token))
        {
            return false;
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, 
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn)
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(TokenCookieName, response.Token, cookieOptions);

        return true;
    }

    public Task LogoutAsync()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(TokenCookieName);
        return Task.CompletedTask;
    }

    public bool IsAuthenticated
    {
        get
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies[TokenCookieName];
            if (string.IsNullOrEmpty(token))
                return false;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                
                return jwtToken.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }

    public string? GetToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[TokenCookieName];
    }

    public int? GetUserId()
    {
        var token = GetToken();
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                return userId;
        }
        catch
        {
            // Token inválido
        }

        return null;
    }

    public string GetFullName()
    {
        var token = GetToken();
        if (string.IsNullOrEmpty(token))
            return string.Empty;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            var name = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "";
            var firstLastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstLastName")?.Value ?? "";
            var secondLastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "SecondLastName")?.Value ?? "";
            
            return $"{name} {firstLastName} {secondLastName}".Trim();
        }
        catch
        {
            return string.Empty;
        }
    }
}
