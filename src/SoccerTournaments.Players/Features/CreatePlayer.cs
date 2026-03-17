using FluentResults;
using FluentValidation;
using SoccerTournaments.Players.Domain;
using SoccerTournaments.Players.Infrastructure.Persistence;

namespace SoccerTournaments.Players.Features;

public sealed record CreatePlayerCommand(
    string Name,
    string Position,
    int JerseyNumber,
    DateTime DateOfBirth);

public sealed class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Position)
            .NotEmpty()
            .Must(p => new[] { "Goalkeeper", "Defender", "Midfielder", "Forward" }.Contains(p))
            .WithMessage("Position must be one of: Goalkeeper, Defender, Midfielder, Forward.");
        RuleFor(x => x.JerseyNumber).InclusiveBetween(1, 99);
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.UtcNow);
    }
}

public sealed class CreatePlayerHandler(IPlayersRepository repository)
{
    public async Task<Result<Player>> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
    {
        var playerResult = Player.Create(
            command.Name,
            command.Position,
            command.JerseyNumber,
            command.DateOfBirth);

        if (playerResult.IsFailed)
        {
            return playerResult;
        }

        var player = playerResult.Value;
        await repository.AddAsync(player, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Ok(player);
    }
}
