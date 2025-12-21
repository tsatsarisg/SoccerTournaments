using Microsoft.EntityFrameworkCore;

namespace SoccerTournaments.Teams;

public class TeamsDbContext : DbContext
{
    public TeamsDbContext(DbContextOptions<TeamsDbContext> options) : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("teams");
            
            entity.HasKey(t => t.Id);
            
            entity.Property(t => t.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedNever();
            
            entity.Property(t => t.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(t => t.City)
                .HasColumnName("city")
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(t => t.CreationDate)
                .HasColumnName("creation_date")
                .IsRequired();
        });
    }
}
