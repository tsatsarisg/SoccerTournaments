using Microsoft.AspNetCore.Builder;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;     

namespace SoccerTournaments.Teams;

public static class TeamsEndpoints
{
    public static void MapTeamsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/teams");

        group.MapPost("/", async (ITeamsService teamsService, TeamRequest request) =>
        {
            var team = await teamsService.AddTeamAsync(request.Name, request.City);
            return Results.Created($"/teams/{team.Name}", team);
        });
    }
}

public record TeamRequest(string Name, string City);