using FluentResults;

namespace SoccerTournaments.Tournaments;

public class GetTournamentTeamsHandler
{
    private readonly ITournamentsRepository _repository;

    public GetTournamentTeamsHandler(ITournamentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<TournamentTeam>>> HandleAsync(GetTournamentTeamsQuery query, CancellationToken cancellationToken = default)
    {
        if (query.TournamentId == Guid.Empty)
            return Result.Fail(AppError.Validation("Tournament ID cannot be empty"));

        var tournament = await _repository.GetByIdAsync(query.TournamentId, cancellationToken);
        if (tournament is null)
            return Result.Fail(AppError.NotFound($"Tournament with ID '{query.TournamentId}' not found"));

        var tournamentTeams = await _repository.GetTournamentTeamsAsync(query.TournamentId, cancellationToken);
        return Result.Ok(tournamentTeams);
    }
}
