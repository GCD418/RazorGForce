using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Facades.Services;
using FuerzaGServicial.Models.UserAccounts;

namespace FuerzaGServicial.Pages.Services;

[Authorize(Roles = UserRoles.CEO)] //nos faltaba este atributo
public class Create : PageModel
{
    private readonly ServiceFacade _serviceFacade;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty] public CreateServiceModel Service { get; set; } = new();

    public Create(ServiceFacade serviceFacade)
    {
        _serviceFacade = serviceFacade;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();

        if (!ModelState.IsValid)
        {
            ValidationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Page();
        }

        var result = await _serviceFacade.Create(Service);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo crear el registro.");
            return Page();
        }

        return RedirectToPage("/Services/ServicePage");
    }

    private string MapErrorToField(string error)
    {
        var e = error.ToLowerInvariant();
        if (e.Contains("nombre")) return "Name";
        if (e.Contains("tipo")) return "Type";
        if (e.Contains("precio")) return "Price";
        if (e.Contains("descripción") || e.Contains("descripcion")) return "Description";
        return string.Empty;
    }
}
