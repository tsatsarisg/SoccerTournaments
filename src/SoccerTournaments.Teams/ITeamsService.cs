namespace SoccerTournaments.Teams;

public interface ITeamsService
{
    Task<Team> AddTeamAsync(string name, string city);
}