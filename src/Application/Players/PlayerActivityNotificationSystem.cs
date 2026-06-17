namespace CTF.Application.Players;

public class PlayerActivityNotificationSystem(
    IDiscordWebhookClient discordWebhookClient) : ISystem
{
    [Event]
    public async Task OnPlayerConnect(Player player)
    {
        var message = new DiscordMessage($"🎮 **{player.Name}** connected to the server.");
        await discordWebhookClient.SendAsync(message);
    }

    [Event]
    public async Task OnPlayerDisconnect(Player player, DisconnectReason _)
    {
        var message = new DiscordMessage($"👋 **{player.Name}** disconnected from the server.");
        await discordWebhookClient.SendAsync(message);
    }
}
