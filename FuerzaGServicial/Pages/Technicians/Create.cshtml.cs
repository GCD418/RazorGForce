using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.ModelsD.Technicians;
using FuerzaGServicial.Services.Facades.Technicians;
// using UserAccountService.Domain.Entities;
// using UserAccountService.Domain.Ports;
// using FuerzaGServicial.Models.UserAccount;
namespace FuerzaGServicial.Pages.Technicians;

// [Authorize(Roles = UserRoles.Manager)]
public class Create : PageModel
{
    private readonly ITechnicianFacade _technicianFacade;
    //private readonly ISessionManager _sessionManager;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty]
    public CreateTechnicianModel Technician { get; set; } = new();

    public Create(
        ITechnicianFacade technicianFacade)
        //ISessionManager sessionManager)
    {
        _technicianFacade = technicianFacade;
        //_sessionManager = sessionManager;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();

        //Technician.UserId = _sessionManager.UserId ?? 9999;

        var result = await _technicianFacade.Create(Technician);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo crear el técnico.");
            return Page();
        }

        return RedirectToPage("/Technicians/TechnicianPage");
    }

    private string MapErrorToField(string error)
    {
        var e = error.ToLowerInvariant();

        if (e.Contains("nombre")) return "Name";
        if (e.Contains("apellido")) return "FirstLastName";
        if (e.Contains("segundo")) return "SecondLastName";
        if (e.Contains("teléfono") || e.Contains("telefono")) return "PhoneNumber";
        if (e.Contains("correo") || e.Contains("email")) return "Email";
        if (e.Contains("documento")) return "DocumentNumber";
        if (e.Contains("extensión") || e.Contains("extension")) return "DocumentExtension";
        if (e.Contains("dirección") || e.Contains("direccion")) return "Address";
        if (e.Contains("salario")) return "BaseSalary";

        return string.Empty;
    }
}
