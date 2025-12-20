using CommunityToolkit.Mvvm.Input;
using FitMan.Application.Interfaces;
using FitMan.Domain.Entities;
using FitMan.WPF.Services;
using System.Collections.ObjectModel;

namespace FitMan.WPF.ViewModels;

public partial class MemberListViewModel : ViewModelBase
{
    private readonly IMemberService _memberService;
    private readonly IDialogService _dialogService;
    
    private ObservableCollection<Member> _members = new();
    private string _searchText = string.Empty;
    private Member? _selectedMember;

    public ObservableCollection<Member> Members
    {
        get => _members;
        set => SetProperty(ref _members, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                _ = SearchMembersAsync();
            }
        }
    }

    public Member? SelectedMember
    {
        get => _selectedMember;
        set => SetProperty(ref _selectedMember, value);
    }

    public MemberListViewModel(IMemberService memberService, IDialogService dialogService)
    {
        _memberService = memberService;
        _dialogService = dialogService;
        Title = "Members";
        _ = LoadMembersAsync();
    }

    private async Task LoadMembersAsync()
    {
        IsBusy = true;
        try
        {
            var members = await _memberService.GetAllMembersAsync();
            Members = new ObservableCollection<Member>(members);
        }
        catch (Exception ex)
        {
            _dialogService.ShowError($"Failed to load members: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SearchMembersAsync()
    {
        try
        {
            var results = await _memberService.SearchMembersAsync(SearchText);
            Members = new ObservableCollection<Member>(results);
        }
        catch (Exception ex)
        {
            _dialogService.ShowError($"Search failed: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        SearchText = string.Empty;
        await LoadMembersAsync();
    }

    [RelayCommand]
    private void AddMember()
    {
        _dialogService.ShowMessage("Add Member feature will be implemented soon!");
        // TODO: Open AddMemberDialog
    }

    [RelayCommand]
    private void EditMember(Member? member)
    {
        if (member == null && SelectedMember == null)
        {
            _dialogService.ShowWarning("Please select a member to edit.");
            return;
        }

        var memberToEdit = member ?? SelectedMember;
        _dialogService.ShowMessage($"Edit Member feature will be implemented soon!\nSelected: {memberToEdit?.FullName}");
        // TODO: Open EditMemberDialog
    }

    [RelayCommand]
    private async Task DeleteMemberAsync(Member? member)
    {
        if (member == null && SelectedMember == null)
        {
            _dialogService.ShowWarning("Please select a member to delete.");
            return;
        }

        var memberToDelete = member ?? SelectedMember;
        var confirm = _dialogService.ShowConfirmation(
            $"Are you sure you want to delete {memberToDelete?.FullName}?\n\nThis action cannot be undone.",
            "Delete Member");

        if (!confirm) return;

        try
        {
            IsBusy = true;
            if (memberToDelete?.Id != null)
            {
                await _memberService.DeleteMemberAsync(memberToDelete.Id);
                await LoadMembersAsync();
                _dialogService.ShowMessage("Member deleted successfully!");
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowError($"Failed to delete member: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ViewMember(Member? member)
    {
        if (member == null && SelectedMember == null)
        {
            _dialogService.ShowWarning("Please select a member to view.");
            return;
        }

        var memberToView = member ?? SelectedMember;
        _dialogService.ShowMessage($"View Member Details feature will be implemented soon!\nSelected: {memberToView?.FullName}");
        // TODO: Open MemberDetailsView
    }
}
