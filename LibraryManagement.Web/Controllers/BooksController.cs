using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Web.Data;
using LibraryManagement.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString, string category, string availability)
        {
            // Set current filter values in ViewData
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentAvailability"] = availability;

            var books = _context.Books.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();
                books = books.Where(b => 
                    b.Title.ToLower().Contains(searchString) || 
                    b.Author.ToLower().Contains(searchString) ||
                    b.ISBN.ToLower().Contains(searchString));
            }

            // Apply category filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                books = books.Where(b => b.Category == category);
            }

            // Apply availability filter
            if (!string.IsNullOrWhiteSpace(availability))
            {
                if (availability == "available")
                {
                    books = books.Where(b => b.AvailableQuantity > 0);
                }
                else if (availability == "unavailable")
                {
                    books = books.Where(b => b.AvailableQuantity == 0);
                }
            }

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.CurrentHolder)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,ISBN,Category,Quantity,Description,CoverImageFile")] Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.CoverImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/books");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + book.CoverImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    Directory.CreateDirectory(uploadsFolder);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await book.CoverImageFile.CopyToAsync(fileStream);
                    }
                    
                    book.CoverImage = "/images/books/" + uniqueFileName;
                }

                book.CreatedAt = DateTime.Now;
                // Initialize available quantity to equal total quantity on creation
                book.AvailableQuantity = book.Quantity;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,Category,Quantity,Description,CoverImageFile")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _context.Books.FindAsync(id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    if (book.CoverImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/books");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + book.CoverImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        
                        Directory.CreateDirectory(uploadsFolder);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await book.CoverImageFile.CopyToAsync(fileStream);
                        }
                        
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingBook.CoverImage))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingBook.CoverImage.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        
                        existingBook.CoverImage = "/images/books/" + uniqueFileName;
                    }

                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.ISBN = book.ISBN;
                    existingBook.Category = book.Category;
                    existingBook.Quantity = book.Quantity;
                    // Clamp AvailableQuantity to Quantity
                    if (existingBook.AvailableQuantity > book.Quantity)
                    {
                        existingBook.AvailableQuantity = book.Quantity;
                    }
                    existingBook.Description = book.Description;
                    existingBook.UpdatedAt = DateTime.Now;

                    _context.Update(existingBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check for related BookIssue records
            var hasIssues = await _context.BookIssues.AnyAsync(bi => bi.BookRefId == id);
            if (hasIssues)
            {
                TempData["ErrorMessage"] = "This book cannot be deleted because it has issued or borrowed records. Please remove those records first.";
                return RedirectToAction("Details", new { id });
            }

            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                // Delete cover image if exists
                if (!string.IsNullOrEmpty(book.CoverImage))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, book.CoverImage.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Books.Remove(book);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
