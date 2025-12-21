using FluentResults;

namespace SoccerTournaments.Teams;

public class GetTeamByNameHandler
{
    private readonly ITeamsRepository _repository;

    public GetTeamByNameHandler(ITeamsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Team>> HandleAsync(GetTeamByNameQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Name))
        {
            return Result.Fail(AppError.Validation("Team name cannot be empty"));
        }

        var team = await _repository.GetByNameAsync(query.Name, cancellationToken);
        
        if (team is null)
        {
            return Result.Fail(AppError.NotFound($"Team with name '{query.Name}' not found"));
        }

        return Result.Ok(team);
    }
}
