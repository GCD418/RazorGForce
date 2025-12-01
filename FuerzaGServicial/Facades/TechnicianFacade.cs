using FuerzaGServicial.Models.Common;
using FuerzaGServicial.Models.Technicians;
using FuerzaGServicial.Services;

namespace FuerzaGServicial.Facades
{
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

        public async Task<ApiResponse<int>> CreateAsync(TechnicianModel technician, int userId)
        {
            return await _apiClient.CreateAsync(technician, userId);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(TechnicianModel technician, int userId)
        {
            return await _apiClient.UpdateAsync(technician, userId);
        }

        public async Task<bool> DeleteByIdAsync(int id, int userId)
        {
            return await _apiClient.DeleteByIdAsync(id, userId);
        }
    }
}
