using CommunityToolkit.Mvvm.Input;
using FitMan.WPF.Services;
using FitMan.WPF.Views;

namespace FitMan.WPF.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public LoginViewModel(
        IAuthenticationService authService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        Title = "FitMan - Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter username and password";
            return;
        }

        IsBusy = true;
        try
        {
            var success = await _authService.LoginAsync(Username, Password);

            if (success)
            {
                _navigationService.NavigateTo<MainWindow>();
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowError($"Login failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
