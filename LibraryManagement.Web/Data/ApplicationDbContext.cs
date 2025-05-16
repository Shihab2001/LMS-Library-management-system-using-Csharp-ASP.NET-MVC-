using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Web.Models;

namespace LibraryManagement.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BookIssue> BookIssues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Book entity
            builder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ISBN).HasMaxLength(20);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CoverImage).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure Member entity
            builder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.StudentId).HasMaxLength(20);
                entity.Property(e => e.Department).HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.JoinedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure BookIssue entity
            builder.Entity<BookIssue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DateIssued).IsRequired();
                entity.Property(e => e.DateDue).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.HasOne(e => e.Book)
                    .WithMany()
                    .HasForeignKey(e => e.BookRefId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Member)
                    .WithMany()
                    .HasForeignKey(e => e.MemberRefId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
