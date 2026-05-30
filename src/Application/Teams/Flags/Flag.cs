namespace CTF.Application.Teams.Flags;

public class Flag
{
    public required FlagModel Model { get; init; }
    public required FlagIcon Icon { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required Color ColorHex { get; init; }

    /// <summary>
    /// Represents the player that holds the flag.
    /// </summary>
    /// <remarks>
    /// It is <c>null</c> when no player holds the flag.
    /// </remarks>
    public Player Carrier { get; private set; }

    /// <summary>
    /// Checks if the flag has been captured by a player.
    /// </summary>
    public bool IsCaptured() => Carrier is not null;

    /// <summary>
    /// Gets the name of the player who captured the flag.
    /// </summary>
    /// <remarks>
    /// If the flag is not captured, returns <c>None</c>.
    /// </remarks>
    public string CarrierName => IsCaptured() ? Carrier.Name : "None";

    /// <summary>
    /// Sets the player who holds the flag.
    /// </summary>
    public void SetCarrier(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        Carrier = player;
        player.SetAttachedObject(
            index: 0, 
            modelId: (int)Model, 
            bone: Bone.Spine, 
            offset: new Vector3(-0.057000f, -0.108999f, 0.075000f), 
            rotation: new Vector3(171.500030f, 66.200012f, -4.100002f), 
            scale: new Vector3(1.0f, 1.0f, 1.0f), 
            materialColor1: ColorHex,
            materialColor2: ColorHex);
    }

    /// <summary>
    /// Removes the flag that the player is holding.
    /// </summary>
    public void RemoveCarrier()
    {
        if (Carrier is not null)
        {
            Carrier.RemoveAttachedObject(0);
            Carrier = default;
        }
    }
}
