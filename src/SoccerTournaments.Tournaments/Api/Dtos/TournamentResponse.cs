namespace SoccerTournaments.Tournaments.Api.Dtos;

public record TournamentResponse(Guid Id, string Name, DateTime StartDate, int MaxTeams, DateTime CreationDate);
