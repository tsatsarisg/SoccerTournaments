using Microsoft.EntityFrameworkCore;
using SoccerTournaments.Players.Domain;

namespace SoccerTournaments.Players.Infrastructure.Persistence;

public sealed class PlayersRepository(PlayersDbContext context) : IPlayersRepository
{
    public async Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Players.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Player>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Players.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Player>> GetByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        return await context.Players.Where(p => p.TeamId == teamId).ToListAsync(cancellationToken);
    }

    public async Task<Player?> GetByTeamAndJerseyNumberAsync(Guid teamId, int jerseyNumber, CancellationToken cancellationToken = default)
    {
        return await context.Players
            .FirstOrDefaultAsync(p => p.TeamId == teamId && p.JerseyNumber == jerseyNumber, cancellationToken);
    }

    public async Task AddAsync(Player player, CancellationToken cancellationToken = default)
    {
        await context.Players.AddAsync(player, cancellationToken);
    }

    public async Task UpdateAsync(Player player, CancellationToken cancellationToken = default)
    {
        context.Players.Update(player);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
