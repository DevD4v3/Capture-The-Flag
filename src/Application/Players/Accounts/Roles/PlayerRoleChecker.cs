namespace CTF.Application.Players.Accounts.Roles;

public class PlayerRoleChecker : IPermissionChecker
{
    public bool HasPermission(Player player, CommandDefinition command)
    {
        if (!command.Tags.TryGetValue("role", out var minimumRequiredRoleValue))
            return true;

        PlayerInfo playerInfo = player.GetRequiredInfo();
        RoleId minimumRequiredRole = Enum.Parse<RoleId>(
            minimumRequiredRoleValue, 
            ignoreCase: true
        );

        return !playerInfo.HasLowerRoleThan(minimumRequiredRole);
    }
}
