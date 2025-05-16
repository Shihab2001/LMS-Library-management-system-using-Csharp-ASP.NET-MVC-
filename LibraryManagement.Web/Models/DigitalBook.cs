using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Web.Models
{
    public class DigitalBook
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public FileType FileType { get; set; }

        [Required]
        public long FileSize { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        // Navigation property
        public Book? Book { get; set; }
    }

    public enum FileType
    {
        PDF,
        EPUB,
        MOBI
    }
}
