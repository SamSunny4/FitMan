using FitMan.Application.DTOs;
using FitMan.Application.Interfaces;
using FitMan.Domain.Enums;
using FitMan.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitMan.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly FitManDbContext _context;

    public DashboardService(FitManDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var today = now.Date;

        var stats = new DashboardStats
        {
            TotalMembers = await _context.Members.CountAsync(),
            ActiveMembers = await _context.Members
                .CountAsync(m => m.Status == MemberStatus.Active),
            InactiveMembers = await _context.Members
                .CountAsync(m => m.Status == MemberStatus.Inactive || m.Status == MemberStatus.Suspended),
            TodayCheckIns = await _context.AttendanceLogs
                .Where(a => a.CheckInTime.Date == today)
                .CountAsync(),
            MonthlyRevenue = await _context.Payments
                .Where(p => p.PaymentDate >= startOfMonth && p.Status == PaymentStatus.Paid)
                .SumAsync(p => (decimal?)p.TotalAmount) ?? 0m,
            TodayRevenue = await _context.Payments
                .Where(p => p.PaymentDate.Date == today && p.Status == PaymentStatus.Paid)
                .SumAsync(p => (decimal?)p.TotalAmount) ?? 0m,
            ExpiringMembershipsCount = await _context.MemberMemberships
                .Where(m => m.Status == MembershipStatus.Active && 
                           m.EndDate <= now.AddDays(7) && 
                           m.EndDate >= now)
                .CountAsync(),
            ActiveMembershipsCount = await _context.MemberMemberships
                .Where(m => m.Status == MembershipStatus.Active && m.EndDate >= now)
                .CountAsync()
        };

        return stats;
    }

    public async Task<List<AttendanceTrend>> GetAttendanceTrendAsync(int days = 7)
    {
        var startDate = DateTime.UtcNow.Date.AddDays(-days + 1);
        var endDate = DateTime.UtcNow.Date.AddDays(1);

        var attendanceData = await _context.AttendanceLogs
            .Where(a => a.CheckInTime >= startDate && a.CheckInTime < endDate)
            .GroupBy(a => a.CheckInTime.Date)
            .Select(g => new AttendanceTrend
            {
                Date = g.Key,
                CheckInCount = g.Count()
            })
            .OrderBy(a => a.Date)
            .ToListAsync();

        // Fill in missing dates with zero counts
        var allDates = Enumerable.Range(0, days)
            .Select(i => startDate.AddDays(i))
            .ToList();

        var result = allDates
            .Select(date => attendanceData.FirstOrDefault(a => a.Date == date) ?? new AttendanceTrend
            {
                Date = date,
                CheckInCount = 0
            })
            .ToList();

        return result;
    }

    public async Task<List<ExpiringMembershipDto>> GetExpiringMembershipsAsync(int daysThreshold = 7)
    {
        var now = DateTime.UtcNow;
        var thresholdDate = now.AddDays(daysThreshold);

        var expiringMemberships = await _context.MemberMemberships
            .Include(mm => mm.Member)
            .Include(mm => mm.MembershipType)
            .Where(mm => mm.Status == MembershipStatus.Active &&
                        mm.EndDate >= now &&
                        mm.EndDate <= thresholdDate)
            .OrderBy(mm => mm.EndDate)
            .Select(mm => new ExpiringMembershipDto
            {
                MemberId = mm.MemberId,
                MemberName = mm.Member.FirstName + " " + mm.Member.LastName,
                MembershipType = mm.MembershipType.Name,
                ExpiryDate = mm.EndDate,
                DaysRemaining = (int)(mm.EndDate - now).TotalDays,
                Phone = mm.Member.Phone
            })
            .ToListAsync();

        return expiringMemberships;
    }
}
