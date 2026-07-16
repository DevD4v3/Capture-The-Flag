namespace CTF.Application.Players.Vitalities;

public class HealthSystem(
    IWorldService worldService,
    IEntityManager entityManager,
    UnixTimeSeconds unixTimeSeconds,
    CommandCooldowns commandCooldowns) : ISystem
{
    [PlayerCommand("addhealth")]
    [RequiresRole(RoleId.Moderator)]
    public void AddHealthToPlayer(
        Player currentPlayer,
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        float amount)
    {
        Result<Vitality> result = Vitality.Create(amount);
        if(result.IsFailed)
        {
            currentPlayer.SendClientMessage(Color.Red, result.Message);
            return;
        }

        {
            var message = Smart.Format(Messages.AddHealthToPlayer, new
            {
                PlayerName = targetPlayer.Name,
                Health = amount
            });
            currentPlayer.SendClientMessage(Color.Yellow, message);
        }

        {
            var message = Smart.Format(Messages.ReceiveHealthFromPlayer, new
            {
                PlayerName = currentPlayer.Name,
                Health = amount
            });
            targetPlayer.SendClientMessage(Color.Yellow, message);
            targetPlayer.AddHealth(amount);
        }
    }

    [PlayerCommand("addallhealth")]
    [RequiresRole(RoleId.Moderator)]
    public void AddHealthToAllPlayers(Player currentPlayer, float amount)
    {
        Result<Vitality> result = Vitality.Create(amount);
        if (result.IsFailed)
        {
            currentPlayer.SendClientMessage(Color.Red, result.Message);
            return;
        }

        IEnumerable<Player> players = entityManager.GetComponents<Player>();
        foreach (Player targetPlayer in players) 
        { 
            targetPlayer.AddHealth(amount);
        }

        var message = Smart.Format(Messages.AddHealthToAllPlayers, new
        {
            PlayerName = currentPlayer.Name,
            Health = amount
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }

    [PlayerCommand("health")]
    [RequiresRole(RoleId.VIP)]
    public void RestoreHealth(Player currentPlayer)
    {
        var waitTimeComponent = currentPlayer.GetComponent<WaitTimeComponent>();
        if (waitTimeComponent.Value > unixTimeSeconds.Value)
        {
            var message = Smart.Format(Messages.TimeRequiredToReuseCommand, new 
            { 
                Minutes = commandCooldowns.Health
            });
            currentPlayer.SendClientMessage(Color.Red, message);
            return;
        }

        static int ConvertMinutesToSeconds(int value) => value * 60;
        int seconds = ConvertMinutesToSeconds(commandCooldowns.Health);
        waitTimeComponent.Value = unixTimeSeconds.Value + seconds;
        currentPlayer.Health = 100;
    }

    [Event]
    public void OnPlayerConnect(Player player)
        => player.AddComponent<WaitTimeComponent>();

    private class WaitTimeComponent : Component
    {
        public long Value { get; set; }
    }
}
