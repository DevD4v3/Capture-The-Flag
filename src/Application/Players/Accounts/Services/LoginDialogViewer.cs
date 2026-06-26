namespace CTF.Application.Players.Accounts.Services;

public class LoginDialogViewer(
    IDialogService dialogService,
    IPasswordHasher passwordHasher)
{
    private readonly InputDialog _loginDialog = new()
    {
        IsPassword = true,
        Caption = "Login",
        Content = "Enter your password",
        Button1 = "Accept"
    };

    public async Task View(Player player)
    {
        InputDialogResponse response = await dialogService.ShowAsync(player, _loginDialog);
        if (response.Response == DialogResponse.Disconnected)
            return;

        if (response.Response == DialogResponse.RightButtonOrCancel)
        {
            await View(player);
            return;
        }

        PlayerInfo playerInfo = player.GetRequiredInfo();
        var enteredPassword = response.InputText ?? string.Empty;
        bool isWrongPassword = !passwordHasher.Verify(enteredPassword, passwordHash: playerInfo.Password);
        if (isWrongPassword)
        {
            const int MaxFailedAttempts = 4;
            var failedAttemptCount = player.GetComponent<FailedAttemptCountComponent>()
                ?? player.AddComponent<FailedAttemptCountComponent>();
            failedAttemptCount.Value++;
            if (failedAttemptCount.Value == MaxFailedAttempts)
            {
                player.Kick();
                return;
            }
            player.SendClientMessage(Color.Red, Messages.WrongPassword);
            await View(player);
            return;
        }

        player.GetComponent<FailedAttemptCountComponent>()?.Destroy();
        player.GetComponent<AccountComponent>().Authenticate();
        player.SendClientMessage(Color.Red, Messages.SuccessfulLogin);
    }

    private class FailedAttemptCountComponent : Component
    {
        public int Value { get; set; } = 0;
    }
}
