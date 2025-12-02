using FuerzaGServicial.Models.Auth;
using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.UserAccounts;

namespace FuerzaGServicial.Services;

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

    public async Task<ApiResponse<int>> CreateAsync(UserAccount userAccount, int userId)
    {
        // var response = await _http.PostAsJsonAsync("api/useraccounts/create", userAccount);
        
        var request = new HttpRequestMessage(HttpMethod.Post, "api/useraccounts/create");
        request.Headers.Add("userId", userId.ToString());
        request.Content = JsonContent.Create(userAccount);
            
        var response = await _http.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            return new ApiResponse<int>
            {
                Success = true,
                Data = successResponse?.Id ?? 0,
                Message = successResponse?.Message ?? "Usuario creado exitosamente"
            };
        }
        
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            return new ApiResponse<int>
            {
                Success = false,
                Message = errorResponse?.Message ?? "Error de validación",
                Errors = errorResponse?.Errors ?? new List<string>()
            };
        }
        
        return new ApiResponse<int>
        {
            Success = false,
            Message = "Error al crear el usuario",
            Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
        };
    }

    public async Task<ApiResponse<bool>> UpdateAsync(UserAccount userAccount, int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "api/useraccounts");
        request.Headers.Add("userId", userId.ToString());
        request.Content = JsonContent.Create(userAccount);
        
        var response = await _http.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = successResponse?.Message ?? "Usuario actualizado exitosamente"
            };
        }
        
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            return new ApiResponse<bool>
            {
                Success = false,
                Message = errorResponse?.Message ?? "Error de validación",
                Errors = errorResponse?.Errors ?? new List<string>()
            };
        }
        
        return new ApiResponse<bool>
        {
            Success = false,
            Message = "Error al actualizar el usuario",
            Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
        };
    }

    public async Task<bool> DeleteByIdAsync(int id, int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/useraccounts/{id}");
        request.Headers.Add("userId", userId.ToString());
        
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest, int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/useraccounts/change-password");
        request.Headers.Add("userId", userId.ToString());
        request.Content = JsonContent.Create(changePasswordRequest);
        
        var response = await _http.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = successResponse?.Message ?? "Contraseña cambiada exitosamente"
            };
        }
        
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            return new ApiResponse<bool>
            {
                Success = false,
                Message = errorResponse?.Message ?? "Error de validación",
                Errors = errorResponse?.Errors ?? new List<string>()
            };
        }
        
        return new ApiResponse<bool>
        {
            Success = false,
            Message = "Error al cambiar la contraseña",
            Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
        };
    }
}
