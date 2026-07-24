namespace CTF.Application.Players.Accounts.Statistics;

public class PlayerStatsPerRound
{
    public int Kills { get; private set; }
    public int Deaths { get; private set; }
    public int KillingSpree { get; private set; }
    public int Coins { get; private set; }

    public void AddKills() => Kills++;
    public void AddDeaths() => Deaths++;
    public void AddKillingSpree() => KillingSpree++;
    public bool HasSufficientCoins(int amount) => Coins >= amount;
    public bool HasInsufficientCoins(int amount) => !HasSufficientCoins(amount);

    public Result AddCoins(int value)
    {
        if (value < 1 || value > 100)
            return Result.Failure(Messages.InvalidAddCoins);

        Coins += value;
        if (Coins > 100) 
            Coins = 100;

        return Result.Success();
    }

    public Result SubtractCoins(int value)
    {
        if (value < -100 || value > -1)
            return Result.Failure(Messages.InvalidSubtractCoins);

        Coins -= -value;
        if (Coins < 0) 
            Coins = 0;

        return Result.Success();
    }

    public void ResetCoins() => Coins = 0;
    public void ResetKills()  => Kills = 0;
    public void ResetDeaths() => Deaths = 0;
    public void ResetKillingSpree() => KillingSpree = 0;
    public void ResetStats()
    {
        Kills = 0;
        Deaths = 0;
        KillingSpree = 0;
        Coins = 0;
    }
}
