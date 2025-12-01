using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.Owners;
using FuerzaGServicial.Services;

namespace FuerzaGServicial.Facades
{
    public class OwnerFacade
    {
        private readonly OwnerApiClient _apiClient;

        public OwnerFacade(OwnerApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<OwnerModel>> GetAllAsync()
        {
            return await _apiClient.GetAllAsync();
        }

        public async Task<OwnerModel?> GetByIdAsync(int id)
        {
            return await _apiClient.GetByIdAsync(id);
        }

        public async Task<ApiResponse<int>> CreateAsync(OwnerModel owner, int userId)
        {
            return await _apiClient.CreateAsync(owner, userId);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OwnerModel owner, int userId)
        {
            return await _apiClient.UpdateAsync(owner, userId);
        }

        public async Task<bool> DeleteByIdAsync(int id, int userId)
        {
            return await _apiClient.DeleteByIdAsync(id, userId);
        }
    }
}
