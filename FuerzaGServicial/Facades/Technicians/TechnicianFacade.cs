using FuerzaGServicial.ModelsD.Technicians;
using FuerzaGServicial.Services.Clients;

namespace FuerzaGServicial.Services.Facades.Technicians
{
    public class TechnicianFacade : ITechnicianFacade
    {
        private readonly TechnicianApiClient _client;

        public TechnicianFacade(TechnicianApiClient client)
        {
            _client = client;
        }

        public Task<List<TechnicianModel>> GetAll() => _client.GetAllAsync();
        public Task<TechnicianModel?> GetById(int id) => _client.GetByIdAsync(id);
        public Task<SuccessResponseModel?> Create(CreateTechnicianModel request) => _client.InsertAsync(request);
        public Task<SuccessResponseModel?> Update(int id, UpdateTechnicianModel request) => _client.UpdateAsync(id, request);
        public Task<bool> Delete(int id, int userId) => _client.DeleteAsync(id, userId);
    }
}
