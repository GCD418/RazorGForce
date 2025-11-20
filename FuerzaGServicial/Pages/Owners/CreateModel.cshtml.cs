using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserAccountService.Domain.Entities;
using UserAccountService.Domain.Ports;
using FuerzaGServicial.ModelsD.Owners;
using FuerzaGServicial.Services.Facades.Owners;
using FuerzaGServicial.Models.UserAccounts;

namespace FuerzaGServicial.Pages.Owners;

[Authorize(Roles = UserRoles.Manager)]
public class CreateModel : PageModel
{
    private readonly IOwnerFacade _ownerFacade;
    private readonly ISessionManager _sessionManager;

    public List<string> ValidationErrors { get; set; } = [];

    [BindProperty]
    public CreateOwnerModel Owner { get; set; } = new();

    public CreateModel(
        IOwnerFacade ownerFacade,
        ISessionManager sessionManager)
    {
        _ownerFacade = ownerFacade;
        _sessionManager = sessionManager;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();

        Owner.UserId = _sessionManager.UserId ?? 9999;

        var result = await _ownerFacade.Create(Owner);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo crear el dueño.");
            return Page();
        }

        return RedirectToPage("/Owners/OwnerPage");
    }
}
