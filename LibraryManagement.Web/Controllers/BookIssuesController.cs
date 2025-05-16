using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Web.Data;
using LibraryManagement.Web.Models;

namespace LibraryManagement.Web.Controllers
{
    public class BookIssuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookIssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookIssues
        public async Task<IActionResult> Index()
        {
            var issues = await _context.BookIssues
                .Include(bi => bi.Book)
                .Include(bi => bi.Member)
                .OrderByDescending(bi => bi.DateIssued)
                .ToListAsync();
            return View(issues);
        }

        // GET: BookIssues/Create
        public IActionResult Create()
        {
            ViewBag.Books = new SelectList(_context.Books.ToList(), "Id", "Title");
            ViewBag.Members = new SelectList(_context.Members.ToList(), "Id", "FullName");
            return View();
        }

        // POST: BookIssues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookIssue issue)
        {
            // Prevent issuing if no available copies
            var book = await _context.Books.FindAsync(issue.BookRefId);
            if (book == null || book.AvailableQuantity <= 0)
            {
                ModelState.AddModelError("BookRefId", "No available copies of this book.");
            }
            // Prevent duplicate issue
            var duplicate = await _context.BookIssues.AnyAsync(bi => bi.BookRefId == issue.BookRefId && bi.MemberRefId == issue.MemberRefId && bi.Status == BookIssueStatus.Issued);
            if (duplicate)
            {
                ModelState.AddModelError("BookRefId", "This member already has this book issued.");
            }
            // Validate due date
            if (issue.DateDue < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("DateDue", "Due date must be today or later.");
            }
            if (ModelState.IsValid)
            {
                issue.DateIssued = DateTime.UtcNow;
                issue.Status = BookIssueStatus.Issued;
                _context.BookIssues.Add(issue);
                // Increment BorrowedBooksCount for the member
                var member = await _context.Members.FindAsync(issue.MemberRefId);
                if (member != null)
                {
                    member.BorrowedBooksCount++;
                }
                // Decrement AvailableQuantity for the book
                if (book != null && book.AvailableQuantity > 0)
                {
                    book.AvailableQuantity--;
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book issued successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Books = new SelectList(_context.Books.ToList(), "Id", "Title", issue.BookRefId);
            ViewBag.Members = new SelectList(_context.Members.ToList(), "Id", "FullName", issue.MemberRefId);
            return View(issue);
        }

        // POST: BookIssues/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var issue = await _context.BookIssues.Include(bi => bi.Member).Include(bi => bi.Book).FirstOrDefaultAsync(bi => bi.Id == id);
            if (issue == null || issue.Status == BookIssueStatus.Returned)
            {
                return RedirectToAction(nameof(Index));
            }
            issue.DateReturned = DateTime.UtcNow;
            issue.Status = BookIssueStatus.Returned;
            if (issue.Member != null && issue.Member.BorrowedBooksCount > 0)
            {
                issue.Member.BorrowedBooksCount--;
            }
            if (issue.Book != null)
            {
                // Only increment if less than Quantity
                if (issue.Book.AvailableQuantity < issue.Book.Quantity)
                {
                    issue.Book.AvailableQuantity++;
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: BookIssues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var issue = await _context.BookIssues
                .Include(bi => bi.Book)
                .Include(bi => bi.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }
            return View(issue);
        }

        // POST: BookIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issue = await _context.BookIssues
                .Include(bi => bi.Book)
                .Include(bi => bi.Member)
                .FirstOrDefaultAsync(bi => bi.Id == id);
            if (issue != null)
            {
                // If not returned, update counts
                if (issue.Status == BookIssueStatus.Issued)
                {
                    if (issue.Member != null && issue.Member.BorrowedBooksCount > 0)
                        issue.Member.BorrowedBooksCount--;
                    if (issue.Book != null && issue.Book.AvailableQuantity < issue.Book.Quantity)
                        issue.Book.AvailableQuantity++;
                }
                _context.BookIssues.Remove(issue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Issued book log deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 