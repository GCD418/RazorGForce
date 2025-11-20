using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services.Session;
using FuerzaGServicial.Services.Facades.Owners;
using FuerzaGServicial.ModelsD.Owners;

namespace FuerzaGServicial.Pages.Owners;

[Authorize(Roles = UserRoles.Manager)]
public class OwnerPage : PageModel
{
    private readonly IOwnerFacade _ownerFacade;
    private readonly IDataProtector _protector;
    private readonly ISessionManager _sessionManager;

    public OwnerPage(
        IOwnerFacade ownerFacade,
        IDataProtectionProvider provider,
        ISessionManager sessionManager)
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