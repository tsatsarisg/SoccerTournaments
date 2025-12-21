using Microsoft.AspNetCore.Builder;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SoccerTournaments.Teams.Api.Dtos;

namespace SoccerTournaments.Teams.Api;

public static class TeamsEndpoints
{
    public static void MapTeamsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/teams");

        group.MapPost("/", async (CreateTeamHandler handler, CreateTeamRequest request, CancellationToken cancellationToken) =>
        {
            var command = new CreateTeamCommand(request.Name, request.City);
            var result = await handler.HandleAsync(command, cancellationToken);
            
            return result.ToIResult(team => 
                Results.Created($"/teams/{team.Id}", new TeamResponse(team.Id, team.Name, team.City, team.CreationDate)));
        });

        group.MapGet("/{id:guid}", async (Guid id, GetTeamByIdHandler handler, CancellationToken cancellationToken) =>
        {
            var query = new GetTeamByIdQuery(id);
            var result = await handler.HandleAsync(query, cancellationToken);
            
            return result.ToIResult(team => 
                Results.Ok(new TeamResponse(team.Id, team.Name, team.City, team.CreationDate)));
        });

        group.MapGet("/", async (GetAllTeamsHandler handler, CancellationToken cancellationToken) =>
        {
            var query = new GetAllTeamsQuery();
            var result = await handler.HandleAsync(query, cancellationToken);
            
            return result.ToIResult(teams =>
            {
                var response = teams.Select(t => new TeamResponse(t.Id, t.Name, t.City, t.CreationDate));
                return Results.Ok(response);
            });
        });
    }
}