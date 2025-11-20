using System.Net.Http.Json;
using FuerzaGServicial.ModelsD.Owners;

namespace FuerzaGServicial.Services.Clients
{
    public class OwnerApiClient
    {
        private readonly HttpClient _http;

        public OwnerApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OwnerModelResponse>> GetAll()
        {
            var result = await _http.GetFromJsonAsync<List<OwnerModelResponse>>("owner/select");
            return result ?? new List<OwnerModelResponse>();
        }

        public async Task<OwnerModelResponse?> GetById(int id)
        {
            return await _http.GetFromJsonAsync<OwnerModelResponse>($"owner/{id}");
        }

        public async Task<SuccessResponseModel?> Create(CreateOwnerModel request)
        {
            var response = await _http.PostAsJsonAsync("owner/insert", request);
            return await response.Content.ReadFromJsonAsync<SuccessResponseModel>();
        }

        public async Task<SuccessResponseModel?> Update(int id, UpdateOwnerModel request)
        {
            var response = await _http.PutAsJsonAsync($"owner/{id}", request);
            return await response.Content.ReadFromJsonAsync<SuccessResponseModel>();
        }

        public async Task<bool> Delete(int id, int userId)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, $"owner/{id}");
            req.Headers.Add("User-Id", userId.ToString());

            var response = await _http.SendAsync(req);
            return response.IsSuccessStatusCode;
        }
    }
}