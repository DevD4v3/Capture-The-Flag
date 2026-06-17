namespace CTF.Application.Players;

public class PlayerActivityNotificationSystem(
    IDiscordWebhookClient discordWebhookClient) : ISystem
{
    [Event]
    public async Task OnPlayerConnect(Player player)
    {
        var content = Smart.Format(Messages.PlayerConnected, new { player.Name });
        await discordWebhookClient.SendAsync(new DiscordMessage(content));
    }

    [Event]
    public async Task OnPlayerDisconnect(Player player, DisconnectReason _)
    {
        var content = Smart.Format(Messages.PlayerDisconnected, new { player.Name });
        await discordWebhookClient.SendAsync(new DiscordMessage(content));
    }
}
