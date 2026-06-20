namespace CTF.Application.Players.Combos.Definitions;

public class FlamethrowerVitality : ICombo
{
    public string Name => "100 Health, 100 Armour and FlameThrower";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetInfo();
        player.Health = 100;
        player.Armour = 100;
        // 1000 (shows in game as 50-50).
        const int ammo = 1000;
        player.GiveWeapon(Weapon.FlameThrower, ammo);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
