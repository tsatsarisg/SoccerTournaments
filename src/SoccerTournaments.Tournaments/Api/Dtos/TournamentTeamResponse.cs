namespace SoccerTournaments.Tournaments.Api.Dtos;

public record TournamentTeamResponse(Guid TournamentId, Guid TeamId, DateTime AddedAt);
