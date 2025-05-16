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
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<bool> BookExistsAsync(int id);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(string category);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm, string? category = null);
        Task<bool> CheckOutBookAsync(int bookId, int memberId);
        Task<bool> ReturnBookAsync(int bookId);
        Task<int> GetTotalBooksCountAsync();
        Task<int> GetNewBooksThisMonthCountAsync();
        Task<int> GetAvailableBooksCountAsync();
    }

    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BookService(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.CurrentHolder)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.CurrentHolder)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            book.CreatedAt = DateTime.UtcNow;
            book.Status = BookStatus.Available;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.Id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {book.Id} not found.");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.ISBN = book.ISBN;
            existingBook.Category = book.Category;
            existingBook.Description = book.Description;
            existingBook.PublicationYear = book.PublicationYear;
            existingBook.Publisher = book.Publisher;
            existingBook.Quantity = book.Quantity;
            existingBook.CoverImage = book.CoverImage;
            existingBook.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
                .Where(b => b.Status == BookStatus.Available)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(string category)
        {
            return await _context.Books
                .Where(b => b.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm, string? category = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(b => 
                    (b.Title ?? "").ToLower().Contains(searchTerm) || 
                    (b.Author ?? "").ToLower().Contains(searchTerm) || 
                    (b.ISBN ?? "").ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(b => b.Category == category);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CheckOutBookAsync(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.Status != BookStatus.Available)
                return false;

            book.Status = BookStatus.CheckedOut;
            book.CurrentHolderId = memberId;
            book.DueDate = DateTime.UtcNow.AddDays(14); // 2 weeks loan period
            book.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.Status != BookStatus.CheckedOut)
                return false;

            book.Status = BookStatus.Available;
            book.CurrentHolderId = null;
            book.DueDate = null;
            book.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalBooksCountAsync()
        {
            return await _context.Books.CountAsync();
        }

        public async Task<int> GetNewBooksThisMonthCountAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return await _context.Books
                .Where(b => b.CreatedAt >= firstDayOfMonth)
                .CountAsync();
        }

        public async Task<int> GetAvailableBooksCountAsync()
        {
            return await _context.Books
                .Where(b => b.Status == BookStatus.Available)
                .CountAsync();
        }
    }
}
