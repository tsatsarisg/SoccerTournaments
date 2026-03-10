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

    public async Task<TournamentTeam> AddTeamAsync(TournamentTeam tournamentTeam, CancellationToken cancellationToken = default)
    {
        _context.TournamentTeams.Add(tournamentTeam);
        await _context.SaveChangesAsync(cancellationToken);
        return tournamentTeam;
    }

    public async Task<TournamentTeam?> GetTournamentTeamAsync(Guid tournamentId, Guid teamId, CancellationToken cancellationToken = default)
    {
        return await _context.TournamentTeams
            .FirstOrDefaultAsync(tt => tt.TournamentId == tournamentId && tt.TeamId == teamId, cancellationToken);
    }

    public async Task<IEnumerable<TournamentTeam>> GetTournamentTeamsAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        return await _context.TournamentTeams
            .Where(tt => tt.TournamentId == tournamentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTournamentTeamCountAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        return await _context.TournamentTeams
            .CountAsync(tt => tt.TournamentId == tournamentId, cancellationToken);
    }

    public async Task<Standing?> GetStandingAsync(Guid tournamentId, Guid teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Standings
            .FirstOrDefaultAsync(s => s.TournamentId == tournamentId && s.TeamId == teamId, cancellationToken);
    }

    public async Task<IEnumerable<Standing>> GetTournamentStandingsAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        return await _context.Standings
            .Where(s => s.TournamentId == tournamentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Standing> AddStandingAsync(Standing standing, CancellationToken cancellationToken = default)
    {
        _context.Standings.Add(standing);
        await _context.SaveChangesAsync(cancellationToken);
        return standing;
    }

    public async Task UpdateStandingAsync(Standing standing, CancellationToken cancellationToken = default)
    {
        _context.Standings.Update(standing);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
