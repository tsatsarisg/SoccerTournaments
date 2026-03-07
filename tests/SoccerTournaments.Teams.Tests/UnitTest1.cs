using SoccerTournaments.Teams;

namespace SoccerTournaments.Teams.Tests;

public class UnitTest1
{
    [Fact]
    public void CreateTeam_ShouldCreateTeam()
    {
        // Act
        var result = Team.Create("Chelsea", "London");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Chelsea", result.Value.Name);
        Assert.Equal("London", result.Value.City);
    }
}
