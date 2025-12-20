using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FitMan.WPF.Services;

public class NavigationService : INavigationService
{
    public void NavigateTo<T>() where T : Window
    {
        var window = App.ServiceProvider.GetRequiredService<T>();
        
        // Close current main window if exists
        System.Windows.Application.Current.MainWindow?.Close();
        
        System.Windows.Application.Current.MainWindow = window;
        window.Show();
    }

    public void ShowDialog<T>() where T : Window
    {
        var window = App.ServiceProvider.GetRequiredService<T>();
        window.ShowDialog();
    }

    public void CloseCurrentWindow()
    {
        System.Windows.Application.Current.MainWindow?.Close();
    }
}
