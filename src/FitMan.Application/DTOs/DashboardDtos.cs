namespace FitMan.Application.DTOs;

public class DashboardStats
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int InactiveMembers { get; set; }
    public int TodayCheckIns { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal TodayRevenue { get; set; }
    public int ExpiringMembershipsCount { get; set; }
    public int ActiveMembershipsCount { get; set; }
}

public class AttendanceTrend
{
    public DateTime Date { get; set; }
    public int CheckInCount { get; set; }
    public string DateLabel => Date.ToString("MM/dd");
}

public class ExpiringMembershipDto
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MembershipType { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public int DaysRemaining { get; set; }
    public string Phone { get; set; } = string.Empty;
}
