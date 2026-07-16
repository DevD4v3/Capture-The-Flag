namespace CTF.Application.Players.Accounts.Statistics;

public class PlayerCoinsSystem(
    IEntityManager entityManager,
    IWorldService worldService,
    PlayerStatsRenderer playerStatsRenderer,
    UnixTimeSeconds unixTimeSeconds,
    CommandCooldowns commandCooldowns) : ISystem
{
    [PlayerCommand("addcoins")]
    [RequiresRole(RoleId.Admin)]
    public void AddCoinsToPlayer(
        Player currentPlayer,
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        int coins)
    {
        PlayerInfo targetPlayerInfo = targetPlayer.GetRequiredInfo();
        Result result = targetPlayerInfo.StatsPerRound.AddCoins(coins);
        if (result.IsFailed)
        {
            currentPlayer.SendClientMessage(Color.Red, result.Message);
            return;
        }

        {
            var message = Smart.Format(Messages.AddCoinsToPlayer, new
            {
                Coins = coins,
                PlayerName = targetPlayer.Name
            });
            currentPlayer.SendClientMessage(Color.Yellow, message);
        }
        {
            var message = Smart.Format(Messages.ReceiveCoinsFromPlayer, new
            {
                Coins = coins,
                PlayerName = currentPlayer.Name
            });
            targetPlayer.SendClientMessage(Color.Yellow, message);
        }
        playerStatsRenderer.UpdateTextDraw(targetPlayer);
    }

    [PlayerCommand("addallcoins")]
    [RequiresRole(RoleId.Admin)]
    public void AddCoinsToAllPlayers(Player currentPlayer, int coins)
    {
        IEnumerable<Player> players = entityManager.GetComponents<Player>();
        foreach (Player targetPlayer in players)
        {
            PlayerInfo targetPlayerInfo = targetPlayer.GetRequiredInfo();
            Result result = targetPlayerInfo.StatsPerRound.AddCoins(coins);
            if (result.IsFailed)
            {
                currentPlayer.SendClientMessage(Color.Red, result.Message);
                return;
            }
            playerStatsRenderer.UpdateTextDraw(targetPlayer);
        }

        var message = Smart.Format(Messages.AddCoinsToAllPlayers, new
        {
            PlayerName = currentPlayer.Name,
            Coins = coins
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }

    [PlayerCommand("givemecoins")]
    [RequiresRole(RoleId.VIP)]
    public void GiveMeCoins(Player currentPlayer) 
    {
        var waitTimeComponent = currentPlayer.GetComponent<WaitTimeComponent>();
        if (waitTimeComponent.Value > unixTimeSeconds.Value)
        {
            var message = Smart.Format(Messages.TimeRequiredToReuseCommand, new 
            { 
                Minutes = commandCooldowns.Coins
            });
            currentPlayer.SendClientMessage(Color.Red, message);
            return;
        }

        static int ConvertMinutesToSeconds(int value) => value * 60;
        int seconds = ConvertMinutesToSeconds(commandCooldowns.Coins);
        waitTimeComponent.Value = unixTimeSeconds.Value + seconds;
        PlayerInfo currentPlayerInfo = currentPlayer.GetRequiredInfo();
        currentPlayerInfo.StatsPerRound.AddCoins(100);
        playerStatsRenderer.UpdateTextDraw(currentPlayer);
        currentPlayer.SendClientMessage(Color.Yellow, Messages.GiveMeCoins);
    }

    [Event]
    public void OnPlayerConnect(Player player)
        => player.AddComponent<WaitTimeComponent>();

    private class WaitTimeComponent : Component
    {
        public long Value { get; set; }
    }
}