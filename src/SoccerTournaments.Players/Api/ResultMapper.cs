using FluentResults;
using Microsoft.AspNetCore.Http;
using SoccerTournaments.Players.Domain;

namespace SoccerTournaments.Players.Api;

public static class ResultMapper
{
    public static IResult ToApiResult<T>(this Result<T> result, Func<T, object> mapper)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(mapper(result.Value));
        }

        var error = result.Errors.First();
        var errorMessage = error.Message;

        if (errorMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return Results.NotFound(new AppError("NotFound", errorMessage));
        }

        return Results.BadRequest(new AppError("Validation", errorMessage));
    }

    public static PlayerResponse ToPlayerResponse(this Player player)
    {
        return new PlayerResponse(
            player.Id,
            player.Name,
            player.Position,
            player.JerseyNumber,
            player.TeamId,
            player.DateOfBirth,
            player.CreationDate);
    }
}
