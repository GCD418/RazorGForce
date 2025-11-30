using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.Owners;
using FuerzaGServicial.Facades;

namespace FuerzaGServicial.Pages.Owners;

[Authorize(Roles = UserRoles.Manager)]
public class OwnerPage : PageModel
{
    private readonly OwnerFacade _ownerFacade;
    private readonly IDataProtector _protector;
    private readonly ISessionManager _sessionManager; /*Cambiar al nuevo como esta en UserAccout*/

    public OwnerPage(
        OwnerFacade ownerFacade,
        IDataProtectionProvider provider,
        ISessionManager sessionManager) /*Cambiar al nuevo como esta en UserAccout*/
    {
        _ownerFacade = ownerFacade;
        _protector = provider.CreateProtector("OwnerProtector");
        _sessionManager = sessionManager;
    }

    public List<OwnerModelResponse> Owners { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Owners = await _ownerFacade.GetAll();
        return Page();
    }

    public string EncryptId(int id)
    {
        return _protector.Protect(id.ToString());
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        var decryptedId = int.Parse(_protector.Unprotect(id));
        var userId = _sessionManager.UserId ?? 9999;

        await _ownerFacade.Delete(decryptedId, userId);

        return RedirectToPage();
    }
}
