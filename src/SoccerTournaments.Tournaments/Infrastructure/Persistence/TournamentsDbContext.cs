using Microsoft.EntityFrameworkCore;

namespace SoccerTournaments.Tournaments;

public class TournamentsDbContext : DbContext
{
    public TournamentsDbContext(DbContextOptions<TournamentsDbContext> options) : base(options)
    {
    }

    public DbSet<Tournament> Tournaments => Set<Tournament>();

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
    }
}
