namespace CTF.Application.Players.Vitalities;

public static class HealthArmourExtensions
{
    /// <summary>
    /// Increases the amount of health of a player.
    /// </summary>
    /// <param name="player">The current player.</param>
    /// <param name="amount">The amount of health to be added.</param>
    public static void AddHealth(this Player player, float amount)
    {
        if (amount < 0)
            amount = -amount;

        float total = player.Health + amount;

        if (total > 100)
            player.Health = 100;
        else
            player.Health += amount;
    }

    /// <summary>
    /// Increases the amount of armour of a player.
    /// </summary>
    /// <param name="player">The current player.</param>
    /// <param name="amount">The amount of armour to be added.</param>
    public static void AddArmour(this Player player, float amount)
    {
        if (amount < 0)
            amount = -amount;

        float total = player.Armour + amount;

        if (total > 100)
            player.Armour = 100;
        else
            player.Armour += amount;
    }
}
