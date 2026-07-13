namespace CTF.Application.Players.Accounts.Roles;

public class AdminListSystem(
    IDialogService dialogService,
    IEntityManager entityManager) : ISystem
{
    [PlayerCommand("admins")]
    public void Show(Player player)
    {
        List<PlayerInfo> admins = entityManager
            .GetComponents<Player>()
            .Select(player => player.GetRequiredInfo())
            .Where(info => info.RoleId >= RoleId.Moderator)
            .OrderByDescending(info => info.RoleId)
            .ToList();

        if (admins.Count == 0)
        {
            player.SendClientMessage(Color.Red, Messages.NoAdminsConnected);
            return;
        }

        var content = new StringBuilder();

        foreach (PlayerInfo admin in admins)
        {
            Color color = admin.RoleId switch
            {
                >= RoleId.Admin => Color.Red,
                >= RoleId.Moderator => Color.LightGreen,
                _ => Color.White
            };

            content.AppendLine($"{color}[{admin.RoleId}] {Color.White}{admin.Name}");
        }

        var dialog = new MessageDialog(
            caption: $"Admins: {admins.Count}",
            content: content.ToString(),
            button1: "Close"
        );

        dialogService.ShowAsync(player, dialog);
    }
}
