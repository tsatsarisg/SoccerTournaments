namespace SoccerTournaments.Players.Api;

public sealed record CreatePlayerRequest(
    string Name,
    string Position,
    int JerseyNumber,
    DateTime DateOfBirth);

public sealed record PlayerResponse(
    Guid Id,
    string Name,
    string Position,
    int JerseyNumber,
    Guid? TeamId,
    DateTime DateOfBirth,
    DateTime CreationDate);

public sealed record AssignPlayerToTeamRequest(Guid TeamId);

public sealed record AppError(string Type, string Message);
