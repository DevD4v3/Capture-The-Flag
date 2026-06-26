namespace CTF.Application.Players.Accounts.Systems;

public class AccountSystem(
    IPlayerRepository playerRepository,
    LoginDialogViewer loginDialogViewer,
    SignupDialogViewer signupDialogViewer) : ISystem
{
    [Event]
    public async Task OnPlayerConnect(Player player)
    {
        PlayerInfo playerInfo = playerRepository.GetOrDefault(player.Name);

        if (playerInfo is null)
        {
            playerInfo = CreatePlayerInfo(player.Name);
            player.AddComponent<AccountComponent>(playerInfo);
            await signupDialogViewer.View(player);
            return;
        }

        player.AddComponent<AccountComponent>(playerInfo);
        await loginDialogViewer.View(player);
    }

    private static PlayerInfo CreatePlayerInfo(string name)
    {
        var playerInfo = new PlayerInfo();
        playerInfo.SetName(name);
        return playerInfo;
    }
}
