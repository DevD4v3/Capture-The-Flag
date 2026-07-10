namespace CTF.Application.Teams.Statistics;

public class TeamStatsPerRound
{
    public int Score { get; private set; }
    public int Kills { get; private set; }
    public int Deaths { get; private set; }

    public void AddScore()  => Score++;
    public void AddKills()  => Kills++;
    public void AddDeaths() => Deaths++;
    public void Reset()
    {
        Score = 0; 
        Kills = 0; 
        Deaths = 0;
    }
}
