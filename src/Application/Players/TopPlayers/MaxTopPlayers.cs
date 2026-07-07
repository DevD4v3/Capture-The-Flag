namespace CTF.Application.Players.TopPlayers;

/// <summary>
/// Represents the maximum number of top players allowed.
/// </summary>
public class MaxTopPlayers
{
    /// <summary>
    /// Gets the value representing the maximum number of top players.
    /// </summary>
    public int Value { get; private set; }
    private MaxTopPlayers(int value) => Value = value;

    /// <summary>
    /// Creates a new instance of <see cref="MaxTopPlayers"/> if the provided value is within the valid range.
    /// </summary>
    /// <param name="value">The desired maximum number of players.</param>
    public static Result<MaxTopPlayers> Create(int value)
    {
        if (value < 5 || value > 15)
            return Result<MaxTopPlayers>.Failure(Messages.InvalidMaxTopPlayers);

        return Result<MaxTopPlayers>.Success(new MaxTopPlayers(value));
    }
}
