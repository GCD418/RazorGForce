using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.Services;

[Authorize(Roles = UserRoles.CEO)]
public class Edit : PageModel
{
    private readonly ServiceFacade _serviceFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    [BindProperty]
    public ServiceModel Service { get; set; } = new();

    public List<string> ValidationErrors { get; set; } = new();

    public Edit(
        ServiceFacade serviceFacade,
        IDataProtectionProvider provider,
        JwtSessionManager sessionManager)
    {
        _serviceFacade = serviceFacade;
        _protector = provider.CreateProtector("ServiceProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage("/Services/ServicePage");

        if (!int.TryParse(_protector.Unprotect(id), out var decryptedId))
            return RedirectToPage("/Services/ServicePage");

        var service = await _serviceFacade.GetByIdAsync(decryptedId);
        if (service == null)
            return RedirectToPage("/Services/ServicePage");

        Service = service;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationErrors.Clear();

        if (!ModelState.IsValid)
            return Page();

        if (string.IsNullOrWhiteSpace(Service.Name))
            ModelState.AddModelError("Service.Name", "El nombre es requerido");

        if (string.IsNullOrWhiteSpace(Service.Type))
            ModelState.AddModelError("Service.Type", "El tipo es requerido");

        if (Service.Price == null || Service.Price <= 0)
            ModelState.AddModelError("Service.Price", "El precio debe ser mayor a 0");

        if (string.IsNullOrWhiteSpace(Service.Description))
            ModelState.AddModelError("Service.Description", "La descripción es requerida");

        if (!ModelState.IsValid)
            return Page();
        
        var existingService = await _serviceFacade.GetByIdAsync(Service.Id);
        
        if (existingService == null)
        {
            ModelState.AddModelError(string.Empty, "El servicio no existe");
            return Page();
        }
        
        existingService.Name = Service.Name.Trim();
        existingService.Type = Service.Type.Trim();
        existingService.Description = Service.Description.Trim();
        existingService.Price = Service.Price;
        
        var response = await _serviceFacade.UpdateAsync(existingService, _sessionManager.UserId ?? 9999);
        
        if (!response.Success)
        {
            ValidationErrors = response.Errors;
            foreach (var error in response.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return Page();
        }

        TempData["SuccessMessage"] = response.Message;
        return RedirectToPage("/Services/ServicePage");
    }
}
