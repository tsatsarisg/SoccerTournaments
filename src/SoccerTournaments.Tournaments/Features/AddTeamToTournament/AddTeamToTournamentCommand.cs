namespace SoccerTournaments.Tournaments;

public record AddTeamToTournamentCommand(Guid TournamentId, Guid TeamId);
