using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Models.Services;
using FuerzaGServicial.Facades.Services;

namespace FuerzaGServicial.Pages.Services;

[Authorize(Roles = "Manager,CEO")] //nos falta
public class ServicePage : PageModel
{
    public IEnumerable<ServiceModel> Services { get; set; } = Enumerable.Empty<ServiceModel>();

    private readonly ServiceFacade _serviceFacade;
    private readonly IDataProtector _protector;
    private readonly ISessionManager _sessionManager; // nos falta

    public ServicePage(ServiceFacade serviceFacade,
                       IDataProtectionProvider provider,
                       ISessionManager sessionManager) //nos falta
    {
        _serviceFacade = serviceFacade;
        _protector = provider.CreateProtector("ServiceProtector");
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Services = await _serviceFacade.GetAll();
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

        var success = await _serviceFacade.Delete(decryptedId, _sessionManager.UserId ?? 9999);

        return RedirectToPage();
    }
}
