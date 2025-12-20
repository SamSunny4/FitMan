using CommunityToolkit.Mvvm.ComponentModel;
using FitMan.Infrastructure.Data;
using FitMan.WPF.Services;
using FitMan.WPF.Views;
using Microsoft.EntityFrameworkCore;

namespace FitMan.WPF.ViewModels;

public partial class SplashScreenViewModel : ViewModelBase
{
    private readonly FitManDbContext _dbContext;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _statusMessage = "Initializing...";

    [ObservableProperty]
    private int _progressValue = 0;

    public SplashScreenViewModel(
        FitManDbContext dbContext,
        INavigationService navigationService)
    {
        _dbContext = dbContext;
        _navigationService = navigationService;
        Title = "FitMan - Loading";
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Step 1: Database Connection
            StatusMessage = "Connecting to database...";
            ProgressValue = 20;
            await Task.Delay(500);

            // Step 2: Database Migration
            StatusMessage = "Checking database migrations...";
            ProgressValue = 40;
            await _dbContext.Database.MigrateAsync();
            await Task.Delay(500);

            // Step 3: License Verification (placeholder)
            StatusMessage = "Verifying license...";
            ProgressValue = 60;
            await Task.Delay(500);

            // Step 4: Initialize Data
            StatusMessage = "Loading application data...";
            ProgressValue = 80;
            await DbInitializer.InitializeAsync(_dbContext);
            await Task.Delay(500);

            // Step 5: Complete
            StatusMessage = "Starting application...";
            ProgressValue = 100;
            await Task.Delay(500);

            // Navigate to main window
            _navigationService.NavigateTo<MainWindow>();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            await Task.Delay(3000);
            System.Windows.Application.Current.Shutdown();
        }
    }
}
