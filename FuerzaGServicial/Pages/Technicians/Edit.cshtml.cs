using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.ModelsD.Technicians;
using FuerzaGServicial.Services.Facades.Technicians;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services.Session;

namespace FuerzaGServicial.Pages.Technicians;

[Authorize(Roles = UserRoles.Manager)]
public class Edit : PageModel
{
    private readonly ITechnicianFacade _technicianFacade;
    private readonly IDataProtector _protector;
    private readonly ISessionManager _sessionManager;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty]
    public UpdateTechnicianModel Technician { get; set; } = new();

    public Edit(
        ITechnicianFacade technicianFacade,
        IDataProtectionProvider provider
        ,ISessionManager sessionManager)
    {
        _technicianFacade = technicianFacade;
        _protector = provider.CreateProtector("TechnicianProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToPage("/Technicians/TechnicianPage");

        int decryptedId;
        try
        {
            decryptedId = int.Parse(_protector.Unprotect(id));
        }
        catch
        {
            return RedirectToPage("/Technicians/TechnicianPage");
        }

        var tech = await _technicianFacade.GetById(decryptedId);

        if (tech == null)
            return RedirectToPage("/Technicians/TechnicianPage");

        Technician = new UpdateTechnicianModel
        {
            Id = tech.Id,
            Name = tech.Name,
            FirstLastName = tech.FirstLastName,
            SecondLastName = tech.SecondLastName,
            PhoneNumber = tech.PhoneNumber,
            Email = tech.Email,
            DocumentNumber = tech.DocumentNumber,
            DocumentExtension = tech.DocumentExtension,
            Address = tech.Address,
            BaseSalary = tech.BaseSalary
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();

       Technician.UserId = _sessionManager.UserId ?? 9999;

        var result = await _technicianFacade.Update(Technician.Id, Technician);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo actualizar el técnico.");
            return Page();
        }

        return RedirectToPage("/Technicians/TechnicianPage");
    }

    private string MapErrorToField(string error)
    {
        var e = error.ToLowerInvariant();

        if (e.Contains("nombre")) return "Name";
        if (e.Contains("apellido")) return "FirstLastName";
        if (e.Contains("segundo")) return "SecondLastName";
        if (e.Contains("teléfono") || e.Contains("telefono")) return "PhoneNumber";
        if (e.Contains("correo") || e.Contains("email")) return "Email";
        if (e.Contains("documento")) return "DocumentNumber";
        if (e.Contains("extensión") || e.Contains("extension")) return "DocumentExtension";
        if (e.Contains("dirección") || e.Contains("direccion")) return "Address";
        if (e.Contains("salario")) return "BaseSalary";

        return string.Empty;
    }
}
