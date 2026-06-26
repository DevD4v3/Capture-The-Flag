namespace CTF.Application.Players.Accounts.Systems;

public class AccountSystem(
    IPlayerRepository playerRepository,
    LoginDialogViewer loginDialogViewer,
    SignupDialogViewer signupDialogViewer) : ISystem
{
    [Event]
    public async Task OnPlayerConnect(Player connectedPlayer)
    {
        AttachAccountComponent(connectedPlayer);
        PlayerInfo playerInfo = playerRepository.GetOrDefault(connectedPlayer.Name);
        if(playerInfo is null)
        {
            await signupDialogViewer.View(connectedPlayer);
            return;
        }
        await loginDialogViewer.View(connectedPlayer, playerInfo);
    }

    private static void AttachAccountComponent(Player player)
    {
        var playerInfo = new PlayerInfo();
        bool isAuthenticated = false;
        playerInfo.SetName(player.Name);
        player.AddComponent<AccountComponent>(playerInfo, isAuthenticated);
    }
}
