using FuerzaGServicial.ModelsD.Technicians;
using System.Net.Http.Json;

namespace FuerzaGServicial.Services.Clients
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
            return await _http.GetFromJsonAsync<List<TechnicianModel>>("api/Technician")
                   ?? new List<TechnicianModel>();
        }

        public async Task<TechnicianModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<TechnicianModel>($"api/Technician/{id}");
        }

        public async Task<SuccessResponseModel?> InsertAsync(CreateTechnicianModel request)
        {
            var msg = new HttpRequestMessage(HttpMethod.Post, "api/Technician/insert")
            {
                Content = JsonContent.Create(request)
            };

            msg.Headers.Add("User-Id", request.UserId.ToString());

            var resp = await _http.SendAsync(msg);
            return await resp.Content.ReadFromJsonAsync<SuccessResponseModel>();
        }

        public async Task<SuccessResponseModel?> UpdateAsync(int id, UpdateTechnicianModel request)
        {
            var msg = new HttpRequestMessage(HttpMethod.Put, $"api/Technician/{id}")
            {
                Content = JsonContent.Create(request)
            };

            msg.Headers.Add("User-Id", request.UserId.ToString());

            var resp = await _http.SendAsync(msg);
            return await resp.Content.ReadFromJsonAsync<SuccessResponseModel>();
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var msg = new HttpRequestMessage(HttpMethod.Delete, $"api/Technician/{id}");
            msg.Headers.Add("User-Id", userId.ToString());

            var resp = await _http.SendAsync(msg);
            return resp.IsSuccessStatusCode;
        }
    }
}
