using FuerzaGServicial.ModelsD.Technicians;

namespace FuerzaGServicial.Services.Facades.Technicians
{
    public interface ITechnicianFacade
    {
        Task<List<TechnicianModel>> GetAll();
        Task<TechnicianModel?> GetById(int id);
        Task<SuccessResponseModel?> Create(CreateTechnicianModel request);
        Task<SuccessResponseModel?> Update(int id, UpdateTechnicianModel request);
        Task<bool> Delete(int id, int userId);
    }
}
