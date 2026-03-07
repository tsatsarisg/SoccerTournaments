using FluentResults;
using FluentValidation;
using SoccerTournaments.Teams;

namespace SoccerTournaments.Tournaments;

public class AddTeamToTournamentHandler
{
    private readonly ITournamentsRepository _tournamentsRepository;
    private readonly ITeamsRepository _teamsRepository;
    private readonly IValidator<AddTeamToTournamentCommand> _validator;

    public AddTeamToTournamentHandler(
        ITournamentsRepository tournamentsRepository,
        ITeamsRepository teamsRepository,
        IValidator<AddTeamToTournamentCommand> validator)
    {
        _tournamentsRepository = tournamentsRepository;
        _teamsRepository = teamsRepository;
        _validator = validator;
    }

    public async Task<Result<TournamentTeam>> HandleAsync(AddTeamToTournamentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => (IError)AppError.Validation(e.ErrorMessage))
                .ToList();
            return Result.Fail<TournamentTeam>(errors);
        }

        // Check tournament exists
        var tournament = await _tournamentsRepository.GetByIdAsync(command.TournamentId, cancellationToken);
        if (tournament is null)
            return Result.Fail(AppError.NotFound($"Tournament with ID '{command.TournamentId}' not found"));

        // Check team exists
        var team = await _teamsRepository.GetByIdAsync(command.TeamId, cancellationToken);
        if (team is null)
            return Result.Fail(AppError.NotFound($"Team with ID '{command.TeamId}' not found"));

        // Check team not already in tournament
        var existing = await _tournamentsRepository.GetTournamentTeamAsync(command.TournamentId, command.TeamId, cancellationToken);
        if (existing is not null)
            return Result.Fail(AppError.Validation($"Team '{team.Name}' is already in this tournament"));

        // Check tournament not full
        var currentCount = await _tournamentsRepository.GetTournamentTeamCountAsync(command.TournamentId, cancellationToken);
        if (currentCount >= tournament.MaxTeams)
            return Result.Fail(AppError.Validation($"Tournament is full ({tournament.MaxTeams} teams max)"));

        var tournamentTeam = new TournamentTeam(command.TournamentId, command.TeamId);
        await _tournamentsRepository.AddTeamAsync(tournamentTeam, cancellationToken);

        return Result.Ok(tournamentTeam);
    }
}
