using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        [StringLength(255)]
        public string? Avatar { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Librarian,
        User
    }
}
