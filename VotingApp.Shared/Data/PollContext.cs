using Microsoft.EntityFrameworkCore;
using VotingApp.Shared.Models;

namespace VotingApp.Shared.Data
{
    public class PollContext : DbContext
    {
        public PollContext(DbContextOptions<PollContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Poll> Polls { get; set; }
        public virtual DbSet<Option> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>()
                .HasIndex(p => p.UrlCode)
                .IsUnique();

            modelBuilder.Entity<Option>()
                .HasOne(p => p.Poll)
                .WithMany(t => t.Options)
                .HasForeignKey(p => p.PollId);
        }
    }
}
