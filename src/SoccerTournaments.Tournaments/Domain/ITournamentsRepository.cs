namespace SoccerTournaments.Tournaments;

public interface ITournamentsRepository
{
    Task<Tournament> AddAsync(Tournament tournament, CancellationToken cancellationToken = default);
    Task<Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tournament?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tournament>> GetAllAsync(CancellationToken cancellationToken = default);
}
