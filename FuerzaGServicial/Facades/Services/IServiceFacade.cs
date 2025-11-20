using FuerzaGServicial.Models.Services;
namespace FuerzaGServicial.Services.Facades.Services
{
    public interface IServiceFacade
    {
        Task<List<ServiceModel>> GetAll();
        Task<ServiceModel?> GetById(int id);
        Task<SuccessResponseModel?> Create(CreateServiceModel request);
        Task<SuccessResponseModel?> Update(int id, UpdateServiceModel request);
        Task<bool> Delete(int id, int userId);
    }
}