using FitMan.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FitMan.WPF.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
    }
}
