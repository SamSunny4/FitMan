using FitMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitMan.Infrastructure.Data;

public class FitManDbContext : DbContext
{
    public FitManDbContext(DbContextOptions<FitManDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<MembershipType> MembershipTypes => Set<MembershipType>();
    public DbSet<MemberMembership> MemberMemberships => Set<MemberMembership>();
    public DbSet<MembershipFreeze> MembershipFreezes => Set<MembershipFreeze>();
    public DbSet<AttendanceLog> AttendanceLogs => Set<AttendanceLog>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<MaintenanceLog> MaintenanceLogs => Set<MaintenanceLog>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Member Configuration
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.MembershipNumber).IsUnique();
            entity.HasIndex(e => e.Phone);
            entity.HasIndex(e => e.Email);
            
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MembershipNumber).IsRequired().HasMaxLength(20);
            
            entity.HasOne(e => e.ReferredBy)
                .WithMany()
                .HasForeignKey(e => e.ReferredById)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // MembershipType Configuration
        modelBuilder.Entity<MembershipType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.TaxPercentage).HasPrecision(5, 2);
        });

        // MemberMembership Configuration
        modelBuilder.Entity<MemberMembership>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.MemberId, e.StartDate });
            
            entity.HasOne(e => e.Member)
                .WithMany(m => m.Memberships)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.MembershipType)
                .WithMany(mt => mt.MemberMemberships)
                .HasForeignKey(e => e.MembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Payment)
                .WithMany()
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // MembershipFreeze Configuration
        modelBuilder.Entity<MembershipFreeze>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.MemberMembership)
                .WithMany(mm => mm.Freezes)
                .HasForeignKey(e => e.MemberMembershipId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedByStaffId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // AttendanceLog Configuration
        modelBuilder.Entity<AttendanceLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.MemberId, e.CheckInTime });
            entity.HasIndex(e => e.CheckInTime);
            
            entity.HasOne(e => e.Member)
                .WithMany(m => m.AttendanceLogs)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.ProcessedBy)
                .WithMany(s => s.ProcessedCheckIns)
                .HasForeignKey(e => e.ProcessedByStaffId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Payment Configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ReceiptNumber).IsUnique();
            entity.HasIndex(e => new { e.MemberId, e.PaymentDate });
            entity.HasIndex(e => e.DueDate);
            
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.ReceiptNumber).IsRequired().HasMaxLength(50);
            
            entity.HasOne(e => e.Member)
                .WithMany(m => m.Payments)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.ProcessedBy)
                .WithMany(s => s.ProcessedPayments)
                .HasForeignKey(e => e.ProcessedByStaffId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Staff Configuration
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EmployeeCode).IsUnique();
            entity.HasIndex(e => e.Phone);
            entity.HasIndex(e => e.Email);
            
            entity.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Salary).HasPrecision(18, 2);
        });

        // Equipment Configuration
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PurchaseCost).HasPrecision(18, 2);
        });

        // MaintenanceLog Configuration
        modelBuilder.Entity<MaintenanceLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EquipmentId, e.MaintenanceDate });
            
            entity.Property(e => e.Cost).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Equipment)
                .WithMany(eq => eq.MaintenanceLogs)
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.PerformedBy)
                .WithMany()
                .HasForeignKey(e => e.PerformedByStaffId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            
            entity.HasOne(e => e.Staff)
                .WithOne(s => s.User)
                .HasForeignKey<User>(e => e.StaffId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
