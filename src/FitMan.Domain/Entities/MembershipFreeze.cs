using FitMan.Domain.Common;

namespace FitMan.Domain.Entities;

public class MembershipFreeze : BaseEntity
{
    public int MemberMembershipId { get; set; }
    public MemberMembership MemberMembership { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
    
    public int CreatedByStaffId { get; set; }
    public Staff CreatedBy { get; set; } = null!;
    
    public int FrozenDays => (EndDate - StartDate).Days;
}
