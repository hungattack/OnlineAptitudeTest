﻿using Microsoft.EntityFrameworkCore;

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
        public DbSet<RegisterManager> Managers { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<Condidate> Condidates { get; set; }
        public DbSet<ResultHistory> resultHistories { get; set; }
        public DbSet<Info> infos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(ul => new { ul.Email }).IsUnique(true);
            modelBuilder.Entity<RegisterManager>()
                .HasIndex(ul => new { ul.userId }).IsUnique(true);
        }

    }
}
