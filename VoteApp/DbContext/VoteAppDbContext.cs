using Microsoft.EntityFrameworkCore;
using VoteApp.Models;

namespace VoteApp.DatabaseContext
{
    public class VoteAppDbContext : DbContext
    {
        public virtual DbSet<Voter> Voters { get; set; }

        public virtual DbSet<Candidate> Candidates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("VoteAppDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>();
            modelBuilder.Entity<Voter>();
        }
    }
}
