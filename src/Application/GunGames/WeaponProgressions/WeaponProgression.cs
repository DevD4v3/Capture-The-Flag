namespace CTF.Application.GunGames.WeaponProgressions;

/// <summary>
/// Represents the weapon progression used by the current GunGame session.
/// </summary>
/// <remarks>
/// Consumers do not need to know which progression implementation is active.
/// The active progression is resolved automatically from the current GunGame session.
/// </remarks>
public class WeaponProgression(
    GunGameSession gunGameSession,
    IDictionary<WeaponProgressionType, WeaponProgressionBase> progressions)
{
    private WeaponProgressionBase Current
        => progressions[gunGameSession.WeaponProgressionType];

    /// <inheritdoc cref="WeaponProgressionBase.GetWeapon"/>
    public IWeapon GetWeapon(WeaponLevel level)
        => Current.GetWeapon(level);

    /// <inheritdoc cref="WeaponProgressionBase.IsFinalLevel"/>
    public bool IsFinalLevel(WeaponLevel level)
        => Current.IsFinalLevel(level);
    
    /// <inheritdoc cref="WeaponProgressionBase.MaxLevel"/>
    public MaxWeaponLevel MaxLevel 
        => Current.MaxLevel;
}
