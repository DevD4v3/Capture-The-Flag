namespace CTF.Application.Players.Accounts.Systems;

public class AccountSystem(
    IPlayerRepository playerRepository,
    LoginDialogViewer loginDialogViewer,
    SignupDialogViewer signupDialogViewer) : ISystem
{
    [Event]
    public async Task OnPlayerConnect(Player connectedPlayer)
    {
        AddDefaultAccount(connectedPlayer);
        PlayerInfo playerInfo = playerRepository.GetOrDefault(connectedPlayer.Name);
        if(playerInfo is null)
        {
            await signupDialogViewer.View(connectedPlayer);
            return;
        }
        await loginDialogViewer.View(connectedPlayer, playerInfo);
    }

    private static void AddDefaultAccount(Player connectedPlayer)
    {
        var playerInfo = new PlayerInfo();
        bool isAuthenticated = false;
        playerInfo.SetName(connectedPlayer.Name);
        connectedPlayer.AddComponent<AccountComponent>(playerInfo, isAuthenticated);
    }
}
