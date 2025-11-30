using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.Services;

[Authorize(Roles = "Manager,CEO")]
public class ServicePage : PageModel
{
    public IEnumerable<ServiceModel> Services { get; set; } = new List<ServiceModel>();
    private readonly ServiceFacade _serviceFacade;
    private readonly IDataProtector _protector;
    private readonly JwtSessionManager _sessionManager;

    public ServicePage(
        ServiceFacade serviceFacade,
        IDataProtectionProvider provider,
        JwtSessionManager sessionManager)
    {
        _serviceFacade = serviceFacade;
        _protector = provider.CreateProtector("ServiceProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Services = await _serviceFacade.GetAllAsync();
        return Page();
    }

    public string ProtectId(int id)
    {
        return _protector.Protect(id.ToString());
    }
    public string EncryptId(int id)
    {
        return _protector.Protect(id.ToString());
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToPage();

        var decryptedId = int.Parse(_protector.Unprotect(id));
        await _serviceFacade.DeleteByIdAsync(decryptedId, _sessionManager.UserId ?? 9999);
        return RedirectToPage();
    }
}
