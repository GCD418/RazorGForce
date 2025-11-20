using FuerzaGServicial.Models.Auth;
using FuerzaGServicial.Models.UserAccounts;
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

    public async Task<List<UserAccount>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<UserAccount>>("api/useraccounts") ?? new List<UserAccount>();
    }

    public async Task<UserAccount?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<UserAccount>($"api/useraccounts/{id}");
    }

    public async Task<bool> CreateAsync(UserAccount userAccount)
    {
        var response = await _http.PostAsJsonAsync("api/useraccounts/create", userAccount);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(UserAccount userAccount, int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "api/useraccounts");
        request.Headers.Add("id", userId.ToString());
        request.Content = JsonContent.Create(userAccount);
        
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteByIdAsync(int id, int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/useraccounts/{id}");
        request.Headers.Add("userId", userId.ToString());
        
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}
