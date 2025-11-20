using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Facades.Auth;

namespace FuerzaGServicial.Pages.UserAccounts;

[Authorize(Roles = UserRoles.CEO)]
public class CreateModel : PageModel
{
    private readonly AuthFacade _authFacade;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty]
    public UserAccount UserAccount { get; set; } = new();

    public CreateModel(AuthFacade authFacade)
    {
        _authFacade = authFacade;
    }

    public void OnGet()
    {
        if (string.IsNullOrEmpty(UserAccount.Role))
            UserAccount.Role = "Manager";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var isSuccess = await _authFacade.CreateUserAccountAsync(UserAccount);
        if (!isSuccess)
        {
            ModelState.AddModelError(string.Empty, "No se pudo crear el usuario. Posiblemente el nombre de usuario ya existe.");
            return Page();
        }

        return RedirectToPage("/UserAccounts/UserPage");
    }
}
