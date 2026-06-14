namespace CTF.Application.Players.Headshots;

/// <summary>
/// Represents settings for headshot events.
/// </summary>
public class HeadshotSettings
{
    /// <summary>
    /// Gets the audio URL played when a player performs a headshot.
    /// </summary>
    public string AudioUrl { get; init; } = string.Empty;
}
