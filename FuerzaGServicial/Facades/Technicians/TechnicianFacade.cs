using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.Technicians;
using FuerzaGServicial.Services;

namespace FuerzaGServicial.Facades;

public class TechnicianFacade
{
    private readonly TechnicianApiClient _apiClient;

    public TechnicianFacade(TechnicianApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<TechnicianModel>> GetAllAsync()
    {
        return await _apiClient.GetAllAsync();
    }

    public async Task<TechnicianModel?> GetByIdAsync(int id)
    {
        return await _apiClient.GetByIdAsync(id);
    }

    public async Task<ApiResponse<int>> CreateAsync(CreateTechnicianModel request)
    {
        return await _apiClient.CreateAsync(request);
    }

    public async Task<ApiResponse<bool>> UpdateAsync(int userId, UpdateTechnicianModel request)
    {
        return await _apiClient.UpdateAsync(request, userId);
    }

    public async Task<bool> DeleteByIdAsync(int id, int userId)
    {
        return await _apiClient.DeleteByIdAsync(id, userId);
    }
}
