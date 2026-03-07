using FluentResults;
using FluentValidation;

namespace SoccerTournaments.Tournaments;

public class CreateTournamentHandler
{
    private readonly ITournamentsRepository _repository;
    private readonly IValidator<CreateTournamentCommand> _validator;

    public CreateTournamentHandler(
        ITournamentsRepository repository,
        IValidator<CreateTournamentCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Tournament>> HandleAsync(CreateTournamentCommand command, CancellationToken cancellationToken = default)
    {
        // Validate command
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => (IError)AppError.Validation(e.ErrorMessage))
                .ToList();

            return Result.Fail<Tournament>(errors);
        }

        // Check for duplicate tournament name
        var existing = await _repository.GetByNameAsync(command.Name, cancellationToken);
        if (existing is not null)
        {
            return Result.Fail(AppError.Validation($"A tournament with the name '{command.Name}' already exists"));
        }

        // Create tournament domain entity
        var tournamentResult = Tournament.Create(command.Name, command.StartDate, command.MaxTeams);
        if (tournamentResult.IsFailed)
        {
            var errors = tournamentResult.Errors
                .Select(e => (IError)AppError.Validation(e.Message))
                .ToList();

            return Result.Fail<Tournament>(errors);
        }

        // Persist tournament
        var tournament = await _repository.AddAsync(tournamentResult.Value, cancellationToken);

        return Result.Ok(tournament);
    }
}
