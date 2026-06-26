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

    private async Task CreatePlayerAccount(Player player, string enteredPassword)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        Result passwordResult = playerInfo.SetPassword(enteredPassword);
        if (passwordResult.IsFailed)
        {
            player.SendClientMessage(Color.Red, passwordResult.Message);
            await View(player);
            return;
        }

        player.GetComponent<AccountComponent>().Authenticate();
        var message = Smart.Format(Messages.CreatePlayerAccount, new { Password = enteredPassword });
        player.SendClientMessage(Color.Red, message);
        playerInfo.SetName(player.Name);
        playerRepository.Create(playerInfo);
    }
}
