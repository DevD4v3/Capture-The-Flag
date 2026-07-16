namespace CTF.Application.Players.Accounts.Roles;

[AttributeUsage(AttributeTargets.Method)]
public class RequiresRoleAttribute(RoleId role) : CommandTagAttribute("role", role.ToString());
