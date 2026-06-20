namespace CTF.Application.Players.Combos.Definitions;

public class TearGasVitality : ICombo
{
    public string Name => "100 Health, 100 Armour and Tear gas";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetInfo();
        player.Health = 100;
        player.Armour = 100;
        player.GiveWeapon(Weapon.Teargas, ammo: 30);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
