namespace SoccerTournaments.Tournaments.Api.Dtos;

public record StandingResponse(
    Guid TournamentId,
    Guid TeamId,
    int MatchesPlayed,
    int Wins,
    int Draws,
    int Losses,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference,
    int Points);
