namespace CTF.Application.Players.Combos.Definitions;

public class TearGasVitality : ICombo
{
    private const int Health = 100;
    private const int Armour = 100;
    private const int TearGasAmmo = 30;

    public string Name => $"{Health} Health, {Armour} Armour and Tear gas";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        player.Health = Health;
        player.Armour = Armour;
        player.GiveWeapon(Weapon.Teargas, TearGasAmmo);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
