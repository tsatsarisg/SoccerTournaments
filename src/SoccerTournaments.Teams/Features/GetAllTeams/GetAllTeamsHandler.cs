using FluentResults;

namespace SoccerTournaments.Teams;

public class GetAllTeamsHandler
{
    private readonly ITeamsRepository _repository;

    public GetAllTeamsHandler(ITeamsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Team>>> HandleAsync(GetAllTeamsQuery query, CancellationToken cancellationToken = default)
    {
        var teams = await _repository.GetAllAsync(cancellationToken);
        return Result.Ok(teams);
    }
}
