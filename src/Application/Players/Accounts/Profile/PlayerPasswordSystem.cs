namespace CTF.Application.Players.Accounts.Profile;

public class PlayerPasswordSystem(
    IPlayerRepository playerRepository,
    IDialogService dialogService) : ISystem
{
    private readonly InputDialog _passwordDialog = new()
    {
        IsPassword = true,
        Caption = "Change Password",
        Content = "Enter your new password",
        Button1 = "Accept",
        Button2 = "Close"
    };

    [PlayerCommand("changepass")]
    public async Task ShowPasswordDialog(Player player)
    {
        InputDialogResponse response = await dialogService.ShowAsync(player, _passwordDialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        var enteredPassword = response.InputText ?? string.Empty;
        await ChangePassword(player, enteredPassword);
    }

    private async Task ChangePassword(Player player, string enteredPassword)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        Result result = playerInfo.SetPassword(enteredPassword);
        if (result.IsFailed)
        {
            player.SendClientMessage(Color.Red, result.Message);
            await ShowPasswordDialog(player);
            return;
        }

        var message = Smart.Format(Messages.PasswordSuccessfullyChanged, new { NewPassword = enteredPassword });
        player.SendClientMessage(Color.Yellow, message);
        playerRepository.UpdatePassword(playerInfo);
    }
}
