using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.Services;
using System.Net.Http.Json;

namespace FuerzaGServicial.Services
{
    public class ServiceApiClient
    {
        private readonly HttpClient _http;

        public ServiceApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ServiceModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<ServiceModel>>("api/service") ?? new List<ServiceModel>();
        }

        public async Task<ServiceModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ServiceModel>($"api/service/{id}");
        }

        public async Task<ApiResponse<int>> CreateAsync(ServiceModel service)
        {
            var response = await _http.PostAsJsonAsync("api/service/create", service);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                return new ApiResponse<int>
                {
                    Success = true,
                    Data = successResponse?.Id ?? 0,
                    Message = successResponse?.Message ?? "Servicio creado exitosamente"
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
                Message = "Error al crear el servicio",
                Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
            };
        }

        public async Task<ApiResponse<bool>> UpdateAsync(ServiceModel service, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/service/{service.Id}");
            request.Headers.Add("User-Id", userId.ToString());
            request.Content = JsonContent.Create(service);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = successResponse?.Message ?? "Servicio actualizado exitosamente"
                };
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var rawContent = await response.Content.ReadAsStringAsync();
                
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(rawContent);
                    var root = doc.RootElement;
                    
                    var message = root.TryGetProperty("message", out var msgElement) 
                        ? msgElement.GetString() 
                        : "Error de validación";
                    
                    var errors = new List<string>();
                    
                    if (root.TryGetProperty("errors", out var errorsElement))
                    {
                        if (errorsElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            foreach (var error in errorsElement.EnumerateArray())
                            {
                                errors.Add(error.GetString() ?? "Error desconocido");
                            }
                        }
                        else if (errorsElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            foreach (var prop in errorsElement.EnumerateObject())
                            {
                                if (prop.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
                                {
                                    foreach (var error in prop.Value.EnumerateArray())
                                    {
                                        errors.Add($"{prop.Name}: {error.GetString()}");
                                    }
                                }
                                else
                                {
                                    errors.Add($"{prop.Name}: {prop.Value.GetString()}");
                                }
                            }
                        }
                    }
                    
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = message,
                        Errors = errors.Count > 0 ? errors : new List<string> { message }
                    };
                }
                catch (Exception ex)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Error al procesar la respuesta del servidor",
                        Errors = new List<string> { rawContent }
                    };
                }
            }

            return new ApiResponse<bool>
            {
                Success = false,
                Message = "Error al actualizar el servicio",
                Errors = new List<string> { response.ReasonPhrase ?? "Error desconocido" }
            };
        }

        public async Task<bool> DeleteByIdAsync(int id, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/service/{id}");
            request.Headers.Add("userId", userId.ToString());

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}