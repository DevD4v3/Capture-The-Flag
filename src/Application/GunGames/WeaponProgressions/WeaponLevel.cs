namespace CTF.Application.GunGames.WeaponProgressions;

/// <summary>
/// Represents a player's current weapon level within a weapon progression.
/// </summary>
public readonly struct WeaponLevel 
    : IComparable<WeaponLevel>, IEquatable<WeaponLevel>
{
    public int Value { get; }

    public static WeaponLevel First { get; } = new(1);

    private WeaponLevel(int value)
        => Value = value;

    /// <summary>
    /// Advances to the next weapon level without exceeding the specified maximum.
    /// </summary>
    public WeaponLevel Next(MaxWeaponLevel maxLevel)
        => new(Value < maxLevel.Value ? Value + 1 : Value);

    /// <summary>
    /// Moves to the previous weapon level.
    /// </summary>
    public WeaponLevel Previous()
        => new(Value > 1 ? Value - 1 : Value);

    /// <summary>
    /// Determines whether this is the final weapon level.
    /// </summary>
    public bool IsMax(MaxWeaponLevel maxLevel) 
        => Value == maxLevel.Value;

    public override string ToString()
        => $"{Value}";

    public int CompareTo(WeaponLevel other)
        => Value.CompareTo(other.Value);

    public bool Equals(WeaponLevel other)
        => Value == other.Value;

    public override bool Equals(object obj)
        => obj is WeaponLevel other && Equals(other);

    public override int GetHashCode()
        => Value.GetHashCode();

    public static bool operator >(WeaponLevel left, WeaponLevel right)
        => left.Value > right.Value;

    public static bool operator <(WeaponLevel left, WeaponLevel right)
        => left.Value < right.Value;

    public static bool operator >=(WeaponLevel left, WeaponLevel right)
        => left.Value >= right.Value;

    public static bool operator <=(WeaponLevel left, WeaponLevel right)
        => left.Value <= right.Value;

    public static bool operator ==(WeaponLevel left, WeaponLevel right)
        => left.Equals(right);

    public static bool operator !=(WeaponLevel left, WeaponLevel right)
        => !left.Equals(right);
}
