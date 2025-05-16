using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Web.Models.ViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Member ID")]
        public string MemberId { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Member type is required")]
        [Display(Name = "Member Type")]
        public MemberType MemberType { get; set; }

        [Display(Name = "Status")]
        public MemberStatus Status { get; set; } = MemberStatus.Active;

        [Display(Name = "Books Borrowed")]
        public int BooksBorrowed { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
