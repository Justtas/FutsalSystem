using FutsalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FutsalSystem.Repository
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchEvent> MatchEvents { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(b => b.Players)
                .HasForeignKey(p => p.TeamId);

            modelBuilder.Entity<Match>()
                .HasOne(p => p.HomeTeam)
                .WithMany(p => p.HomeMatches)
                .HasForeignKey(p => p.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(p => p.AwayTeam)
                .WithMany(p => p.AwayMatches)
                .HasForeignKey(p => p.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(t => t.Team)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
