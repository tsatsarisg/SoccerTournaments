using FluentResults;
using FluentValidation;
using SoccerTournaments.Tournaments.Domain;

namespace SoccerTournaments.Tournaments.Features;

public sealed record UpdateMatchStatusCommand(
    Guid MatchId,
    MatchStatus NewStatus,
    int? HomeGoals = null,
    int? AwayGoals = null);

public sealed class UpdateMatchStatusCommandValidator : AbstractValidator<UpdateMatchStatusCommand>
{
    public UpdateMatchStatusCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.NewStatus).IsInEnum();
        RuleFor(x => x.HomeGoals)
            .GreaterThanOrEqualTo(0)
            .When(x => x.HomeGoals.HasValue)
            .WithMessage("Home goals cannot be negative.");
        RuleFor(x => x.AwayGoals)
            .GreaterThanOrEqualTo(0)
            .When(x => x.AwayGoals.HasValue)
            .WithMessage("Away goals cannot be negative.");
        RuleFor(x => x)
            .Must(x => x.NewStatus != MatchStatus.Completed || (x.HomeGoals.HasValue && x.AwayGoals.HasValue))
            .WithMessage("Home goals and away goals are required when completing a match.");
    }
}

public sealed class UpdateMatchStatusHandler
{
    private readonly ITournamentsRepository _repository;

    public UpdateMatchStatusHandler(ITournamentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Match>> Handle(UpdateMatchStatusCommand command, CancellationToken cancellationToken)
    {
        var match = await _repository.GetMatchByIdAsync(command.MatchId, cancellationToken);
        if (match is null)
        {
            return Result.Fail<Match>($"Match with ID {command.MatchId} not found.");
        }

        Result result = command.NewStatus switch
        {
            MatchStatus.InProgress => match.StartMatch(),
            MatchStatus.Completed => match.CompleteMatch(command.HomeGoals!.Value, command.AwayGoals!.Value),
            MatchStatus.Cancelled => match.CancelMatch(),
            _ => Result.Fail($"Cannot update match to status {command.NewStatus}.")
        };

        if (result.IsFailed)
        {
            return Result.Fail<Match>(result.Errors);
        }

        await _repository.UpdateMatchAsync(match, cancellationToken);
        return Result.Ok(match);
    }
}
