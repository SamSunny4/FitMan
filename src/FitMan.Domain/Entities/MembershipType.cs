using FitMan.Domain.Common;

namespace FitMan.Domain.Entities;

public class MembershipType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationInDays { get; set; }
    
    public decimal Price { get; set; }
    public decimal TaxPercentage { get; set; }
    public decimal TotalPrice => Price + (Price * TaxPercentage / 100);
    
    public int MaxFreezeDays { get; set; }
    public int MaxGuestVisits { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public ICollection<MemberMembership> MemberMemberships { get; set; } = new List<MemberMembership>();
}
