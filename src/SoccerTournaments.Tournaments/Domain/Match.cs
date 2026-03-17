using FluentResults;

namespace SoccerTournaments.Tournaments.Domain;

public sealed class Match
{
    public required Guid Id { get; init; }
    public required Guid TournamentId { get; init; }
    public required Guid HomeTeamId { get; init; }
    public required Guid AwayTeamId { get; init; }
    public DateTime? ScheduledDate { get; private set; }
    public MatchStatus Status { get; private set; }
    public int? HomeGoals { get; private set; }
    public int? AwayGoals { get; private set; }
    public required DateTime CreatedAt { get; init; }

    private Match() { }

    public static Result<Match> Create(
        Guid tournamentId,
        Guid homeTeamId,
        Guid awayTeamId,
        DateTime? scheduledDate = null)
    {
        if (tournamentId == Guid.Empty)
        {
            return Result.Fail<Match>("Tournament ID cannot be empty.");
        }

        if (homeTeamId == Guid.Empty || awayTeamId == Guid.Empty)
        {
            return Result.Fail<Match>("Team IDs cannot be empty.");
        }

        if (homeTeamId == awayTeamId)
        {
            return Result.Fail<Match>("Home and away teams must be different.");
        }

        if (scheduledDate.HasValue && scheduledDate.Value <= DateTime.UtcNow)
        {
            return Result.Fail<Match>("Scheduled date must be in the future.");
        }

        var match = new Match
        {
            Id = Guid.NewGuid(),
            TournamentId = tournamentId,
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            ScheduledDate = scheduledDate?.ToUniversalTime(),
            Status = MatchStatus.Scheduled,
            CreatedAt = DateTime.UtcNow
        };

        return Result.Ok(match);
    }

    public Result Schedule(DateTime scheduledDate)
    {
        if (scheduledDate <= DateTime.UtcNow)
        {
            return Result.Fail("Scheduled date must be in the future.");
        }

        if (Status != MatchStatus.Scheduled)
        {
            return Result.Fail($"Cannot reschedule a match with status {Status}.");
        }

        ScheduledDate = scheduledDate.ToUniversalTime();
        return Result.Ok();
    }

    public Result StartMatch()
    {
        if (Status != MatchStatus.Scheduled)
        {
            return Result.Fail($"Cannot start a match with status {Status}.");
        }

        Status = MatchStatus.InProgress;
        return Result.Ok();
    }

    public Result CompleteMatch(int homeGoals, int awayGoals)
    {
        if (homeGoals < 0 || awayGoals < 0)
        {
            return Result.Fail("Goals cannot be negative.");
        }

        if (Status != MatchStatus.InProgress && Status != MatchStatus.Scheduled)
        {
            return Result.Fail($"Cannot complete a match with status {Status}.");
        }

        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
        Status = MatchStatus.Completed;
        return Result.Ok();
    }

    public Result CancelMatch()
    {
        if (Status == MatchStatus.Completed)
        {
            return Result.Fail("Cannot cancel a completed match.");
        }

        Status = MatchStatus.Cancelled;
        return Result.Ok();
    }
}

public enum MatchStatus
{
    Scheduled = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}
