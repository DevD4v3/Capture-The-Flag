namespace CTF.Application.Teams.Services;

public class TeamSoundsService
{
    /// <summary>
    /// Plays the sound when the team's flag is taken.
    /// </summary>
    public void PlayFlagTakenSound(Team team)
        => PlayAudioStreamToAll(team.Sounds.FlagTaken);

    /// <summary>
    /// Plays the sound when the team's flag is dropped.
    /// </summary>
    public void PlayFlagDroppedSound(Team team)
        => PlayAudioStreamToAll(team.Sounds.FlagDropped);

    /// <summary>
    /// Plays the sound when the team's flag is returned.
    /// </summary>
    public void PlayFlagReturnedSound(Team team)
        => PlayAudioStreamToAll(team.Sounds.FlagReturned);

    /// <summary>
    /// Plays the sound when the team scores.
    /// </summary>
    public void PlayTeamScoresSound(Team team)
        => PlayAudioStreamToAll(team.Sounds.TeamScores);

    private void PlayAudioStreamToAll(string url)
    {
        IEnumerable<Player> players = MatchPlayers.GetAll();
        foreach (Player player in players)
            player.PlayAudioStream(url);
    }
}
