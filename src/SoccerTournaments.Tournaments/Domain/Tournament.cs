using FluentResults;

namespace SoccerTournaments.Tournaments;

public class Tournament
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public int MaxTeams { get; private set; }
    public DateTime CreationDate { get; private set; }

    private Tournament() { } // For EF Core

    private Tournament(string name, DateTime startDate, int maxTeams)
    {
        Id = Guid.NewGuid();
        Name = name;
        StartDate = startDate;
        MaxTeams = maxTeams;
        CreationDate = DateTime.UtcNow;
    }

    public static Result<Tournament> Create(string name, DateTime startDate, int maxTeams)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Fail("Tournament name cannot be empty");

        if (name.Length > 150)
            return Result.Fail("Tournament name cannot exceed 150 characters");

        if (maxTeams < 2)
            return Result.Fail("A tournament must have at least 2 teams");

        if (maxTeams > 128)
            return Result.Fail("A tournament cannot have more than 128 teams");

        var normalizedStartDate = NormalizeToUtc(startDate);

        return Result.Ok(new Tournament(name, normalizedStartDate, maxTeams));
    }

    private static DateTime NormalizeToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
