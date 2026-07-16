namespace CTF.Application.Players.Accounts.Roles;

public class PlayerRoleChecker : IPermissionChecker
{
    public bool HasPermission(Player player, CommandDefinition command)
    {
        if (!command.Tags.TryGetValue("role", out var value))
            return true;

        PlayerInfo playerInfo = player.GetRequiredInfo();
        RoleId role = Enum.Parse<RoleId>(value, ignoreCase: true);

        if (playerInfo.HasLowerRoleThan(role))
            return false;

        return true;
    }
}
