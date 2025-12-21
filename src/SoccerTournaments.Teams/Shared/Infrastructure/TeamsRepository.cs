using Microsoft.EntityFrameworkCore;

namespace SoccerTournaments.Teams;

public class TeamsRepository : ITeamsRepository
{
    private readonly TeamsDbContext _context;

    public TeamsRepository(TeamsDbContext context)
    {
        _context = context;
    }

    public async Task<Team> AddAsync(Team team, CancellationToken cancellationToken = default)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync(cancellationToken);
        return team;
    }

    public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Teams.ToListAsync(cancellationToken);
    }
}
