using FitMan.Domain.Common;

namespace FitMan.Domain.Entities;

public class User : BaseEntity
{
    public int? StaffId { get; set; }
    public Staff? Staff { get; set; }
    
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
    public string? Permissions { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime? LastLogin { get; set; }
}
