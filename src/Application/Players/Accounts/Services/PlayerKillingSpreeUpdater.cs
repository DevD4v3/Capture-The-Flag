namespace CTF.Application.Players.Accounts.Services;

public class PlayerKillingSpreeUpdater(
    IWorldService worldService,
    IPlayerRepository playerRepository,
    IGunGameMode gunGameMode)
{
    private const int MinimumKillingSpree = 2;
    private const int EarnedCoins = 20;
    private const int EarnedHealth = 10;
    private const int ConsecutiveKillsBonusHealth = 40;

    public void Update(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        playerInfo.StatsPerRound.AddKillingSpree();
        int currentKillingSpree = playerInfo.StatsPerRound.KillingSpree;

        if (currentKillingSpree < MinimumKillingSpree)
            return;

        if (playerInfo.HasSurpassedMaxKillingSpree())
        {
            playerInfo.SetMaxKillingSpree(currentKillingSpree);
            playerRepository.UpdateMaxKillingSpree(playerInfo);
        }

        if (gunGameMode.IsEnabled)
            return;

        player.GameText($"KILL X{currentKillingSpree}", TimeSpan.FromSeconds(3), GameTextStyle.Style3);
        playerInfo.StatsPerRound.AddCoins(EarnedCoins);
        player.AddHealth(EarnedHealth);

        if (currentKillingSpree % 3 == 0)
        {
            var message = Smart.Format(Messages.ConsecutiveKills, new
            {
                PlayerName = player.Name,
                Kills = currentKillingSpree
            });

            // Sample Message:
            // Dave has had 3 consecutive kills without dying.
            worldService.SendClientMessage(Color.Orange, message);
            player.AddHealth(ConsecutiveKillsBonusHealth);
        }
    }
}
