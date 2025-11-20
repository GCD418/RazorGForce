using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Facades.UserAccounts;
using FuerzaGServicial.Services.Session;

namespace FuerzaGServicial.Pages.UserAccounts;

[Authorize(Roles = UserRoles.CEO)]
public class EditModel : PageModel
{
    private readonly UserAccountFacade _userAccountFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    [BindProperty]
    public UserAccount UserAccount { get; set; } = new();

    public List<string> ValidationErrors { get; set; } = new();

    public EditModel(
        UserAccountFacade userAccountFacade,
        IDataProtectionProvider provider,
        JwtSessionManager sessionManager)
    {
        _userAccountFacade = userAccountFacade;
        _protector = provider.CreateProtector("UserAccountProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage("/UserAccounts/UserPage");

        if (!int.TryParse(_protector.Unprotect(id), out var decryptedId))
            return RedirectToPage("/UserAccounts/UserPage");

        var user = await _userAccountFacade.GetByIdAsync(decryptedId);
        if (user == null)
            return RedirectToPage("/UserAccounts/UserPage");

        UserAccount = user;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationErrors.Clear();
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Mantener UserName existente
        var existingUser = await _userAccountFacade.GetByIdAsync(UserAccount.Id);
        if (existingUser != null)
            UserAccount.UserName = existingUser.UserName;

        var response = await _userAccountFacade.UpdateAsync(UserAccount, _sessionManager.UserId ?? 9999);
        
        if (!response.Success)
        {
            ValidationErrors = response.Errors;
            ModelState.AddModelError(string.Empty, response.Message);
            return Page();
        }

        TempData["SuccessMessage"] = response.Message;
        return RedirectToPage("/UserAccounts/UserPage");
    }
}
