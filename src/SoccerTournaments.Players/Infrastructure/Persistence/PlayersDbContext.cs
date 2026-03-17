using Microsoft.EntityFrameworkCore;
using SoccerTournaments.Players.Domain;

namespace SoccerTournaments.Players.Infrastructure.Persistence;

public sealed class PlayersDbContext(DbContextOptions<PlayersDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("players");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
            entity.Property(p => p.Position).HasMaxLength(50).IsRequired();
            entity.Property(p => p.JerseyNumber).IsRequired();
            entity.Property(p => p.TeamId);
            entity.Property(p => p.DateOfBirth).IsRequired();
            entity.Property(p => p.CreationDate).IsRequired();

            entity.HasIndex(p => p.TeamId);
            entity.HasIndex(p => new { p.TeamId, p.JerseyNumber }).IsUnique();
        });
    }
}
