namespace SoccerTournaments.Tournaments.Api.Dtos;

public record RecordMatchResultRequest(Guid HomeTeamId, Guid AwayTeamId, int HomeGoals, int AwayGoals);
