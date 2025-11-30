using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Technicians;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.Technicians;

[Authorize(Roles = UserRoles.Manager + "," + UserRoles.CEO)]
public class TechnicianPage : PageModel
{
    private readonly TechnicianFacade _technicianFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    public IEnumerable<TechnicianModel> Technicians { get; set; } = Enumerable.Empty<TechnicianModel>();

    public TechnicianPage(
        TechnicianFacade technicianFacade,
        IDataProtectionProvider provider,
        JwtSessionManager sessionManager)
    {
        _technicianFacade = technicianFacade;
        _protector = provider.CreateProtector("TechnicianProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Technicians = await _technicianFacade.GetAllAsync();
        return Page();
    }

    public string EncryptId(int id)
    {
        return _protector.Protect(id.ToString());
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToPage();

        int decryptedId;
        try
        {
            decryptedId = int.Parse(_protector.Unprotect(id));
        }
        catch
        {
            return RedirectToPage();
        }

        var userId = _sessionManager.UserId ?? 9999;

        var success = await _technicianFacade.DeleteByIdAsync(decryptedId, userId);

        return RedirectToPage();
    }
}
