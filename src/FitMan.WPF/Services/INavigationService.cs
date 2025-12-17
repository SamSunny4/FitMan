using System.Windows;

namespace FitMan.WPF.Services;

public interface INavigationService
{
    void NavigateTo<T>() where T : Window;
    void ShowDialog<T>() where T : Window;
    void CloseCurrentWindow();
}
