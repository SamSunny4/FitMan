using FitMan.Domain.Common;

namespace FitMan.Domain.Entities;

public class MaintenanceLog : BaseEntity
{
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    
    public DateTime MaintenanceDate { get; set; }
    public string WorkPerformed { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    
    public string? Technician { get; set; }
    public DateTime? NextServiceDate { get; set; }
    
    public int? PerformedByStaffId { get; set; }
    public Staff? PerformedBy { get; set; }
}
