using Microsoft.EntityFrameworkCore;

namespace OnlineAptitudeTest.Model
{
    public class AptitudeTestDbText : DbContext
    {
        public AptitudeTestDbText(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<CateParts> CateParts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionHistory> QuestionHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(ul => new { ul.UserName, ul.Name }).IsUnique(true);
            modelBuilder.Entity<Question>()
               .HasIndex(ul => new { ul.QuestionName }).IsUnique(true);
            modelBuilder.Entity<Roles>()
               .HasIndex(ul => new { ul.Name }).IsUnique(true);
            modelBuilder.Entity<CateParts>()
               .HasIndex(ul => new { ul.Name }).IsUnique(true);
        }

    }
}
