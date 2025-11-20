using System.Net.Http.Json;
using FuerzaGServicial.ModelsD.Owners;

namespace FuerzaGServicial.Services;

public class OwnerApiClient
{
    private readonly HttpClient _http;

    public OwnerApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<OwnerModel>> GetAll() =>
        await _http.GetFromJsonAsync<List<OwnerModel>>("api/Owner") 
        ?? new List<OwnerModel>();

    public async Task<OwnerModel?> GetById(int id) =>
        await _http.GetFromJsonAsync<OwnerModel>($"api/Owner/{id}");

    public async Task<SuccessResponseModel?> Create(CreateOwnerModel model)
    {
        var response = await _http.PostAsJsonAsync("api/Owner/insert", model);
        return await response.Content.ReadFromJsonAsync<SuccessResponseModel>();
    }

    public async Task<SuccessResponseModel?> Update(int id, UpdateOwnerModel model)
    {
        var response = await _http.PutAsJsonAsync($"api/Owner/{id}", model);
        return await response.Content.ReadFromJsonAsync<SuccessResponseModel>();
    }

    public async Task<bool> Delete(int id)
    {
        var response = await _http.DeleteAsync($"api/Owner/{id}");
        return response.IsSuccessStatusCode;
    }
}
