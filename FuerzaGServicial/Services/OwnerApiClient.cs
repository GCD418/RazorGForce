using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.Owners;
using System.Net.Http.Json;

namespace FuerzaGServicial.Services
{
    public class OwnerApiClient
    {
        private readonly HttpClient _http;

        public OwnerApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OwnerModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<OwnerModel>>("api/owner") ?? new List<OwnerModel>();
        }

        public async Task<OwnerModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<OwnerModel>($"api/owner/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(OwnerModel owner, int userId)
        {
            // var response = await _http.PostAsJsonAsync("api/owner/create", owner);
            
            var request = new HttpRequestMessage(HttpMethod.Post, "api/owner/create");
            request.Headers.Add("userId", userId.ToString());
            request.Content = JsonContent.Create(owner);
            
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                return new ApiResponse<int>
                {
                    Success = true,
                    Data = successResponse?.Id ?? 0,
                    Message = successResponse?.Message ?? "Owner creado exitosamente"
                };
            }

            if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = "Error al crear el owner",
                    Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
                };
            var errorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            return new ApiResponse<int>
            {
                Success = false,
                Message = errorResponse?.Message ?? "Error de validación",
                Errors = errorResponse?.Errors ?? new List<string>()
            };

        }

        public async Task<ApiResponse<bool>> UpdateAsync(OwnerModel owner, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "api/owner");
            request.Headers.Add("userId", userId.ToString());
            request.Content = JsonContent.Create(owner);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = successResponse?.Message ?? "Owner actualizado exitosamente"
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
                Message = "Error al actualizar el owner",
                Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
            };
        }

        public async Task<bool> DeleteByIdAsync(int id, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/owner/{id}");
            request.Headers.Add("userId", userId.ToString());

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}