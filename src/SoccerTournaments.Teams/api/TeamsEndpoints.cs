using Microsoft.AspNetCore.Builder;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;     

namespace SoccerTournaments.Teams;

public static class TeamsEndpoints
{
    public static void MapTeamsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/teams");

        group.MapPost("/", async (CreateTeamHandler handler, CreateTeamRequest request, CancellationToken cancellationToken) =>
        {
            var command = new CreateTeamCommand(request.Name, request.City);
            var result = await handler.HandleAsync(command, cancellationToken);
            
            if (result.IsFailed)
            {
                return Results.BadRequest(new { errors = result.Errors.Select(e => e.Message) });
            }

            var team = result.Value;
            return Results.Created($"/teams/{team.Id}", new TeamResponse(team.Id, team.Name, team.City, team.CreationDate));
        });

        group.MapGet("/{id:guid}", async (Guid id, ITeamsRepository repository, CancellationToken cancellationToken) =>
        {
            var team = await repository.GetByIdAsync(id, cancellationToken);
            
            if (team == null)
            {
                return Results.NotFound(new { message = $"Team with id '{id}' not found" });
            }

            return Results.Ok(new TeamResponse(team.Id, team.Name, team.City, team.CreationDate));
        });

        group.MapGet("/", async (ITeamsRepository repository, CancellationToken cancellationToken) =>
        {
            var teams = await repository.GetAllAsync(cancellationToken);
            var response = teams.Select(t => new TeamResponse(t.Id, t.Name, t.City, t.CreationDate));
            return Results.Ok(response);
        });
    }
}

public record CreateTeamRequest(string Name, string City);
public record TeamResponse(Guid Id, string Name, string City, DateTime CreationDate);