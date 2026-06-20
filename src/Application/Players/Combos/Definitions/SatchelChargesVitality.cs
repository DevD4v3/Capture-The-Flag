namespace CTF.Application.Players.Combos.Definitions;

public class SatchelChargesVitality : ICombo
{
    public string Name => "100 Health, 100 Armour and Satchel charges";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetInfo();
        player.Health = 100;
        player.Armour = 100;
        player.GiveWeapon(Weapon.SatchelCharge, ammo: 6);
        player.GiveWeapon(Weapon.Detonator, ammo: 1);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
