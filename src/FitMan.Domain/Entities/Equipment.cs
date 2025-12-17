using FitMan.Domain.Common;

namespace FitMan.Domain.Entities;

public class Equipment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchaseCost { get; set; }
    
    public string? Vendor { get; set; }
    public DateTime? WarrantyEndDate { get; set; }
    
    public string? LocationZone { get; set; }
    public string Status { get; set; } = "Working";
    
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    
    // Navigation Properties
    public ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
}
