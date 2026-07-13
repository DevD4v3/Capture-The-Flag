namespace CTF.Application.Players.Accounts.Roles;

public class VIPListSystem(
    IEntityManager entityManager,
    IDialogService dialogService) : ISystem
{
    [PlayerCommand("vips")]
    public void Show(Player player)
    {
        List<PlayerInfo> vips = entityManager
            .GetComponents<Player>()
            .Select(player => player.GetRequiredInfo())
            .Where(info => info.RoleId >= RoleId.VIP)
            .ToList();

        if (vips.Count == 0)
        {
            player.SendClientMessage(Color.Red, Messages.NoVIPsConnected);
            return;
        }

        var content = new StringBuilder();
        Color vipColor = Color.Yellow;

        foreach (PlayerInfo vip in vips)
        {
            content.AppendLine($"{vipColor}[VIP] {Color.White}{vip.Name}");
        }

        var dialog = new MessageDialog(
            caption: $"VIPs: {vips.Count}",
            content: content.ToString(),
            button1: "Close"
        );

        dialogService.ShowAsync(player, dialog);
    }
}
