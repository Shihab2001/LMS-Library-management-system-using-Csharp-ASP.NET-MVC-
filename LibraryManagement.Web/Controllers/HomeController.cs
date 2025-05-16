using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Web.Data;
using LibraryManagement.Web.Models;
using LibraryManagement.Web.Models.ViewModels;
using LibraryManagement.Web.Services;

namespace LibraryManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            IBookService bookService,
            IMemberService memberService,
            ApplicationDbContext context)
        {
            _logger = logger;
            _bookService = bookService;
            _memberService = memberService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalMembers = await _context.Members.CountAsync(),
                BookCategories = await _context.Books
                    .GroupBy(b => b.Category)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Category, x => x.Count),
                MemberTypes = await _context.Members
                    .GroupBy(m => m.Type)
                    .Select(g => new { Type = g.Key.ToString(), Count = g.Count() })
                    .ToDictionaryAsync(x => x.Type, x => x.Count),
                RecentlyBorrowedBooks = await _context.BookIssues
                    .Include(bi => bi.Book)
                    .Include(bi => bi.Member)
                    .Where(bi => bi.Status == BookIssueStatus.Issued || bi.Status == BookIssueStatus.Returned)
                    .OrderByDescending(bi => bi.DateIssued)
                    .Take(6)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
