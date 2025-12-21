using FluentResults;
using FluentValidation;

namespace SoccerTournaments.Teams;

public class CreateTeamHandler
{
    private readonly ITeamsRepository _repository;
    private readonly IValidator<CreateTeamCommand> _validator;

    public CreateTeamHandler(ITeamsRepository repository, IValidator<CreateTeamCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Team>> HandleAsync(CreateTeamCommand command, CancellationToken cancellationToken = default)
    {
        // Validate command
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(string.Join(", ", errors));
        }

        // Check for duplicate team name
        var existingTeam = await _repository.GetByNameAsync(command.Name, cancellationToken);
        if (existingTeam != null)
        {
            return Result.Fail($"A team with the name '{command.Name}' already exists");
        }

        // Create team domain entity
        var teamResult = Team.Create(command.Name, command.City);
        if (teamResult.IsFailed)
        {
            return teamResult;
        }

        // Persist team
        var team = await _repository.AddAsync(teamResult.Value, cancellationToken);
        
        return Result.Ok(team);
    }
}
