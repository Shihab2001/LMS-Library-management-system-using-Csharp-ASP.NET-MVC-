using System.Collections.Generic;
using LibraryManagement.Web.Models;

namespace LibraryManagement.Web.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int OverdueBooks { get; set; }
        public int IssuedBooks { get; set; }
        public Dictionary<string, int> BookCategories { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MemberTypes { get; set; } = new Dictionary<string, int>();
        public List<RecentActivityViewModel> RecentActivity { get; set; } = new List<RecentActivityViewModel>();
        public List<BookIssue> RecentlyBorrowedBooks { get; set; } = new List<BookIssue>();
    }

    public class RecentActivityViewModel
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusColor { get; set; } = string.Empty;
    }
}
