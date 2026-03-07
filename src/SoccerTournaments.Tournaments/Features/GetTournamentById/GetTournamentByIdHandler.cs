using FluentResults;

namespace SoccerTournaments.Tournaments;

public class GetTournamentByIdHandler
{
    private readonly ITournamentsRepository _repository;

    public GetTournamentByIdHandler(ITournamentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Tournament>> HandleAsync(GetTournamentByIdQuery query, CancellationToken cancellationToken = default)
    {
        if (query.Id == Guid.Empty)
        {
            return Result.Fail(AppError.Validation("Tournament ID cannot be empty"));
        }

        var tournament = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (tournament is null)
        {
            return Result.Fail(AppError.NotFound($"Tournament with ID '{query.Id}' not found"));
        }

        return Result.Ok(tournament);
    }
}
