using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Web.Models;
using LibraryManagement.Web.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Web.Data;

namespace LibraryManagement.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    UserName = model.Email, 
                    Email = model.Email,
                    Role = UserRole.User // Default role for new registrations
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string? returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException("Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == user.Email);
            if (member == null)
            {
                // Create a new member record for the user
                member = new Member
                {
                    FullName = user.Name ?? user.Email!,
                    Email = user.Email!,
                    Type = user.Role == UserRole.Admin ? MemberType.Staff : MemberType.Student,
                    Status = MemberStatus.Active,
                    JoinedDate = DateTime.UtcNow
                };
                _context.Members.Add(member);
                await _context.SaveChangesAsync();
            }

            var vm = new MemberProfileViewModel
            {
                Id = member.Id,
                FullName = member.FullName ?? string.Empty,
                Email = member.Email ?? string.Empty,
                Phone = member.Phone ?? string.Empty,
                Department = member.Department ?? string.Empty,
                StudentId = member.StudentId ?? string.Empty,
                Address = member.Address ?? string.Empty,
                ImageUrl = member.ImageUrl ?? string.Empty
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(MemberProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == user.Email);
            if (member == null) return NotFound();

            member.FullName = vm.FullName;
            member.Email = vm.Email;
            member.Phone = vm.Phone;
            member.Department = vm.Department;
            member.StudentId = vm.StudentId;
            member.Address = vm.Address;
            member.ImageUrl = vm.ImageUrl;

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, vm.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(vm);
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
