using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Web.Models
{
    public class BookIssue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a book.")]
        public int BookRefId { get; set; }
        public Book? Book { get; set; }

        [Required(ErrorMessage = "Please select a member.")]
        public int MemberRefId { get; set; }
        public Member? Member { get; set; }

        [Required(ErrorMessage = "Issue date is required.")]
        public DateTime DateIssued { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DateDue { get; set; }

        public DateTime? DateReturned { get; set; }

        [Required]
        public BookIssueStatus Status { get; set; } = BookIssueStatus.Issued;

        public string? Note { get; set; }
    }

    public enum BookIssueStatus
    {
        Issued,
        Returned,
        Overdue,
        Lost
    }
} 