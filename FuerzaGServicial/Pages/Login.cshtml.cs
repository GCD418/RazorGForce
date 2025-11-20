using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FuerzaGServicial.Facades.Auth;

namespace FuerzaGServicial.Pages;

public class Login : PageModel
{
    private readonly AuthFacade _authFacade;
    [BindProperty] public InputModel Input { get; set; } = new();

    public Login(AuthFacade authFacade)
    {
        _authFacade = authFacade;
    }

    public IActionResult OnGet()
    {
        if (_authFacade.IsAuthenticated)
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        bool isSuccess = await _authFacade.LoginAsync(Input.Username, Input.Password);
        
        if (!isSuccess)
        {
            var ErrorMessage = "Usuario o contraseña incorrectos.";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }

        return RedirectToPage("/Index");
    }

    public class InputModel
    {
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")] public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await _authFacade.LogoutAsync();
        return RedirectToPage("/Login");
    }
}