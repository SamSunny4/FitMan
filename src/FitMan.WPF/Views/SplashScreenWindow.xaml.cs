using FitMan.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FitMan.WPF.Views;

public partial class SplashScreenWindow : Window
{
    private readonly SplashScreenViewModel _viewModel;

    public SplashScreenWindow()
    {
        InitializeComponent();
        _viewModel = App.ServiceProvider.GetRequiredService<SplashScreenViewModel>();
        DataContext = _viewModel;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.InitializeAsync();
    }
}
