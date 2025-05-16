using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Web.Models.ViewModels
{
    public class MemberProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Department { get; set; }

        [StringLength(20)]
        public string? StudentId { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public string? ImageUrl { get; set; }

        [StringLength(100)]
        public string? Password { get; set; } // Plain text for demo only
    }
} 