namespace SoccerTournaments.Tournaments;

public class TournamentTeam
{
    public Guid TournamentId { get; private set; }
    public Guid TeamId { get; private set; }
    public DateTime AddedAt { get; private set; }

    private TournamentTeam() { } // For EF Core

    public TournamentTeam(Guid tournamentId, Guid teamId)
    {
        TournamentId = tournamentId;
        TeamId = teamId;
        AddedAt = DateTime.UtcNow;
    }
}
