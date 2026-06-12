namespace CTF.Application.Players;

public class PlayerCommandLockMiddleware(
    IEntityManager entityManager,
    EventDelegate next,
    MapRotationService mapRotationService)
{
    /// <summary>
    /// Invokes the middleware logic to lock player commands if certain conditions are met.
    /// </summary>
    /// <param name="context">Contains context information about the fired event.</param>
    /// <returns>
    /// <see langword="true"/> if any condition is met to block the command.
    /// Otherwise, it proceeds to the next middleware or action.
    /// </returns>
    public object Invoke(EventContext context)
    {
        EntityId playerId = (EntityId)context.Arguments[0];
        Player player = entityManager.GetComponent<Player>(playerId);

        if (player.IsUnauthenticated())
        {
            player.SendClientMessage(Color.Red, Messages.LoginOrRegisterToContinue);
            return true;
        }

        if (player.IsInClassSelection())
        {
            player.SendClientMessage(Color.Red, Messages.CommandLockClassSelection);
            return true;
        }

        if (mapRotationService.IsMapLoading)
        {
            player.SendClientMessage(Color.Red, Messages.CommandLockMapLoading);
            return true;
        }

        return next(context);
    }
}
