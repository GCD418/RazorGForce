using FuerzaGServicial.Facades;
using FuerzaGServicial.Services.Session;
namespace FuerzaGServicial.Services;

public class JwtSessionManager : ISessionManager
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

    public string? Role => _authFacade.GetRole();
}


