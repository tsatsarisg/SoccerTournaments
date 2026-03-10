using FluentValidation;

namespace SoccerTournaments.Tournaments;

public class RecordMatchResultCommandValidator : AbstractValidator<RecordMatchResultCommand>
{
    public RecordMatchResultCommandValidator()
    {
        RuleFor(x => x.TournamentId)
            .NotEmpty()
            .WithMessage("Tournament ID is required");

        RuleFor(x => x.HomeTeamId)
            .NotEmpty()
            .WithMessage("Home team ID is required");

        RuleFor(x => x.AwayTeamId)
            .NotEmpty()
            .WithMessage("Away team ID is required");

        RuleFor(x => x.HomeGoals)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Home goals cannot be negative");

        RuleFor(x => x.AwayGoals)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Away goals cannot be negative");

        RuleFor(x => x)
            .Must(x => x.HomeTeamId != x.AwayTeamId)
            .WithMessage("Home team and away team cannot be the same");
    }
}
