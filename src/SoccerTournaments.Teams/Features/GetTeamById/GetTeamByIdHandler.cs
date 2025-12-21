using FluentResults;

namespace SoccerTournaments.Teams;

public class GetTeamByIdHandler
{
    private readonly ITeamsRepository _repository;

    public GetTeamByIdHandler(ITeamsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Team>> HandleAsync(GetTeamByIdQuery query, CancellationToken cancellationToken = default)
    {
        if (query.Id == Guid.Empty)
        {
            return Result.Fail(AppError.Validation("Team ID cannot be empty"));
        }

        var team = await _repository.GetByIdAsync(query.Id, cancellationToken);
        
        if (team is null)
        {
            return Result.Fail(AppError.NotFound($"Team with ID '{query.Id}' not found"));
        }

        return Result.Ok(team);
    }
}
