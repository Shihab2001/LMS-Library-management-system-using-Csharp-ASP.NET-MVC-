using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Web.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        public MemberType Type { get; set; }

        [StringLength(50)]
        public string? Department { get; set; }

        [StringLength(20)]
        public string? StudentId { get; set; }

        [Required]
        public MemberStatus Status { get; set; } = MemberStatus.Active;

        public int BorrowedBooksCount { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

        [StringLength(200)]
        public string? Address { get; set; }

        public string? ImageUrl { get; set; }

        // Navigation properties
        public ICollection<Book> CheckedOutBooks { get; set; } = new List<Book>();
        public ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();

        // Computed property for member type
        [NotMapped]
        public MemberType MemberType => Type;
    }

    public enum MemberType
    {
        Student,
        Faculty,
        Staff,
        Alumni
    }

    public enum MemberStatus
    {
        Active,
        Inactive,
        Suspended,
        Graduated
    }
}
