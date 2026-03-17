using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using SoccerTournaments.Players.Features;

namespace SoccerTournaments.Players.Api;

public static class PlayersEndpoints
{
    public static IEndpointRouteBuilder MapPlayersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/players").WithTags("Players");

        group.MapPost("/", CreatePlayer)
            .WithName("CreatePlayer");

        group.MapGet("/{id:guid}", GetPlayerById)
            .WithName("GetPlayerById");

        group.MapGet("/", GetAllPlayers)
            .WithName("GetAllPlayers");

        group.MapGet("/team/{teamId:guid}", GetPlayersByTeam)
            .WithName("GetPlayersByTeam");

        group.MapPost("/{id:guid}/team", AssignPlayerToTeam)
            .WithName("AssignPlayerToTeam");

        return app;
    }

    private static async Task<IResult> CreatePlayer(
        CreatePlayerRequest request,
        CreatePlayerHandler handler,
        IValidator<CreatePlayerCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = new CreatePlayerCommand(
            request.Name,
            request.Position,
            request.JerseyNumber,
            request.DateOfBirth);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Results.BadRequest(new AppError("Validation", errors));
        }

        var result = await handler.Handle(command, cancellationToken);
        return result.ToApiResult(player => player.ToPlayerResponse());
    }

    private static async Task<IResult> GetPlayerById(
        Guid id,
        GetPlayerByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPlayerByIdQuery(id);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToApiResult(player => player.ToPlayerResponse());
    }

    private static async Task<IResult> GetAllPlayers(
        GetAllPlayersHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetAllPlayersQuery();
        var result = await handler.Handle(query, cancellationToken);
        return result.ToApiResult(players => players.Select(p => p.ToPlayerResponse()));
    }

    private static async Task<IResult> GetPlayersByTeam(
        Guid teamId,
        GetPlayersByTeamHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPlayersByTeamQuery(teamId);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToApiResult(players => players.Select(p => p.ToPlayerResponse()));
    }

    private static async Task<IResult> AssignPlayerToTeam(
        Guid id,
        AssignPlayerToTeamRequest request,
        AssignPlayerToTeamHandler handler,
        IValidator<AssignPlayerToTeamCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = new AssignPlayerToTeamCommand(id, request.TeamId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Results.BadRequest(new AppError("Validation", errors));
        }

        var result = await handler.Handle(command, cancellationToken);
        return result.ToApiResult(player => player.ToPlayerResponse());
    }
}
