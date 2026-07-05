namespace CTF.Application.GunGames.Results;

/// <summary>
/// Handles the <see cref="GunGameResult.ReachedFinalLevel"/> result.
/// </summary>
public class PlayerReachedFinalLevel(
    IWorldService worldService,
    WeaponProgression weaponProgression) : IGunGameResultHandler
{
    public GunGameResult Result => GunGameResult.ReachedFinalLevel;
    public void Handle(KillContext context)
    {
        var killerProgression = context.Killer.GetComponent<PlayerProgression>();
        IWeapon newWeapon = weaponProgression.GetWeapon(killerProgression.WeaponLevel);
        context.Killer.RemoveWeapon(context.Reason);
        context.Killer.GiveWeapon(newWeapon.Id, IWeapon.UnlimitedAmmo);

        var message = Smart.Format(GunGameMessages.PlayerReachedFinalLevel, new
        {
            Killer = context.Killer.Name,
            Weapon = newWeapon.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }
}
