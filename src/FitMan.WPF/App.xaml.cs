using FitMan.Infrastructure.Data;
using FitMan.Infrastructure.Repositories;
using FitMan.Domain.Interfaces;
using FitMan.WPF.Services;
using FitMan.WPF.ViewModels;
using FitMan.WPF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using Serilog;

namespace FitMan.WPF;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;
    public static IConfiguration Configuration { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Setup configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();

        // Setup logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/fitman-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Setup dependency injection
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        // Initialize database
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<FitManDbContext>();
            DbInitializer.Initialize(context);
        }

        // Show login window
        var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
        loginWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Add DbContext
        services.AddDbContext<FitManDbContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

        // Add repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();

        // Add ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<MemberListViewModel>();
        services.AddTransient<MemberDetailViewModel>();
        services.AddTransient<CheckInViewModel>();
        services.AddTransient<PaymentViewModel>();

        // Add Windows
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
