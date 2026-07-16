namespace CTF.Application.Players.Vitalities;

public class ArmourSystem(
    IWorldService worldService,
    IEntityManager entityManager,
    UnixTimeSeconds unixTimeSeconds,
    CommandCooldowns commandCooldowns) : ISystem
{
    [PlayerCommand("addarmour")]
    [RequiresMinimumRole(RoleId.Moderator)]
    public void AddArmourToPlayer(
        Player currentPlayer,
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        float amount)
    {
        Result<Vitality> result = Vitality.Create(amount);
        if (result.IsFailed)
        {
            currentPlayer.SendClientMessage(Color.Red, result.Message);
            return;
        }

        {
            var message = Smart.Format(Messages.AddArmourToPlayer, new
            {
                PlayerName = targetPlayer.Name,
                Armour = amount
            });
            currentPlayer.SendClientMessage(Color.Yellow, message);
        }

        {
            var message = Smart.Format(Messages.ReceiveArmourFromPlayer, new
            {
                PlayerName = currentPlayer.Name,
                Armour = amount
            });
            targetPlayer.SendClientMessage(Color.Yellow, message);
            targetPlayer.AddArmour(amount);
        }
    }

    [PlayerCommand("addallarmour")]
    [RequiresMinimumRole(RoleId.Moderator)]
    public void AddArmourToAllPlayers(Player currentPlayer, float amount)
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
            targetPlayer.AddArmour(amount);
        }

        var message = Smart.Format(Messages.AddArmourToAllPlayers, new
        {
            PlayerName = currentPlayer.Name,
            Armour = amount
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }

    [PlayerCommand("armour")]
    [RequiresMinimumRole(RoleId.VIP)]
    public void RestoreArmour(Player currentPlayer)
    {
        var waitTimeComponent = currentPlayer.GetComponent<WaitTimeComponent>();
        if (waitTimeComponent.Value > unixTimeSeconds.Value)
        {
            var message = Smart.Format(Messages.TimeRequiredToReuseCommand, new 
            { 
                Minutes = commandCooldowns.Armour
            });
            currentPlayer.SendClientMessage(Color.Red, message);
            return;
        }

        static int ConvertMinutesToSeconds(int value) => value * 60;
        int seconds = ConvertMinutesToSeconds(commandCooldowns.Armour);
        waitTimeComponent.Value = unixTimeSeconds.Value + seconds;
        currentPlayer.Armour = 100;
    }

    [Event]
    public void OnPlayerConnect(Player player)
        => player.AddComponent<WaitTimeComponent>();

    private class WaitTimeComponent : Component
    {
        public long Value { get; set; }
    }
}
