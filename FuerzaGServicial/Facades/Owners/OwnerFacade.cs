using FuerzaGServicial.ModelsD.Owners;
using FuerzaGServicial.Services;

namespace FuerzaGServicial.Services.Facades.Owners;

public class OwnerFacade : IOwnerFacade
{
    private readonly OwnerApiClient _client;

    public OwnerFacade(OwnerApiClient client)
    {
        _client = client;
    }

    public Task<List<OwnerModel>> GetAll() => _client.GetAll();
    public Task<OwnerModel?> GetById(int id) => _client.GetById(id);
    public Task<SuccessResponseModel?> Create(CreateOwnerModel request) => _client.Create(request);
    public Task<SuccessResponseModel?> Update(int id, UpdateOwnerModel request) => _client.Update(id, request);
    public Task<bool> Delete(int id) => _client.Delete(id);
}
