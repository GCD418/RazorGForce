using FuerzaGServicial.Models.Services;
using System.Net.Http.Json;

namespace FuerzaGServicial.Services.Clients
{
    public class ServiceApiClient
    {
        private readonly HttpClient _http;

        public ServiceApiClient(HttpClient http)
        {
            _http = http;
        }

        // Obtener todos los servicios
        public async Task<List<ServiceModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<ServiceModel>>("api/Service") ?? new List<ServiceModel>();
        }

        // Obtener servicio por Id
        public async Task<ServiceModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ServiceModel>($"api/Service/{id}");
        }

        // Insertar nuevo servicio
        public async Task<SuccessResponseModel?> InsertAsync(CreateServiceModel request)
        {
            var response = await _http.PostAsJsonAsync("api/Service/insert", request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<SuccessResponseModel>();
            return null;
        }

        // Actualizar servicio
        public async Task<SuccessResponseModel?> UpdateAsync(int id, UpdateServiceModel request)
        {
            var response = await _http.PutAsJsonAsync($"api/Service/{id}", request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<SuccessResponseModel>();
            return null;
        }

        // Eliminar servicio
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Service/{id}");
            request.Headers.Add("User-Id", userId.ToString());
            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}