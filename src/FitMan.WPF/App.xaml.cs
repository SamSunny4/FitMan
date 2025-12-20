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

public partial class App : System.Windows.Application
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
        services.AddScoped<FitMan.Application.Interfaces.IDashboardService, FitMan.Infrastructure.Services.DashboardService>();
        services.AddScoped<FitMan.Application.Interfaces.IMemberService, FitMan.Infrastructure.Services.MemberService>();

        // Add ViewModels
        services.AddTransient<SplashScreenViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<MemberListViewModel>();
        services.AddTransient<MemberDetailViewModel>();
        services.AddTransient<CheckInViewModel>();
        services.AddTransient<PaymentViewModel>();

        // Add Windows
        services.AddTransient<SplashScreenWindow>();
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
