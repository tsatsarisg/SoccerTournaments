using SoccerTournaments.Players.Domain;

namespace SoccerTournaments.Players.Infrastructure.Persistence;

public interface IPlayersRepository
{
    Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Player>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Player>> GetByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<Player?> GetByTeamAndJerseyNumberAsync(Guid teamId, int jerseyNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Player player, CancellationToken cancellationToken = default);
    Task UpdateAsync(Player player, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
