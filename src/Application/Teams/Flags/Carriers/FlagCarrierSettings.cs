namespace CTF.Application.Teams.Flags.Carriers;

/// <summary>
/// Represents the settings for the flag carrier.
/// </summary>
public class FlagCarrierSettings
{
    /// <summary>
    /// Gets the maximum duration (in seconds) that the flag carrier can be idle (AFK) while holding the flag.
    /// </summary>
    public int PauseTime { get; init; } = 30;

    /// <summary>
    /// Gets a value indicating whether the flag carrier should be shown on the radar map.
    /// </summary>
    public bool ShowOnRadarMap { get; set; } = true;
}
