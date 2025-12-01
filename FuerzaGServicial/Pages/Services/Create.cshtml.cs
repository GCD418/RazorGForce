using FuerzaGServicial.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Models.UserAccounts;

namespace FuerzaGServicial.Pages.Services;

[Authorize(Roles = UserRoles.CEO)]
public class Create : PageModel
{
    private readonly ServiceFacade _serviceFacade;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty]
    public ServiceModel Service { get; set; } = new();

    public Create(ServiceFacade serviceFacade)
    {
        _serviceFacade = serviceFacade;
    }

    public void OnGet()
    {
       
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationErrors.Clear();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var response = await _serviceFacade.CreateAsync(Service);

        if (!response.Success)
        {
            ValidationErrors = response.Errors;
            ModelState.AddModelError(string.Empty, response.Message);
            return Page();
        }

        return RedirectToPage("/Services/ServicePage");
    }
}
