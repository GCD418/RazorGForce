using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Owners;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.Owners;

[Authorize(Roles = UserRoles.Manager)]
public class EditModel : PageModel
{
    private readonly OwnerFacade _ownerFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    [BindProperty]
    public OwnerModel OwnerModel { get; set; } = new();

    public List<string> ValidationErrors { get; set; } = new();

    public EditModel(
        OwnerFacade ownerFacade,
        IDataProtectionProvider provider,
        JwtSessionManager sessionManager)
    {
        _ownerFacade = ownerFacade;
        _protector = provider.CreateProtector("OwnerProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage("/Owners/OwnerPage");

        if (!int.TryParse(_protector.Unprotect(id), out var decryptedId))
            return RedirectToPage("/Owners/OwnerPage");

        var owner = await _ownerFacade.GetByIdAsync(decryptedId);
        if (owner == null)
            return RedirectToPage("/Owners/OwnerPage");

        OwnerModel = owner;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationErrors.Clear();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingOwner = await _ownerFacade.GetByIdAsync(OwnerModel.Id);
        if (existingOwner != null)
        {
            OwnerModel.CreatedAt = existingOwner.CreatedAt;
        }

        var response = await _ownerFacade.UpdateAsync(OwnerModel, _sessionManager.UserId ?? 9999);

        if (!response.Success)
        {
            ValidationErrors = response.Errors;
            ModelState.AddModelError(string.Empty, response.Message);
            return Page();
        }

        TempData["SuccessMessage"] = response.Message;
        return RedirectToPage("/Owners/OwnerPage");
    }
}
