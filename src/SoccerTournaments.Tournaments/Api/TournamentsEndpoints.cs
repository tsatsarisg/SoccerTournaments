using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SoccerTournaments.Tournaments.Api.Dtos;

namespace SoccerTournaments.Tournaments.Api;

public static class TournamentsEndpoints
{
    public static void MapTournamentsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tournaments");

        group.MapPost("/", async (CreateTournamentHandler handler, CreateTournamentRequest request, CancellationToken cancellationToken) =>
        {
            var command = new CreateTournamentCommand(request.Name, request.StartDate, request.MaxTeams);
            var result = await handler.HandleAsync(command, cancellationToken);

            return result.ToIResult(tournament =>
                Results.Created($"/tournaments/{tournament.Id}", new TournamentResponse(tournament.Id, tournament.Name, tournament.StartDate, tournament.MaxTeams, tournament.CreationDate)));
        });

        group.MapGet("/{id:guid}", async (Guid id, GetTournamentByIdHandler handler, CancellationToken cancellationToken) =>
        {
            var query = new GetTournamentByIdQuery(id);
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.ToIResult(tournament =>
                Results.Ok(new TournamentResponse(tournament.Id, tournament.Name, tournament.StartDate, tournament.MaxTeams, tournament.CreationDate)));
        });

        group.MapGet("/", async (GetAllTournamentsHandler handler, CancellationToken cancellationToken) =>
        {
            var query = new GetAllTournamentsQuery();
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.ToIResult(tournaments =>
            {
                var response = tournaments.Select(t => new TournamentResponse(t.Id, t.Name, t.StartDate, t.MaxTeams, t.CreationDate));
                return Results.Ok(response);
            });
        });

        group.MapPost("/{tournamentId:guid}/teams", async (Guid tournamentId, AddTeamToTournamentHandler handler, AddTeamToTournamentRequest request, CancellationToken cancellationToken) =>
        {
            var command = new AddTeamToTournamentCommand(tournamentId, request.TeamId);
            var result = await handler.HandleAsync(command, cancellationToken);

            return result.ToIResult(tt =>
                Results.Created($"/tournaments/{tt.TournamentId}/teams", new TournamentTeamResponse(tt.TournamentId, tt.TeamId, tt.AddedAt)));
        });

        group.MapGet("/{tournamentId:guid}/teams", async (Guid tournamentId, GetTournamentTeamsHandler handler, CancellationToken cancellationToken) =>
        {
            var query = new GetTournamentTeamsQuery(tournamentId);
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.ToIResult(teams =>
            {
                var response = teams.Select(tt => new TournamentTeamResponse(tt.TournamentId, tt.TeamId, tt.AddedAt));
                return Results.Ok(response);
            });
        });

        group.MapPost("/{tournamentId:guid}/results", async (Guid tournamentId, RecordMatchResultHandler handler, RecordMatchResultRequest request, CancellationToken cancellationToken) =>
        {
            var command = new RecordMatchResultCommand(tournamentId, request.HomeTeamId, request.AwayTeamId, request.HomeGoals, request.AwayGoals);
            var result = await handler.HandleAsync(command, cancellationToken);

            return result.ToIResult(standings =>
            {
                var response = standings.Select(s => new StandingResponse(s.TournamentId, s.TeamId, s.MatchesPlayed, s.Wins, s.Draws, s.Losses, s.GoalsFor, s.GoalsAgainst, s.GoalDifference, s.Points));
                return Results.Ok(response);
            });
        });

        group.MapGet("/{tournamentId:guid}/standings", async (Guid tournamentId, GetTournamentStandingsHandler handler, CancellationToken cancellationToken) =>
        {
            var query = new GetTournamentStandingsQuery(tournamentId);
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.ToIResult(standings =>
            {
                var response = standings.Select(s => new StandingResponse(s.TournamentId, s.TeamId, s.MatchesPlayed, s.Wins, s.Draws, s.Losses, s.GoalsFor, s.GoalsAgainst, s.GoalDifference, s.Points));
                return Results.Ok(response);
            });
        });
    }
}
