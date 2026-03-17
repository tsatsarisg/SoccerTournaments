using FluentResults;
using SoccerTournaments.Players.Domain;
using SoccerTournaments.Players.Infrastructure.Persistence;

namespace SoccerTournaments.Players.Features;

public sealed record GetPlayerByIdQuery(Guid Id);

public sealed class GetPlayerByIdHandler(IPlayersRepository repository)
{
    public async Task<Result<Player>> Handle(GetPlayerByIdQuery query, CancellationToken cancellationToken)
    {
        var player = await repository.GetByIdAsync(query.Id, cancellationToken);

        if (player is null)
        {
            return Result.Fail<Player>($"Player with ID {query.Id} not found.");
        }

        return Result.Ok(player);
    }
}
