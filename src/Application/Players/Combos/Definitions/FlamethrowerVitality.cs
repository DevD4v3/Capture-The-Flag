namespace CTF.Application.Players.Combos.Definitions;

public class FlamethrowerVitality : ICombo
{
    private const int Health = 100;
    private const int Armour = 100;
    // 1000 (shows in game as 50-50).
    private const int FlamethrowerAmmo = 1000;

    public string Name => $"{Health} Health, {Armour} Armour and FlameThrower";
    public int RequiredCoins => 100;

    public Result Give(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        player.Health = Health;
        player.Armour = Armour;
        player.GiveWeapon(Weapon.FlameThrower, FlamethrowerAmmo);
        playerInfo.StatsPerRound.ResetCoins();
        return Result.Success();
    }
}
