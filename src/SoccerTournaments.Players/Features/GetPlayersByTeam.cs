using FluentResults;
using SoccerTournaments.Players.Domain;
using SoccerTournaments.Players.Infrastructure.Persistence;

namespace SoccerTournaments.Players.Features;

public sealed record GetPlayersByTeamQuery(Guid TeamId);

public sealed class GetPlayersByTeamHandler(IPlayersRepository repository)
{
    public async Task<Result<IEnumerable<Player>>> Handle(GetPlayersByTeamQuery query, CancellationToken cancellationToken)
    {
        var players = await repository.GetByTeamIdAsync(query.TeamId, cancellationToken);
        return Result.Ok(players);
    }
}
