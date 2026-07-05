namespace CTF.Application.GunGames;

/// <summary>
/// Processes a player kill according to the GunGame rules.
/// </summary>
/// <remarks>
/// This class encapsulates the core GunGame progression logic, such as
/// leveling up, leveling down, reaching the final level, and scoring
/// the final kill.
/// </remarks>
public readonly struct GunGame(
    WeaponProgression weaponProgression,
    KillsRequiredPerLevel killsRequiredPerLevel)
{

    /// <summary>
    /// Processes a kill and returns the resulting GunGame result.
    /// </summary>
    /// <param name="killer">
    /// The player who performed the kill.
    /// </param>
    /// <param name="victim">
    /// The player who was killed.
    /// </param>
    /// <param name="reason">
    /// The weapon used to perform the kill.
    /// </param>
    /// <returns>
    /// The result produced after applying the GunGame rules.
    /// </returns>
    public GunGameResult ProcessKill(
        PlayerProgression killer,
        PlayerProgression victim,
        Weapon reason)
    {
        if (reason == Weapon.Knife)
        {
            if (killer.WeaponLevel.IsMax(weaponProgression.MaxLevel))
                return GunGameResult.ScoredFinalKill;

            if (victim.WeaponLevel == WeaponLevel.First)
                return GunGameResult.None;

            victim.LevelDown();
            return GunGameResult.LeveledDown;
        }

        IWeapon expectedWeapon = weaponProgression.GetWeapon(killer.WeaponLevel);
        if (expectedWeapon.Id != reason)
            return GunGameResult.None;

        if (killer.WeaponLevel.IsMax(weaponProgression.MaxLevel))
            return GunGameResult.ScoredFinalKill;

        killer.AddKillsTowardsNextLevel();

        if (!killer.CanLevelUp(killsRequiredPerLevel))
            return GunGameResult.None;

        killer.LevelUp(weaponProgression.MaxLevel);

        return killer.WeaponLevel.IsMax(weaponProgression.MaxLevel) ? 
            GunGameResult.ReachedFinalLevel :
            GunGameResult.LeveledUp;
    }
}
