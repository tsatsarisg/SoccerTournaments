using FluentResults;

namespace SoccerTournaments.Tournaments;

public class GetTournamentStandingsHandler
{
    private readonly ITournamentsRepository _repository;

    public GetTournamentStandingsHandler(ITournamentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Standing>>> HandleAsync(GetTournamentStandingsQuery query, CancellationToken cancellationToken = default)
    {
        if (query.TournamentId == Guid.Empty)
            return Result.Fail(AppError.Validation("Tournament ID cannot be empty"));

        var tournament = await _repository.GetByIdAsync(query.TournamentId, cancellationToken);
        if (tournament is null)
            return Result.Fail(AppError.NotFound($"Tournament with ID '{query.TournamentId}' not found"));

        var standings = await _repository.GetTournamentStandingsAsync(query.TournamentId, cancellationToken);

        var sorted = standings
            .OrderByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.GoalsFor);

        return Result.Ok<IEnumerable<Standing>>(sorted);
    }
}
