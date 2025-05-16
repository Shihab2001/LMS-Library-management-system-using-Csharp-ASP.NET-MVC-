using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Web.Data;
using LibraryManagement.Web.Models;
using LibraryManagement.Web.Models.ViewModels;

namespace LibraryManagement.Web.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task<Member?> GetMemberByIdAsync(int id);
        Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm);
        Task<Member> CreateMemberAsync(Member member);
        Task<Member> UpdateMemberAsync(Member member);
        Task DeleteMemberAsync(int id);
        Task<bool> MemberExistsAsync(int id);
        Task<IEnumerable<Book>> GetCheckedOutBooksAsync(int memberId);
        Task<int> GetTotalMembersCountAsync();
        Task<int> GetNewMembersThisMonthCountAsync();
        Task<int> GetActiveMembersCountAsync();
    }

    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .OrderBy(m => m.FullName)
                .ToListAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllMembersAsync();

            searchTerm = searchTerm.ToLower();
            return await _context.Members
                .Where(m => 
                    m.FullName.ToLower().Contains(searchTerm) ||
                    m.Email.ToLower().Contains(searchTerm) ||
                    (m.StudentId ?? "").ToLower().Contains(searchTerm) ||
                    (m.Phone ?? "").ToLower().Contains(searchTerm))
                .OrderBy(m => m.FullName)
                .ToListAsync();
        }

        public async Task<Member> CreateMemberAsync(Member member)
        {
            member.JoinedDate = DateTime.UtcNow;
            member.Status = MemberStatus.Active;

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> UpdateMemberAsync(Member member)
        {
            var existingMember = await _context.Members.FindAsync(member.Id);
            if (existingMember == null)
                throw new KeyNotFoundException($"Member with ID {member.Id} not found.");

            // Preserve the original join date
            member.JoinedDate = existingMember.JoinedDate;

            _context.Entry(existingMember).CurrentValues.SetValues(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task DeleteMemberAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MemberExistsAsync(int id)
        {
            return await _context.Members.AnyAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Book>> GetCheckedOutBooksAsync(int memberId)
        {
            return await _context.Books
                .Where(b => b.CurrentHolderId == memberId && b.Status == BookStatus.CheckedOut)
                .ToListAsync();
        }

        public async Task<int> GetTotalMembersCountAsync()
        {
            return await _context.Members.CountAsync();
        }

        public async Task<int> GetNewMembersThisMonthCountAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return await _context.Members
                .Where(m => m.JoinedDate >= firstDayOfMonth)
                .CountAsync();
        }

        public async Task<int> GetActiveMembersCountAsync()
        {
            return await _context.Members
                .Where(m => m.Status == MemberStatus.Active)
                .CountAsync();
        }
    }
}
