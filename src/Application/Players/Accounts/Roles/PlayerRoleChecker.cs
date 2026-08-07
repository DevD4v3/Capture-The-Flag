namespace CTF.Application.Players.Accounts.Roles;

public class PlayerRoleChecker : IPermissionChecker
{
    public bool HasPermission(Player player, CommandDefinition command)
    {
        string minimumRequiredRoleValue = command.GetTag("role");
        if (minimumRequiredRoleValue is null)
            return true;

        PlayerInfo playerInfo = player.GetRequiredInfo();
        RoleId minimumRequiredRole = Enum.Parse<RoleId>(
            minimumRequiredRoleValue, 
            ignoreCase: true
        );

        if (playerInfo.HasLowerRoleThan(minimumRequiredRole))
        {
            var message = Smart.Format(
                Messages.NoPermissions, 
                new { Role = minimumRequiredRole.ToString() }
            );

            player.SendClientMessage(Color.Red, message);
            return false;
        }

        return true;
    }
}
