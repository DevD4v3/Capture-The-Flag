namespace CTF.Application.GunGames;

/// <summary>
/// Represents a player's progression in the GunGame mode.
/// </summary>
public class PlayerProgression : Component
{
    public WeaponLevel WeaponLevel { get; private set; } = WeaponLevel.First;
    public int KillsTowardsNextLevel { get; private set; }

    public void AddKillsTowardsNextLevel()
        => KillsTowardsNextLevel++;

    public bool CanLevelUp(KillsRequiredPerLevel requiredKills)
        => KillsTowardsNextLevel >= requiredKills.Value;

    public void LevelUp(MaxWeaponLevel maxLevel)
    {
        WeaponLevel = WeaponLevel.Next(maxLevel);
        KillsTowardsNextLevel = 0;
    }

    public void LevelDown()
    {
        WeaponLevel = WeaponLevel.Previous();
        KillsTowardsNextLevel = 0;
    }

    public void Reset()
    {
        WeaponLevel = WeaponLevel.First;
        KillsTowardsNextLevel = 0;
    }
}
