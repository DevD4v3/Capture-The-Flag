namespace CTF.Application.Players;

public class RconSecuritySystem(IEntityManager entityManager) : ISystem
{
    /// <summary>
    /// This callback is called when someone attempts to log in to RCON in-game, 
    /// regardless of whether this attempt is successful or not.
    /// </summary>
    /// <param name="ip">
    /// The IP address of the player who attempted to log in to RCON.
    /// </param>
    /// <param name="password">
    /// The password used in the login attempt.
    /// </param>
    /// <param name="success">
    /// false if the password was incorrect, or true if it was correct.
    /// </param>
    [Event]
    public void OnRconLoginAttempt(string ip, string password, bool success)
    {
        var players = entityManager.GetComponents<Player>();
        foreach (Player player in players)
        {
            if (player.Ip == ip)
            {
                player.Kick();
                break;
            }
        }
    }
}
