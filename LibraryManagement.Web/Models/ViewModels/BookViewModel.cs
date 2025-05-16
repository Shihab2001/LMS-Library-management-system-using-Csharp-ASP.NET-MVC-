using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Web.Models.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        [Display(Name = "Author")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Cover Image")]
        public IFormFile? CoverImageFile { get; set; }

        public string? ExistingCoverImage { get; set; }

        [Display(Name = "Available")]
        public int AvailableQuantity { get; set; }
    }
}
