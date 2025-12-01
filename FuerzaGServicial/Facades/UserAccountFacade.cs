using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services;

namespace FuerzaGServicial.Facades;

public class UserAccountFacade
{
    private readonly UserAccountApiClient _apiClient;

    public UserAccountFacade(UserAccountApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<UserAccount>> GetAllAsync()
    {
        return await _apiClient.GetAllAsync();
    }

    public async Task<UserAccount?> GetByIdAsync(int id)
    {
        return await _apiClient.GetByIdAsync(id);
    }

    public async Task<ApiResponse<int>> CreateAsync(UserAccount userAccount, int userId)
    {
        return await _apiClient.CreateAsync(userAccount, userId);
    }

    public async Task<ApiResponse<bool>> UpdateAsync(UserAccount userAccount, int userId)
    {
        return await _apiClient.UpdateAsync(userAccount, userId);
    }

    public async Task<bool> DeleteByIdAsync(int id, int userId)
    {
        return await _apiClient.DeleteByIdAsync(id, userId);
    }
}
