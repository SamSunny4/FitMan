using CommunityToolkit.Mvvm.Input;
using FitMan.WPF.Services;
using FitMan.WPF.Views;
using System.Windows;

namespace FitMan.WPF.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private object? _currentView;
    private string _currentUserName = string.Empty;

    public object? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public string CurrentUserName
    {
        get => _currentUserName;
        set => SetProperty(ref _currentUserName, value);
    }

    public MainViewModel(
        IAuthenticationService authService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _dialogService = dialogService;

        Title = "FitMan - Gym Management System";
        CurrentUserName = _authService.CurrentStaff?.FullName ?? "User";

        // Load dashboard by default
        LoadDashboard();
    }

    [RelayCommand]
    private void LoadDashboard()
    {
        CurrentView = App.ServiceProvider.GetService(typeof(DashboardViewModel));
    }

    [RelayCommand]
    private void LoadMembers()
    {
        CurrentView = App.ServiceProvider.GetService(typeof(MemberListViewModel));
    }

    [RelayCommand]
    private void LoadCheckIn()
    {
        CurrentView = App.ServiceProvider.GetService(typeof(CheckInViewModel));
    }

    [RelayCommand]
    private void LoadPayments()
    {
        CurrentView = App.ServiceProvider.GetService(typeof(PaymentViewModel));
    }

    [RelayCommand]
    private void Logout()
    {
        var confirm = _dialogService.ShowConfirmation("Are you sure you want to logout?", "Logout");
        if (confirm)
        {
            _authService.Logout();
            _navigationService.NavigateTo<LoginWindow>();
        }
    }
}
