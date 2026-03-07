using Microsoft.EntityFrameworkCore;

namespace SoccerTournaments.Tournaments;

public class TournamentsDbContext : DbContext
{
    public TournamentsDbContext(DbContextOptions<TournamentsDbContext> options) : base(options)
    {
    }

    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.ToTable("tournaments");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedNever();

            entity.Property(t => t.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.StartDate)
                .HasColumnName("start_date")
                .IsRequired();

            entity.Property(t => t.MaxTeams)
                .HasColumnName("max_teams")
                .IsRequired();

            entity.Property(t => t.CreationDate)
                .HasColumnName("creation_date")
                .IsRequired();
        });

        modelBuilder.Entity<TournamentTeam>(entity =>
        {
            entity.ToTable("tournament_teams");

            entity.HasKey(tt => new { tt.TournamentId, tt.TeamId });

            entity.Property(tt => tt.TournamentId)
                .HasColumnName("tournament_id")
                .HasColumnType("uuid");

            entity.Property(tt => tt.TeamId)
                .HasColumnName("team_id")
                .HasColumnType("uuid");

            entity.Property(tt => tt.AddedAt)
                .HasColumnName("added_at")
                .IsRequired();

            entity.HasOne<Tournament>()
                .WithMany()
                .HasForeignKey(tt => tt.TournamentId);

            entity.HasIndex(tt => tt.TeamId);
        });
    }
}
