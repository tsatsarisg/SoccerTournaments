namespace SoccerTournaments.Teams;

public interface ITeamsRepository
{
    Task<Team> AddAsync(Team team, CancellationToken cancellationToken = default);
    Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default);
}
