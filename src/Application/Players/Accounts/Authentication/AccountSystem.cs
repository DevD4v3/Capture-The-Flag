namespace CTF.Application.Players.Accounts.Authentication;

public class AccountSystem(
    IPlayerRepository playerRepository,
    AuthenticationDialog authenticationDialog) : ISystem
{
    [Event]
    public async Task OnPlayerConnect(Player player)
    {
        PlayerInfo playerInfo = playerRepository.GetOrDefault(player.Name);

        if (playerInfo is null)
        {
            playerInfo = CreatePlayerInfo(player.Name);
            player.AddComponent<AccountComponent>(playerInfo);
            await authenticationDialog.ShowSignup(player);
            return;
        }

        player.AddComponent<AccountComponent>(playerInfo);
        await authenticationDialog.ShowLogin(player);
    }

    private static PlayerInfo CreatePlayerInfo(string name)
    {
        var playerInfo = new PlayerInfo();
        playerInfo.SetName(name);
        return playerInfo;
    }
}
