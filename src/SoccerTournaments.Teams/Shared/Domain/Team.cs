using FluentResults;

namespace SoccerTournaments.Teams;

public class Team
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public DateTime CreationDate { get; private set; }

    private Team() { } // For EF Core

    private Team(string name, string city)
    {
        Id = Guid.NewGuid();
        Name = name;
        City = city;
        CreationDate = DateTime.UtcNow;
    }

    public static Result<Team> Create(string name, string city)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Fail("Team name cannot be empty");
        
        if (name.Length > 100)
            return Result.Fail("Team name cannot exceed 100 characters");
        
        if (string.IsNullOrWhiteSpace(city))
            return Result.Fail("Team city cannot be empty");
        
        if (city.Length > 100)
            return Result.Fail("Team city cannot exceed 100 characters");

        return Result.Ok(new Team(name, city));
    }
}