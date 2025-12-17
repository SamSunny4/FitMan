using FitMan.Domain.Entities;
using FitMan.Domain.Interfaces;
using FitMan.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitMan.WPF.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly FitManDbContext _context;
    public User? CurrentUser { get; private set; }
    public Staff? CurrentStaff { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;

    public AuthenticationService(FitManDbContext context)
    {
        _context = context;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _context.Users
            .Include(u => u.Staff)
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

        if (user == null)
            return false;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return false;

        CurrentUser = user;
        CurrentStaff = user.Staff;
        
        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
        CurrentStaff = null;
    }

    public bool IsInRole(string role)
    {
        return CurrentUser?.Role.Equals(role, StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
