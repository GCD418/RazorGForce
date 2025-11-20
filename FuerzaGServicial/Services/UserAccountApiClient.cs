using FuerzaGServicial.Models.Auth;
using System.Net.Http.Json;

namespace FuerzaGServicial.Services.Clients;

public class UserAccountApiClient
{
    private readonly HttpClient _http;

    public UserAccountApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        
        return null;
    }
}
