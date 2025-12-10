using SoccerTournaments.Teams;

namespace SoccerTournaments.Teams.Tests;

public class UnitTest1
{
    [Fact]
    public async Task AddTeamAsync_ShouldAddTeam()
    {
        // Arrange
        var service = new TeamsService();

        // Act
        var team = await service.AddTeamAsync("Chelsea", "London");

        // Assert
        Assert.Equal("Chelsea", team.Name);
        Assert.Equal("London", team.City);
    }
}
