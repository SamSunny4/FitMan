using FitMan.Domain.Common;
using FitMan.Domain.Enums;

namespace FitMan.Domain.Entities;

public class MemberMembership : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    
    public int MembershipTypeId { get; set; }
    public MembershipType MembershipType { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public MembershipStatus Status { get; set; } = MembershipStatus.Active;
    
    public int? PaymentId { get; set; }
    public Payment? Payment { get; set; }
    
    public bool AutoRenew { get; set; }
    
    public int DaysUntilExpiry => (EndDate - DateTime.UtcNow).Days;
    public bool IsExpired => DateTime.UtcNow > EndDate;
    public bool IsExpiringSoon => DaysUntilExpiry <= 7 && DaysUntilExpiry >= 0;
    
    // Navigation Properties
    public ICollection<MembershipFreeze> Freezes { get; set; } = new List<MembershipFreeze>();
}
