using FitMan.Domain.Common;
using FitMan.Domain.Enums;

namespace FitMan.Domain.Entities;

public class AttendanceLog : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    
    public DateTime CheckInTime { get; set; } = DateTime.UtcNow;
    public DateTime? CheckOutTime { get; set; }
    
    public EntryMethod EntryMethod { get; set; }
    public string? FacilityArea { get; set; }
    
    public int? ProcessedByStaffId { get; set; }
    public Staff? ProcessedBy { get; set; }
    
    public TimeSpan? Duration => CheckOutTime.HasValue 
        ? CheckOutTime.Value - CheckInTime 
        : null;
}
