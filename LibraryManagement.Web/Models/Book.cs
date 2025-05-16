using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Web.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public int PublicationYear { get; set; }

        [StringLength(200)]
        public string? Publisher { get; set; }

        public int Quantity { get; set; } = 1;

        [StringLength(200)]
        public string? CoverImage { get; set; }

        [NotMapped]
        public IFormFile? CoverImageFile { get; set; }

        public BookStatus Status { get; set; } = BookStatus.Available;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int AvailableQuantity { get; set; }

        public string? ImageUrl { get; set; }

        // Navigation properties
        public int? CurrentHolderId { get; set; }
        public Member? CurrentHolder { get; set; }
        public ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
    }

    public enum BookStatus
    {
        Available,
        CheckedOut,
        Lost,
        Damaged
    }
}
