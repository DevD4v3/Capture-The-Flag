using System.Net.Http.Json;

namespace CTF.Host.Services;

public sealed class DiscordWebhookClient : IDiscordWebhookClient
{
    private readonly ILogger<DiscordWebhookClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _discordWebhookUrl;

    private record DiscordWebhookPayload(string Content);

    public DiscordWebhookClient(
        HttpClient httpClient,
        ILogger<DiscordWebhookClient> logger)
    {
        var envReader = new EnvReader();
        if (!envReader.TryGetStringValue("DISCORD_WEBHOOK_URL", out var webhookUrl))
        {
            logger.LogWarning("'DISCORD_WEBHOOK_URL' has not been set as an environment variable");
        }

        _discordWebhookUrl = webhookUrl ?? string.Empty;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<bool> SendAsync(DiscordMessage message)
    {
        if (string.IsNullOrWhiteSpace(_discordWebhookUrl))
        {
            _logger.LogWarning("Discord webhook URL is not configured.");
            return false;
        }

        try
        {
            var payload = new DiscordWebhookPayload(message.Content);
            var response = await _httpClient.PostAsJsonAsync(_discordWebhookUrl, payload);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex) when (
            ex is TaskCanceledException or OperationCanceledException)
        {
            _logger.LogError(ex, "Discord webhook request timed out.");
            return false;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Discord webhook request failed.");
            return false;
        }
    }
}
