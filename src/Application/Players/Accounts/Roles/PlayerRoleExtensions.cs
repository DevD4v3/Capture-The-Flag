namespace CTF.Application.Players.Accounts.Roles;

public static class PlayerRoleExtensions
{
    public static bool HasLowerRoleThan(this Player player, RoleId id)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();

        if (playerInfo.HasLowerRoleThan(id))
        {
            player.SendClientMessage(Color.Red, Messages.NoPermissions);
            return true;
        }

        return false;
    }
}
