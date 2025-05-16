namespace LibraryManagement.Web.Models.ViewModels
{
    public class ReportViewModel
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int BooksIssued { get; set; }
        public int OverdueBooks { get; set; }
        public int NewBooksThisMonth { get; set; }
        public int NewMembersThisMonth { get; set; }
    }
} 