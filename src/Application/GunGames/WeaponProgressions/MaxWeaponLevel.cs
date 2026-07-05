namespace CTF.Application.GunGames.WeaponProgressions;

/// <summary>
/// Represents the highest weapon level available in a weapon progression.
/// </summary>
public readonly struct MaxWeaponLevel
{
    public int Value { get; }

    public MaxWeaponLevel(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);
        Value = value;
    }
}
