using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.Technicians;
using FuerzaGServicial.Services.Facades.Technicians;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Facades;

namespace FuerzaGServicial.Pages.Technicians;

[Authorize(Roles = UserRoles.Manager)]
public class Create : PageModel
{
    private readonly TechnicianFacade _technicianFacade;
    private readonly AuthFacade _authFacade;

    public List<string> ValidationErrors { get; set; } = new();

    // ⬅ NECESARIO, igual que en UserAccount/Create
    [BindProperty]
    public CreateUserAccountModel UserAccount { get; set; } = new();

    [BindProperty]
    public CreateTechnicianModel Technician { get; set; } = new();

    public Create(
        TechnicianFacade technicianFacade,
        AuthFacade authFacade)
    {
        _technicianFacade = technicianFacade;
        _authFacade = authFacade;
    }

    public async Task<IActionResult> OnGet()
    {
        var userInfo = await _authFacade.GetCurrentUserAsync();

        if (userInfo == null)
        {
            return RedirectToPage("/Account/Login");
        }

        Technician.UserId = userInfo.Id;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationErrors.Clear();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userResponse = await _authFacade.CreateUserAccountAsync(UserAccount);

        if (!userResponse.Success)
        {
            ValidationErrors = userResponse.Errors;
            ModelState.AddModelError(string.Empty, userResponse.Message);
            return Page();
        }

        Technician.UserId = userResponse.CreatedId;

        var techResponse = await _technicianFacade.CreateTechnicianAsync(Technician);

        if (!techResponse.Success)
        {
            ValidationErrors = techResponse.Errors;
            ModelState.AddModelError(string.Empty, techResponse.Message);
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
