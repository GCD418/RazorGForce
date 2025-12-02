using System.ComponentModel.DataAnnotations;
using FuerzaGServicial.Facades;
using FuerzaGServicial.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FuerzaGServicial.Pages.UserAccounts;

[Authorize]
public class ForceChangePasswordModel : PageModel
{
    private readonly AuthFacade _authFacade;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public List<string> ValidationErrors { get; set; } = new();

    public ForceChangePasswordModel(AuthFacade authFacade)
    {
        _authFacade = authFacade;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = _authFacade.GetUserId();
        if (userId == null)
        {
            ErrorMessage = "Usuario no autenticado";
            return Page();
        }

        var request = new ChangePasswordRequest
        {
            CurrentPassword = null, 
            NewPassword = Input.NewPassword,
            ConfirmPassword = Input.ConfirmPassword
        };

        var result = await _authFacade.ChangePasswordAsync(request, userId.Value);

        if (!result.Success)
        {
            ErrorMessage = result.Message;
            ValidationErrors = result.Errors ?? new List<string>();
            return Page();
        }

        await _authFacade.LogoutAsync();

        TempData["SuccessMessage"] = "Contraseña cambiada exitosamente. Por favor, inicia sesión con tu nueva contraseña.";
        return RedirectToPage("/Login");
    }

    public class InputModel
    {
        [Display(Name = "Nueva Contraseña")]
        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "Debe confirmar su nueva contraseña")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
