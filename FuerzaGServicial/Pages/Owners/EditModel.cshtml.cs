using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.ModelsD.Owners;
using FuerzaGServicial.Services.Facades.Owners;
using FuerzaGServicial.Models.UserAccounts;
using FuerzaGServicial.Services.Session;

namespace FuerzaGServicial.Pages.Owners;

[Authorize(Roles = UserRoles.Manager)]
public class EditModel : PageModel
{
    private readonly IOwnerFacade _ownerFacade;
    private readonly ISessionManager _sessionManager;
    private readonly IDataProtector _protector;

    public EditModel(
        IOwnerFacade ownerFacade,
        IDataProtectionProvider provider,
        ISessionManager sessionManager)
    {
        _ownerFacade = ownerFacade;
        _sessionManager = sessionManager;
        _protector = provider.CreateProtector("OwnerProtector");
    }

    public string EncryptedId { get; set; } = string.Empty;

    [BindProperty]
    public UpdateOwnerModel OwnerModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var decryptedId = int.Parse(_protector.Unprotect(id));

        var data = await _ownerFacade.GetById(decryptedId);

        if (data == null)
            return RedirectToPage("/Owners/OwnerPage");

        OwnerModel = new UpdateOwnerModel
        {
            Id = data.Id,
            Name = data.Name,
            FirstLastname = data.FirstLastname,
            SecondLastname = data.SecondLastname,
            PhoneNumber = data.PhoneNumber,
            Email = data.Email,
            DocumentNumber = data.DocumentNumber,
            DocumentExtension = data.DocumentExtension,
            Address = data.Address
        };

        EncryptedId = id;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();

        OwnerModel.UserId = _sessionManager.UserId ?? 9999;

        var result = await _ownerFacade.Update(OwnerModel.Id, OwnerModel);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo actualizar el dueño.");
            return Page();
        }

        return RedirectToPage("/Owners/OwnerPage");
    }
}
