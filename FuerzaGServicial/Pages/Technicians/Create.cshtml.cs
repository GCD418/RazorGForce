using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Technicians;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.Technicians;

[Authorize(Roles = UserRoles.Manager)]
public class Create : PageModel
{
    private readonly TechnicianFacade _technicianFacade;
    private readonly JwtSessionManager _sessionManager;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty]
    public TechnicianModel Technician { get; set; } = new();

    public Create(
        TechnicianFacade technicianFacade,
        JwtSessionManager sessionManager)
    {
        _technicianFacade = technicianFacade;
        _sessionManager = sessionManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationErrors.Clear();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var response = await _technicianFacade.CreateAsync(Technician, _sessionManager.UserId ?? 9999);

        if (!response.Success)
        {
            ValidationErrors = response.Errors;
            ModelState.AddModelError(string.Empty, response.Message);
            return Page();
        }

        return RedirectToPage("/Technicians/TechnicianPage");
    }
}
