using FitMan.Domain.Entities;

namespace FitMan.WPF.Services;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(string username, string password);
    void Logout();
    User? CurrentUser { get; }
    Staff? CurrentStaff { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
