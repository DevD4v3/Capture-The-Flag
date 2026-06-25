namespace CTF.Application.Players.Chats.Types;

public class PrivateVipChat(IEntityManager entityManager) : IChatMessage
{
    public char Id => '$';
    public bool SendToAllPlayers(PlayerInfo sender, string message)
    {
        if (sender.HasLowerRoleThan(RoleId.VIP))
            return false;

        var players = entityManager.GetComponents<Player>();
        foreach (Player player in players)
        {
            if (player.IsInClassSelection())
                continue;

            PlayerInfo playerInfo = player.GetRequiredInfo();
            if (playerInfo.HasLowerRoleThan(RoleId.VIP))
                continue;

            player.SendClientMessage($"{{8b0000}}[Vip Chat] {sender.Name}: {message}");
        }
        return true;
    }
}
