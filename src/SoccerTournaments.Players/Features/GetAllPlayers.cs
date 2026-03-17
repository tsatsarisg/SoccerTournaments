using FluentResults;
using SoccerTournaments.Players.Domain;
using SoccerTournaments.Players.Infrastructure.Persistence;

namespace SoccerTournaments.Players.Features;

public sealed record GetAllPlayersQuery;

public sealed class GetAllPlayersHandler(IPlayersRepository repository)
{
    public async Task<Result<IEnumerable<Player>>> Handle(GetAllPlayersQuery query, CancellationToken cancellationToken)
    {
        var players = await repository.GetAllAsync(cancellationToken);
        return Result.Ok(players);
    }
}
