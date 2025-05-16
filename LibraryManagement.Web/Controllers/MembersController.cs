using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Web.Data;
using LibraryManagement.Web.Models;
using LibraryManagement.Web.Models.ViewModels;
using LibraryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Web.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MembersController(IMemberService memberService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _memberService = memberService;
            _context = context;
            _userManager = userManager;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Email,Phone,Type,Department,StudentId,Address")] Member member, string Password)
        {
            if (ModelState.IsValid)
            {
                member.JoinedDate = DateTime.UtcNow;
                member.Status = MemberStatus.Active;
                await _memberService.CreateMemberAsync(member);

                // Create user account with the provided password
                var user = new ApplicationUser 
                { 
                    UserName = member.Email, 
                    Email = member.Email,
                    Role = member.Type == MemberType.Student ? UserRole.User : UserRole.Admin
                };
                var result = await _userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, member.Type == MemberType.Student ? "User" : "Admin");
                }
                else
                {
                    // If user creation fails, delete the member
                    await _memberService.DeleteMemberAsync(member.Id);
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(member);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberService.GetMemberByIdAsync(id.Value);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Phone,Type,Department,StudentId,Status,Address")] Member member, string Password)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _memberService.UpdateMemberAsync(member);

                    // Update password if provided
                    if (!string.IsNullOrWhiteSpace(Password))
                    {
                        var user = await _userManager.FindByEmailAsync(member.Email);
                        if (user != null)
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await _userManager.ResetPasswordAsync(user, token, Password);
                            if (!result.Succeeded)
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                                return View(member);
                            }
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberService.GetMemberByIdAsync(id.Value);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _memberService.DeleteMemberAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Members/EditProfile/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProfile(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();
            var vm = new MemberProfileViewModel
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                Phone = member.Phone,
                Department = member.Department,
                StudentId = member.StudentId,
                Address = member.Address,
                ImageUrl = member.ImageUrl
            };
            return View(vm);
        }

        // POST: Members/EditProfile/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(MemberProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var member = await _context.Members.FindAsync(vm.Id);
            if (member == null) return NotFound();
            member.FullName = vm.FullName;
            member.Email = vm.Email;
            member.Phone = vm.Phone;
            member.Department = vm.Department;
            member.StudentId = vm.StudentId;
            member.Address = vm.Address;
            member.ImageUrl = vm.ImageUrl;
            // For demo: store password as plain text (not for production!)
            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                member.ImageUrl = vm.Password; // For demo, store in ImageUrl or add a Password property
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Member profile updated.";
            return RedirectToAction("Index");
        }

        // GET: Members/Dashboard
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userEmail = User.Identity?.Name;
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == userEmail);
            if (member == null) return NotFound();
            var issues = await _context.BookIssues.Include(bi => bi.Book).Where(bi => bi.MemberRefId == member.Id).ToListAsync();
            ViewBag.Issues = issues;
            return View(member);
        }
    }
}
