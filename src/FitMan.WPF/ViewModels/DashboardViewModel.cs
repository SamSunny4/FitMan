using CommunityToolkit.Mvvm.Input;
using FitMan.Application.DTOs;
using FitMan.Application.Interfaces;
using System.Collections.ObjectModel;

namespace FitMan.WPF.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IDashboardService _dashboardService;
    
    private DashboardStats? _stats;
    private ObservableCollection<AttendanceTrend> _attendanceTrends = new();
    private ObservableCollection<ExpiringMembershipDto> _expiringMemberships = new();

    public DashboardStats? Stats
    {
        get => _stats;
        set => SetProperty(ref _stats, value);
    }

    public ObservableCollection<AttendanceTrend> AttendanceTrends
    {
        get => _attendanceTrends;
        set => SetProperty(ref _attendanceTrends, value);
    }

    public ObservableCollection<ExpiringMembershipDto> ExpiringMemberships
    {
        get => _expiringMemberships;
        set => SetProperty(ref _expiringMemberships, value);
    }

    public DashboardViewModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
        Title = "Dashboard";
        _ = LoadDashboardDataAsync();
    }

    private async Task LoadDashboardDataAsync()
    {
        IsBusy = true;
        try
        {
            var statsTask = _dashboardService.GetDashboardStatsAsync();
            var trendsTask = _dashboardService.GetAttendanceTrendAsync(7);
            var expiringTask = _dashboardService.GetExpiringMembershipsAsync(7);

            await Task.WhenAll(statsTask, trendsTask, expiringTask);

            Stats = await statsTask;
            AttendanceTrends = new ObservableCollection<AttendanceTrend>(await trendsTask);
            ExpiringMemberships = new ObservableCollection<ExpiringMembershipDto>(await expiringTask);
        }
        catch (Exception ex)
        {
            // Log error
            System.Diagnostics.Debug.WriteLine($"Error loading dashboard: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDashboardDataAsync();
    }
}
