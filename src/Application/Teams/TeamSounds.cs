namespace CTF.Application.Teams;

public class TeamSounds
{
    private string _flagDropped;
    private string _flagReturned;
    private string _flagTaken;
    private string _teamScores;

    public static readonly TeamSounds None;
    public static readonly TeamSounds Alpha;
    public static readonly TeamSounds Beta;

    static TeamSounds()
    {
        var reader = new EnvReader();
        var defaultValue = string.Empty;

        Alpha = new()
        {
            _flagDropped  = reader.EnvString("RedFlagDroppedUrl",  defaultValue),
            _flagReturned = reader.EnvString("RedFlagReturnedUrl", defaultValue),
            _flagTaken    = reader.EnvString("RedFlagTakenUrl",    defaultValue),
            _teamScores   = reader.EnvString("RedTeamScoresUrl",   defaultValue)
        };

        Beta = new()
        {
            _flagDropped  = reader.EnvString("BlueFlagDroppedUrl",  defaultValue),
            _flagReturned = reader.EnvString("BlueFlagReturnedUrl", defaultValue),
            _flagTaken    = reader.EnvString("BlueFlagTakenUrl",    defaultValue),
            _teamScores   = reader.EnvString("BlueTeamScoresUrl",   defaultValue)
        };

        None = new();
    }

    private TeamSounds() { }

    /// <summary>
    /// Plays the sound when the team's flag is taken.
    /// </summary>
    public void PlayFlagTakenSound()
        => PlayAudioStreamToAll(_flagTaken);

    /// <summary>
    /// Plays the sound when the team's flag is dropped.
    /// </summary>
    public void PlayFlagDroppedSound()
        => PlayAudioStreamToAll(_flagDropped);

    /// <summary>
    /// Plays the sound when the team's flag is returned.
    /// </summary>
    public void PlayFlagReturnedSound()
        => PlayAudioStreamToAll(_flagReturned);

    /// <summary>
    /// Plays the sound when the team scores.
    /// </summary>
    public void PlayTeamScoresSound()
        => PlayAudioStreamToAll(_teamScores);

    private static void PlayAudioStreamToAll(string url)
    {
        IEnumerable<Player> players = MatchPlayers.GetAll();
        foreach (Player player in players)
            player.PlayAudioStream(url);
    }
}
