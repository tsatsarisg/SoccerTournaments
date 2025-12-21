using FluentValidation;

namespace SoccerTournaments.Teams;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Team name is required")
            .MaximumLength(100)
            .WithMessage("Team name cannot exceed 100 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("Team city is required")
            .MaximumLength(100)
            .WithMessage("Team city cannot exceed 100 characters");
    }
}
