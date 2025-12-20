using FitMan.Domain.Entities;

namespace FitMan.Application.Interfaces;

public interface IMemberService
{
    Task<List<Member>> GetAllMembersAsync();
    Task<Member?> GetMemberByIdAsync(int id);
    Task<Member?> GetMemberByNumberAsync(string membershipNumber);
    Task<List<Member>> SearchMembersAsync(string searchTerm);
    Task<Member> AddMemberAsync(Member member);
    Task UpdateMemberAsync(Member member);
    Task DeleteMemberAsync(int id);
    Task<string> GenerateMembershipNumberAsync();
}
