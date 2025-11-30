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
public class OwnerPageModel : PageModel
{
    public IEnumerable<OwnerModel> Owners { get; set; } = new List<OwnerModel>();
    private readonly OwnerFacade _ownerFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    public OwnerPageModel(
        OwnerFacade ownerFacade,
        IDataProtectionProvider provider,
        JwtSessionManager sessionManager)
    {
        _ownerFacade = ownerFacade;
        _protector = provider.CreateProtector("OwnerProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Owners = await _ownerFacade.GetAllAsync();
        return Page();
    }

    public string ProtectId(int id)
    {
        return _protector.Protect(id.ToString());
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage();

        var decryptedId = int.Parse(_protector.Unprotect(id));
        await _ownerFacade.DeleteByIdAsync(decryptedId, _sessionManager.UserId ?? 9999);
        return RedirectToPage();
    }
}
