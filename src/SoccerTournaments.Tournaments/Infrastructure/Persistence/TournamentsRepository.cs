using Microsoft.EntityFrameworkCore;

namespace SoccerTournaments.Tournaments;

public class TournamentsRepository : ITournamentsRepository
{
    private readonly TournamentsDbContext _context;

    public TournamentsRepository(TournamentsDbContext context)
    {
        _context = context;
    }

    public async Task<Tournament> AddAsync(Tournament tournament, CancellationToken cancellationToken = default)
    {
        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync(cancellationToken);
        return tournament;
    }

    public async Task<Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tournaments
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Tournament?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Tournaments
            .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Tournament>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tournaments.ToListAsync(cancellationToken);
    }
}
