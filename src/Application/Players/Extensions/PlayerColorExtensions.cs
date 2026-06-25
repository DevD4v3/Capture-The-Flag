namespace CTF.Application.Players.Extensions;

public static class PlayerColorExtensions
{
    /// <summary>
    /// Sets the player's color to make them visible on the radar map.
    /// </summary>
    /// <remarks>
    /// This method updates the player's color based on their team color, 
    /// setting the alpha value to fully opaque (255).
    /// </remarks>
    public static void ShowOnRadarMap(this Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        Color teamColor = playerInfo.Team.ColorHex;
        player.Color = new Color(teamColor.R, teamColor.G, teamColor.B, 255f);
    }

    /// <summary>
    ///  Sets the player's color to hide them from the radar map.
    /// </summary>
    /// <remarks>
    /// This method updates the player's color based on their team color, 
    /// setting the alpha value to zero (0) to make the player fully transparent.
    /// </remarks>
    public static void HideOnRadarMap(this Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        Color teamColor = playerInfo.Team.ColorHex;
        player.Color = new Color(teamColor.R, teamColor.G, teamColor.B, 00f);
    }
}
