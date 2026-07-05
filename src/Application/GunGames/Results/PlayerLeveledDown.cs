namespace CTF.Application.GunGames.Results;

/// <summary>
/// Handles the <see cref="GunGameResult.LeveledDown"/> result.
/// </summary>
public class PlayerLeveledDown(
    IWorldService worldService,
    WeaponProgression weaponProgression) : IGunGameResultHandler
{
    public GunGameResult Result => GunGameResult.LeveledDown;

    public void Handle(KillContext context)
    {
        var victimProgression = context.Victim.GetComponent<PlayerProgression>();
        IWeapon newWeapon = weaponProgression.GetWeapon(victimProgression.WeaponLevel);
        context.Victim.ResetWeapons(); 
        context.Victim.GiveWeapon(Weapon.Knife, 1); 
        context.Victim.GiveWeapon(newWeapon.Id, IWeapon.UnlimitedAmmo);

        var message = Smart.Format(GunGameMessages.PlayerLeveledDown, new
        {
            Killer = context.Killer.Name,
            Victim = context.Victim.Name,
            Level  = victimProgression.WeaponLevel,
            Weapon = newWeapon.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }
}
