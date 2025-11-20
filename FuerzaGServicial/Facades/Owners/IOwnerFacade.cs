using FuerzaGServicial.ModelsD.Owners;

namespace FuerzaGServicial.Services.Facades.Owners
{
    public interface IOwnerFacade
    {
        Task<List<OwnerModelResponse>> GetAll();
        Task<OwnerModelResponse?> GetById(int id);
        Task<SuccessResponseModel?> Create(CreateOwnerModel request);
        Task<SuccessResponseModel?> Update(int id, UpdateOwnerModel request);
        Task<bool> Delete(int id, int userId);
    }
}