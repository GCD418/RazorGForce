using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Owners;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.Owners;

[Authorize(Roles = UserRoles.Manager)]
public class CreateModel : PageModel
{
    private readonly OwnerFacade _ownerFacade;
    private readonly JwtSessionManager _sessionManager;
    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty]
    public OwnerModel Owner { get; set; } = new();

    public CreateModel(OwnerFacade ownerFacade, JwtSessionManager sessionManager)
    {
        _ownerFacade = ownerFacade;
        _sessionManager = sessionManager;
    }

    public void OnGet()
    {
        Owner.IsActive = true;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationErrors.Clear();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var response = await _ownerFacade.CreateAsync(Owner, _sessionManager.UserId ?? 9999);

        if (!response.Success)
        {
            ValidationErrors = response.Errors;
            ModelState.AddModelError(string.Empty, response.Message);
            return Page();
        }

        return RedirectToPage("/Owners/OwnerPage");
    }
}
