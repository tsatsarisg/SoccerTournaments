using Microsoft.EntityFrameworkCore;

namespace SoccerTournaments.Tournaments;

public class TournamentsDbContext : DbContext
{
    public TournamentsDbContext(DbContextOptions<TournamentsDbContext> options) : base(options)
    {
    }

    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
    public DbSet<Standing> Standings => Set<Standing>();

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

        modelBuilder.Entity<Standing>(entity =>
        {
            entity.ToTable("standings");

            entity.HasKey(s => new { s.TournamentId, s.TeamId });

            entity.Property(s => s.TournamentId)
                .HasColumnName("tournament_id")
                .HasColumnType("uuid");

            entity.Property(s => s.TeamId)
                .HasColumnName("team_id")
                .HasColumnType("uuid");

            entity.Property(s => s.MatchesPlayed)
                .HasColumnName("matches_played")
                .IsRequired();

            entity.Property(s => s.Wins)
                .HasColumnName("wins")
                .IsRequired();

            entity.Property(s => s.Draws)
                .HasColumnName("draws")
                .IsRequired();

            entity.Property(s => s.Losses)
                .HasColumnName("losses")
                .IsRequired();

            entity.Property(s => s.GoalsFor)
                .HasColumnName("goals_for")
                .IsRequired();

            entity.Property(s => s.GoalsAgainst)
                .HasColumnName("goals_against")
                .IsRequired();

            entity.Ignore(s => s.GoalDifference);
            entity.Ignore(s => s.Points);

            entity.HasOne<Tournament>()
                .WithMany()
                .HasForeignKey(s => s.TournamentId);

            entity.HasIndex(s => s.TeamId);
        });
    }
}
