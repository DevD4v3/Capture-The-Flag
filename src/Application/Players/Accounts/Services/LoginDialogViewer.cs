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

    public async Task View(Player connectedPlayer, PlayerInfo connectedPlayerInfo)
    {
        InputDialogResponse response = await dialogService.ShowAsync(connectedPlayer, _loginDialog);
        if (response.Response == DialogResponse.Disconnected)
            return;

        if (response.Response == DialogResponse.RightButtonOrCancel)
        {
            await View(connectedPlayer, connectedPlayerInfo);
            return;
        }

        var enteredPassword = response.InputText ?? string.Empty;
        bool isWrongPassword = !passwordHasher.Verify(enteredPassword, passwordHash: connectedPlayerInfo.Password);
        if (isWrongPassword)
        {
            const int MaxFailedAttempts = 4;
            var failedAttemptCount = connectedPlayer.GetComponent<FailedAttemptCountComponent>()
                ?? connectedPlayer.AddComponent<FailedAttemptCountComponent>();
            failedAttemptCount.Value++;
            if (failedAttemptCount.Value == MaxFailedAttempts)
            {
                connectedPlayer.Kick();
                return;
            }
            connectedPlayer.SendClientMessage(Color.Red, Messages.WrongPassword);
            await View(connectedPlayer, connectedPlayerInfo);
            return;
        }

        bool isAuthenticated = true;
        connectedPlayer.GetComponent<FailedAttemptCountComponent>()?.Destroy();
        connectedPlayer.GetComponent<AccountComponent>().Destroy();
        connectedPlayer.AddComponent<AccountComponent>(connectedPlayerInfo, isAuthenticated);
        connectedPlayer.SendClientMessage(Color.Red, Messages.SuccessfulLogin);
    }

    private class FailedAttemptCountComponent : Component
    {
        public int Value { get; set; } = 0;
    }
}
