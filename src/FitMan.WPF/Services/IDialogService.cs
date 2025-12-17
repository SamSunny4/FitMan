namespace FitMan.WPF.Services;

public interface IDialogService
{
    void ShowMessage(string message, string title = "Information");
    void ShowError(string message, string title = "Error");
    bool ShowConfirmation(string message, string title = "Confirmation");
    void ShowWarning(string message, string title = "Warning");
}
