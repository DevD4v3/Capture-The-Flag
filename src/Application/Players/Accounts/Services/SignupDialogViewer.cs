namespace CTF.Application.Players.Accounts.Services;

public class SignupDialogViewer(
    IDialogService dialogService,
    IPlayerRepository playerRepository)
{
    private readonly InputDialog _signupDialog = new()
    {
        IsPassword = true,
        Caption = "Signup",
        Content = "Enter a password",
        Button1 = "Accept"
    };

    public async Task View(Player connectedPlayer)
    {
        InputDialogResponse response = await dialogService.ShowAsync(connectedPlayer, _signupDialog);
        if (response.Response == DialogResponse.Disconnected)
            return;

        if (response.Response == DialogResponse.RightButtonOrCancel)
        {
            await View(connectedPlayer);
            return;
        }

        var enteredPassword = response.InputText ?? string.Empty;
        await CreatePlayerAccount(connectedPlayer, enteredPassword);
    }

    private async Task CreatePlayerAccount(Player connectedPlayer, string enteredPassword)
    {
        PlayerInfo playerInfo = connectedPlayer.GetRequiredInfo();
        Result passwordResult = playerInfo.SetPassword(enteredPassword);
        if (passwordResult.IsFailed)
        {
            connectedPlayer.SendClientMessage(Color.Red, passwordResult.Message);
            await View(connectedPlayer);
            return;
        }
        
        bool isAuthenticated = true;
        connectedPlayer.GetComponent<AccountComponent>().Destroy();
        connectedPlayer.AddComponent<AccountComponent>(playerInfo, isAuthenticated);
        var message = Smart.Format(Messages.CreatePlayerAccount, new { Password = enteredPassword });
        connectedPlayer.SendClientMessage(Color.Red, message);
        playerInfo.SetName(connectedPlayer.Name);
        playerRepository.Create(playerInfo);
    }
}
