namespace SoccerTournaments.Tournaments;

public record CreateTournamentCommand(string Name, DateTime StartDate, int MaxTeams);
