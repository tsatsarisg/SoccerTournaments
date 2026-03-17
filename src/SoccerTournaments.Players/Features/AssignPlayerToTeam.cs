using FluentResults;
using FluentValidation;
using SoccerTournaments.Players.Domain;
using SoccerTournaments.Players.Infrastructure.Persistence;

namespace SoccerTournaments.Players.Features;

public sealed record AssignPlayerToTeamCommand(Guid PlayerId, Guid TeamId);

public sealed class AssignPlayerToTeamCommandValidator : AbstractValidator<AssignPlayerToTeamCommand>
{
    public AssignPlayerToTeamCommandValidator()
    {
        RuleFor(x => x.PlayerId).NotEmpty();
        RuleFor(x => x.TeamId).NotEmpty();
    }
}

public sealed class AssignPlayerToTeamHandler(IPlayersRepository repository)
{
    public async Task<Result<Player>> Handle(AssignPlayerToTeamCommand command, CancellationToken cancellationToken)
    {
        var player = await repository.GetByIdAsync(command.PlayerId, cancellationToken);
        if (player is null)
        {
            return Result.Fail<Player>($"Player with ID {command.PlayerId} not found.");
        }

        // Check for jersey number conflict
        var existingPlayer = await repository.GetByTeamAndJerseyNumberAsync(
            command.TeamId, player.JerseyNumber, cancellationToken);

        if (existingPlayer is not null && existingPlayer.Id != player.Id)
        {
            return Result.Fail<Player>(
                $"Team already has a player with jersey number {player.JerseyNumber}.");
        }

        var assignResult = player.AssignToTeam(command.TeamId);
        if (assignResult.IsFailed)
        {
            return Result.Fail<Player>(assignResult.Errors);
        }

        await repository.UpdateAsync(player, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Ok(player);
    }
}
