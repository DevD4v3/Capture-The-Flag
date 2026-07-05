namespace CTF.Application.GunGames;

/// <summary>
/// Represents the number of kills required to advance to the next weapon level.
/// </summary>
public readonly struct KillsRequiredPerLevel
{
    public int Value { get; }

    public KillsRequiredPerLevel(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);
        Value = value;
    }
}
