using FitMan.Domain.Common;
using FitMan.Domain.Enums;

namespace FitMan.Domain.Entities;

public class Member : BaseEntity
{
    public string MembershipNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AlternatePhone { get; set; }
    
    public string? EmergencyContactName { get; set; }
    public string? EmergencyPhone { get; set; }
    
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    
    public string? PhotoPath { get; set; }
    public string? MedicalConditions { get; set; }
    public string? FitnessGoals { get; set; }
    public string? BloodGroup { get; set; }
    
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public int? ReferredById { get; set; }
    public Member? ReferredBy { get; set; }
    
    public MemberStatus Status { get; set; } = MemberStatus.Active;
    
    // Navigation Properties
    public ICollection<MemberMembership> Memberships { get; set; } = new List<MemberMembership>();
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
