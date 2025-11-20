using FuerzaGServicial.ModelsD.Owners;
using FuerzaGServicial.Services.Clients;

namespace FuerzaGServicial.Services.Facades.Owners
{
    public class OwnerFacade : IOwnerFacade
    {
        private readonly OwnerApiClient _api;

        public OwnerFacade(OwnerApiClient api)
        {
            _api = api;
        }

        public Task<List<OwnerModelResponse>> GetAll()
            => _api.GetAll();

        public Task<OwnerModelResponse?> GetById(int id)
            => _api.GetById(id);

        public Task<SuccessResponseModel?> Create(CreateOwnerModel request)
            => _api.Create(request);

        public Task<SuccessResponseModel?> Update(int id, UpdateOwnerModel request)
            => _api.Update(id, request);

        public Task<bool> Delete(int id, int userId)
            => _api.Delete(id, userId);
    }
}