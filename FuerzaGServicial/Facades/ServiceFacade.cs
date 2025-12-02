using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Services;

namespace FuerzaGServicial.Facades
{
    public class ServiceFacade
    {
        private readonly ServiceApiClient _apiClient;

        public ServiceFacade(ServiceApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<ServiceModel>> GetAllAsync()
        {
            return await _apiClient.GetAllAsync();
        }

        public async Task<ServiceModel?> GetByIdAsync(int id)
        {
            return await _apiClient.GetByIdAsync(id);
        }

        public async Task<ApiResponse<int>> CreateAsync(ServiceModel service, int userId)
        {
            return await _apiClient.CreateAsync(service, userId);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(ServiceModel service, int userId)
        {
            return await _apiClient.UpdateAsync(service, userId);
        }

        public async Task<bool> DeleteByIdAsync(int id, int userId)
        {
            return await _apiClient.DeleteByIdAsync(id, userId);
        }
    }
}
