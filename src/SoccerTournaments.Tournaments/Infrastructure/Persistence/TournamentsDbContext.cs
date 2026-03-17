using Microsoft.EntityFrameworkCore;
using SoccerTournaments.Tournaments.Domain;

namespace SoccerTournaments.Tournaments;

public class TournamentsDbContext : DbContext
{
    public TournamentsDbContext(DbContextOptions<TournamentsDbContext> options) : base(options)
    {
    }

    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
    public DbSet<Standing> Standings => Set<Standing>();
    public DbSet<Match> Matches => Set<Match>();

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

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("matches");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedNever();

            entity.Property(m => m.TournamentId)
                .HasColumnName("tournament_id")
                .HasColumnType("uuid")
                .IsRequired();

            entity.Property(m => m.HomeTeamId)
                .HasColumnName("home_team_id")
                .HasColumnType("uuid")
                .IsRequired();

            entity.Property(m => m.AwayTeamId)
                .HasColumnName("away_team_id")
                .HasColumnType("uuid")
                .IsRequired();

            entity.Property(m => m.ScheduledDate)
                .HasColumnName("scheduled_date");

            entity.Property(m => m.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            entity.Property(m => m.HomeGoals)
                .HasColumnName("home_goals");

            entity.Property(m => m.AwayGoals)
                .HasColumnName("away_goals");

            entity.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasOne<Tournament>()
                .WithMany()
                .HasForeignKey(m => m.TournamentId);

            entity.HasIndex(m => m.TournamentId);
            entity.HasIndex(m => m.Status);
        });
    }
}
