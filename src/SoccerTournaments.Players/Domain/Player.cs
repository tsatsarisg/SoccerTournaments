using FluentResults;

namespace SoccerTournaments.Players.Domain;

public sealed class Player
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Position { get; init; }
    public required int JerseyNumber { get; init; }
    public Guid? TeamId { get; private set; }
    public required DateTime DateOfBirth { get; init; }
    public required DateTime CreationDate { get; init; }

    private Player() { }

    public static Result<Player> Create(
        string name,
        string position,
        int jerseyNumber,
        DateTime dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
        {
            return Result.Fail<Player>("Name is required and must be 100 characters or less.");
        }

        var validPositions = new[] { "Goalkeeper", "Defender", "Midfielder", "Forward" };
        if (string.IsNullOrWhiteSpace(position) || !validPositions.Contains(position))
        {
            return Result.Fail<Player>("Position must be one of: Goalkeeper, Defender, Midfielder, Forward.");
        }

        if (jerseyNumber < 1 || jerseyNumber > 99)
        {
            return Result.Fail<Player>("Jersey number must be between 1 and 99.");
        }

        if (dateOfBirth >= DateTime.UtcNow)
        {
            return Result.Fail<Player>("Date of birth must be in the past.");
        }

        var player = new Player
        {
            Id = Guid.NewGuid(),
            Name = name,
            Position = position,
            JerseyNumber = jerseyNumber,
            DateOfBirth = dateOfBirth.ToUniversalTime(),
            CreationDate = DateTime.UtcNow
        };

        return Result.Ok(player);
    }

    public Result AssignToTeam(Guid teamId)
    {
        if (teamId == Guid.Empty)
        {
            return Result.Fail("Team ID cannot be empty.");
        }

        TeamId = teamId;
        return Result.Ok();
    }

    public Result RemoveFromTeam()
    {
        TeamId = null;
        return Result.Ok();
    }
}
