namespace CTF.Application.Players.Chats;

public class PrivateMessageSystem(IEntityManager entityManager) : ISystem
{
    [PlayerCommand("pm")]
    public void SendMessageToPlayer(
        Player sender,
        [CommandParameter(Name = "playerId")]Player receiver,
        string message)
    {
        if (sender == receiver)
        {
            sender.SendClientMessage(Color.Red, Messages.PlayerIsEqualsToTargetPlayer);
            return;
        }

        var privateMessageComponent = receiver.GetComponent<PrivateMessageComponent>();
        if (privateMessageComponent.IsBlocked) 
        {
            sender.SendClientMessage(Color.Red, Messages.PrivateMessagesBlocked);
            return;
        }

        int senderId = sender.Id;
        int receiverId = receiver.Id;
        sender.SendClientMessage(Color.Yellow, $"PM to {receiver.Name}({receiverId}): {message}");
        sender.PlaySound(1058);
        receiver.SendClientMessage(Color.Yellow, $"PM from {sender.Name}({senderId}): {message}");
        receiver.PlaySound(1058);

        // Send private message to the STAFF.
        var players = entityManager.GetComponents<Player>();
        foreach (Player player in players)
        {
            PlayerInfo playerInfo = player.GetRequiredInfo();
            if (playerInfo.HasLowerRoleThan(RoleId.Moderator))
                continue;

            // This prevents double messaging.
            if (player == sender || player == receiver)
                continue;

            var messageForStaff = $"[PM] {sender.Name} writes to {receiver.Name}: {message}";
            player.SendClientMessage(Color.Yellow, messageForStaff);
        }
    }

    [PlayerCommand("blockpm")]
    public void Block(Player player)
    {
        var privateMessageComponent = player.GetComponent<PrivateMessageComponent>();
        privateMessageComponent.IsBlocked = true;
        player.SendClientMessage(Color.Yellow, Messages.PrivateMessagesDisabled);
        player.PlaySound(1139);
    }

    [PlayerCommand("unblockpm")]
    public void Unblock(Player player)
    {
        var privateMessageComponent = player.GetComponent<PrivateMessageComponent>();
        privateMessageComponent.IsBlocked = false;
        player.SendClientMessage(Color.Yellow, Messages.PrivateMessagesEnabled);
        player.PlaySound(1139);
    }

    [Event]
    public void OnPlayerConnect(Player player) 
    {
        player.AddComponent<PrivateMessageComponent>();
    }

    private class PrivateMessageComponent : Component
    {
        public bool IsBlocked { get; set; }
    }
}
