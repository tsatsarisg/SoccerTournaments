using FluentValidation;

namespace SoccerTournaments.Tournaments;

public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
{
    public CreateTournamentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tournament name is required")
            .MaximumLength(150)
            .WithMessage("Tournament name cannot exceed 150 characters");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(x => x.MaxTeams)
            .GreaterThanOrEqualTo(2)
            .WithMessage("A tournament must have at least 2 teams")
            .LessThanOrEqualTo(128)
            .WithMessage("A tournament cannot have more than 128 teams");
    }
}
