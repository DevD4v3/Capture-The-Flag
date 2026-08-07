namespace CTF.Application.GunGames.WeaponProgressions;

/// <summary>
/// Represents the active weapon progression for the current GunGame session.
/// </summary>
/// <remarks>
/// Consumers do not need to know which weapon progression is active.
/// This class always exposes the progression selected for the current session.
/// </remarks>
public class ActiveWeaponProgression(
    GunGameSession gunGameSession,
    FrozenDictionary<WeaponProgressionType, WeaponProgression> progressions)
{
    private WeaponProgression Current
        => progressions[gunGameSession.WeaponProgressionType];

    /// <inheritdoc cref="WeaponProgression.GetWeapon"/>
    public IWeapon GetWeapon(WeaponLevel level)
        => Current.GetWeapon(level);

    /// <inheritdoc cref="WeaponProgression.IsFinalLevel"/>
    public bool IsFinalLevel(WeaponLevel level)
        => Current.IsFinalLevel(level);
    
    /// <inheritdoc cref="WeaponProgression.MaxLevel"/>
    public MaxWeaponLevel MaxLevel 
        => Current.MaxLevel;
}
