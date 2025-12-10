namespace SoccerTournaments.Teams;

public class TeamsService : ITeamsService
{
    private readonly List<Team> _teams = new();

    public Task<Team> AddTeamAsync(string name, string city)
    {
        var team = new Team(name, city);
        _teams.Add(team);
        return Task.FromResult(team);
    }
}