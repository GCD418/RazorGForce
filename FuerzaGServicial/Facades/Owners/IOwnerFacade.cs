using FuerzaGServicial.ModelsD.Owners;

namespace FuerzaGServicial.Services.Facades.Owners;

public interface IOwnerFacade
{
    Task<List<OwnerModel>> GetAll();
    Task<OwnerModel?> GetById(int id);
    Task<SuccessResponseModel?> Create(CreateOwnerModel request);
    Task<SuccessResponseModel?> Update(int id, UpdateOwnerModel request);
    Task<bool> Delete(int id);
}
