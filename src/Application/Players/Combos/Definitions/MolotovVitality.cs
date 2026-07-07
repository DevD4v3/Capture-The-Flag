namespace CTF.Application.Players.Combos.Definitions;

public class MolotovVitality : ICombo
{
    private const int Health = 100;
    private const int Armour = 100;
    private const int MolotovAmmo = 6;

    public string Name => $"{Health} Health, {Armour} Armour and Molotov cocktail";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        player.Health = Health;
        player.Armour = Armour;
        player.GiveWeapon(Weapon.Moltov, MolotovAmmo);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
