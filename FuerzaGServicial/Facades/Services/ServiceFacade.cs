using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Services.Clients;
using FuerzaGServicial.Services.Facades.Services;

namespace FuerzaGServicial.Facades.Services
{
    public class ServiceFacade : IServiceFacade
    {
        private readonly ServiceApiClient _client;

        public ServiceFacade(ServiceApiClient client)
        {
            _client = client;
        }

        public Task<List<ServiceModel>> GetAll() => _client.GetAllAsync();
        public Task<ServiceModel?> GetById(int id) => _client.GetByIdAsync(id);
        public Task<SuccessResponseModel?> Create(CreateServiceModel request) => _client.InsertAsync(request);
        public Task<SuccessResponseModel?> Update(int id, UpdateServiceModel request) => _client.UpdateAsync(id, request);
        public Task<bool> Delete(int id, int userId) => _client.DeleteAsync(id, userId);
    }
}
