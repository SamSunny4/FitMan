using FitMan.Application.Interfaces;
using FitMan.Domain.Entities;
using FitMan.Domain.Interfaces;
using FitMan.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitMan.Infrastructure.Services;

public class MemberService : IMemberService
{
    private readonly FitManDbContext _context;
    private readonly IRepository<Member> _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MemberService(
        FitManDbContext context,
        IRepository<Member> memberRepository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Member>> GetAllMembersAsync()
    {
        return await _context.Members
            .Include(m => m.Memberships)
                .ThenInclude(mm => mm.MembershipType)
            .OrderByDescending(m => m.Id)
            .ToListAsync();
    }

    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        return await _context.Members
            .Include(m => m.Memberships)
                .ThenInclude(mm => mm.MembershipType)
            .Include(m => m.Payments)
            .Include(m => m.AttendanceLogs)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Member?> GetMemberByNumberAsync(string membershipNumber)
    {
        return await _context.Members
            .Include(m => m.Memberships)
                .ThenInclude(mm => mm.MembershipType)
            .FirstOrDefaultAsync(m => m.MembershipNumber == membershipNumber);
    }

    public async Task<List<Member>> SearchMembersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllMembersAsync();

        searchTerm = searchTerm.ToLower().Trim();
        
        return await _context.Members
            .Include(m => m.Memberships)
                .ThenInclude(mm => mm.MembershipType)
            .Where(m =>
                m.FirstName.ToLower().Contains(searchTerm) ||
                m.LastName.ToLower().Contains(searchTerm) ||
                m.Phone.Contains(searchTerm) ||
                m.Email.ToLower().Contains(searchTerm) ||
                m.MembershipNumber.ToLower().Contains(searchTerm))
            .OrderByDescending(m => m.Id)
            .ToListAsync();
    }

    public async Task<Member> AddMemberAsync(Member member)
    {
        // Generate membership number if not provided
        if (string.IsNullOrEmpty(member.MembershipNumber))
        {
            member.MembershipNumber = await GenerateMembershipNumberAsync();
        }

        member.CreatedDate = DateTime.UtcNow;
        member.EnrollmentDate = DateTime.UtcNow;

        await _memberRepository.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return member;
    }

    public async Task UpdateMemberAsync(Member member)
    {
        member.ModifiedDate = DateTime.UtcNow;
        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMemberAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member != null)
        {
            _memberRepository.Remove(member);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<string> GenerateMembershipNumberAsync()
    {
        var lastMember = await _context.Members
            .OrderByDescending(m => m.Id)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastMember != null)
        {
            var numberPart = lastMember.MembershipNumber.Replace("GYM", "");
            if (int.TryParse(numberPart, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"GYM{nextNumber:D3}"; // GYM001, GYM002, etc.
    }
}
