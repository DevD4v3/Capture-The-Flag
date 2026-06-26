namespace CTF.Application.Players.Combos.Definitions;

public class GrenadesVitality : ICombo
{
    public string Name => "100 Health, 100 Armour and Grenades";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        player.Health = 100;
        player.Armour = 100;
        player.GiveWeapon(Weapon.Grenade, ammo: 6);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
