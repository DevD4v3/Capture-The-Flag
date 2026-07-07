namespace CTF.Application.Players.Combos.Definitions;

public class SatchelChargesVitality : ICombo
{
    private const int Health = 100;
    private const int Armour = 100;
    private const int SatchelAmmo = 6;

    public string Name => $"{Health} Health, {Armour} Armour and Satchel charges";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        player.Health = Health;
        player.Armour = Armour;
        player.GiveWeapon(Weapon.SatchelCharge, SatchelAmmo);
        player.GiveWeapon(Weapon.Detonator, 1);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
