namespace SoccerTournaments.Tournaments;

public interface ITournamentsRepository
{
    Task<Tournament> AddAsync(Tournament tournament, CancellationToken cancellationToken = default);
    Task<Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tournament?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tournament>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TournamentTeam> AddTeamAsync(TournamentTeam tournamentTeam, CancellationToken cancellationToken = default);
    Task<TournamentTeam?> GetTournamentTeamAsync(Guid tournamentId, Guid teamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TournamentTeam>> GetTournamentTeamsAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task<int> GetTournamentTeamCountAsync(Guid tournamentId, CancellationToken cancellationToken = default);
}
