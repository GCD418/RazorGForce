using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.Technicians;
using FuerzaGServicial.Facades.TechnicianFacade;


namespace FuerzaGServicial.Pages.Technicians;

[Authorize(Roles = "Manager,CEO")]
public class TechnicianPage : PageModel
{
    private readonly TechnicianFacade _technicianFacade;
    private readonly IDataProtector _protector;
    private readonly ISessionManager _sessionManager;

    public IEnumerable<TechnicianModel> Technicians { get; set; } = Enumerable.Empty<TechnicianModel>();

    public TechnicianPage(
        TechnicianFacade technicianFacade,
        IDataProtectionProvider provider,
        ISessionManager sessionManager)
    {
        _technicianFacade = technicianFacade;
        _protector = provider.CreateProtector("TechnicianProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Technicians = await _technicianFacade.GetAll();
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

        var success = await _technicianFacade.Delete(
            decryptedId,
            _sessionManager.UserId ?? 9999
        );

        return RedirectToPage();
    }
}
