using FluentResults;
using SoccerTournaments.Tournaments.Domain;

namespace SoccerTournaments.Tournaments.Features;

public sealed record GetTournamentMatchesQuery(Guid TournamentId);

public sealed class GetTournamentMatchesHandler
{
    private readonly ITournamentsRepository _repository;

    public GetTournamentMatchesHandler(ITournamentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Match>>> Handle(GetTournamentMatchesQuery query, CancellationToken cancellationToken)
    {
        var tournament = await _repository.GetByIdAsync(query.TournamentId, cancellationToken);
        if (tournament is null)
        {
            return Result.Fail<IEnumerable<Match>>($"Tournament with ID {query.TournamentId} not found.");
        }

        var matches = await _repository.GetTournamentMatchesAsync(query.TournamentId, cancellationToken);
        return Result.Ok(matches);
    }
}
