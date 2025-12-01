using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.Technicians;
using System.Net.Http.Json;

namespace FuerzaGServicial.Services
{
    public class TechnicianApiClient
    {
        private readonly HttpClient _http;

        public TechnicianApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TechnicianModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<TechnicianModel>>("api/technician") ?? new List<TechnicianModel>();
        }

        public async Task<TechnicianModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<TechnicianModel>($"api/technician/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(TechnicianModel technician, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/technician/create");
            request.Headers.Add("userId", userId.ToString());
            request.Content = JsonContent.Create(technician);
            
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                return new ApiResponse<int>
                {
                    Success = true,
                    Data = successResponse?.Id ?? 0,
                    Message = successResponse?.Message ?? "Técnico creado exitosamente"
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
                Message = "Error al crear el técnico",
                Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
            };
        }

        public async Task<ApiResponse<bool>> UpdateAsync(TechnicianModel technician, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "api/technician");
            request.Headers.Add("userId", userId.ToString());
            request.Content = JsonContent.Create(technician);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = successResponse?.Message ?? "Técnico actualizado exitosamente"
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
                Message = "Error al actualizar el técnico",
                Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
            };
        }

        public async Task<bool> DeleteByIdAsync(int id, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/technician/{id}");
            request.Headers.Add("userId", userId.ToString());

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
