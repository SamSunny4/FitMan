using FitMan.Domain.Entities;
using FitMan.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitMan.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(FitManDbContext context)
    {
        context.Database.EnsureCreated();

        // Check if database is already seeded
        if (context.MembershipTypes.Any())
        {
            return;
        }

        SeedMembershipTypes(context);
        SeedStaff(context);
        SeedUsers(context);
        SeedSampleData(context);

        context.SaveChanges();
    }

    private static void SeedMembershipTypes(FitManDbContext context)
    {
        var membershipTypes = new[]
        {
            new MembershipType
            {
                Name = "Daily Pass",
                Description = "Single day access to all gym facilities",
                DurationInDays = 1,
                Price = 10.00m,
                TaxPercentage = 5.00m,
                MaxFreezeDays = 0,
                MaxGuestVisits = 0,
                IsActive = true
            },
            new MembershipType
            {
                Name = "Monthly",
                Description = "30 days membership with full gym access",
                DurationInDays = 30,
                Price = 50.00m,
                TaxPercentage = 5.00m,
                MaxFreezeDays = 3,
                MaxGuestVisits = 1,
                IsActive = true
            },
            new MembershipType
            {
                Name = "Quarterly",
                Description = "90 days membership with full gym access and 1 free PT session",
                DurationInDays = 90,
                Price = 135.00m,
                TaxPercentage = 5.00m,
                MaxFreezeDays = 7,
                MaxGuestVisits = 3,
                IsActive = true
            },
            new MembershipType
            {
                Name = "Half-Yearly",
                Description = "180 days membership with full gym access and 2 free PT sessions",
                DurationInDays = 180,
                Price = 250.00m,
                TaxPercentage = 5.00m,
                MaxFreezeDays = 14,
                MaxGuestVisits = 5,
                IsActive = true
            },
            new MembershipType
            {
                Name = "Annual",
                Description = "365 days premium membership with all facilities and 5 free PT sessions",
                DurationInDays = 365,
                Price = 450.00m,
                TaxPercentage = 5.00m,
                MaxFreezeDays = 30,
                MaxGuestVisits = 10,
                IsActive = true
            }
        };

        context.MembershipTypes.AddRange(membershipTypes);
    }

    private static void SeedStaff(FitManDbContext context)
    {
        var staff = new[]
        {
            new Staff
            {
                EmployeeCode = "EMP001",
                FirstName = "Admin",
                LastName = "User",
                Phone = "1234567890",
                Email = "admin@fitman.com",
                Address = "123 Main St",
                City = "City",
                State = "State",
                ZipCode = "12345",
                DateOfBirth = new DateTime(1990, 1, 1),
                Role = StaffRole.Admin,
                JoinDate = DateTime.UtcNow,
                Salary = 5000m,
                IsActive = true
            },
            new Staff
            {
                EmployeeCode = "EMP002",
                FirstName = "John",
                LastName = "Receptionist",
                Phone = "1234567891",
                Email = "reception@fitman.com",
                Address = "124 Main St",
                City = "City",
                State = "State",
                ZipCode = "12345",
                DateOfBirth = new DateTime(1995, 3, 15),
                Role = StaffRole.Receptionist,
                JoinDate = DateTime.UtcNow,
                Salary = 3000m,
                IsActive = true
            }
        };

        context.Staff.AddRange(staff);
        context.SaveChanges(); // Save to get Staff IDs for User creation
    }

    private static void SeedUsers(FitManDbContext context)
    {
        var adminStaff = context.Staff.First(s => s.EmployeeCode == "EMP001");
        var receptionStaff = context.Staff.First(s => s.EmployeeCode == "EMP002");

        var users = new[]
        {
            new User
            {
                StaffId = adminStaff.Id,
                Username = "admin",
                // Password: admin123 (hashed with BCrypt)
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                IsActive = true,
                LastLogin = null
            },
            new User
            {
                StaffId = receptionStaff.Id,
                Username = "reception",
                // Password: reception123
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("reception123"),
                Role = "Receptionist",
                IsActive = true,
                LastLogin = null
            }
        };

        context.Users.AddRange(users);
    }

    private static void SeedSampleData(FitManDbContext context)
    {
        // Get membership types
        var monthlyMembership = context.MembershipTypes.First(m => m.Name == "Monthly");
        var annualMembership = context.MembershipTypes.First(m => m.Name == "Annual");
        var adminStaff = context.Staff.First(s => s.EmployeeCode == "EMP001");

        // Create sample members
        var members = new[]
        {
            new Member
            {
                MembershipNumber = "GYM001",
                FirstName = "Sarah",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1995, 5, 20),
                Gender = Gender.Female,
                Phone = "9876543210",
                Email = "sarah.johnson@email.com",
                Address = "456 Oak Avenue",
                City = "Springfield",
                State = "IL",
                ZipCode = "62701",
                EnrollmentDate = DateTime.UtcNow.AddMonths(-3),
                Status = MemberStatus.Active,
                FitnessGoals = "Weight loss and cardio fitness"
            },
            new Member
            {
                MembershipNumber = "GYM002",
                FirstName = "Michael",
                LastName = "Smith",
                DateOfBirth = new DateTime(1988, 8, 15),
                Gender = Gender.Male,
                Phone = "9876543211",
                Email = "michael.smith@email.com",
                Address = "789 Pine Street",
                City = "Springfield",
                State = "IL",
                ZipCode = "62702",
                EnrollmentDate = DateTime.UtcNow.AddMonths(-6),
                Status = MemberStatus.Active,
                FitnessGoals = "Muscle building and strength training"
            }
        };

        context.Members.AddRange(members);
        context.SaveChanges(); // Save to get Member IDs

        // Create payments for memberships
        var payments = new[]
        {
            new Payment
            {
                MemberId = members[0].Id,
                Amount = monthlyMembership.Price,
                TaxAmount = monthlyMembership.Price * monthlyMembership.TaxPercentage / 100,
                TotalAmount = monthlyMembership.TotalPrice,
                PaymentDate = DateTime.UtcNow.AddMonths(-3),
                DueDate = DateTime.UtcNow.AddMonths(-3),
                PaymentMethod = PaymentMethod.Card,
                PaymentType = PaymentType.Membership,
                Status = PaymentStatus.Paid,
                ReceiptNumber = $"REC{DateTime.UtcNow:yyyyMMdd}001",
                ProcessedByStaffId = adminStaff.Id
            },
            new Payment
            {
                MemberId = members[1].Id,
                Amount = annualMembership.Price,
                TaxAmount = annualMembership.Price * annualMembership.TaxPercentage / 100,
                TotalAmount = annualMembership.TotalPrice,
                PaymentDate = DateTime.UtcNow.AddMonths(-6),
                DueDate = DateTime.UtcNow.AddMonths(-6),
                PaymentMethod = PaymentMethod.Cash,
                PaymentType = PaymentType.Membership,
                Status = PaymentStatus.Paid,
                ReceiptNumber = $"REC{DateTime.UtcNow:yyyyMMdd}002",
                ProcessedByStaffId = adminStaff.Id
            }
        };

        context.Payments.AddRange(payments);
        context.SaveChanges();

        // Create memberships
        var memberships = new[]
        {
            new MemberMembership
            {
                MemberId = members[0].Id,
                MembershipTypeId = monthlyMembership.Id,
                StartDate = DateTime.UtcNow.AddMonths(-3),
                EndDate = DateTime.UtcNow.AddMonths(-2),
                Status = MembershipStatus.Expired,
                PaymentId = payments[0].Id,
                AutoRenew = true
            },
            new MemberMembership
            {
                MemberId = members[1].Id,
                MembershipTypeId = annualMembership.Id,
                StartDate = DateTime.UtcNow.AddMonths(-6),
                EndDate = DateTime.UtcNow.AddMonths(6),
                Status = MembershipStatus.Active,
                PaymentId = payments[1].Id,
                AutoRenew = true
            }
        };

        context.MemberMemberships.AddRange(memberships);

        // Create attendance logs
        var attendanceLogs = new List<AttendanceLog>();
        var random = new Random();
        
        // Generate attendance for the past 30 days for active member
        for (int i = 0; i < 30; i++)
        {
            if (random.Next(0, 3) != 0) // 66% chance of attendance each day
            {
                attendanceLogs.Add(new AttendanceLog
                {
                    MemberId = members[1].Id,
                    CheckInTime = DateTime.UtcNow.AddDays(-i).AddHours(random.Next(6, 20)),
                    CheckOutTime = DateTime.UtcNow.AddDays(-i).AddHours(random.Next(7, 21)),
                    EntryMethod = EntryMethod.Manual,
                    FacilityArea = "Main Gym",
                    ProcessedByStaffId = adminStaff.Id
                });
            }
        }

        context.AttendanceLogs.AddRange(attendanceLogs);
    }
}
