namespace CTF.Application.Teams.Flags.AutoReturn;

/// <summary>
/// Represents settings for automatic flag return.
/// </summary>
public class FlagAutoReturnSettings
{
    /// <summary>
    /// Gets the delay, in seconds, before a dropped flag is returned automatically.
    /// </summary>
    public int Delay { get; init; } = 120;
}
