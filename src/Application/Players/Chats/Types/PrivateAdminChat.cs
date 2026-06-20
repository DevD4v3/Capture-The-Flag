namespace CTF.Application.Players.Chats.Types;

public class PrivateAdminChat(IEntityManager entityManager) : IChatMessage
{
    public char Id => '#';
    public bool SendToAllPlayers(PlayerInfo sender, string message)
    {
        if (sender.HasLowerRoleThan(RoleId.Admin))
            return false;

        var players = entityManager.GetComponents<Player>();
        foreach (Player player in players)
        {
            if (player.IsInClassSelection())
                continue;

            PlayerInfo playerInfo = player.GetInfo();
            if (playerInfo.HasLowerRoleThan(RoleId.Admin))
                continue;

            player.SendClientMessage(new Color(0x33FF33AA), $"[Admin Chat] {sender.Name}: {message}");
        }
        return true;
    }
}
