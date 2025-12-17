using FitMan.Domain.Common;
using FitMan.Domain.Enums;

namespace FitMan.Domain.Entities;

public class Staff : BaseEntity
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }
    public StaffRole Role { get; set; }
    
    public DateTime JoinDate { get; set; }
    public DateTime? ExitDate { get; set; }
    
    public decimal Salary { get; set; }
    public string? ShiftTimings { get; set; }
    public string? Certifications { get; set; }
    public string? Specializations { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public ICollection<Payment> ProcessedPayments { get; set; } = new List<Payment>();
    public ICollection<AttendanceLog> ProcessedCheckIns { get; set; } = new List<AttendanceLog>();
    public User? User { get; set; }
}
