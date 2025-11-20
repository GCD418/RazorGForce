using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.DataProtection;
using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Services.Facades.Services;

namespace FuerzaGServicial.Pages.Services;

[Authorize(Roles = UserRoles.CEO)] //falta agregar el atributo
public class Edit : PageModel
{
    private readonly IServiceFacade _serviceFacade;
    private readonly IDataProtector _protector;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty] public UpdateServiceModel Service { get; set; } = new();

    public Edit(IServiceFacade serviceFacade, IDataProtectionProvider provider)
    {
        _serviceFacade = serviceFacade;
        _protector = provider.CreateProtector("ServiceProtector");
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToPage("/Services/ServicePage");

        int decryptedId;
        try
        {
            decryptedId = int.Parse(_protector.Unprotect(id));
        }
        catch
        {
            return RedirectToPage("/Services/ServicePage");
        }

        var service = await _serviceFacade.GetById(decryptedId);
        if (service == null)
            return RedirectToPage("/Services/ServicePage");

        Service = new UpdateServiceModel
        {
            Id = service.Id,
            Name = service.Name,
            Type = service.Type,
            Price = service.Price,
            Description = service.Description
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();

        var result = await _serviceFacade.Update(Service.Id, Service);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo actualizar el servicio.");
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
