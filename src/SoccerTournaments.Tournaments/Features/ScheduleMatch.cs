using FluentResults;
using FluentValidation;
using SoccerTournaments.Tournaments.Domain;

namespace SoccerTournaments.Tournaments.Features;

public sealed record ScheduleMatchCommand(
    Guid TournamentId,
    Guid HomeTeamId,
    Guid AwayTeamId,
    DateTime? ScheduledDate);

public sealed class ScheduleMatchCommandValidator : AbstractValidator<ScheduleMatchCommand>
{
    public ScheduleMatchCommandValidator()
    {
        RuleFor(x => x.TournamentId).NotEmpty();
        RuleFor(x => x.HomeTeamId).NotEmpty();
        RuleFor(x => x.AwayTeamId).NotEmpty();
        RuleFor(x => x.HomeTeamId).NotEqual(x => x.AwayTeamId)
            .WithMessage("Home and away teams must be different.");
        RuleFor(x => x.ScheduledDate)
            .Must(date => !date.HasValue || date.Value > DateTime.UtcNow)
            .When(x => x.ScheduledDate.HasValue)
            .WithMessage("Scheduled date must be in the future.");
    }
}

public sealed class ScheduleMatchHandler
{
    private readonly ITournamentsRepository _repository;

    public ScheduleMatchHandler(ITournamentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Match>> Handle(ScheduleMatchCommand command, CancellationToken cancellationToken)
    {
        // Verify tournament exists
        var tournament = await _repository.GetByIdAsync(command.TournamentId, cancellationToken);
        if (tournament is null)
        {
            return Result.Fail<Match>($"Tournament with ID {command.TournamentId} not found.");
        }

        // Verify both teams are in the tournament
        var homeTeam = await _repository.GetTournamentTeamAsync(
            command.TournamentId, command.HomeTeamId, cancellationToken);
        if (homeTeam is null)
        {
            return Result.Fail<Match>($"Home team with ID {command.HomeTeamId} is not in this tournament.");
        }

        var awayTeam = await _repository.GetTournamentTeamAsync(
            command.TournamentId, command.AwayTeamId, cancellationToken);
        if (awayTeam is null)
        {
            return Result.Fail<Match>($"Away team with ID {command.AwayTeamId} is not in this tournament.");
        }

        var matchResult = Match.Create(
            command.TournamentId,
            command.HomeTeamId,
            command.AwayTeamId,
            command.ScheduledDate);

        if (matchResult.IsFailed)
        {
            return matchResult;
        }

        var match = await _repository.AddMatchAsync(matchResult.Value, cancellationToken);
        return Result.Ok(match);
    }
}
