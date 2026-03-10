namespace SoccerTournaments.Tournaments;

public record RecordMatchResultCommand(Guid TournamentId, Guid HomeTeamId, Guid AwayTeamId, int HomeGoals, int AwayGoals);
