using FluentValidation;

namespace SoccerTournaments.Tournaments;

public class AddTeamToTournamentCommandValidator : AbstractValidator<AddTeamToTournamentCommand>
{
    public AddTeamToTournamentCommandValidator()
    {
        RuleFor(x => x.TournamentId)
            .NotEmpty()
            .WithMessage("Tournament ID is required");

        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("Team ID is required");
    }
}
