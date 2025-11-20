using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Facades.UserAccounts;
using FuerzaGServicial.Services.Session;

namespace FuerzaGServicial.Pages.UserAccounts;

[Authorize(Roles = UserRoles.CEO)]
public class UserPageModel : PageModel
{
    public IEnumerable<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
    private readonly UserAccountFacade _userAccountFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    public UserPageModel(
        UserAccountFacade userAccountFacade,
        IDataProtectionProvider provider,
        JwtSessionManager sessionManager)
    {
        _userAccountFacade = userAccountFacade;
        _protector = provider.CreateProtector("UserAccountProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        UserAccounts = await _userAccountFacade.GetAllAsync();
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
        await _userAccountFacade.DeleteByIdAsync(decryptedId, _sessionManager.UserId ?? 9999);
        return RedirectToPage();
    }
}
