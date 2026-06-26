namespace CTF.Application.Players.Accounts.Services;

/// <summary>
/// Represents a service responsible for authenticating player accounts.
/// </summary>
/// <remarks>
/// Verifies player credentials during login or signup, authenticates
/// the associated account, and persists newly created accounts.
/// </remarks>
public class AccountAuthenticator(
    IPasswordHasher passwordHasher,
    IPlayerRepository playerRepository)
{
    public Result Signup(Player player, string enteredPassword)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        Result passwordResult = playerInfo.SetPassword(enteredPassword);
        if (passwordResult.IsFailed)
        {
            player.SendClientMessage(Color.Red, passwordResult.Message);
            return Result.Failure();
        }

        player.GetComponent<AccountComponent>().Authenticate();
        var message = Smart.Format(Messages.CreatePlayerAccount, new { Password = enteredPassword });
        player.SendClientMessage(Color.Red, message);
        playerInfo.SetName(player.Name);
        playerRepository.Create(playerInfo);
        return Result.Success();
    }

    public Result Login(Player player, string enteredPassword)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
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
                return Result.Failure();
            }

            player.SendClientMessage(Color.Red, Messages.WrongPassword);
            return Result.Failure();
        }

        player.GetComponent<FailedAttemptCountComponent>()?.Destroy();
        player.GetComponent<AccountComponent>().Authenticate();
        player.SendClientMessage(Color.Red, Messages.SuccessfulLogin);
        return Result.Success();
    }

    private class FailedAttemptCountComponent : Component
    {
        public int Value { get; set; } = 0;
    }
}
