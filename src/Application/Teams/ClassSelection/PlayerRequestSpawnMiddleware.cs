namespace CTF.Application.Teams.ClassSelection;

public class PlayerRequestSpawnMiddleware(
    IEntityManager entityManager,
    EventDelegate next,
    MapRotationService mapRotationService)
{
    /// <summary>
    /// Invokes the middleware logic to handle player spawn requests.
    /// </summary>
    /// <param name="context">Contains context information about the fired event.</param>
    /// <returns>
    /// <see langword="false"/> if any condition is met to prevent the player from spawning.
    /// Otherwise, it proceeds to the next middleware or action.
    /// </returns>
    public object Invoke(EventContext context)
    {
        EntityId playerId = (EntityId)context.Arguments[0];
        Player player = entityManager.GetComponent<Player>(playerId);
        if (player.IsUnauthenticated())
        {
            player.SendClientMessage(Color.Red, Messages.LoginOrRegisterToContinue);
            return false;
        }

        if (mapRotationService.IsMapLoading)
        {
            player.SendClientMessage(Color.Red, Messages.MapIsLoading);
            return false;
        }

        Team selectedTeam = player.Team == (int)TeamId.Alpha ? Team.Alpha : Team.Beta;
        if (selectedTeam.IsFull())
        {
            string gameText = selectedTeam.GetAvailabilityMessage();
            player.GameText(gameText, TimeSpan.FromMilliseconds(999999999), GameTextStyle.Style3);
            return false;
        }

        return next(context);
    }
}
