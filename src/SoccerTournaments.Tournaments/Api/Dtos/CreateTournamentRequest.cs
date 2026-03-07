namespace SoccerTournaments.Tournaments.Api.Dtos;

public record CreateTournamentRequest(string Name, DateTime StartDate, int MaxTeams);
