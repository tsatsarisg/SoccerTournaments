using FluentResults;
using FluentValidation;

namespace SoccerTournaments.Tournaments;

public class RecordMatchResultHandler
{
    private readonly ITournamentsRepository _repository;
    private readonly IValidator<RecordMatchResultCommand> _validator;

    public RecordMatchResultHandler(
        ITournamentsRepository repository,
        IValidator<RecordMatchResultCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<IEnumerable<Standing>>> HandleAsync(RecordMatchResultCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => (IError)AppError.Validation(e.ErrorMessage))
                .ToList();
            return Result.Fail<IEnumerable<Standing>>(errors);
        }

        // Check tournament exists
        var tournament = await _repository.GetByIdAsync(command.TournamentId, cancellationToken);
        if (tournament is null)
            return Result.Fail(AppError.NotFound($"Tournament with ID '{command.TournamentId}' not found"));

        // Check both teams are in the tournament
        var homeTeamEntry = await _repository.GetTournamentTeamAsync(command.TournamentId, command.HomeTeamId, cancellationToken);
        if (homeTeamEntry is null)
            return Result.Fail(AppError.Validation($"Team with ID '{command.HomeTeamId}' is not part of this tournament"));

        var awayTeamEntry = await _repository.GetTournamentTeamAsync(command.TournamentId, command.AwayTeamId, cancellationToken);
        if (awayTeamEntry is null)
            return Result.Fail(AppError.Validation($"Team with ID '{command.AwayTeamId}' is not part of this tournament"));

        // Get or create standings for both teams
        var homeStanding = await GetOrCreateStandingAsync(command.TournamentId, command.HomeTeamId, cancellationToken);
        var awayStanding = await GetOrCreateStandingAsync(command.TournamentId, command.AwayTeamId, cancellationToken);

        // Update standings based on result
        if (command.HomeGoals > command.AwayGoals)
        {
            homeStanding.RecordWin(command.HomeGoals, command.AwayGoals);
            awayStanding.RecordLoss(command.AwayGoals, command.HomeGoals);
        }
        else if (command.HomeGoals < command.AwayGoals)
        {
            homeStanding.RecordLoss(command.HomeGoals, command.AwayGoals);
            awayStanding.RecordWin(command.AwayGoals, command.HomeGoals);
        }
        else
        {
            homeStanding.RecordDraw(command.HomeGoals, command.AwayGoals);
            awayStanding.RecordDraw(command.AwayGoals, command.HomeGoals);
        }

        await _repository.UpdateStandingAsync(homeStanding, cancellationToken);
        await _repository.UpdateStandingAsync(awayStanding, cancellationToken);

        return Result.Ok<IEnumerable<Standing>>([homeStanding, awayStanding]);
    }

    private async Task<Standing> GetOrCreateStandingAsync(Guid tournamentId, Guid teamId, CancellationToken cancellationToken)
    {
        var standing = await _repository.GetStandingAsync(tournamentId, teamId, cancellationToken);
        if (standing is not null)
            return standing;

        var newStanding = new Standing(tournamentId, teamId);
        return await _repository.AddStandingAsync(newStanding, cancellationToken);
    }
}
