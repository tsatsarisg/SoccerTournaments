using FluentResults;

namespace SoccerTournaments.Tournaments;

public class GetAllTournamentsHandler
{
    private readonly ITournamentsRepository _repository;

    public GetAllTournamentsHandler(ITournamentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Tournament>>> HandleAsync(GetAllTournamentsQuery query, CancellationToken cancellationToken = default)
    {
        var tournaments = await _repository.GetAllAsync(cancellationToken);
        return Result.Ok(tournaments);
    }
}
