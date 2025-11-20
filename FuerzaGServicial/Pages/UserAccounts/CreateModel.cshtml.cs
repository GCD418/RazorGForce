using FuerzaGServicial.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.UserAccounts;

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
        ValidationErrors.Clear();
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var response = await _authFacade.CreateUserAccountAsync(UserAccount);
        
        if (!response.Success)
        {
            ValidationErrors = response.Errors;
            ModelState.AddModelError(string.Empty, response.Message);
            return Page();
        }

        return RedirectToPage("/UserAccounts/UserPage");
    }
}
