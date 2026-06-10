namespace CTF.Application.Players.Accounts.Services;

public class KillingSpreeUpdater(
    IWorldService worldService,
    IPlayerRepository playerRepository)
{
    public void Update(Player player, PlayerInfo playerInfo)
    {
        playerInfo.StatsPerRound.AddKillingSpree();
        int currentKillingSpree = playerInfo.StatsPerRound.KillingSpree;
        if (currentKillingSpree >= 2)
        {
            player.GameText($"KILL X{currentKillingSpree}", TimeSpan.FromSeconds(3), GameTextStyle.Style3);
            const int earnedCoins = 20;
            playerInfo.StatsPerRound.AddCoins(earnedCoins);
            player.AddHealth(10);

            if (playerInfo.HasSurpassedMaxKillingSpree())
            {
                playerInfo.SetMaxKillingSpree(currentKillingSpree);
                playerRepository.UpdateMaxKillingSpree(playerInfo);
            }

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
                player.AddHealth(40);
            }
        }
    }
}
