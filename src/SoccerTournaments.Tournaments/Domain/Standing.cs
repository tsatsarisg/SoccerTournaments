namespace SoccerTournaments.Tournaments;

public class Standing
{
    public Guid TournamentId { get; private set; }
    public Guid TeamId { get; private set; }
    public int MatchesPlayed { get; private set; }
    public int Wins { get; private set; }
    public int Draws { get; private set; }
    public int Losses { get; private set; }
    public int GoalsFor { get; private set; }
    public int GoalsAgainst { get; private set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points => (Wins * 3) + Draws;

    private Standing() { } // For EF Core

    public Standing(Guid tournamentId, Guid teamId)
    {
        TournamentId = tournamentId;
        TeamId = teamId;
    }

    public void RecordWin(int goalsFor, int goalsAgainst)
    {
        MatchesPlayed++;
        Wins++;
        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;
    }

    public void RecordDraw(int goalsFor, int goalsAgainst)
    {
        MatchesPlayed++;
        Draws++;
        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;
    }

    public void RecordLoss(int goalsFor, int goalsAgainst)
    {
        MatchesPlayed++;
        Losses++;
        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;
    }
}
