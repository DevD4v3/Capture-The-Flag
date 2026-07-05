namespace CTF.Application.GunGames.Results;

/// <summary>
/// Handles the <see cref="GunGameResult.LeveledUp"/> result.
/// </summary>
public class PlayerLeveledUp(
    IWorldService worldService,
    WeaponProgression weaponProgression) : IGunGameResultHandler
{
    public GunGameResult Result => GunGameResult.LeveledUp;

    public void Handle(KillContext context)
    {
        var killerProgression = context.Killer.GetComponent<PlayerProgression>();
        IWeapon newWeapon = weaponProgression.GetWeapon(killerProgression.WeaponLevel);
        context.Killer.RemoveWeapon(context.Reason);
        context.Killer.GiveWeapon(newWeapon.Id, IWeapon.UnlimitedAmmo);

        var message = Smart.Format(GunGameMessages.PlayerLeveledUp, new
        {
            Killer = context.Killer.Name,
            Level  = killerProgression.WeaponLevel,
            Weapon = newWeapon.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }
}
