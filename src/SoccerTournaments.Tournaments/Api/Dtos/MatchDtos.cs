namespace SoccerTournaments.Tournaments.Api.Dtos;

public sealed record ScheduleMatchRequest(
    Guid HomeTeamId,
    Guid AwayTeamId,
    DateTime? ScheduledDate);

public sealed record MatchResponse(
    Guid Id,
    Guid TournamentId,
    Guid HomeTeamId,
    Guid AwayTeamId,
    DateTime? ScheduledDate,
    string Status,
    int? HomeGoals,
    int? AwayGoals,
    DateTime CreatedAt);

public sealed record UpdateMatchStatusRequest(
    string Status,
    int? HomeGoals,
    int? AwayGoals);
