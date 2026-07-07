namespace CTF.Application.Players.Combos.Definitions;

public class RocketLauncherVitality(ComboSettings comboSettings) : ICombo
{
    private const int Health = 100;
    private const int RocketLauncherAmmo = 2;

    public string Name => $"{Health} Health and Rocket launcher(RPG)";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        if (comboSettings.IsRocketLauncherDisabled)
        {
            player.SendClientMessage(Color.Red, Messages.RocketLauncherDisabled);
            return Result.Failure();
        }

        PlayerInfo playerInfo = player.GetRequiredInfo();
        player.Health = Health;
        player.GiveWeapon(Weapon.RocketLauncher, RocketLauncherAmmo);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
