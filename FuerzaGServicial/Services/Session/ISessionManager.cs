namespace FuerzaGServicial.Services.Session
{
    public interface ISessionManager
    {
        bool IsAuthenticated { get; }
        int? UserId { get; }
        string FullName { get; }
        string? Token { get; }
        string? Role { get; }
    }
}
