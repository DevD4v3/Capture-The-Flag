namespace CTF.Application.Players.Accounts.Roles;

public class RoleCollection
{
    private RoleCollection() { }
    private static readonly RoleId[] s_roles = Enum.GetValues<RoleId>();
    public static IReadOnlyList<RoleId> GetAll() => s_roles;
    public static int Count => s_roles.Length;
}
