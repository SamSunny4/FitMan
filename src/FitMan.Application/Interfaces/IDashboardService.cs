using FitMan.Application.DTOs;

namespace FitMan.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardStats> GetDashboardStatsAsync();
    Task<List<AttendanceTrend>> GetAttendanceTrendAsync(int days = 7);
    Task<List<ExpiringMembershipDto>> GetExpiringMembershipsAsync(int daysThreshold = 7);
}
