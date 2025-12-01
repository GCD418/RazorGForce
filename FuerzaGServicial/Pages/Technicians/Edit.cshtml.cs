using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Technicians;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.Technicians;

[Authorize(Roles = UserRoles.Manager)]
public class Edit : PageModel
{
    private readonly TechnicianFacade _technicianFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    public List<string> ValidationErrors { get; set; } = new();

    [BindProperty]
    public TechnicianModel Technician { get; set; } = new();

    public Edit(TechnicianFacade technicianFacade, IDataProtectionProvider provider, JwtSessionManager sessionManager)
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

        var tech = await _technicianFacade.GetByIdAsync(decryptedId);
        if (tech == null)
            return RedirectToPage("/Technicians/TechnicianPage");

        Technician = new TechnicianModel
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

        if (!ModelState.IsValid)
            return Page();

        var result = await _technicianFacade.UpdateAsync(Technician, _sessionManager.UserId ?? 9999);
        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo actualizar el técnico.");
            return Page();
        }

        return RedirectToPage("/Technicians/TechnicianPage");
    }
}
