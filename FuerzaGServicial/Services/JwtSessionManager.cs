using FuerzaGServicial.Facades.Auth;

namespace FuerzaGServicial.Services.Session;

public class JwtSessionManager
{
    private readonly AuthFacade _authFacade;

    public JwtSessionManager(AuthFacade authFacade)
    {
        _authFacade = authFacade;
    }

    public bool IsAuthenticated => _authFacade.IsAuthenticated;

    public int? UserId => _authFacade.GetUserId();

    public string FullName => _authFacade.GetFullName();

    public string? Token => _authFacade.GetToken();
}
