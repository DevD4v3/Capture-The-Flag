namespace CTF.Host;

public class GameModeInit(
    IServerService serverService,
    ServerSettings serverSettings) : ISystem
{
    [Event]
    public void OnGameModeInit()
    {
        Console.WriteLine("\n----------------------------------");
        Console.WriteLine("       Red vs Blue");
        Console.WriteLine("    Capture the Flag");
        Console.WriteLine("----------------------------------\n");

        serverService.SetServerName(serverSettings.HostName);
        serverService.SetLanguage(serverSettings.LanguageText);
        serverService.SetWebsiteUrl(serverSettings.WebUrl);
        serverService.SetGameModeText(serverSettings.GameModeText);
        serverService.UsePlayerPedAnims();
        serverService.DisableInteriorEnterExits();
    }
}
