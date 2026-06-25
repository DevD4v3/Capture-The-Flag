namespace CTF.Application.Players.Extensions;

public static class PlayerExtensions
{
    /// <summary>
    /// Gets the information associated with the specified player.
    /// </summary>
    /// <param name="player">
    /// The player whose information should be retrieved.
    /// </param>
    /// <returns>
    /// The player's information.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the player does not have an attached
    /// <see cref="AccountComponent"/>.
    /// </exception>
    public static PlayerInfo GetRequiredInfo(this Player player)
    {
        AccountComponent accountComponent = player.GetComponent<AccountComponent>();
        return accountComponent?.PlayerInfo
            ?? throw new InvalidOperationException(
                $"The player is missing the required {nameof(AccountComponent)}.");
    }

    /// <summary>
    /// Determines whether the specified player is unauthenticated.
    /// </summary>
    /// <param name="player">
    /// The player whose authentication status should be checked.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the player is unauthenticated;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the player does not have an attached
    /// <see cref="AccountComponent"/>.
    /// </exception>
    public static bool IsUnauthenticated(this Player player)
    {
        AccountComponent accountComponent = player.GetComponent<AccountComponent>();
        return accountComponent?.IsUnauthenticated
            ?? throw new InvalidOperationException(
                $"The player is missing the required {nameof(AccountComponent)}.");
    }
}
